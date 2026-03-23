using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;

namespace PropertyManagementConsole.Data;

public static class DbTest
{
    public static void Run()
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        // Basic test: count tenants
        string sql = "SELECT COUNT(*) FROM Tenants;";
        using var cmd = new SqlCommand(sql, conn);

        int count = (int)cmd.ExecuteScalar()!;
        Console.WriteLine("DB Connected ✅");
        Console.WriteLine("Number of tenants in DB: " + count);
    }
}