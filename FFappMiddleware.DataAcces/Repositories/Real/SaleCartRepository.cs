using FFappMiddleware.DataAcces.DataBaseConnection;
using FFappMiddleware.DataBase.Logger;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.SaleChart;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using static FFAppMiddleware.Model.Models.SaleChart.PreCheckModel;
using static FFAppMiddleware.Model.Models.UserManagement.UserModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FFappMiddleware.DataBase.Repositories.Real
{
    public class SaleCartRepository : ISaleCartRepository
    {
        private readonly DataBaseAccesConfig _db;

        public SaleCartRepository()
        {
            _db = new DataBaseAccesConfig();
        }

        //public async Task<List<Cart>> PreCeckCart(List<CartItem> itms, int clientId, long cardId)
        //{
        //    try
        //    {
        //        using var connection = await _db.ConnectionAsync;
        //        //await connection.OpenAsync();

        //        using var transaction = connection.BeginTransaction();


        //        string deleteItemsSql = @"
        //    DELETE FROM CartItems 
        //    WHERE CartId IN (SELECT CartId FROM Cart WHERE ClientId = @ClientId AND CardId = @CardId)";
        //        using (SqlCommand cmd = new(deleteItemsSql, connection, transaction))
        //        {
        //            cmd.Parameters.AddWithValue("@ClientId", clientId);
        //            cmd.Parameters.AddWithValue("@CardId", cardId);
        //            await cmd.ExecuteNonQueryAsync();
        //        }


        //        string deleteCartSql = "DELETE FROM Cart WHERE ClientId = @ClientId AND CardId = @CardId";
        //        using (SqlCommand cmd = new(deleteCartSql, connection, transaction))
        //        {
        //            cmd.Parameters.AddWithValue("@ClientId", clientId);
        //            cmd.Parameters.AddWithValue("@CardId", cardId);
        //            await cmd.ExecuteNonQueryAsync();
        //        }


        //        Guid newCartId = Guid.NewGuid();
        //        string insertCartSql = @"
        //    INSERT INTO Cart (CartId, ClientId, CardId, CreatedAt, UpdatedAt)
        //    VALUES (@CartId, @ClientId, @CardId, @CreatedAt, @UpdatedAt)";
        //        using (SqlCommand cmd = new(insertCartSql, connection, transaction))
        //        {
        //            cmd.Parameters.AddWithValue("@CartId", newCartId);
        //            cmd.Parameters.AddWithValue("@ClientId", clientId);
        //            cmd.Parameters.AddWithValue("@CardId", cardId);
        //            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
        //            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
        //            await cmd.ExecuteNonQueryAsync();
        //        }


        //        string insertItemSql = @"
        //    INSERT INTO CartItems (CartId, ProductId, Quantity, UnitPrice, Discount, PromotionId, PointsEarned)
        //    VALUES (@CartId, @ProductId, @Quantity, @UnitPrice, @Discount, @PromotionId, @PointsEarned)";

        //        foreach (var item in itms)
        //        {
        //            using (SqlCommand cmd = new(insertItemSql, connection, transaction))
        //            {
        //                cmd.Parameters.AddWithValue("@CartId", newCartId);
        //                cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
        //                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
        //                cmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
        //                cmd.Parameters.AddWithValue("@Discount", item.Discount);
        //                cmd.Parameters.AddWithValue("@PromotionId", (object?)item.PromotionId ?? DBNull.Value);
        //                cmd.Parameters.AddWithValue("@PointsEarned", item.PointsEarned);

        //                await cmd.ExecuteNonQueryAsync();
        //            }
        //        }

        //        await transaction.CommitAsync();

        //        //aici o sa facem recalc la cos


        //        var result = new List<Cart>();

        //        string selectSql = @"
        //    SELECT c.CartId, c.ClientId, c.CardId, c.CreatedAt, c.UpdatedAt,
        //           ci.ProductId, ci.Quantity, ci.UnitPrice, ci.Discount, ci.PromotionId, ci.PointsEarned
        //    FROM Cart c
        //    LEFT JOIN CartItems ci ON c.CartId = ci.CartId
        //    WHERE c.CartId = @CartId";

        //        using (SqlCommand cmd = new(selectSql, connection))
        //        {
        //            cmd.Parameters.AddWithValue("@CartId", newCartId);

        //            using var reader = await cmd.ExecuteReaderAsync();
        //            Cart currentCart = null;

        //            while (await reader.ReadAsync())
        //            {
        //                if (currentCart == null)
        //                {
        //                    currentCart = new Cart
        //                    {
        //                        CartId = reader.GetGuid(reader.GetOrdinal("CartId")),
        //                        ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
        //                        CardId = reader.GetInt64(reader.GetOrdinal("CardId")),
        //                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
        //                        UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
        //                        Items = new List<CartItem>()
        //                    };
        //                    result.Add(currentCart);
        //                }

        //                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
        //                {
        //                    var item = new CartItem
        //                    {
        //                        CartId = reader.GetGuid(reader.GetOrdinal("CartId")),
        //                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
        //                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
        //                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
        //                        Discount = reader.GetDecimal(reader.GetOrdinal("Discount")),
        //                        PromotionId = reader.IsDBNull(reader.GetOrdinal("PromotionId")) ? null : reader.GetInt64(reader.GetOrdinal("PromotionId")),
        //                        PointsEarned = reader.GetInt32(reader.GetOrdinal("PointsEarned"))
        //                    };
        //                    currentCart.Items.Add(item);
        //                }
        //            }
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.DB.Error($"Error in PreCeckCart: {ex.Message}", ex);

        //        throw new Exception($"Error in PreCeckCart: {ex.Message}", ex);
        //    }
        //    finally
        //    {
        //        await _db.DisposeAsync();
        //    }
        //}

        public async Task<List<Cart>> PreCheckCart(PreCheckApiModel preCheck)
        {
            await using SqlConnection sqlConnection = await _db.ConnectionAsync;
            await using SqlTransaction transaction = (SqlTransaction)await sqlConnection.BeginTransactionAsync();

            try
            {
               
                string deleteItemsSql = @"
            DELETE FROM crm_CartItems 
            WHERE CartId IN (
                SELECT CartId FROM crm_Cart 
                WHERE ClientId = @ClientId AND CardId = @CardId)";

                await using (SqlCommand command = new(deleteItemsSql, sqlConnection, transaction))
                {
                    command.Parameters.AddWithValue("@ClientId", preCheck.ClientId);
                    command.Parameters.AddWithValue("@CardId", preCheck.CardId);
                    await command.ExecuteNonQueryAsync();
                }

            
                string deleteCartSql = "DELETE FROM crm_Cart WHERE ClientId = @ClientId AND CardId = @CardId";
                await using (SqlCommand cmd = new(deleteCartSql, sqlConnection, transaction))
                {
                    cmd.Parameters.AddWithValue("@ClientId", preCheck.ClientId);
                    cmd.Parameters.AddWithValue("@CardId", preCheck.CardId);
                    await cmd.ExecuteNonQueryAsync();
                }

                
                Guid newCartId = Guid.NewGuid();
                string insertCartSql = @"
            INSERT INTO crm_Cart (CartId, ClientId, CardId, CreatedAt, UpdatedAt)
            VALUES (@CartId, @ClientId, @CardId, @CreatedAt, @UpdatedAt)";

                await using (SqlCommand cmd = new(insertCartSql, sqlConnection, transaction))
                {
                    cmd.Parameters.AddWithValue("@CartId", newCartId);
                    cmd.Parameters.AddWithValue("@ClientId", preCheck.ClientId);
                    cmd.Parameters.AddWithValue("@CardId", preCheck.CardId);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
                    await cmd.ExecuteNonQueryAsync();
                }

              
                string insertItemSql = @"
            INSERT INTO crm_CartItems 
                (CartId, ProductId, Quantity, UnitPrice, Discount, PromotionId, PointsEarned)
            VALUES 
                (@CartId, @ProductId, @Quantity, @UnitPrice, @Discount, @PromotionId, @PointsEarned)";

                await using (SqlCommand cmd = new(insertItemSql, sqlConnection, transaction))
                {
                    foreach (var item in preCheck.Details)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@CartId", newCartId);
                        cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                        cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                        cmd.Parameters.AddWithValue("@Discount", item.Discount);
                        cmd.Parameters.AddWithValue("@PromotionId", (object?)item.PromotionId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PointsEarned", item.PointsEarned);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

         
                await transaction.CommitAsync();

                await RecalculateCartAsync(newCartId);
 
                return await GetCartById(newCartId);
            }
            catch (SqlException sqlex)
            {
                await transaction.RollbackAsync();
                WriteLog.DB.Error($"SQL Error in PreCheckCart: {sqlex.Message}", sqlex);
                return null;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                WriteLog.DB.Error($"Error in PreCheckCart: {ex.Message}", ex);
                return null;
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
        public async Task<List<Cart>> ConfirmPreCheckCart(Cart preCheck)
        {
            await using SqlConnection sqlConnection = await _db.ConnectionAsync;
            await using SqlTransaction transaction = (SqlTransaction)await sqlConnection.BeginTransactionAsync();

            try
            {
                string sql = @"EXEC [dbo].[cp_confirm_cart_and_insert_purchase]
                             @CartId = @CartId,
                             @ClientId = @ClientId,
                             @CardId = @CardId;  ";

                await using (SqlCommand cmd = new(sql, sqlConnection, transaction))
                {
                    cmd.Parameters.AddWithValue("@CartId", preCheck.CartId);
                    cmd.Parameters.AddWithValue("@ClientId", preCheck.ClientId);
                    cmd.Parameters.AddWithValue("@CardId", preCheck.CardId);

                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                var result = await GetCartById(preCheck.CartId);
                if (result != null && result.Any())
                    result.First().Message = "SUCCESS";

                return result;
            }
            catch (Exception ex)
            {
                if (transaction.Connection != null)
                    await transaction.RollbackAsync();

                WriteLog.DB.Error($"Error in ConfirmPreCheckCart: {ex.Message}", ex);
                return new List<Cart>
                {
                     new Cart { Message = $"ERROR: {ex.Message}" }
                };
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }


        public async Task RecalculateCartAsync(Guid cartId)
        {
            await using SqlConnection sqlConnection = await _db.ConnectionAsync;
            await using SqlTransaction transaction = (SqlTransaction)await sqlConnection.BeginTransactionAsync();

            try
            {
                string sql = @"           
         
            EXEC [dbo].[cp_func_apply_best_promo_for_crm_cart]
                @cart_id = @CartId,
                @agent_id = @AgentId,
                @show_promo_messages = 1;

            EXEC [dbo].[cp_func_recalculate_crm_cart]
                @cart_id = @CartId,
                @show_promo_messages = 1;";

                await using (SqlCommand cmd = new(sql, sqlConnection, transaction))
                {
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.Parameters.AddWithValue("@AgentId", 272);

                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                if (transaction.Connection != null)
                    await transaction.RollbackAsync();

                WriteLog.DB.Error($"Error in RecalculateCartAsync: {ex.Message}", ex);
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }

        public async Task<List<Cart>> GetCartById(Guid newCartId)
        {
            await using SqlConnection sqlConnection = await _db.ConnectionAsync;
            List<Cart> result = new();

            string selectSql = @"
        SELECT c.CartId, c.ClientId, c.CardId, c.CreatedAt, c.UpdatedAt,
               ci.ProductId, ci.Quantity, ci.UnitPrice, ci.Discount, ci.PromotionId, ci.PointsEarned
        FROM crm_Cart c
        LEFT JOIN crm_CartItems ci ON c.CartId = ci.CartId
        WHERE c.CartId = @CartId";

            await using SqlCommand command = new(selectSql, sqlConnection);
            command.Parameters.AddWithValue("@CartId", newCartId);

            await using SqlDataReader reader = await command.ExecuteReaderAsync();
            Cart? currentCart = null;

            while (await reader.ReadAsync())
            {
                if (currentCart == null)
                {
                    currentCart = new Cart
                    {
                        CartId = reader.GetGuid("CartId"),
                        ClientId = reader.GetGuid("ClientId"),
                        CardId = reader.GetInt64("CardId"),
                        CreatedAt = reader.GetDateTime("CreatedAt"),
                        UpdatedAt = reader.GetDateTime("UpdatedAt"),
                        Items = new List<CartItem>()
                    };
                    result.Add(currentCart);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                {
                    currentCart.Items.Add(new CartItem
                    {
                        CartId = currentCart.CartId,
                        ProductId = reader.GetInt32("ProductId"),
                        Quantity = reader.GetInt32("Quantity"),
                        UnitPrice = reader.GetDecimal("UnitPrice"),
                        Discount = reader.GetDecimal("Discount"),
                        PromotionId = reader.IsDBNull(reader.GetOrdinal("PromotionId")) ? null : reader.GetInt64("PromotionId"),
                        PointsEarned = reader.GetInt32("PointsEarned")
                    });
                }
            }

            return result;
        }


    }
}
