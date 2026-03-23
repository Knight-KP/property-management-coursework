using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Tests;

[TestClass]
public class InvoiceModelTests
{
    [TestMethod]
    public void GrandTotal_ReturnsBaseRentPlusExtras()
    {
        var invoice = new Invoice
        {
            BaseRent = 1200m,
            ExtrasTotal = 75m
        };

        Assert.AreEqual(1275m, invoice.GrandTotal);
    }

    [TestMethod]
    public void GrandTotal_WithZeroExtras_ReturnsBaseRentOnly()
    {
        var invoice = new Invoice
        {
            BaseRent = 900m,
            ExtrasTotal = 0m
        };

        Assert.AreEqual(900m, invoice.GrandTotal);
    }
}
