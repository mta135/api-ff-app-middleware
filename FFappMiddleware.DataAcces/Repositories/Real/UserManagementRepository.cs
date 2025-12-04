using FFappMiddleware.DataAcces.DataBaseConnection;
using FFappMiddleware.DataBase.Logger;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.UserManagement;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Numerics;
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

        public async Task<List<LoyaltyUser>> GetUserByPhone(PhoneNumberRequest phone)
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
                , sc.[accept_abm_email_notify],
CASE 
     WHEN @key = 'en' THEN locality_name_en
     WHEN @key = 'ru' THEN locality_name_ru
     ELSE locality_name
	 END AS locality_name
            FROM [services_clients] sc
            INNER JOIN localities l ON l.locality_id = sc.locality_id
            WHERE sc.phone = @phone", connection);

                command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = phone.PhoneNumber;
                command.Parameters.Add("@key", SqlDbType.NVarChar, 3).Value = "";
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
                    reader.Close();
                } 
             
                foreach (var user in users)
                {
                    user.Cards = await GetUserCardsAsync(user.Id);
                    user.Balance = await GetUserBalanceAsync(user.Id);
                    user.Coupons = await GetUserCouponsAsync(user.Id);

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
        public async Task<List<LoyaltyUser>> GetUserByDiscountCardBarcode(DiscountCardNumberRequest barcode)
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

                command.Parameters.Add("@barcode", SqlDbType.NVarChar).Value = barcode.DiscountCardNumber;

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
                       reader.Close();
                }
                
                foreach (var user in users)
                {
                    user.Cards = await GetUserCardsAsync(user.Id);
                    user.Balance = await GetUserBalanceAsync(user.Id);
                    user.Coupons = await GetUserCouponsAsync(user.Id);
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
            SELECT discount_card_id, discount_card_barcode, is_active, client_id,is_default
  FROM [discount_cards]
  WHERE client_id = @userId", connection);

                command.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var card = new Card
                    {
                        Id = Convert.ToInt64(reader["discount_card_id"]),
                        Number = reader["discount_card_barcode"].ToString(),
                        IsActive = reader["is_active"] != DBNull.Value && Convert.ToInt32(reader["is_active"]) == 1,
                        IsDefault = reader["is_default"] != DBNull.Value && Convert.ToInt32(reader["is_default"]) == 1
                    };
                    cards.Add(card);
                }
                reader.Close();
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
        public async Task<List<Coupons>> GetUserCouponsAsync(Guid userId)
        {
            List<Coupons> couponsList = new();

            await using var connection = await _db.ConnectionAsync;

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await using var command = new SqlCommand(@"
       SELECT 
       cd.coupon_name,
       c.description,
       cuc.valid_to,
       c.max_usage_limit,
       cs.name AS status_name,
       crt.name AS remuneration_name,
       COUNT(crmCuc.coupon_id) AS usageCount
FROM crm_users_coupons cuc
INNER JOIN coupons_details cd
       ON cd.coupon_detail_id = cuc.coupon_id
INNER JOIN coupons c
       ON c.entity_id = cd.entity_id
INNER JOIN coupon_states cs
       ON cd.status_id = cs.id
INNER JOIN coupon_remuneration_types crt 
       ON crt.id = c.remuneration_type_id
LEFT JOIN crm_used_coupons crmCuc
       ON crmCuc.coupon_id = cd.coupon_detail_id
        WHERE cuc.user_id = @userId
GROUP BY
       cd.coupon_name,
       c.description,
       cuc.valid_to,
       c.max_usage_limit,
       cs.name,
       crt.name;
    ", connection);

            command.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;

            try
            {
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    couponsList.Add(new Coupons
                    {
                        Coupon = reader["coupon_name"]?.ToString() ?? string.Empty,
                        Description = reader["description"]?.ToString() ?? string.Empty,
                        ValidTo = reader["valid_to"] == DBNull.Value
                                  ? DateTime.MinValue
                                  : (DateTime)reader["valid_to"],
                        MaxUsageLimit = reader["max_usage_limit"] == DBNull.Value
                                        ? 0
                                        : Convert.ToInt32(reader["max_usage_limit"]),
                        Status = reader["status_name"]?.ToString() ?? string.Empty,
                        RemunerationType = reader["remuneration_name"]?.ToString() ?? string.Empty,
                        UsageCount = reader["usageCount"] == DBNull.Value
                                     ? 0
                                     : Convert.ToInt32(reader["usageCount"])
                    });
                }
                reader.Close();

                return couponsList;
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
        public async Task<List<Balance>> GetUserBalanceAsync(Guid userId)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string sql = @"
            SELECT 
                ISNULL(SUM([TotalActivePoints]), 0) AS totalBalance,
                ISNULL(SUM(1), 0) AS usedBalance,
                ISNULL(SUM([TotalActivePoints]) - SUM(1), 0) AS currentBalance
            FROM [crm_LoyaltyBalance]
            WHERE ClientId = @userId;";

                await using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;

                await using var reader = await command.ExecuteReaderAsync();

                var result = new List<Balance>();

                if (await reader.ReadAsync())
                {
                    var balance = new Balance
                    {
                        TotalBalance =28, //reader.GetDecimal(reader.GetOrdinal("totalBalance")),
                        UsedBalance =15,// reader.GetDecimal(reader.GetOrdinal("usedBalance")),
                        CurrentBalance =28-15 //reader.GetDecimal(reader.GetOrdinal("currentBalance"))
                    };

                    result.Add(balance);
                }

                reader.Close();
                return result;
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
        public async Task<UserRegistrationResult> UserRegistration(LoyaltyUserEdit userInfo)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                await connection.OpenAsync();

                const string checkQuery =
                    "SELECT COUNT(1) FROM [dbo].[services_clients] WHERE [phone] = @Phone";

                await using (var checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 50)
                        .Value = userInfo.Phone ?? string.Empty;

                    var existsObj = await checkCmd.ExecuteScalarAsync();
                    int exists = existsObj != null ? Convert.ToInt32(existsObj) : 0;

                    if (exists > 0)
                    {
                        return new UserRegistrationResult
                        {
                            Success = false,
                            Message = "Un utilizator cu acest numar de telefon exista deja."
                        };
                    }
                }

                // --------- INSERT USER ---------
                const string insertQuery = @"
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
    [use_abm_loyalty],
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
    @UseAbmLoyalty,
    @AcceptAbmSmsNotify,
    @AcceptAbmEmailNotify
);";

                await using var command = new SqlCommand(insertQuery, connection);

                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = userInfo.LastName ?? "";
                command.Parameters.Add("@Surname", SqlDbType.NVarChar, 50).Value = userInfo.FirstName ?? "";
                command.Parameters.Add("@Patronymic", SqlDbType.NVarChar, 50).Value = (object?)userInfo.MiddleName ?? DBNull.Value;
                command.Parameters.Add("@Gender", SqlDbType.Int).Value = userInfo.Gender;
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)userInfo.Email ?? DBNull.Value;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = userInfo.Phone ?? "";
                command.Parameters.Add("@Birthdate", SqlDbType.Date).Value = userInfo.BirthDay;
                command.Parameters.Add("@Address", SqlDbType.NVarChar, 200).Value = (object?)userInfo.Address ?? DBNull.Value;
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = DateTime.UtcNow;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = 761;

                command.Parameters.Add("@DiscountCardId", SqlDbType.Int).Value = DBNull.Value;
                command.Parameters.Add("@DiscountCardIssueDate", SqlDbType.DateTime).Value = DBNull.Value;
                command.Parameters.Add("@IsTrusted", SqlDbType.Bit).Value = true;

                command.Parameters.Add("@LocalityId", SqlDbType.Int).Value = DBNull.Value;
                command.Parameters.Add("@AgentId", SqlDbType.Int).Value = DBNull.Value;
                command.Parameters.Add("@SelectedBrands", SqlDbType.NVarChar).Value = DBNull.Value;
                command.Parameters.Add("@SelectedDomains", SqlDbType.NVarChar).Value = DBNull.Value;

                command.Parameters.Add("@UseAbmLoyalty", SqlDbType.Bit).Value = true;
                command.Parameters.Add("@AcceptAbmSmsNotify", SqlDbType.Bit).Value = false;
                command.Parameters.Add("@AcceptAbmEmailNotify", SqlDbType.Bit).Value = false;

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return new UserRegistrationResult
                {
                    Success = rowsAffected > 0,
                    Message = rowsAffected > 0 ? "Utilizator înregistrat cu succes." : "Inserarea a eșuat.",
                    User = userInfo
                };
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
        }
        public async Task<List<UserTransaction>> UserTransactions(UserRequest user)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string sql = @"
