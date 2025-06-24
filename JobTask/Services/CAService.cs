using JobTask.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace JobTask.Services
{
    public class CAService
    {
        private readonly string _connectionString;

        public CAService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<CA> GetAllAccounts()
        {
            List<CA> accounts = new();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM ChartOfAccounts", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                accounts.Add(new CA
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Number = reader["Number"].ToString(),
                    Description = reader["Description"].ToString(),
                    AccountType = reader["AccountType"].ToString()
                });
            }

            return accounts;
        }

        public void CreateAccount(CA ca)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Number", ca.Number);
            cmd.Parameters.AddWithValue("@Description", ca.Description);
            cmd.Parameters.AddWithValue("@AccountType", ca.AccountType);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteAccount(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM ChartOfAccounts WHERE ID = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
