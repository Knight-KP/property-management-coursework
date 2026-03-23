using System.Globalization;
using PropertyManagementConsole.Utils;

namespace PropertyManagementConsole.Tests;

[TestClass]
public class InputHelperTests
{
    [TestMethod]
    public void ReadInt_WithValidNumber_ReturnsParsedValue()
    {
        using var input = new StringReader("42" + Environment.NewLine);
        Console.SetIn(input);

        int? result = InputHelper.ReadInt("Enter number: ");

        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void ReadInt_WithInvalidNumber_ReturnsNull()
    {
        using var input = new StringReader("abc" + Environment.NewLine);
        Console.SetIn(input);

        int? result = InputHelper.ReadInt("Enter number: ");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void AskYesNo_WithYesInput_ReturnsTrue()
    {
        using var input = new StringReader("yes" + Environment.NewLine);
        Console.SetIn(input);

        bool result = InputHelper.AskYesNo("Continue? ");

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void AskYesNo_WithNoInput_ReturnsFalse()
    {
        using var input = new StringReader("n" + Environment.NewLine);
        Console.SetIn(input);

        bool result = InputHelper.AskYesNo("Continue? ");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ReadRequiredText_TrimsWhitespace()
    {
        using var input = new StringReader("  Krish Patel  " + Environment.NewLine);
        Console.SetIn(input);

        string result = InputHelper.ReadRequiredText("Name: ");

        Assert.AreEqual("Krish Patel", result);
    }
}
