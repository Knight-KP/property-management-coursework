using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Data.SqlClient;

namespace PropertyManagementConsole.Data.Repositories;

public class FlatRepository
{
    public decimal? GetBaseRentByFlatId(int flatId)
    {
        using var conn = new SqlConnection(DbConfig.ConnectionString);
        conn.Open();

        string sql = "SELECT BaseRent FROM Flats WHERE FlatId = @FlatId;";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@FlatId", flatId);

        var result = cmd.ExecuteScalar();
        if (result == null) return null;

        return (decimal)result;
    }
}