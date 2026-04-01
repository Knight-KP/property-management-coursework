using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;
using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Data.Repositories;

public class ComplaintRepository
{
    // Dev2 update: refined electric data handling and status integration
    // Dev2 update: reviewed electric complaint flow for final system demonstration
    public void AddComplaint(Complaint complaint)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            INSERT INTO Complaints (TenantId, FlatId, Category, Description, Status, CreatedAt)
            VALUES (@TenantId, @FlatId, @Category, @Description, @Status, GETDATE());
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", complaint.TenantId);
        cmd.Parameters.AddWithValue("@FlatId", complaint.FlatId);
        cmd.Parameters.AddWithValue("@Category", complaint.Category);
        cmd.Parameters.AddWithValue("@Description", complaint.Description);
        cmd.Parameters.AddWithValue("@Status", complaint.Status);

        cmd.ExecuteNonQuery();
    }

    // Dev2 update: improved electric complaint data handling and processing flow
    public List<Complaint> GetComplaintsByTenant(int tenantId)
    {
        var list = new List<Complaint>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT ComplaintId, TenantId, FlatId, Category, Description, Status, CreatedAt
            FROM Complaints
            WHERE TenantId = @TenantId
            ORDER BY CreatedAt DESC;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Complaint
            {
                ComplaintId = reader.GetInt32(0),
                TenantId = reader.GetInt32(1),
                FlatId = reader.GetInt32(2),
                Category = reader.GetString(3),
                Description = reader.GetString(4),
                Status = reader.GetString(5),
                CreatedAt = reader.GetDateTime(6)
            });
        }

        return list;
    }

    public List<Complaint> GetOpenComplaints()
    {
        var list = new List<Complaint>();

        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            SELECT ComplaintId, TenantId, FlatId, Category, Description, Status, CreatedAt
            FROM Complaints
            WHERE Status <> 'Resolved'
            ORDER BY CreatedAt DESC;
        ";

        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Complaint
            {
                ComplaintId = reader.GetInt32(0),
                TenantId = reader.GetInt32(1),
                FlatId = reader.GetInt32(2),
                Category = reader.GetString(3),
                Description = reader.GetString(4),
                Status = reader.GetString(5),
                CreatedAt = reader.GetDateTime(6)
            });
        }

        return list;
    }

    public bool UpdateComplaintStatus(int complaintId, string newStatus)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = @"
            UPDATE Complaints
            SET Status = @Status
            WHERE ComplaintId = @ComplaintId;
        ";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Status", newStatus);
        cmd.Parameters.AddWithValue("@ComplaintId", complaintId);

        int rows = cmd.ExecuteNonQuery();
        return rows > 0;
    }
}
