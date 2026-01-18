using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.Catalog;

public class OptionValueIdTests
{
    [Fact]
    public void OptionValueId_Creation_WithGuid_ReturnsCorrectValue()
    {
        Guid guid = Guid.NewGuid();

        OptionValueId optionValueId = new OptionValueId(guid);

        Assert.Equal(guid, optionValueId.Value);
    }

    [Fact]
    public void OptionValueId_Equality_WithSameGuids_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        OptionValueId id1 = new OptionValueId(guid);
        OptionValueId id2 = new OptionValueId(guid);

        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void OptionValueId_Inequality_WithDifferentGuids_ReturnsFalse()
    {
        OptionValueId id1 = new OptionValueId(Guid.NewGuid());
        OptionValueId id2 = new OptionValueId(Guid.NewGuid());

        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void OptionValueId_ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        OptionValueId optionValueId = new OptionValueId(guid);

        string result = optionValueId.ToString();

        Assert.Equal(guid.ToString(), result);
    }
}
