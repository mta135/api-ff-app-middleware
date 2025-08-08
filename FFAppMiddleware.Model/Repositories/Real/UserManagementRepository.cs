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
        private DataBaseAccesConfig _db;

        public UserManagementRepository()
        {
            _db = new DataBaseAccesConfig();
        }

        public List<UserApiModel> RetrieveRegisteredUsers()
        {
            try
            {
                List<UserApiModel> users = new();
                SqlCommand command = new SqlCommand("SELECT * FROM users u", _db.Connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserApiModel user = new UserApiModel
                        {
                            Id = Convert.ToInt64(reader["user_id"]),
                            UserLogin = Convert.ToString(reader["user_login"]),
                            UserPassword = Convert.ToString(reader["user_password"]),
      
                            Idnp = Convert.ToString(reader["user_idnp"]),
                            BirthDate = reader["birthday"] == DBNull.Value ? null : (DateTime?)reader["birthday"]
                        };
                        users.Add(user);
                    }
                }

                return users;
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL exception (not implemented here)
                throw new Exception($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Log the general exception (not implemented here)
                throw new Exception($"Error retrieving users: {ex.Message}");
            }
            finally
            {
                _db.Dispose();
            }
        }
    }
}
