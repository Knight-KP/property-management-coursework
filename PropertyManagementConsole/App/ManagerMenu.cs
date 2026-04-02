using System;
using System.Collections.Generic;
using PropertyManagementConsole.Data.Repositories;
using PropertyManagementConsole.Models;
using PropertyManagementConsole.Services;
using PropertyManagementConsole.Utils;

namespace PropertyManagementConsole.App;

// Leader update: reviewed manager-side workflow readiness before final code freeze
public static class ManagerMenu
{
    public static void Show()
    {
        Console.WriteLine("\nManager Access");

        var tenantRepo = new TenantRepository();
        var complaintRepo = new ComplaintRepository();

        while (true)
        {
            Console.WriteLine("\n--- Manager Menu ---");
            Console.WriteLine("1) View tenants");
            Console.WriteLine("2) Manage tenants (add/remove)");
            Console.WriteLine("3) Generate invoice");
            Console.WriteLine("4) View invoices as manager");
            Console.WriteLine("5) View open complaints");
            Console.WriteLine("6) Update complaint status");
            Console.WriteLine("0) Logout");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();

            if (choice == "0") break;

            switch (choice)
            {
                case "1":
                    ViewTenants(tenantRepo);
                    break;
                case "2":
                    ManageTenants(tenantRepo);
                    break;
                case "3":
                    ShowInvoiceGenerationMenu();
                    break;
                case "4":
                    ViewInvoicesAsManager();
                    break;
                case "5":
                    ViewOpenComplaints(complaintRepo);
                    break;
                case "6":
                    UpdateComplaintStatus(complaintRepo);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void ViewTenants(TenantRepository repo)
    {
        var tenants = repo.GetAllTenants();

        if (tenants.Count == 0)
        {
            Console.WriteLine("No tenants found.");
            return;
        }

        Console.WriteLine("\n--- Tenants ---");
        foreach (var t in tenants)
        {
            Console.WriteLine($"{t.TenantId}: {t.FullName} | FlatId: {t.FlatId} | Move-in: {t.MoveInDate:yyyy-MM-dd}");
        }
    }

    private static void ManageTenants(TenantRepository repo)
    {
        while (true)
        {
            Console.WriteLine("\n--- Manage Tenants ---");
            Console.WriteLine("1) Add tenant");
            Console.WriteLine("2) Remove tenant");
            Console.WriteLine("3) View tenants");
            Console.WriteLine("0) Back");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();
            if (choice == "0") break;

            switch (choice)
            {
                case "1":
                    AddTenant(repo);
                    break;
                case "2":
                    RemoveTenant(repo);
                    break;
                case "3":
                    ViewTenants(repo);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void AddTenant(TenantRepository repo)
    {
        string fullName = InputHelper.ReadRequiredText("Full name: ");
        if (string.IsNullOrWhiteSpace(fullName))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        int? flatId = InputHelper.ReadInt("Flat ID (1-10): ");
        if (flatId == null || flatId < 1 || flatId > 10)
        {
            Console.WriteLine("Flat ID must be between 1 and 10.");
            return;
        }

        DateTime? moveInDate = InputHelper.ReadDate("Move-in date (yyyy-mm-dd): ");
        if (moveInDate == null)
        {
            Console.WriteLine("Invalid move-in date.");
            return;
        }

        if (!repo.FlatExists(flatId.Value))
        {
            Console.WriteLine("Selected flat does not exist in the database.");
            return;
        }

        if (repo.IsFlatOccupied(flatId.Value))
        {
            Console.WriteLine("This flat is already occupied. Please choose another flat.");
            return;
        }

        var tenant = new Tenant
        {
            FullName = fullName,
            FlatId = flatId.Value,
            MoveInDate = moveInDate.Value
        };

        try
        {
            int newTenantId = repo.AddTenantAndReturnId(tenant);
            Console.WriteLine($"Tenant added ✅ Tenant ID: {newTenantId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not add tenant: {ex.Message}");
        }
    }

    private static void RemoveTenant(TenantRepository repo)
    {
        int? tenantId = InputHelper.ReadInt("\nEnter Tenant ID to remove: ");
        if (tenantId == null)
        {
            Console.WriteLine("Invalid Tenant ID.");
            return;
        }

        var tenant = repo.GetTenantById(tenantId.Value);
        if (tenant == null)
        {
            Console.WriteLine("Tenant not found.");
            return;
        }

        Console.WriteLine($"Removing tenant: {tenant.FullName} (Flat {tenant.FlatId})");
        if (!InputHelper.AskYesNo("Are you sure? (y/n): "))
        {
            Console.WriteLine("Removal cancelled.");
            return;
        }

        try
        {
            bool removed = repo.RemoveTenant(tenantId.Value);
            Console.WriteLine(removed ? "Tenant removed ✅" : "Could not remove tenant.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not remove tenant: {ex.Message}");
        }
    }

    private static void ShowJobModuleMenu(JobModuleService service)
    {
        while (true)
        {
            Console.WriteLine($"\n--- {service.ModuleName} Menu ---");
            Console.WriteLine($"1) Add {service.ModuleName.ToLower()} job");
            Console.WriteLine($"2) View {service.ModuleName.ToLower()} jobs for tenant");
            Console.WriteLine("0) Back");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();
            if (choice == "0") break;

            switch (choice)
            {
                case "1":
                    AddModuleJob(service);
                    break;
                case "2":
                    ViewModuleJobs(service);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void AddModuleJob(JobModuleService service)
    {
        int? tenantId = InputHelper.ReadInt("\nTenant ID: ");
        if (tenantId == null)
        {
            Console.WriteLine("Invalid Tenant ID.");
            return;
        }

        int? flatId = InputHelper.ReadInt("Flat ID: ");
        if (flatId == null)
        {
            Console.WriteLine("Invalid Flat ID.");
            return;
        }

        DateTime? jobDate = InputHelper.ReadDate("Job date (yyyy-mm-dd): ");
        if (jobDate == null)
        {
            Console.WriteLine("Invalid date.");
            return;
        }

        decimal? cost = InputHelper.ReadDecimal("Cost: ");
        if (cost == null)
        {
            Console.WriteLine("Invalid cost.");
            return;
        }

        string notes = InputHelper.ReadRequiredText("Notes (optional): ");

        service.AddJob(tenantId.Value, flatId.Value, jobDate.Value, cost.Value, notes);
        Console.WriteLine($"{service.ModuleName} job added ✅");
    }

    private static void ViewModuleJobs(JobModuleService service)
    {
        int? tenantId = InputHelper.ReadInt("\nTenant ID: ");
        if (tenantId == null)
        {
            Console.WriteLine("Invalid Tenant ID.");
            return;
        }

        List<MaintenanceJob> jobs;

        if (InputHelper.AskYesNo("Filter by month/year? (y/n): "))
        {
            int? month = InputHelper.ReadInt("Month (1-12): ");
            if (month == null || month < 1 || month > 12)
            {
                Console.WriteLine("Invalid month.");
                return;
            }

            int? year = InputHelper.ReadInt("Year (e.g. 2026): ");
            if (year == null)
            {
                Console.WriteLine("Invalid year.");
                return;
            }

            jobs = service.GetJobsForTenantMonth(tenantId.Value, month.Value, year.Value);
        }
        else
        {
            jobs = service.GetJobsForTenant(tenantId.Value);
        }

        if (jobs.Count == 0)
        {
            Console.WriteLine($"No {service.ModuleName.ToLower()} jobs found.");
            return;
        }

        Console.WriteLine($"\n--- {service.ModuleName} Jobs ---");
        foreach (var j in jobs)
        {
            Console.WriteLine($"#{j.JobId} | Tenant {j.TenantId} | Flat {j.FlatId} | {j.JobDate:yyyy-MM-dd} | £{j.Cost}");
            if (!string.IsNullOrWhiteSpace(j.Notes))
            {
                Console.WriteLine($"   Notes: {j.Notes}");
            }
        }
    }

    // Leader update: reviewed manager workflow for stable integration across billing and complaint handling
    private static void ShowInvoiceGenerationMenu()
    {
        var service = new InvoiceService();

        while (true)
        {
            Console.WriteLine("\n--- Generate Invoice Menu ---");
            Console.WriteLine("1) Generate monthly rent invoice");
            Console.WriteLine("2) Generate invoice with custom extra charges");
            Console.WriteLine("3) Exit invoice menu");
            Console.Write("Choose: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    GenerateMonthlyInvoice(service);
                    break;
                case "2":
                    GenerateCustomInvoice(service);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static (int TenantId, int FlatId, int Month, int Year)? ReadInvoiceHeaderInput()
    {
        int? tenantId = InputHelper.ReadInt("\nTenant ID: ");
        if (tenantId == null)
        {
            Console.WriteLine("Invalid Tenant ID.");
            return null;
        }

        int? flatId = InputHelper.ReadInt("Flat ID: ");
        if (flatId == null)
        {
            Console.WriteLine("Invalid Flat ID.");
            return null;
        }

        int? month = InputHelper.ReadInt("Month (1-12): ");
        if (month == null || month < 1 || month > 12)
        {
            Console.WriteLine("Invalid month.");
            return null;
        }

        int? year = InputHelper.ReadInt("Year (e.g. 2026): ");
        if (year == null)
        {
            Console.WriteLine("Invalid year.");
            return null;
        }

        return (tenantId.Value, flatId.Value, month.Value, year.Value);
    }

    private static void GenerateMonthlyInvoice(InvoiceService service)
    {
        var input = ReadInvoiceHeaderInput();
        if (input == null)
        {
            return;
        }

        try
        {
            int invoiceId = service.GenerateMonthlyInvoice(input.Value.TenantId, input.Value.FlatId, input.Value.Month, input.Value.Year);
            var invoice = service.GetInvoiceById(invoiceId);
            Console.WriteLine($"Invoice generated ✅ Invoice ID: {invoiceId} | Total £{invoice?.GrandTotal ?? 0}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not generate invoice: {ex.Message}");
        }
    }

    private static void GenerateCustomInvoice(InvoiceService service)
    {
        var input = ReadInvoiceHeaderInput();
        if (input == null)
        {
            return;
        }

        Console.WriteLine("\nYou can now add extra charge lines for complaints or any other charges.");
        Console.WriteLine("Example descriptions: Plumbing damage, Cleaning penalty, Electrical repair charge.");

        var lines = new List<InvoiceLine>();

        while (true)
        {
            string description = InputHelper.ReadRequiredText("Extra line description (or type DONE to finish): ");
            if (description.Equals("DONE", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty.");
                continue;
            }

            decimal? amount = InputHelper.ReadDecimal("Amount to charge: £");
            if (amount == null)
            {
                Console.WriteLine("Invalid amount.");
                continue;
            }

            string category = InputHelper.ReadRequiredText("Category (Complaint Charge/Other): ");
            if (string.IsNullOrWhiteSpace(category))
            {
                category = "Complaint Charge";
            }

            lines.Add(new InvoiceLine
            {
                Description = description,
                Amount = amount.Value,
                Category = category
            });

            Console.WriteLine("Extra line added ✅");
        }

        if (lines.Count == 0)
        {
            Console.WriteLine("No extra lines entered. Invoice not generated.");
            return;
        }

        Console.WriteLine("\nCustom lines to be added:");
        decimal totalExtras = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {lines[i].Description} | {lines[i].Category} | £{lines[i].Amount}");
            totalExtras += lines[i].Amount;
        }
        Console.WriteLine($"Extra charges total: £{totalExtras}");

        if (!InputHelper.AskYesNo("Generate invoice with these extra charges? (y/n): "))
        {
            Console.WriteLine("Invoice creation cancelled.");
            return;
        }

        try
        {
            int invoiceId = service.GenerateInvoiceWithCustomCharges(input.Value.TenantId, input.Value.FlatId, input.Value.Month, input.Value.Year, lines);
            var invoice = service.GetInvoiceById(invoiceId);
            Console.WriteLine($"Invoice generated ✅ Invoice ID: {invoiceId} | Total £{invoice?.GrandTotal ?? 0}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not generate invoice: {ex.Message}");
        }
    }

    // Leader update: reviewed manager menu flow to ensure correct integration between system features
    private static void ViewInvoicesAsManager()
    {
        var repo = new InvoiceRepository();

        int? tenantId = InputHelper.ReadInt("\nEnter Tenant ID: ");
        if (tenantId == null)
        {
            Console.WriteLine("Invalid Tenant ID.");
            return;
        }

        var invoices = repo.GetInvoicesByTenant(tenantId.Value);
        if (invoices.Count == 0)
        {
            Console.WriteLine("No invoices found for this tenant.");
            return;
        }

        Console.WriteLine("\n--- Invoices ---");
        foreach (var inv in invoices)
        {
            Console.WriteLine($"Invoice #{inv.InvoiceId} | {inv.PeriodMonth:D2}/{inv.PeriodYear} | Rent £{inv.BaseRent} | Extras £{inv.ExtrasTotal} | Total £{inv.GrandTotal}");
        }

        int? invoiceId = InputHelper.ReadInt("\nEnter Invoice ID to view lines (or 0 to go back): ");
        if (invoiceId == null)
        {
            Console.WriteLine("Invalid input.");
            return;
        }

        if (invoiceId == 0) return;

        var lines = repo.GetInvoiceLines(invoiceId.Value);
        if (lines.Count == 0)
        {
            Console.WriteLine("No invoice lines found.");
            return;
        }

        Console.WriteLine("\nInvoice Lines:");
        foreach (var line in lines)
        {
            Console.WriteLine($"- {line.Category}: {line.Description} = £{line.Amount}");
        }
    }

    private static void ViewOpenComplaints(ComplaintRepository repo)
    {
        var complaints = repo.GetOpenComplaints();

        if (complaints.Count == 0)
        {
            Console.WriteLine("No open complaints.");
            return;
        }

        Console.WriteLine("\n--- Open Complaints ---");
        foreach (var c in complaints)
        {
            Console.WriteLine($"#{c.ComplaintId} | Tenant {c.TenantId} | Flat {c.FlatId} | {c.Category} | {c.Status} | {c.CreatedAt:yyyy-MM-dd}");
            Console.WriteLine($"   {c.Description}");
        }
    }

    private static void UpdateComplaintStatus(ComplaintRepository repo)
    {
        int? complaintId = InputHelper.ReadInt("\nComplaint ID: ");
        if (complaintId == null)
        {
            Console.WriteLine("Invalid Complaint ID.");
            return;
        }

        Console.WriteLine("Status options: Open / In Progress / Resolved");
        string status = InputHelper.ReadRequiredText("New status: ");
        if (string.IsNullOrWhiteSpace(status))
        {
            Console.WriteLine("Status cannot be empty.");
            return;
        }

        bool updated = repo.UpdateComplaintStatus(complaintId.Value, status);
        Console.WriteLine(updated ? "Complaint status updated ✅" : "Complaint not found.");
    }
}
