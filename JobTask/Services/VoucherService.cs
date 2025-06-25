using JobTask.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace JobTask.Services
{
    public class VoucherService
    {
        private readonly string _connectionString;

        public VoucherService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void InsertVoucher(VoucherEntries entry)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_SaveVoucher", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VoucherType", entry.VoucherType);
            cmd.Parameters.AddWithValue("@VoucherDate", entry.VoucherDate);
            cmd.Parameters.AddWithValue("@ReferenceNo", entry.ReferenceNo);
            cmd.Parameters.AddWithValue("@AccountID", entry.AccountID);
            cmd.Parameters.AddWithValue("@DebitAmount", entry.DebitAmount);
            cmd.Parameters.AddWithValue("@CreditAmount", entry.CreditAmount);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<VoucherEntries> GetAllVouchers()
        {
            var list = new List<VoucherEntries>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM VoucherEntries ORDER BY VoucherDate DESC", conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new VoucherEntries
                {
                    VoucherType = reader["VoucherType"].ToString(),
                    VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                    ReferenceNo = reader["ReferenceNo"].ToString(),
                    AccountID = Convert.ToInt32(reader["AccountID"]),
                    DebitAmount = Convert.ToDecimal(reader["DebitAmount"]),
                    CreditAmount = Convert.ToDecimal(reader["CreditAmount"])
                });
            }

            return list;
        }
    }
}
