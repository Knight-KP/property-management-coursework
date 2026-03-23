namespace PropertyManagementConsole.Models;

public class InvoiceLine
{
    public int LineId { get; set; }
    public int InvoiceId { get; set; }
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
    public string Category { get; set; } = ""; // Rent/Maintenance/Other
}
