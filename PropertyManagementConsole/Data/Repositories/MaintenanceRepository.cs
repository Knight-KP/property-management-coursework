using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Data.Repositories;

public class MaintenanceRepository
{
    // Dev1 update: improved plumbing job handling validation
    public void AddJob(MaintenanceJob job)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            INSERT INTO MaintenanceJobs (FlatId, TenantId, JobType, JobDate, Cost, Notes)
            VALUES (@FlatId, @TenantId, @JobType, @JobDate, @Cost, @Notes);
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@FlatId", job.FlatId);
        cmd.Parameters.AddWithValue("@TenantId", job.TenantId);
        cmd.Parameters.AddWithValue("@JobType", job.JobType);
        cmd.Parameters.AddWithValue("@JobDate", job.JobDate);
        cmd.Parameters.AddWithValue("@Cost", job.Cost);
        cmd.Parameters.AddWithValue("@Notes", (object?)job.Notes ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    public List<MaintenanceJob> GetJobsByTenant(int tenantId)
    {
        var list = new List<MaintenanceJob>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT JobId, FlatId, TenantId, JobType, JobDate, Cost, Notes
            FROM MaintenanceJobs
            WHERE TenantId = @TenantId
            ORDER BY JobDate DESC;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new MaintenanceJob
            {
                JobId = reader.GetInt32(0),
                FlatId = reader.GetInt32(1),
                TenantId = reader.GetInt32(2),
                JobType = reader.GetString(3),
                JobDate = reader.GetDateTime(4),
                Cost = reader.GetDecimal(5),
                Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }

        return list;
    }

    public List<MaintenanceJob> GetJobsByTenantMonth(int tenantId, int month, int year)
    {
        var list = new List<MaintenanceJob>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT JobId, FlatId, TenantId, JobType, JobDate, Cost, Notes
            FROM MaintenanceJobs
            WHERE TenantId = @TenantId
              AND MONTH(JobDate) = @Month
              AND YEAR(JobDate) = @Year
            ORDER BY JobDate DESC;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new MaintenanceJob
            {
                JobId = reader.GetInt32(0),
                FlatId = reader.GetInt32(1),
                TenantId = reader.GetInt32(2),
                JobType = reader.GetString(3),
                JobDate = reader.GetDateTime(4),
                Cost = reader.GetDecimal(5),
                Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }

        return list;
    }

    public List<MaintenanceJob> GetJobsByTenantAndType(int tenantId, string jobType)
    {
        var list = new List<MaintenanceJob>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT JobId, FlatId, TenantId, JobType, JobDate, Cost, Notes
            FROM MaintenanceJobs
            WHERE TenantId = @TenantId
              AND JobType = @JobType
            ORDER BY JobDate DESC;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);
        cmd.Parameters.AddWithValue("@JobType", jobType);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new MaintenanceJob
            {
                JobId = reader.GetInt32(0),
                FlatId = reader.GetInt32(1),
                TenantId = reader.GetInt32(2),
                JobType = reader.GetString(3),
                JobDate = reader.GetDateTime(4),
                Cost = reader.GetDecimal(5),
                Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }

        return list;
    }

    public List<MaintenanceJob> GetJobsByTenantTypeMonth(int tenantId, string jobType, int month, int year)
    {
        var list = new List<MaintenanceJob>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT JobId, FlatId, TenantId, JobType, JobDate, Cost, Notes
            FROM MaintenanceJobs
            WHERE TenantId = @TenantId
              AND JobType = @JobType
              AND MONTH(JobDate) = @Month
              AND YEAR(JobDate) = @Year
            ORDER BY JobDate DESC;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);
        cmd.Parameters.AddWithValue("@JobType", jobType);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new MaintenanceJob
            {
                JobId = reader.GetInt32(0),
                FlatId = reader.GetInt32(1),
                TenantId = reader.GetInt32(2),
                JobType = reader.GetString(3),
                JobDate = reader.GetDateTime(4),
                Cost = reader.GetDecimal(5),
                Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }

        return list;
    }

}
