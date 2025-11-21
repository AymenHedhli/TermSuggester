// ==============================================
// Unit Tests (xUnit)
// ==============================================
using System.Collections.Generic;
using System.Linq;
using TermSuggester.BackOfficeEngine;
using Xunit;


public class TermSuggesterTests
{
    [Fact]
    public void ReturnsExactAndClosestMatches()
    {
        List<string> choices = new List<string> { "gros", "gras", "graisse", "agressif", "go", "ros", "gro" };
        int numberOfSuggestions = 2;
        var suggester = new TermSuggestions();

        var result = suggester.GetSuggestions("gros", choices, numberOfSuggestions);

        Assert.Equal(new List<string> { "gros", "gras" }, result);
    }


    [Fact]
    public void ReturnsEmptyList_WhenNoMatchesFound()
    {
        List<string> choices = new List<string> { "alpha", "beta", "gamma" };
        int numberOfSuggestions = 3;
        var suggester = new TermSuggestions();

        var result = suggester.GetSuggestions("zzz", choices, numberOfSuggestions);

        Assert.Empty(result);
    }


    [Fact]
    public void OrdersByScoreThenLengthThenAlphabetically()
    {
        List<string> choices = new List<string> { "aaaa", "aaab", "abaa", "zzzz" };
        int numberOfSuggestions = 3;
        var suggester = new TermSuggestions();

        var result = suggester.GetSuggestions("aaaa", choices, numberOfSuggestions);

        Assert.Equal(new List<string> { "aaaa", "aaab", "abaa" }, result);
    }


    [Fact]
    public void LimitsResultsToN()
    {
        List<string> choices = new List<string> { "test", "tast", "tost", "tist" };
        int numberOfSuggestions = 2;
        var suggester = new TermSuggestions();

        var result = suggester.GetSuggestions("test", choices, numberOfSuggestions);

        Assert.Equal(numberOfSuggestions, result.Count());
    }
}

