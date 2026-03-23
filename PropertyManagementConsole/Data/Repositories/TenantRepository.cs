using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Data.Repositories;

public class TenantRepository
{
    public List<Tenant> GetAllTenants()
    {
        var tenants = new List<Tenant>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        const string sql = @"SELECT TenantId, FullName, FlatId, MoveInDate
                             FROM Tenants
                             ORDER BY TenantId;";

        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            tenants.Add(new Tenant
            {
                TenantId = reader.GetInt32(0),
                FullName = reader.GetString(1),
                FlatId = reader.GetInt32(2),
                MoveInDate = reader.GetDateTime(3)
            });
        }

        return tenants;
    }

    public Tenant? GetTenantById(int tenantId)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        const string sql = @"SELECT TenantId, FullName, FlatId, MoveInDate
                             FROM Tenants
                             WHERE TenantId = @TenantId;";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            return null;

        return new Tenant
        {
            TenantId = reader.GetInt32(0),
            FullName = reader.GetString(1),
            FlatId = reader.GetInt32(2),
            MoveInDate = reader.GetDateTime(3)
        };
    }

    public bool FlatExists(int flatId)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        const string sql = "SELECT COUNT(*) FROM Flats WHERE FlatId = @FlatId;";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@FlatId", flatId);

        return (int)cmd.ExecuteScalar()! > 0;
    }

    public bool IsFlatOccupied(int flatId)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        const string sql = "SELECT COUNT(*) FROM Tenants WHERE FlatId = @FlatId;";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@FlatId", flatId);

        return (int)cmd.ExecuteScalar()! > 0;
    }

    public int AddTenantAndReturnId(Tenant tenant)
    {
        if (!FlatExists(tenant.FlatId))
            throw new Exception("Selected flat does not exist.");

        if (IsFlatOccupied(tenant.FlatId))
            throw new Exception("This flat already has a tenant. Please choose another flat.");

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        const string sql = @"INSERT INTO Tenants (FullName, FlatId, MoveInDate)
                             VALUES (@FullName, @FlatId, @MoveInDate);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@FullName", tenant.FullName);
        cmd.Parameters.AddWithValue("@FlatId", tenant.FlatId);
        cmd.Parameters.AddWithValue("@MoveInDate", tenant.MoveInDate);

        return (int)cmd.ExecuteScalar()!;
    }

    public bool AddTenant(Tenant tenant)
    {
        return AddTenantAndReturnId(tenant) > 0;
    }

    public bool RemoveTenant(int tenantId)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        using var tx = conn.BeginTransaction();

        try
        {
            DeleteByTenant(conn, tx, "DELETE FROM InvoiceLines WHERE InvoiceId IN (SELECT InvoiceId FROM Invoices WHERE TenantId = @TenantId);", tenantId);
            DeleteByTenant(conn, tx, "DELETE FROM Invoices WHERE TenantId = @TenantId;", tenantId);
            DeleteByTenant(conn, tx, "DELETE FROM Complaints WHERE TenantId = @TenantId;", tenantId);
            DeleteByTenant(conn, tx, "DELETE FROM MaintenanceJobs WHERE TenantId = @TenantId;", tenantId);

            using var tenantCmd = new SqlCommand("DELETE FROM Tenants WHERE TenantId = @TenantId;", conn, tx);
            tenantCmd.Parameters.AddWithValue("@TenantId", tenantId);
            int rows = tenantCmd.ExecuteNonQuery();

            tx.Commit();
            return rows > 0;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    private static void DeleteByTenant(SqlConnection conn, SqlTransaction tx, string sql, int tenantId)
    {
        using var cmd = new SqlCommand(sql, conn, tx);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);
        cmd.ExecuteNonQuery();
    }
}
