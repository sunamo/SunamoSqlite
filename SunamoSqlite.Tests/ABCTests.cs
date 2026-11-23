using SunamoSqlite._public.SunamoData.Data;

namespace SunamoSqlite.Tests;

/// <summary>
/// Tests for the name-value pairs passed to the stored procedures.
/// </summary>
public class ABCTests
{
    /// <summary>
    /// Tests that the names and the values of the pairs are returned in the order they were added.
    /// </summary>
    [Fact]
    public void OnlyAsAndOnlyBsKeepOrder()
    {
        var pairs = new ABC(AB.Get("ID", 1), AB.Get("Nazev", "x"));

        Assert.Equal(["ID", "Nazev"], pairs.OnlyAs());
        Assert.Equal([1, "x"], pairs.OnlyBs());
        Assert.Equal(2, pairs.Length);
    }
}
