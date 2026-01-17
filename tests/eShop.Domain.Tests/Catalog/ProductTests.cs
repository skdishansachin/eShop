using System.Reflection;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.Catalog;

public class ProductTests
{
    [Fact]
    public void Product_Create_WithValidParameters_ReturnsProductObject()
    {
        var id = new ProductId(Guid.NewGuid());
        var title = "Test Product";
        var description = "This is a test product description.";

        var product = Product.Create(id, title, description);

        Assert.Equal(id, product.Id);
        Assert.Equal(title, product.Title);
        Assert.Equal(description, product.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Product_Create_WithInvalidTitle_ThrowsArgumentException(string invalidTitle)
    {
        var id = new ProductId(Guid.NewGuid());
        var description = "Valid description.";

        Assert.Throws<ArgumentException>(() => Product.Create(id, invalidTitle, description));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Product_Create_WithInvalidDescription_ThrowsArgumentException(
        string invalidDescription
    )
    {
        var id = new ProductId(Guid.NewGuid());
        var title = "Valid Title.";

        Assert.Throws<ArgumentException>(() => Product.Create(id, title, invalidDescription));
    }

    [Fact]
    public void Product_Create_WithTooLongTitle_ThrowsArgumentException()
    {
        var id = new ProductId(Guid.NewGuid());
        var longTitle = new string('A', 1001); // MAX_TITLE_LENGTH is 1000
        var description = "Valid description.";

        Assert.Throws<ArgumentException>(() => Product.Create(id, longTitle, description));
    }

    [Fact]
    public void Product_Create_WithTooLongDescription_ThrowsArgumentException()
    {
        var id = new ProductId(Guid.NewGuid());
        var title = "Valid Title.";
        var longDescription = new string('A', 1001); // MAX_DESCRIPTION_LENGTH is 1000

        Assert.Throws<ArgumentException>(() => Product.Create(id, title, longDescription));
    }

    [Fact]
    public void Product_AddOption_WithValidNewOption_AddsOptionToList()
    {
        var product = Product.Create(new ProductId(Guid.NewGuid()), "Shirt", "Cool shirt");
        var optionId = new ProductOptionId(Guid.NewGuid());
        var optionName = new OptionName("Color");

        var option = product.AddOption(optionId, optionName);

        Assert.Single(product.Options);
        Assert.Equal(optionId, option.Id);
        Assert.Equal(optionName, option.Name);
    }

    [Fact]
    public void Product_AddOption_WithDuplicateOptionName_ThrowsInvalidOperationException()
    {
        var product = Product.Create(new ProductId(Guid.NewGuid()), "Shirt", "Cool shirt");
        var optionId = new ProductOptionId(Guid.NewGuid());
        var optionName = new OptionName("Color");
        product.AddOption(optionId, optionName);

        Assert.Throws<InvalidOperationException>(() =>
            product.AddOption(new ProductOptionId(Guid.NewGuid()), optionName)
        );
    }

    [Fact]
    public void Product_AddOption_AfterVariantsCreated_ThrowsInvalidOperationException()
    {
        var product = Product.Create(new ProductId(Guid.NewGuid()), "Shirt", "Cool shirt");

        var color = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Color"));
        var red = product.AddOptionValue(
            color.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Red")
        );

        var sku = Sku.Create("SHIRT-RED");
        var price = Money.Create(20m, "USD");
        Dictionary<ProductOptionId, OptionValueId> selections = new() { { color.Id, red.Id } };

        product.AddVariant(sku, price, selections);

        var sizeOptionId = new ProductOptionId(Guid.NewGuid());
        var sizeOptionName = new OptionName("Size");

        Assert.Throws<InvalidOperationException>(() =>
            product.AddOption(sizeOptionId, sizeOptionName)
        );
    }

    [Fact]
    public void Product_AddVariant_WithValidSelections_AddsVariantToList()
    {
        var product = Product.Create(new ProductId(Guid.NewGuid()), "Shirt", "Cool shrit");

        var color = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Color"));
        var red = product.AddOptionValue(
            color.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Red")
        );
        var blue = product.AddOptionValue(
            color.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Blue")
        );

        var size = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Size"));
        var small = product.AddOptionValue(
            size.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("S")
        );
        var medium = product.AddOptionValue(
            size.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("M")
        );

        var variant1 = product.AddVariant(
            Sku.Create("SHIRT-RED-S"),
            Money.Create(1400m, "LKR"),
            new Dictionary<ProductOptionId, OptionValueId>
            {
                { color.Id, red.Id },
                { size.Id, small.Id },
            }
        );

        var variant2 = product.AddVariant(
            Sku.Create("SHIRT-BLUE-M"),
            Money.Create(1400m, "LKR"),
            new Dictionary<ProductOptionId, OptionValueId>
            {
                { color.Id, blue.Id },
                { size.Id, medium.Id },
            }
        );

        Assert.Equal(2, product.Variants.Count);

        Assert.NotNull(variant1);
        Assert.Equal(Sku.Create("SHIRT-RED-S"), variant1.Sku);
        Assert.Contains(red.Id, variant1.Values);
        Assert.Contains(small.Id, variant1.Values);

        Assert.NotNull(variant2);
        Assert.Equal(Sku.Create("SHIRT-BLUE-M"), variant2.Sku);
        Assert.Contains(blue.Id, variant2.Values);

        Assert.Contains(variant1, product.Variants);
        Assert.Contains(variant2, product.Variants);
    }

    [Fact]
    public void Product_AddVariant_WithMissingSelectionForOption_ThrowsInvalidOperationException()
    {
        var product = Product.Create(new ProductId(Guid.NewGuid()), "Shirt", "Cool shirt");

        var color = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Color"));
        var red = product.AddOptionValue(
            color.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Red")
        );

        var size = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Size"));
        var small = product.AddOptionValue(
            size.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Small")
        );

        var sku = Sku.Create("SHIRT-MISSING-SIZE");
        Money price = Money.Create(20m, "USD");
        Dictionary<ProductOptionId, OptionValueId> selections = new() { { color.Id, red.Id } };

        Assert.Throws<InvalidOperationException>(() => product.AddVariant(sku, price, selections));
    }

    [Fact]
    public void Product_AddVariant_WithInvalidValueForOption_ThrowsInvalidOperationException()
    {
        var product = Product.Create(new ProductId(Guid.NewGuid()), "Shirt", "Cool shirt");

        var color = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Color"));
        var red = product.AddOptionValue(
            color.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Red")
        );

        var size = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Size"));
        var small = product.AddOptionValue(
            size.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Small")
        );

        var invalidValueId = new OptionValueId(Guid.NewGuid());
        var sku = Sku.Create("SHIRT-INVALID-COLOR");
        var price = Money.Create(20m, "USD");
        Dictionary<ProductOptionId, OptionValueId> selections = new()
        {
            { color.Id, invalidValueId }, // Invalid value for color option
            { size.Id, new OptionValueId(Guid.NewGuid()) },
        };

        Assert.Throws<InvalidOperationException>(() => product.AddVariant(sku, price, selections));
    }

    [Fact]
    public void Product_AddVariant_WithDuplicateVariantCombination_ThrowsInvalidOperationException()
    {
        var product = Product.Create(new ProductId(Guid.NewGuid()), "Shirt", "Cool shirt");

        var color = product.AddOption(new ProductOptionId(Guid.NewGuid()), new OptionName("Color"));
        var red = product.AddOptionValue(
            color.Id,
            new OptionValueId(Guid.NewGuid()),
            new OptionValueName("Red")
        );

        Sku sku1 = Sku.Create("SHIRT-RED-1");
        Money price1 = Money.Create(20m, "USD");
        Dictionary<ProductOptionId, OptionValueId> selections1 = new() { { color.Id, red.Id } };
        product.AddVariant(sku1, price1, selections1); // Add first variant

        Sku sku2 = Sku.Create("SHIRT-RED-2"); // Different SKU but same selections
        Money price2 = Money.Create(22m, "USD");
        Dictionary<ProductOptionId, OptionValueId> selections2 = new() { { color.Id, red.Id } };

        Assert.Throws<InvalidOperationException>(() =>
            product.AddVariant(sku2, price2, selections2)
        );
    }
}
