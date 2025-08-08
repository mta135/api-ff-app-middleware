using FFAppMiddleware.Model.DataBaseConnection;
using FFAppMiddleware.Model.Models.UserManagement;
using FFAppMiddleware.Model.Repositories.Abstract;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Repositories.Real
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly DataBaseAccesConfig _db;

        public UserManagementRepository()
        {
            _db = new DataBaseAccesConfig();
        }

        public async Task<List<RegisteredUsersApiModel>> RetrieveRegisteredUsers()
        {
            try
            {
                List<RegisteredUsersApiModel> users = new();

                SqlCommand command = new("SELECT * FROM users u", await _db.ConnectionAsync);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        RegisteredUsersApiModel user = new RegisteredUsersApiModel
                        {
                            Id = Convert.ToInt64(reader["user_id"]),
                            UserLogin = Convert.ToString(reader["user_login"]),
                            UserPassword = Convert.ToString(reader["user_password"]),
     
                            BirthDate = reader["birthday"] == DBNull.Value ? null : (DateTime?)reader["birthday"]
                        };
                        users.Add(user);
                    }
                }

                return users;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users: {ex.Message}");
            }
            finally
            {
                await _db.DisposeAsync(); 
            }
        }
    }
}
