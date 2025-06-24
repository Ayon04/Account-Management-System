using System.Data;
using System.Data.SqlClient;
using JobTask.Models;
using Microsoft.Extensions.Configuration;

namespace JobTask.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                 ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        }

        public bool RegisterUser(User user, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("user_insert_proc", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                return rows > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
                {
                    errorMessage = "Email already exists. Please use a different one.";
                    return false;
                }

                errorMessage = "Database error: " + ex.Message;
                return false;
            }
        }

        public User? LoginUser(string email, string password)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("SELECT * FROM [User] WHERE Email = @Email AND Password = @Password", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password); // TODO: hash password

                conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        FullName = reader["FullName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }

                return null; // user not found
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login Error: " + ex.Message);
                throw;
            }
        }


    }
}
