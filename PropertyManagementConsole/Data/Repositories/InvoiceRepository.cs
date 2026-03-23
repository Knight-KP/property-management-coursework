using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Data.Repositories;

public class InvoiceRepository
{
    // Save invoice (header)
    public int CreateInvoice(Invoice invoice)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            INSERT INTO Invoices (TenantId, PeriodMonth, PeriodYear, BaseRent, ExtrasTotal)
            OUTPUT INSERTED.InvoiceId
            VALUES (@TenantId, @Month, @Year, @BaseRent, @ExtrasTotal);
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", invoice.TenantId);
        cmd.Parameters.AddWithValue("@Month", invoice.PeriodMonth);
        cmd.Parameters.AddWithValue("@Year", invoice.PeriodYear);
        cmd.Parameters.AddWithValue("@BaseRent", invoice.BaseRent);
        cmd.Parameters.AddWithValue("@ExtrasTotal", invoice.ExtrasTotal);

        return (int)cmd.ExecuteScalar()!;
    }

    // Save invoice line
    public void AddInvoiceLine(InvoiceLine line)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            INSERT INTO InvoiceLines (InvoiceId, Description, Amount, Category)
            VALUES (@InvoiceId, @Description, @Amount, @Category);
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@InvoiceId", line.InvoiceId);
        cmd.Parameters.AddWithValue("@Description", line.Description);
        cmd.Parameters.AddWithValue("@Amount", line.Amount);
        cmd.Parameters.AddWithValue("@Category", line.Category);

        cmd.ExecuteNonQuery();
    }

    // Read invoices for a tenant
    public List<Invoice> GetInvoicesByTenant(int tenantId)
    {
        var invoices = new List<Invoice>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT InvoiceId, TenantId, PeriodMonth, PeriodYear, BaseRent, ExtrasTotal, CreatedAt
            FROM Invoices
            WHERE TenantId = @TenantId
            ORDER BY PeriodYear DESC, PeriodMonth DESC;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            invoices.Add(new Invoice
            {
                InvoiceId = reader.GetInt32(0),
                TenantId = reader.GetInt32(1),
                PeriodMonth = reader.GetInt32(2),
                PeriodYear = reader.GetInt32(3),
                BaseRent = reader.GetDecimal(4),
                ExtrasTotal = reader.GetDecimal(5),
                CreatedAt = reader.GetDateTime(6)
            });
        }

        return invoices;
    }


    public Invoice? GetInvoiceById(int invoiceId)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT InvoiceId, TenantId, PeriodMonth, PeriodYear, BaseRent, ExtrasTotal, CreatedAt
            FROM Invoices
            WHERE InvoiceId = @InvoiceId;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Invoice
        {
            InvoiceId = reader.GetInt32(0),
            TenantId = reader.GetInt32(1),
            PeriodMonth = reader.GetInt32(2),
            PeriodYear = reader.GetInt32(3),
            BaseRent = reader.GetDecimal(4),
            ExtrasTotal = reader.GetDecimal(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }

    // Read invoice lines
    public List<InvoiceLine> GetInvoiceLines(int invoiceId)
    {
        var lines = new List<InvoiceLine>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT LineId, InvoiceId, Description, Amount, Category
            FROM InvoiceLines
            WHERE InvoiceId = @InvoiceId;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lines.Add(new InvoiceLine
            {
                LineId = reader.GetInt32(0),
                InvoiceId = reader.GetInt32(1),
                Description = reader.GetString(2),
                Amount = reader.GetDecimal(3),
                Category = reader.GetString(4)
            });
        }

        return lines;
    }


    public int CreateInvoiceWithoutDuplicateRule(Invoice invoice)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            INSERT INTO Invoices (TenantId, PeriodMonth, PeriodYear, BaseRent, ExtrasTotal)
            OUTPUT INSERTED.InvoiceId
            VALUES (@TenantId, @Month, @Year, @BaseRent, @ExtrasTotal);
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", invoice.TenantId);
        cmd.Parameters.AddWithValue("@Month", invoice.PeriodMonth);
        cmd.Parameters.AddWithValue("@Year", invoice.PeriodYear);
        cmd.Parameters.AddWithValue("@BaseRent", invoice.BaseRent);
        cmd.Parameters.AddWithValue("@ExtrasTotal", invoice.ExtrasTotal);

        return (int)cmd.ExecuteScalar()!;
    }

}