SELECT  
      c.cheque_id,
      c.cheque_number,
      c.timestamp_close,
      c.cheque_sum,
      c.discount_sum,
      cd.product_unit_id,
      cd.quantity,
      cd.sale_price,
      cd.discount_price,
      p.product_id,
      p.product_name + ' ' + u.unit_name AS product_name,
      a.agent_name AS storename,
      a.agent_id AS storeid
FROM cheques c
INNER JOIN cheques_details cd ON cd.cheque_id = c.cheque_id
INNER JOIN agents a ON a.agent_id = c.agent_id
INNER JOIN products_units pu ON pu.product_unit_id = cd.product_unit_id
INNER JOIN units u ON u.unit_id = pu.unit_id
INNER JOIN products p ON p.product_id = pu.product_id
WHERE c.discount_card_id IN (
        SELECT DISTINCT discount_card_id
        FROM discount_cards
        WHERE client_id = @userId)
  order by timestamp_close;";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = user.UserId;

                await using var reader = await cmd.ExecuteReaderAsync();

                var transactions = new Dictionary<Guid, UserTransaction>();

                while (await reader.ReadAsync())
                {
                    var chequeId = reader.GetGuid(reader.GetOrdinal("cheque_id"));

                    if (!transactions.ContainsKey(chequeId))
                    {
                        transactions[chequeId] = new UserTransaction
                        {
                            TransactionId = chequeId,
                            TransactionNumber = reader.GetString(reader.GetOrdinal("cheque_number")),
                            TransactionDate = reader.GetDateTime(reader.GetOrdinal("timestamp_close")),

                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("cheque_sum")),
                            DiscountAmount = reader.GetDecimal(reader.GetOrdinal("discount_sum")),

                            PointsUsed = 0,
                            PointsReceived = 0,

                            StoreName = reader.GetString(reader.GetOrdinal("storename")),
                            StoreId = reader.GetInt64(reader.GetOrdinal("storeid")),

                            Products = new List<ProductDetail>()
                        };
                        
                    }

                    transactions[chequeId].Products.Add(new ProductDetail
                    {
                        ProductId = reader.GetInt64(reader.GetOrdinal("product_id")),
                        ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("sale_price")),
                        Quantity = reader.GetInt64(reader.GetOrdinal("quantity")),
                        Discount = reader.GetDecimal(reader.GetOrdinal("discount_price"))
                    });
                    
                }
                reader.Close();

                return transactions.Values.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.DB?.Error($"Error in UserTransactions: {ex.Message}");
                throw;
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<List<UserTransaction>> UserOrders(UserRequest user)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                const string sql = @"
SELECT  
      c.PurchaseId,
      c.PurchaseDate,
      c.TotalAmount,
      c.TotalDiscount,      
      cd.Quantity,
      cd.UnitPrice,
      cd.UnitDiscount,
      p.product_id,
      p.product_name,
	  c.PurchaseStatus_id,
