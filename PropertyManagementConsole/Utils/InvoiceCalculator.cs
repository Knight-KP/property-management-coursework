using System.Collections.Generic;
using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Utils;

public static class InvoiceCalculator
{
    public static decimal CalculateExtrasTotal(List<MaintenanceJob> jobs)
    {
        decimal total = 0;

        foreach (var job in jobs)
        {
            total += job.Cost;
        }

        return total;
    }

    public static decimal CalculateGrandTotal(decimal baseRent, decimal extrasTotal)
    {
        return baseRent + extrasTotal;
    }
}
