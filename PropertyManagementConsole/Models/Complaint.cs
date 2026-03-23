using System;

namespace PropertyManagementConsole.Models;

public class Complaint
{
    public int ComplaintId { get; set; }
    public int TenantId { get; set; }
    public int FlatId { get; set; }
    public string Category { get; set; } = "";     // Electric/Water/Other
    public string Description { get; set; } = "";
    public string Status { get; set; } = "Open";   // Open/InProgress/Resolved
    public DateTime CreatedAt { get; set; }
}
