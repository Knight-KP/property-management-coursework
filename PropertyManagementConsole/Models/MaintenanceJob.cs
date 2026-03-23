using System;

namespace PropertyManagementConsole.Models;

public class MaintenanceJob
{
    public int JobId { get; set; }
    public int FlatId { get; set; }
    public int TenantId { get; set; }
    public string JobType { get; set; } = ""; // Plumber/Electrician/Other
    public DateTime JobDate { get; set; }
    public decimal Cost { get; set; }
    public string? Notes { get; set; }
}
