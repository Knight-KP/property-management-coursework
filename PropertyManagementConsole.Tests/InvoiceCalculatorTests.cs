using PropertyManagementConsole.Models;
using PropertyManagementConsole.Utils;

namespace PropertyManagementConsole.Tests;

[TestClass]
public class InvoiceCalculatorTests
{
    [TestMethod]
    public void CalculateExtrasTotal_WithNoJobs_ReturnsZero()
    {
        var jobs = new List<MaintenanceJob>();

        var result = InvoiceCalculator.CalculateExtrasTotal(jobs);

        Assert.AreEqual(0m, result);
    }

    [TestMethod]
    public void CalculateExtrasTotal_WithMultipleJobs_ReturnsCorrectSum()
    {
        var jobs = new List<MaintenanceJob>
        {
            new MaintenanceJob { Cost = 50m },
            new MaintenanceJob { Cost = 125.50m },
            new MaintenanceJob { Cost = 24.50m }
        };

        var result = InvoiceCalculator.CalculateExtrasTotal(jobs);

        Assert.AreEqual(200m, result);
    }

    [TestMethod]
    public void CalculateGrandTotal_AddsBaseRentAndExtras()
    {
        var result = InvoiceCalculator.CalculateGrandTotal(950m, 175m);

        Assert.AreEqual(1125m, result);
    }
}
