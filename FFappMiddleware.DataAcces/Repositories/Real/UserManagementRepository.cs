using FFappMiddleware.DataAcces.DataBaseConnection;
using FFappMiddleware.DataBase.Logger;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.UserManagement;
using Microsoft.Data.SqlClient;
using System.Data;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;


namespace FFappMiddleware.DataBase.Repositories.Real
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly DataBaseAccesConfig _db;

        public UserManagementRepository()
        {
            _db = new DataBaseAccesConfig();
        }


        public async Task<List<LoyaltyUser>> GetUserByPhone(string phone)
        {
            try
            {
                List<LoyaltyUser> users = new();

                await using var connection = await _db.ConnectionAsync;
                await using var command = new SqlCommand(@"
            SELECT TOP 1 
                  sc.[id]
                , sc.[name]
                , sc.[surname]
                , sc.[patronymic]
                , sc.[gender]
                , sc.[email]
                , sc.[phone]
                , sc.[birthdate]
                , sc.[address]
                , sc.[time_stamp]
                , sc.[accept_abm_sms_notify]
                , sc.[accept_abm_email_notify]
            FROM [services_clients] sc
            INNER JOIN localities l ON l.locality_id = sc.locality_id
            WHERE sc.phone = @phone", connection);

                command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = phone;

                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new LoyaltyUser
                        {
                            Id = Guid.Parse(reader["id"].ToString()!),
                            LastName = reader["name"] as string,
                            FirstName = reader["surname"] as string,
                            MiddleName = reader["patronymic"] as string,
                            Gender =  (int)Convert.ToInt32(reader["gender"]),
                            Email = reader["email"] as string,
                            Phone = reader["phone"] as string,
                            BirthDay =  (DateTime)reader["birthdate"],
                            Address = reader["address"] as string,
                            Created =  (DateTime)reader["time_stamp"],
                            SmsNotify = reader["accept_abm_sms_notify"] != DBNull.Value && Convert.ToInt32(reader["accept_abm_sms_notify"]) == 1,
                            EmailNotify = reader["accept_abm_email_notify"] != DBNull.Value && Convert.ToInt32(reader["accept_abm_email_notify"]) == 1,
                            
                        };

                        users.Add(user);
                    }
                } 
                foreach (var user in users)
                {
                    user.Cards = await GetUserCardsAsync(user.Id);
                    user.Balance = await GetUserBalanceAsync(user.Id);

                }

                return users;
            }
            catch (SqlException sqlEx)
            {

                WriteLog.DB.Error($"SQL Error: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB.Error($"Error: {ex.Message}");
                throw new Exception($"Error: {ex.Message}", ex);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(string barcode)
        {
            try
            {
                List<LoyaltyUser> users = new();

                await using var connection = await _db.ConnectionAsync;
                await using var command = new SqlCommand(@"
            SELECT TOP 1 
                  sc.[id]
                , sc.[name]
                , sc.[surname]
                , sc.[patronymic]
                , sc.[gender]
                , sc.[email]
                , sc.[phone]
                , sc.[birthdate]
                , sc.[address]
                , sc.[time_stamp]
                , sc.[accept_abm_sms_notify]
                , sc.[accept_abm_email_notify]
            FROM [services_clients] sc
            INNER JOIN discount_cards dc ON dc.discount_card_id =sc.discount_card_id
            INNER JOIN localities l ON l.locality_id = sc.locality_id
            WHERE dc.discount_card_barcode = @barcode", connection);

                command.Parameters.Add("@barcode", SqlDbType.NVarChar).Value = barcode;

                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new LoyaltyUser
                        {
                            Id = Guid.Parse(reader["id"].ToString()!),
                            LastName = reader["name"] as string,
                            FirstName = reader["surname"] as string,
                            MiddleName = reader["patronymic"] as string,
                            Gender = (int)Convert.ToInt32(reader["gender"]),
                            Email = reader["email"] as string,
                            Phone = reader["phone"] as string,
                            BirthDay = (DateTime)reader["birthdate"],
                            Address = reader["address"] as string,
                            Created = (DateTime)reader["time_stamp"],
                            SmsNotify = reader["accept_abm_sms_notify"] != DBNull.Value && Convert.ToInt32(reader["accept_abm_sms_notify"]) == 1,
                            EmailNotify = reader["accept_abm_email_notify"] != DBNull.Value && Convert.ToInt32(reader["accept_abm_email_notify"]) == 1
                        };

                        users.Add(user);
                    }
                }
                foreach (var user in users)
                {
                    user.Cards = await GetUserCardsAsync(user.Id);
                    user.Balance = await GetUserBalanceAsync(user.Id);
                }

                return users;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }



        public async Task<List<Card>> GetUserCardsAsync(Guid userId)
        {
            try
            {
                List<Card> cards = new();

                await using var connection = await _db.ConnectionAsync;
                await using var command = new SqlCommand(@"
            SELECT discount_card_id, discount_card_barcode, is_active, client_id
  FROM [discount_cards]
  WHERE client_id = @userId", connection);

                command.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var card = new Card
                    {
                        Id = reader["discount_card_id"].ToString(),
                        Number = reader["discount_card_barcode"].ToString(),
                        Status = reader["is_active"] != DBNull.Value && Convert.ToInt32(reader["is_active"]) == 1
                    };
                    cards.Add(card);
                }

                return cards;


            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<decimal> GetUserBalanceAsync(Guid userId)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string sql = @"
            SELECT ISNULL(SUM([TotalActivePoints]), 0)
            FROM [crm_LoyaltyBalance]
            WHERE ClientId = @userId;";

                await using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;

                var scalar = await command.ExecuteScalarAsync();
                return (scalar == null || scalar == DBNull.Value) ? 0m : Convert.ToDecimal(scalar);
            }
            catch (SqlException sqlEx)
            {
                WriteLog.DB?.Error($"SQL Error in GetUserBalanceAsync: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB?.Error($"Error in GetUserBalanceAsync: {ex.Message}");
                throw;
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }

        public async Task<UserRegistrationResult> UserRegistration(LoyaltyUser userInfo)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                await connection.OpenAsync();

                
                string checkQuery = "SELECT COUNT(1) FROM [dbo].[services_clients] WHERE [phone] = @Phone";
                await using (var checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Phone", userInfo.Phone ?? string.Empty);

                    int exists = (int)await checkCmd.ExecuteScalarAsync();
                    if (exists > 0)
                    {
                        return new UserRegistrationResult
                        {
                            Success = false,
                            Message = "Un utilizator cu acest numar de telefon exista deja."
                        };
                    }
                }

                string insertQuery = @"
            INSERT INTO [dbo].[services_clients]
            (
                [id],
                [name],
                [surname],
                [patronymic],
                [gender],
                [email],
                [phone],
                [birthdate],
                [address],
                [time_stamp],
                [user_id],
                [discount_card_id],
                [discount_card_issue_date],
                [is_trusted],
                [locality_id],
                [agent_id],
                [selected_brands],
                [selected_domains],
                [use_abm_loialty],
                [accept_abm_sms_notify],
                [accept_abm_email_notify]
            )
            VALUES
            (
                @Id,
                @Name,
                @Surname,
                @Patronymic,
                @Gender,
                @Email,
                @Phone,
                @Birthdate,
                @Address,
                @TimeStamp,
                @UserId,
                @DiscountCardId,
                @DiscountCardIssueDate,
                @IsTrusted,
                @LocalityId,
                @AgentId,
                @SelectedBrands,
                @SelectedDomains,
                @UseAbmLoialty,
                @AcceptAbmSmsNotify,
                @AcceptAbmEmailNotify
            )";

                await using var command = new SqlCommand(insertQuery, connection);

                
                command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                command.Parameters.AddWithValue("@Name", userInfo.LastName);
                command.Parameters.AddWithValue("@Surname", userInfo.FirstName);
                command.Parameters.AddWithValue("@Patronymic", (object?)userInfo.MiddleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@Gender", userInfo.Gender);
                command.Parameters.AddWithValue("@Email", (object?)userInfo.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@Phone", (object?)userInfo.Phone ?? DBNull.Value);
                command.Parameters.AddWithValue("@Birthdate", userInfo.BirthDay);
                command.Parameters.AddWithValue("@Address", (object?)userInfo.Address ?? DBNull.Value);
                command.Parameters.AddWithValue("@TimeStamp", DateTime.UtcNow);
                command.Parameters.AddWithValue("@UserId", 761);
                command.Parameters.AddWithValue("@DiscountCardId", null);
                command.Parameters.AddWithValue("@DiscountCardIssueDate", null);
                command.Parameters.AddWithValue("@IsTrusted", true);
                command.Parameters.AddWithValue("@LocalityId", null);
                command.Parameters.AddWithValue("@AgentId", null);
                command.Parameters.AddWithValue("@SelectedBrands",null);
                command.Parameters.AddWithValue("@SelectedDomains", null);
                command.Parameters.AddWithValue("@UseAbmLoialty", true);
                command.Parameters.AddWithValue("@AcceptAbmSmsNotify", 0);
                command.Parameters.AddWithValue("@AcceptAbmEmailNotify", 0);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return new UserRegistrationResult
                {
                    Success = rowsAffected > 0,
                    Message = rowsAffected > 0 ? "Utilizator inregistrat cu succes." : "Inserarea a esuat.",
                    User = userInfo
                };
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<UserRegistrationResult> GenerateDiscountCard(LoyaltyUser userInfo)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                await connection.OpenAsync();


                string checkQuery = "SELECT COUNT(1) FROM [dbo].[services_clients] WHERE [phone] = @Phone";
                await using (var checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Phone", userInfo.Phone ?? string.Empty);

                    int exists = (int)await checkCmd.ExecuteScalarAsync();
                    if (exists > 0)
                    {
                        return new UserRegistrationResult
                        {
                            Success = false,
                            Message = "Un utilizator cu acest numar de telefon exista deja."
                        };
                    }
                }

                string insertQuery = @"
            INSERT INTO [dbo].[services_clients]
            (
                [id],
                [name],
                [surname],
                [patronymic],
                [gender],
                [email],
                [phone],
                [birthdate],
                [address],
                [time_stamp],
                [user_id],
                [discount_card_id],
                [discount_card_issue_date],
                [is_trusted],
                [locality_id],
                [agent_id],
                [selected_brands],
                [selected_domains],
                [use_abm_loialty],
                [accept_abm_sms_notify],
                [accept_abm_email_notify]
            )
            VALUES
            (
                @Id,
                @Name,
                @Surname,
                @Patronymic,
                @Gender,
                @Email,
                @Phone,
                @Birthdate,
                @Address,
                @TimeStamp,
                @UserId,
                @DiscountCardId,
                @DiscountCardIssueDate,
                @IsTrusted,
                @LocalityId,
                @AgentId,
                @SelectedBrands,
                @SelectedDomains,
                @UseAbmLoialty,
                @AcceptAbmSmsNotify,
                @AcceptAbmEmailNotify
            )";

                await using var command = new SqlCommand(insertQuery, connection);


                command.Parameters.AddWithValue("@Id", Guid.NewGuid());
                command.Parameters.AddWithValue("@Name", userInfo.LastName);
                command.Parameters.AddWithValue("@Surname", userInfo.FirstName);
                command.Parameters.AddWithValue("@Patronymic", (object?)userInfo.MiddleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@Gender", userInfo.Gender);
                command.Parameters.AddWithValue("@Email", (object?)userInfo.Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@Phone", (object?)userInfo.Phone ?? DBNull.Value);
                command.Parameters.AddWithValue("@Birthdate", userInfo.BirthDay);
                command.Parameters.AddWithValue("@Address", (object?)userInfo.Address ?? DBNull.Value);
                command.Parameters.AddWithValue("@TimeStamp", DateTime.UtcNow);
                command.Parameters.AddWithValue("@UserId", 761);
                command.Parameters.AddWithValue("@DiscountCardId", null);
                command.Parameters.AddWithValue("@DiscountCardIssueDate", null);
                command.Parameters.AddWithValue("@IsTrusted", true);
                command.Parameters.AddWithValue("@LocalityId", null);
                command.Parameters.AddWithValue("@AgentId", null);
                command.Parameters.AddWithValue("@SelectedBrands", null);
                command.Parameters.AddWithValue("@SelectedDomains", null);
                command.Parameters.AddWithValue("@UseAbmLoialty", true);
                command.Parameters.AddWithValue("@AcceptAbmSmsNotify", 0);
                command.Parameters.AddWithValue("@AcceptAbmEmailNotify", 0);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return new UserRegistrationResult
                {
                    Success = rowsAffected > 0,
                    Message = rowsAffected > 0 ? "Utilizator inregistrat cu succes." : "Inserarea a esuat.",
                    User = userInfo
                };
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }

        public async Task<List<LoyaltyUser>> UserInfoByToken(Guid token)
        {
            try
            {
                List<LoyaltyUser> users = new();

                await using var connection = await _db.ConnectionAsync;
                await using var command = new SqlCommand(@"
            SELECT TOP 1 
                  sc.[id]
                , sc.[name]
                , sc.[surname]
                , sc.[patronymic]
                , sc.[gender]
                , sc.[email]
                , sc.[phone]
                , sc.[birthdate]
                , sc.[address]
                , sc.[time_stamp]
                , sc.[accept_abm_sms_notify]
                , sc.[accept_abm_email_notify]
            FROM [services_clients] sc
            INNER JOIN localities l ON l.locality_id = sc.locality_id
            WHERE sc.id = @token", connection);

                command.Parameters.Add("@token", SqlDbType.NVarChar).Value = token;

                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new LoyaltyUser
                        {
                            Id = Guid.Parse(reader["id"].ToString()!),
                            LastName = reader["name"] as string,
                            FirstName = reader["surname"] as string,
                            MiddleName = reader["patronymic"] as string,
                            Gender = (int)Convert.ToInt32(reader["gender"]),
                            Email = reader["email"] as string,
                            Phone = reader["phone"] as string,
                            BirthDay = (DateTime)reader["birthdate"],
                            Address = reader["address"] as string,
                            Created = (DateTime)reader["time_stamp"],
                            SmsNotify = reader["accept_abm_sms_notify"] != DBNull.Value && Convert.ToInt32(reader["accept_abm_sms_notify"]) == 1,
                            EmailNotify = reader["accept_abm_email_notify"] != DBNull.Value && Convert.ToInt32(reader["accept_abm_email_notify"]) == 1,

                        };

                        users.Add(user);
                    }
                }
                foreach (var user in users)
                {
                    user.Cards = await GetUserCardsAsync(user.Id);

                }

                return users;
            }
            catch (SqlException sqlEx)
            {

                WriteLog.DB.Error($"SQL Error: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB.Error($"Error: {ex.Message}");
                throw new Exception($"Error: {ex.Message}", ex);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }

        
        public async Task<List<Localites>> GetLocalites(string key)
        {
            try
            {
                List<Localites> localities = new();

                await using var connection = await _db.ConnectionAsync;
                await using var command = new SqlCommand(@"
SELECT 
    locality_id AS Id,
    CASE 
        WHEN @key = 'en' THEN locality_name_en
        WHEN @key = 'ru' THEN locality_name_ru
        ELSE locality_name
    END AS Name
FROM localities;", connection);

                command.Parameters.Add("@key", SqlDbType.NVarChar, 3).Value = key;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    localities.Add(new Localites
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"] as string
                    });
                }

                return localities;
            }
            catch (SqlException sqlEx)
            {
                WriteLog.DB.Error($"SQL Error: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB.Error($"Error: {ex.Message}");
                throw new Exception($"Error: {ex.Message}", ex);
            }
        }
        public async Task<List<GenderTypes>> GetGenderTypes(string key)
        {
            try
            {
                List<GenderTypes> genders = new();

                await using var connection = await _db.ConnectionAsync;
                await using var command = new SqlCommand(@"
SELECT 
    id AS Id,
    CASE 
        WHEN @key = 'en' THEN name_en
        WHEN @key = 'ru' THEN name_ru
        ELSE name
    END AS Name
FROM genders;", connection);

                command.Parameters.Add("@key", SqlDbType.NVarChar, 3).Value = key;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    genders.Add(new GenderTypes
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"] as string
                    });
                }

                return genders;
            }
            catch (SqlException sqlEx)
            {
                WriteLog.DB.Error($"SQL Error: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB.Error($"Error: {ex.Message}");
                throw new Exception($"Error: {ex.Message}", ex);
            }
        }

    }
}