CASE 
     WHEN @key = 'en' THEN name_en
     WHEN @key = 'ru' THEN name_ru
     ELSE name
	 END AS Statusname

FROM crm_Purchases c
INNER JOIN crm_PurchaseItems cd ON cd.PurchaseId = c.PurchaseId
INNER JOIN products p ON p.product_id = cd.ProductId
INNER JOIN crm_Purchase_status cps on	cps.id = c.PurchaseStatus_id
WHERE c.CardId IN (
        SELECT DISTINCT discount_card_id 
        FROM discount_cards 
        WHERE client_id = @userId
)
ORDER BY c.PurchaseDate;";

                await using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = user.UserId;
                cmd.Parameters.Add("@key", SqlDbType.VarChar).Value = user.LanguageKey;
                await using var reader = await cmd.ExecuteReaderAsync();

                var transactions = new Dictionary<Guid, UserTransaction>();

                while (await reader.ReadAsync())
                {
                    var purchaseId = reader.GetGuid(reader.GetOrdinal("PurchaseId"));

                    if (!transactions.ContainsKey(purchaseId))
                    {
                        transactions[purchaseId] = new UserTransaction
                        {
                            TransactionId = purchaseId,
                            TransactionNumber = purchaseId.ToString(),
                           
                            TransactionDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            StatusId = reader.GetInt32(reader.GetOrdinal("PurchaseStatus_id")),
                            StatusName = reader.GetString(reader.GetOrdinal("Statusname")),
                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                            DiscountAmount = reader.GetDecimal(reader.GetOrdinal("TotalDiscount")),

                            PointsUsed = 0,
                            PointsReceived = 0,
                            StoreName = "",  
                            StoreId = 0,
                            Products = new List<ProductDetail>()
                        };
                    }

                    transactions[purchaseId].Products.Add(new ProductDetail
                    {
                        ProductId = reader.GetInt64(reader.GetOrdinal("product_id")),
                        ProductName = reader.GetString(reader.GetOrdinal("product_name")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        Quantity = reader.GetDecimal(reader.GetOrdinal("Quantity")),
                        Discount = reader.GetDecimal(reader.GetOrdinal("UnitDiscount"))
                    });
                }

                reader.Close();
                return transactions.Values.ToList();
            }
            catch (Exception ex)
            {
                WriteLog.DB?.Error($"Error in UserOrders: {ex.Message}");
                throw;
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<LoyaltyUser> GenerateDiscountCard(UserRequest userInfo)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await using var transaction = connection.BeginTransaction();

                
                const string getMaxBarcodeQuery = @"
SELECT ISNULL(MAX(TRY_CAST(discount_card_barcode AS BIGINT)), 0)
FROM [dbo].[discount_cards] WITH (UPDLOCK, HOLDLOCK)
WHERE TRY_CAST(discount_card_barcode AS BIGINT) IS NOT NULL";

                long lastBarcode;
                await using (var cmd = new SqlCommand(getMaxBarcodeQuery, connection, transaction))
                {
                    var result = await cmd.ExecuteScalarAsync();
                    lastBarcode = result != null && result != DBNull.Value ? Convert.ToInt64(result) : 0;
                }

               
                long newBarcodeNumeric = lastBarcode + 1;
                string newBarcode = newBarcodeNumeric.ToString("D12");

                
                const string insertCardQuery = @"
INSERT INTO [dbo].[discount_cards]
(
    [discount_card_barcode],
    [is_cumulative],
    [limitation_use_agent_ids],
    [is_active],
    [discount_card_type_id],
    [discount_card_description],
    [discount_card_valability_term],
    [client_id],
    [is_default]
)
VALUES
(
    @Barcode,
    0,
    NULL,
    1,
    60,
    NULL,
    NULL,
    @ClientId,
    0
);
SELECT SCOPE_IDENTITY();";

                long discountCardId;
                await using (var insertCardCmd = new SqlCommand(insertCardQuery, connection, transaction))
                {
                    insertCardCmd.Parameters.Add("@Barcode", SqlDbType.VarChar, 50).Value = newBarcode;
                    insertCardCmd.Parameters.Add("@ClientId", SqlDbType.UniqueIdentifier).Value = userInfo.UserId;

                    discountCardId = Convert.ToInt64(await insertCardCmd.ExecuteScalarAsync());
                }

                await transaction.CommitAsync();

            
                var users = await UserInfoByToken(userInfo);
                var user = users.FirstOrDefault();
                //if (user != null)
                //    user.DiscountCardBarcode = newBarcode;

                return user;
            }
            catch (SqlException sqlEx)
            {
                WriteLog.DB?.Error($"SQL Error: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB?.Error($"Error: {ex.Message}");
                throw new Exception($"Error: {ex.Message}", ex);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<List<LoyaltyUser>> UserInfoByToken(UserRequest token)
        {
            try
            {
                var users = new List<LoyaltyUser>();

                await using var connection = await _db.ConnectionAsync;
                if (connection.State != System.Data.ConnectionState.Open)
                    await connection.OpenAsync();

                const string sql = @"
SELECT TOP 1 
      sc.[id],
      sc.[name],
      sc.[surname],
      sc.[patronymic],
      sc.[gender],
      sc.[email],
      sc.[phone],
      sc.[birthdate],
      sc.[address],
      sc.[time_stamp],
      sc.[accept_abm_sms_notify],
      sc.[accept_abm_email_notify]
FROM [services_clients] sc
INNER JOIN localities l ON l.locality_id = sc.locality_id
WHERE sc.id = @id;
";

                await using var command = new SqlCommand(sql, connection);
                command.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = token.UserId;

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var user = new LoyaltyUser
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("id")),
                        LastName = reader["name"] as string,
                        FirstName = reader["surname"] as string,
                        MiddleName = reader["patronymic"] as string,
                        Gender = Convert.ToInt32(reader["gender"]),
                        Email = reader["email"] as string,
                        Phone = reader["phone"] as string,
                        BirthDay = reader.GetDateTime(reader.GetOrdinal("birthdate")),
                        Address = reader["address"] as string,
                        Created = reader.GetDateTime(reader.GetOrdinal("time_stamp")),
                        SmsNotify = reader["accept_abm_sms_notify"] != DBNull.Value && Convert.ToBoolean(reader["accept_abm_sms_notify"]),
                        EmailNotify = reader["accept_abm_email_notify"] != DBNull.Value && Convert.ToBoolean(reader["accept_abm_email_notify"])
                    };

                    users.Add(user);
                }

               
                foreach (var user in users)
                {
                    user.Cards = await GetUserCardsAsync(user.Id);
                    user.Balance = await GetUserBalanceAsync(user.Id);
                    user.Coupons = await GetUserCouponsAsync(user.Id);
                }

                return users;
            }
            catch (SqlException sqlEx)
            {
                WriteLog.DB?.Error($"SQL Error: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB?.Error($"Error: {ex.Message}");
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
                reader.Close();

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
                reader.Close();

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
        public async Task<List<LoyaltyUser>> UpdateClientInformation(LoyaltyUserEdit inform)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                if (connection.State != System.Data.ConnectionState.Open)
                    await connection.OpenAsync();

                //var defaultCard = inform.Cards.FirstOrDefault(s => s.IsDefault);
                //long? cardIdValue = defaultCard?.Id;

                using var transaction = connection.BeginTransaction();

                const string updateClientSql = @"
UPDATE [dbo].[services_clients]
SET
    [name] = @FirstName,
    [surname] = @LastName,
    [patronymic] = @MiddleName,
    [email] = @Email,
    [phone] = @Phone,
    [birthdate] = @BirthDay,
    [address] = @Address,
    [gender] = @Gender,
    [accept_abm_sms_notify] = @SmsNotify,
    [accept_abm_email_notify] = @EmailNotify,
    [time_stamp] = GETDATE()
WHERE [id] = @Id;
";

                await using (var updateCmd = new SqlCommand(updateClientSql, connection, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@Id", inform.Id);
                    updateCmd.Parameters.AddWithValue("@FirstName", (object)inform.FirstName ?? DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@LastName", (object)inform.LastName ?? DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@MiddleName", (object)inform.MiddleName ?? DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@Email", (object)inform.Email ?? DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@Phone", (object)inform.Phone ?? DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@BirthDay", inform.BirthDay);
                    updateCmd.Parameters.AddWithValue("@Address", (object)inform.Address ?? DBNull.Value);
                    updateCmd.Parameters.AddWithValue("@Gender", inform.Gender);
                    updateCmd.Parameters.AddWithValue("@SmsNotify", inform.SmsNotify);
                    updateCmd.Parameters.AddWithValue("@EmailNotify", inform.EmailNotify);

                    int rowsAffected = await updateCmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                        throw new Exception("Client not found or no changes applied.");
                }

                
//                if (cardIdValue.HasValue && cardIdValue.Value > 0)
//                {
//                    const string updateCardsSql = @"
//UPDATE [discount_cards]
//SET is_default = 0
//WHERE client_id = @Id;

//UPDATE [discount_cards]
//SET is_default = 1
//WHERE discount_card_id = @cardId AND client_id = @Id;
//";
//                    await using var cardCmd = new SqlCommand(updateCardsSql, connection, transaction);
//                    cardCmd.Parameters.AddWithValue("@Id", inform.Id);
//                    cardCmd.Parameters.AddWithValue("@cardId", cardIdValue.Value);

//                    await cardCmd.ExecuteNonQueryAsync();
//                }

                transaction.Commit();


                var userId = new UserRequest
                {
                    UserId = inform.Id
                };
                var users = await UserInfoByToken(userId);
                if (users == null || !users.Any())
                    throw new Exception("Client not found after update.");

                return users;
            }
            catch (SqlException sqlEx)
            {
                WriteLog.DB?.Error($"SQL Error: {sqlEx.Message}");
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                WriteLog.DB?.Error($"Error: {ex.Message}");
                throw new Exception($"Error: {ex.Message}", ex);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<CardEdit> UpdateDiscountCardInformation(CardEdit card)
        {
            try
            {
                await using var connection = await _db.ConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var transaction = connection.BeginTransaction();

                
                const string getClientSql = @"
SELECT client_id 
FROM discount_cards 
WHERE discount_card_id = @CardId";

                Guid? clientId = null;

                await using (var getCmd = new SqlCommand(getClientSql, connection, transaction))
                {
                    getCmd.Parameters.AddWithValue("@CardId", card.Id);

                    var result = await getCmd.ExecuteScalarAsync();
                    if (result == null)
                        throw new Exception("Cardul nu a fost găsit!");

                    clientId = (Guid)result;
                }
                
                const string updateCardSql = @"
UPDATE discount_cards
SET 
    is_active = @IsActive,
    is_default = @IsDefault
WHERE discount_card_id = @CardId;
";

                await using (var updateCmd = new SqlCommand(updateCardSql, connection, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@CardId", card.Id);
                    updateCmd.Parameters.AddWithValue("@IsActive", card.IsActive);
                    updateCmd.Parameters.AddWithValue("@IsDefault", card.IsDefault);

                    await updateCmd.ExecuteNonQueryAsync();
                }


                
                if (card.IsDefault)
                {
                    const string resetDefaultsSql = @"
UPDATE discount_cards
SET is_default = 0
WHERE client_id = @ClientId AND discount_card_id <> @CardId;

UPDATE discount_cards
SET is_default = 1
WHERE discount_card_id = @CardId;
";

                    await using (var resetCmd = new SqlCommand(resetDefaultsSql, connection, transaction))
                    {
                        resetCmd.Parameters.AddWithValue("@ClientId", clientId);
                        resetCmd.Parameters.AddWithValue("@CardId", card.Id);

                        await resetCmd.ExecuteNonQueryAsync();
                    }
                }

                transaction.Commit();

                return card;
            }
            catch (Exception ex)
            {
                WriteLog.DB?.Error($"Error: {ex.Message}");
                throw;
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }

    }
}
