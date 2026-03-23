using System;

namespace PropertyManagementConsole.Models;

public class Tenant
{
    public int TenantId { get; set; }
    public string FullName { get; set; } = "";
    public int FlatId { get; set; }
    public DateTime MoveInDate { get; set; }
}
