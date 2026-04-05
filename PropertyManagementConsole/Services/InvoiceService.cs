// Leader: Invoice generation logic reviewed and structured
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PropertyManagementConsole.Data.Repositories;
using PropertyManagementConsole.Models;
using PropertyManagementConsole.Utils;

namespace PropertyManagementConsole.Services;

// Leader update: reviewed billing flow for stability before final development phase
// Leader update: finalized invoice and system integration before code freeze
public class InvoiceService
{
    private readonly FlatRepository _flatRepository = new FlatRepository();
    private readonly MaintenanceRepository _maintenanceRepository = new MaintenanceRepository();
    private readonly InvoiceRepository _invoiceRepository = new InvoiceRepository();

    // Leader update: reviewed invoice workflow stability ahead of final integration phase
    public int GenerateMonthlyInvoice(int tenantId, int flatId, int month, int year)
    {
        decimal? baseRent = _flatRepository.GetBaseRentByFlatId(flatId);
        if (baseRent == null)
        {
            throw new Exception("Flat not found or BaseRent missing.");
        }

        var monthlyJobs = _maintenanceRepository.GetJobsByTenantMonth(tenantId, month, year);
        decimal extraCharges = InvoiceCalculator.CalculateExtrasTotal(monthlyJobs);

        // Leader update: reviewed monthly and custom invoice flow for final billing consistency
        var invoice = new Invoice
        {
            TenantId = tenantId,
            PeriodMonth = month,
            PeriodYear = year,
            BaseRent = baseRent.Value,
            ExtrasTotal = extraCharges
        };

        int newInvoiceId = CreateInvoiceHeader(invoice);

        _invoiceRepository.AddInvoiceLine(new InvoiceLine
        {
            InvoiceId = newInvoiceId,
            Description = $"Monthly Rent ({month:D2}/{year})",
            Amount = baseRent.Value,
            Category = "Rent"
        });

        // Leader update: refined custom invoice handling and ensured consistent billing flow
        foreach (var job in monthlyJobs)
        {
            string jobDescription = $"{job.JobType} on {job.JobDate:yyyy-MM-dd}";
            if (!string.IsNullOrWhiteSpace(job.Notes))
            {
                jobDescription += $" ({job.Notes})";
            }

            _invoiceRepository.AddInvoiceLine(new InvoiceLine
            {
                InvoiceId = newInvoiceId,
                Description = jobDescription,
                Amount = job.Cost,
                Category = "Maintenance"
            });
        }

        return newInvoiceId;
    }

    // Leader update: reviewed invoice workflow stability ahead of final integration phase
    public int GenerateInvoiceWithCustomCharges(int tenantId, int flatId, int month, int year, List<InvoiceLine> extraLines)
    {
        if (extraLines == null || extraLines.Count == 0)
        {
            throw new Exception("Please add at least one extra charge line.");
        }

        decimal extraCharges = 0;
        foreach (var extraLine in extraLines)
        {
            extraCharges += extraLine.Amount;
        }

        var invoice = new Invoice
        {
            TenantId = tenantId,
            PeriodMonth = month,
            PeriodYear = year,
            BaseRent = 0,
            ExtrasTotal = extraCharges
        };

        int newInvoiceId = CreateCustomInvoiceHeader(invoice);

        foreach (var extraLine in extraLines)
        {
            extraLine.InvoiceId = newInvoiceId;
            _invoiceRepository.AddInvoiceLine(extraLine);
        }

        return newInvoiceId;
    }

    public Invoice? GetInvoiceById(int invoiceId)
    {
        return _invoiceRepository.GetInvoiceById(invoiceId);
    }

    private int CreateInvoiceHeader(Invoice invoice)
    {
        try
        {
            return _invoiceRepository.CreateInvoice(invoice);
        }
        catch (SqlException)
        {
            throw new Exception("Monthly rent invoice already exists for this tenant and month/year.");
        }
    }

    private int CreateCustomInvoiceHeader(Invoice invoice)
    {
        return _invoiceRepository.CreateInvoiceWithoutDuplicateRule(invoice);
    }
}
