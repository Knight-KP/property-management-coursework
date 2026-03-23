using System;

namespace PropertyManagementConsole.Models;

public class Invoice
{
    public int InvoiceId { get; set; }
    public int TenantId { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public decimal BaseRent { get; set; }
    public decimal ExtrasTotal { get; set; }
    public DateTime CreatedAt { get; set; }

    public decimal GrandTotal => BaseRent + ExtrasTotal;
}
