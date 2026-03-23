using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyManagementConsole.Data;

public static class DbConfig
{
    // IMPORTANT: Change Server if needed
    public static string ConnectionString =
        "Server=(localdb)\\MSSQLLocalDB;Database=PropertyManagementDB;Trusted_Connection=True;TrustServerCertificate=True;";
}