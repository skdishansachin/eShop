namespace eShop.Domain.Catalog;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed class Product : AggregateRoot
{
    private const int MAX_TITLE_LENGTH = 1000;
    private const int MAX_DESCRIPTION_LENGTH = 1000;

    private Product()
    {
        Title = null!;
        Description = null!;
    }

    private Product(ProductId id, string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        if (title.Length > MAX_TITLE_LENGTH)
            throw new ArgumentException(
                $"Title cannot exceed {MAX_TITLE_LENGTH} characters.",
                nameof(title)
            );

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        if (description.Length > MAX_DESCRIPTION_LENGTH)
            throw new ArgumentException(
                $"Description cannot exceed {MAX_DESCRIPTION_LENGTH} characters.",
                nameof(description)
            );

        Id = id;
        Title = title;
        Description = description;
    }

    public ProductId Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    private readonly List<ProductOption> _options = new();
    public IReadOnlyCollection<ProductOption> Options => _options.AsReadOnly();

    private readonly List<ProductVariant> _variants = new();
    public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

    public static Product Create(ProductId id, string title, string description) =>
        new Product(id, title, description);

    public ProductVariant AddVariant(
        Sku sku,
        Money price,
        Dictionary<ProductOptionId, OptionValueId> selections
    )
    {
        // Every option has exactly one value selected
        if (selections.Count != _options.Count)
            throw new InvalidOperationException("A value must be selected for every product option.");

        foreach (var option in _options)
        {
            // Check if the dictionary has a key for this option
            if (!selections.TryGetValue(option.Id, out var selectedValueId))
                throw new InvalidOperationException($"Missing selection for option: {option.Name}");

            // Does this ValueId actually belong to this Option?
            if (!option.HasValue(selectedValueId))
                throw new InvalidOperationException(
                    $"Value {selectedValueId} is not valid for Option {option.Name}"
                );
        }

        // Does this combination already exist?
        // We convert the dictionary values to a comparable set for checking existing variants.
        var valueIds = selections.Values.ToList();

        if (_variants.Any(v => v.HasSameSelections(valueIds)))
            throw new InvalidOperationException("This variant combination already exists.");

        var variant = new ProductVariant(
            new ProductVariantId(Guid.NewGuid()),
            sku,
            price,
            valueIds
        );

        _variants.Add(variant);

        return variant;
    }

    public ProductOption AddOption(ProductOptionId id, OptionName name)
    {
        if (_variants.Any())
            throw new InvalidOperationException(
                "Cannot add new options after variants have been created."
            );

        if (_options.Any(o => o.Name == name))
            throw new InvalidOperationException("Product name already exists.");

        var option = new ProductOption(id, name);

        _options.Add(option);

        return option;
    }

    public OptionValue AddOptionValue(
        ProductOptionId optionId,
        OptionValueId optionValueId,
        OptionValueName name
    )
    {
        var option = _options.FirstOrDefault(o => o.Id == optionId);
        if (option is null)
            throw new InvalidOperationException("Option not found.");

        // if (_variants.Any()) { }

        var value = new OptionValue(optionValueId, name);

        option.AddValue(value);

        return value;
    }
}