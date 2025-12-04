using FFappMiddleware.DataAcces.DataBaseConnection;
using FFappMiddleware.DataBase.Logger;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.WebOrderModel;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace FFappMiddleware.DataBase.Repositories.Real
{
    public class WebOrderRepository : IWebOrderRepository
    {
        private readonly DataBaseAccesConfig _db;

        public WebOrderRepository()
        {
            _db = new DataBaseAccesConfig();
        }

        private SqlCommand CommandCreateWebOrder(WebOrderModel orderModel)
        {
            string sql = @"
        INSERT INTO [dbo].[web_order] (
            [cart_token],
            [checkout_id],
            [confirmation_number],
            [confirmed],
            [order_number], 
            [order_created_at],
            [currency],
            [current_subtotal_price],
            [current_total_discounts], 
            [current_total_price], 
            [current_total_tax],
            [financial_status],
            [order_status_url], 
            [customer_created_at],
            [customer_updated_at],
            [customer_first_name],
            [customer_last_name], 
            [customer_address_1],
            [customer_address_2],
            [customer_city],
            [customer_email], 
            [customer_phone],
            [customer_country_name],
            [shipping_address_full_name],
            [shipping_address_phone],
            [shipping_address_city], 
            [shipping_address_contry],
            [shipping_lines_code],
            [shipping_lines_discounted_price],
            [billing_address_address1],
            [billing_address_name],
            [billing_address_city],
            [billing_address_country],
            [order_pharmacy_pharmacy_id]

           
        ) VALUES (
            @cart_token,
            @checkout_id,
            @confirmation_number,
            @confirmed, 
            @order_number, 
            @order_created_at,  
            @currency,
            @current_subtotal_price,
            @current_total_discounts, 
            @current_total_price,
            @current_total_tax,
            @financial_status,
            @order_status_url, 
            @customer_created_at,
            @customer_updated_at,
            @customer_first_name,
            @customer_last_name, 
            @customer_address_1,
            @customer_address_2,
            @customer_city,
            @customer_email, 
            @customer_phone,
            @customer_country_name,
            @shipping_address_full_name,
            @shipping_address_phone,
            @shipping_address_city, 
            @shipping_address_contry,
            @shipping_lines_code,
            @shipping_lines_discounted_price,
            @billing_address_address1,
            @billing_address_name,
            @billing_address_city,
            @billing_address_country,
            @order_pharmacy_pharmacy_id

        );

        SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(sql);

            command.Parameters.Add("@cart_token", SqlDbType.NVarChar, 100).Value = (object)orderModel.Order.CartToken ?? DBNull.Value;
            command.Parameters.Add("@checkout_id", SqlDbType.BigInt).Value = (object)orderModel.Order.CheckoutId ?? DBNull.Value;
            command.Parameters.Add("@confirmation_number", SqlDbType.NVarChar, 200).Value = (object)orderModel.Order.ConfirmationNumber ?? DBNull.Value;
            command.Parameters.Add("@confirmed", SqlDbType.Bit).Value = (object)orderModel.Order.Confirmed ?? DBNull.Value;

            command.Parameters.Add("@order_number", SqlDbType.BigInt).Value = (object)orderModel.Order.OrderNumber ?? DBNull.Value;
            command.Parameters.Add("@order_created_at", SqlDbType.DateTime2).Value = (object)orderModel.Order.CreatedAt ?? DBNull.Value;
            command.Parameters.Add("@currency", SqlDbType.NVarChar, 50).Value = (object)orderModel.Order.Currency ?? DBNull.Value;

            SqlParameter paramCurrentSubtotalPrice = new("@current_subtotal_price", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = (object)orderModel.Order.CurrentSubtotalPrice ?? DBNull.Value
            };
            command.Parameters.Add(paramCurrentSubtotalPrice);

            SqlParameter currentTotatlDiscounts = new("@current_total_discounts", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = (object)orderModel.Order.CurrentTotalDiscounts ?? DBNull.Value
            };
            command.Parameters.Add(currentTotatlDiscounts);

            SqlParameter currentTotalPrice = new("@current_total_price", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = (object)orderModel.Order.CurrentTotalPrice ?? DBNull.Value
            };
            command.Parameters.Add(currentTotalPrice);

            SqlParameter currentTotalTax = new("@current_total_tax", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = (object)orderModel.Order.CurrentTotalTax ?? DBNull.Value
            };
            command.Parameters.Add(currentTotalTax);

            command.Parameters.Add("@financial_status", SqlDbType.NVarChar, 50).Value = (object)orderModel.Order.FinancialStatus ?? DBNull.Value;
            command.Parameters.Add("@order_status_url", SqlDbType.NVarChar, 1000).Value = (object)orderModel.Order.OrderStatusUrl ?? DBNull.Value;

            command.Parameters.Add("@customer_created_at", SqlDbType.DateTime2).Value = (object)orderModel.Order.Customer.CreatedAt ?? DBNull.Value;
            command.Parameters.Add("@customer_updated_at", SqlDbType.DateTime2).Value = (object)orderModel.Order.Customer.UpdatedAt ?? DBNull.Value;
            command.Parameters.Add("@customer_first_name", SqlDbType.NVarChar, 150).Value = (object)orderModel.Order.Customer.FirstName ?? DBNull.Value;
            command.Parameters.Add("@customer_last_name", SqlDbType.NVarChar, 150).Value = (object)orderModel.Order.Customer.LastName ?? DBNull.Value;
            command.Parameters.Add("@customer_address_1", SqlDbType.NVarChar, 500).Value = (object)orderModel.Order.Customer.DefaultAddress.Address1 ?? DBNull.Value;
            command.Parameters.Add("@customer_address_2", SqlDbType.NVarChar, 500).Value = (object)orderModel.Order.Customer.DefaultAddress.Address2 ?? DBNull.Value;

            command.Parameters.Add("@customer_city", SqlDbType.NVarChar, 100).Value = (object)orderModel.Order.Customer.DefaultAddress.City ?? DBNull.Value;
            command.Parameters.Add("@customer_email", SqlDbType.NVarChar, 200).Value = (object)orderModel.Order.Customer.Email ?? DBNull.Value;
            command.Parameters.Add("@customer_phone", SqlDbType.NVarChar, 50).Value = (object)orderModel.Order.Customer.Phone ?? DBNull.Value;

            command.Parameters.Add("@customer_country_name", SqlDbType.NVarChar, 200).Value = (object)orderModel.Order.Customer.DefaultAddress.Country ?? DBNull.Value;
            command.Parameters.Add("@shipping_address_full_name", SqlDbType.NVarChar, 150).Value = (object)orderModel.Order.ShippingAddress.Name ?? DBNull.Value;

            command.Parameters.Add("@shipping_address_phone", SqlDbType.NVarChar, 50).Value = (object)orderModel.Order.ShippingAddress.Phone ?? DBNull.Value;
            command.Parameters.Add("@shipping_address_city", SqlDbType.NVarChar, 100).Value = (object)orderModel.Order.ShippingAddress.City ?? DBNull.Value;
            command.Parameters.Add("@shipping_address_contry", SqlDbType.NVarChar, 200).Value = (object)orderModel.Order.ShippingAddress.Country ?? DBNull.Value;

            command.Parameters.Add("@shipping_lines_code", SqlDbType.NVarChar, 50).Value = (object)orderModel.Order.ShippingLines[0].Code ?? DBNull.Value;

            SqlParameter shippingLinesDiscountedPrice = new SqlParameter("@shipping_lines_discounted_price", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = (object)orderModel.Order.ShippingLines[0].DiscountedPrice ?? DBNull.Value
            };
            command.Parameters.Add(shippingLinesDiscountedPrice);

            command.Parameters.Add("@billing_address_address1", SqlDbType.NVarChar, 500).Value = (object)orderModel.Order.BillingAddress.Address1 ?? DBNull.Value;

            command.Parameters.Add("@billing_address_name", SqlDbType.NVarChar, 300).Value = (object)orderModel.Order.BillingAddress.Name ?? DBNull.Value;
            command.Parameters.Add("@billing_address_city", SqlDbType.NVarChar, 100).Value = (object)orderModel.Order.BillingAddress.City ?? DBNull.Value;
            command.Parameters.Add("@billing_address_country", SqlDbType.NVarChar, 200).Value = (object)orderModel.Order.BillingAddress.Country ?? DBNull.Value;
            command.Parameters.Add("@order_pharmacy_pharmacy_id", SqlDbType.BigInt).Value = (object)orderModel.Order.Pharmacy?.PharmacyId ?? DBNull.Value;


            return command;
        }

        private SqlCommand CommandWebOrderDetails(LineItemModel detail, long orderId)
        {
            string sql = @"
        INSERT INTO [dbo].[web_order_details] (
            [web_order_id],
            [current_quantity],
            [grams],
            [price], 
            [quantity],
            [product_id],
            [total_discount],
            [product_exists]
        ) VALUES (
            @web_order_id,
            @current_quantity,
            @grams,
            @price, 
            @quantity,
            @product_id,
            @total_discount,
            @product_exists
        );";

            SqlCommand command = new(sql);

            command.Parameters.Add("@web_order_id", SqlDbType.BigInt).Value = orderId;
            command.Parameters.Add("@current_quantity", SqlDbType.Int).Value = (object)detail.CurrentQuantity ?? DBNull.Value;
            command.Parameters.Add("@grams", SqlDbType.Int).Value = (object)detail.Grams ?? DBNull.Value;

            SqlParameter price = new("@price", SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = (object)detail.Price ?? DBNull.Value
            };
            command.Parameters.Add(price);

            command.Parameters.Add("@quantity", SqlDbType.Int).Value = (object)detail.Quantity ?? DBNull.Value;
            command.Parameters.Add("@product_id", SqlDbType.BigInt).Value = (object)detail.Sku ?? DBNull.Value;

            command.Parameters.Add("@total_discount", SqlDbType.NVarChar, 50).Value = (object)detail.TotalDiscount ?? DBNull.Value;

            command.Parameters.Add("@product_exists", SqlDbType.BigInt).Value = (object)detail.ProductExists ?? DBNull.Value;
            return command;
        }

        public async Task<WebOrderSaveResult> CreateWebOrderAsync(WebOrderModel orderModel)
        {
            using SqlConnection connection = await _db.PharmaFFConnectionAsync;

            using SqlTransaction transaction = connection.BeginTransaction();

            long webOrderId = 0;

            SqlCommand cmdSaveWebOrder = null;
            SqlCommand cmdSaveWebOrderDetails = null;

            try
            {
                WriteLog.DB.Debug($"WebOrder Data: {JsonSerializer.Serialize(orderModel)}");
                cmdSaveWebOrder = CommandCreateWebOrder(orderModel);

                cmdSaveWebOrder.Connection = connection;
                cmdSaveWebOrder.Transaction = transaction;

                long newOrderId = Convert.ToInt64(await cmdSaveWebOrder.ExecuteScalarAsync());
                webOrderId = newOrderId;

                foreach (var detail in orderModel.Order.LineItems)
                {
                    cmdSaveWebOrderDetails = CommandWebOrderDetails(detail, webOrderId);

                    cmdSaveWebOrderDetails.Connection = connection;
                    cmdSaveWebOrderDetails.Transaction = transaction;

                    await cmdSaveWebOrderDetails.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                return new WebOrderSaveResult
                {
                    Code = 0,
                    Message = "Comanda salvata cu succes."
                };
            }

            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();

                WriteLog.DB.Error($"SQL Error (Code: {sqlEx.Number}): {sqlEx.Message}");

                return new WebOrderSaveResult
                {
                    Code = -2,
                    Message = "Eroare baza de date: " + sqlEx.Message
                };
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                WriteLog.DB.Error($"CreateWebOrderAsync give an error: {ex}");

                return new WebOrderSaveResult
                {
                    Code = -1,
                    Message = ex.Message
                };
            }

            finally
            {
                await _db.DisposeObjectsAsync(connection, cmdSaveWebOrder, cmdSaveWebOrderDetails);
            }
        }





        #region Refactroing Old Code

        public async Task<WebOrderSaveResult> CreateWebOrderAsyncV2(WebOrderModel orderModel)
        {
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                WriteLog.DB.Debug($"WebOrder Data: {JsonSerializer.Serialize(orderModel)}");

                connection = await _db.PharmaFFConnectionAsync;
                transaction = connection.BeginTransaction();

                long webOrderId = await SaveWebOrder(orderModel, connection, transaction);

                await SaveWebOrderDetails(orderModel.Order.LineItems, webOrderId, connection, transaction);

                WriteLog.DB.Info($"Saved {orderModel.Order.LineItems.Count} line items");

                await transaction.CommitAsync();

                return new WebOrderSaveResult
                {
                    Code = 0,
                    Message = "Comanda salvată cu succes.",
                };
            }

            catch (WebOrderValidationException vex)
            {
                await transaction.RollbackAsync();
                WriteLog.DB.Error($"Validation error: {vex.Message}");

                return new WebOrderSaveResult
                {
                    Code = -3,
                    Message = vex.Message,
                    ErrorDetails = vex.Field
                };
            }

            catch (SqlException sqlEx)
            {
                await transaction.RollbackAsync();

                WriteLog.DB.Error($"SQL Error (Code: {sqlEx.Number}): {sqlEx.Message}");

                string userMessage = sqlEx.Number switch
                {
                    2627 => "Comanda există deja în sistem.",
                    547 => "Eroare de referință: verificați ID-urile.",
                    515 => "Lipsesc date obligatorii.",
                    _ => "Eroare bază de date."
                };

                return new WebOrderSaveResult
                {
                    Code = -2,
                    Message = userMessage,
                    ErrorDetails = $"SQL Error {sqlEx.Number}: {sqlEx.Message}"
                };
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                WriteLog.DB.Error($"Unexpected error: {ex.Message}", ex);

                return new WebOrderSaveResult
                {
                    Code = -1,
                    Message = "Eroare la salvarea comenzii.",
                    ErrorDetails = ex.Message
                };
            }

            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                    WriteLog.DB.Debug("Transaction disposed");
                }

                if (connection != null)
                {
                    await connection.CloseAsync();
                    await connection.DisposeAsync();
                    WriteLog.DB.Debug("Connection closed and disposed");
                }
            }
        }

        private async Task<long> SaveWebOrder(WebOrderModel orderModel, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"
        INSERT INTO [dbo].[web_order] (
            [cart_token],
            [checkout_id],
            [confirmation_number],
            [confirmed],
            [order_number], 
            [order_created_at],
            [currency],
            [current_subtotal_price],
            [current_total_discounts], 
            [current_total_price], 
            [current_total_tax],
            [financial_status],
            [order_status_url], 
            [customer_created_at],
            [customer_updated_at],
            [customer_first_name],
            [customer_last_name], 
            [customer_address_1],
            [customer_address_2],
            [customer_city],
            [customer_email], 
            [customer_phone],
            [customer_country_name],
            [shipping_address_full_name],
            [shipping_address_phone],
            [shipping_address_city], 
            [shipping_address_contry],
            [shipping_lines_code],
            [shipping_lines_discounted_price],
            [billing_address_address1],
            [billing_address_name],
            [billing_address_city],
            [billing_address_country],
            [order_pharmacy_pharmacy_id]

           
        ) VALUES (
            @cart_token,
            @checkout_id,
            @confirmation_number,
            @confirmed, 
            @order_number, 
            @order_created_at,  
            @currency,
            @current_subtotal_price,
            @current_total_discounts, 
            @current_total_price,
            @current_total_tax,
            @financial_status,
            @order_status_url, 
            @customer_created_at,
            @customer_updated_at,
            @customer_first_name,
            @customer_last_name, 
            @customer_address_1,
            @customer_address_2,
            @customer_city,
            @customer_email, 
            @customer_phone,
            @customer_country_name,
            @shipping_address_full_name,
            @shipping_address_phone,
            @shipping_address_city, 
            @shipping_address_contry,
            @shipping_lines_code,
            @shipping_lines_discounted_price,
            @billing_address_address1,
            @billing_address_name,
            @billing_address_city,
            @billing_address_country,
            @order_pharmacy_pharmacy_id

        );

        SELECT SCOPE_IDENTITY();";

            using SqlCommand cmd = new SqlCommand(sql, connection, transaction);

            OrderModel order = orderModel.Order;
            CustomerModel customer = order.Customer;
            AddressModel shippingAddr = order.ShippingAddress;

            AddressModel billingAddr = order.BillingAddress;
            AddressModel defaultAddr = customer.DefaultAddress;
            ShippingLine shippingLine = order.ShippingLines?.FirstOrDefault();

            // Parametri Order
            cmd.Parameters.Add("@cart_token", SqlDbType.NVarChar, 100).Value = (object)order.CartToken ?? DBNull.Value;
            cmd.Parameters.Add("@checkout_id", SqlDbType.BigInt).Value = (object)order.CheckoutId ?? DBNull.Value;
            cmd.Parameters.Add("@confirmation_number", SqlDbType.NVarChar, 200).Value = (object)order.ConfirmationNumber ?? DBNull.Value;
            cmd.Parameters.Add("@confirmed", SqlDbType.Bit).Value = (object)order.Confirmed ?? DBNull.Value;

            cmd.Parameters.Add("@order_number", SqlDbType.BigInt).Value = (object)order.OrderNumber ?? DBNull.Value;
            cmd.Parameters.Add("@order_created_at", SqlDbType.DateTime2).Value = (object)order.CreatedAt ?? DBNull.Value;
            cmd.Parameters.Add("@currency", SqlDbType.NVarChar, 50).Value = (object)order.Currency ?? DBNull.Value;

            // Parametri Decimal
            AddDecimalParameter(cmd, "@current_subtotal_price", order.CurrentSubtotalPrice);
            AddDecimalParameter(cmd, "@current_total_discounts", order.CurrentTotalDiscounts);
            AddDecimalParameter(cmd, "@current_total_price", order.CurrentTotalPrice);
            AddDecimalParameter(cmd, "@current_total_tax", order.CurrentTotalTax);

            cmd.Parameters.Add("@financial_status", SqlDbType.NVarChar, 50).Value = (object)order.FinancialStatus ?? DBNull.Value;
            cmd.Parameters.Add("@order_status_url", SqlDbType.NVarChar, 1000).Value = (object)order.OrderStatusUrl ?? DBNull.Value;

            // Parametri Customer
            cmd.Parameters.Add("@customer_created_at", SqlDbType.DateTime2).Value = (object)customer.CreatedAt ?? DBNull.Value;
            cmd.Parameters.Add("@customer_updated_at", SqlDbType.DateTime2).Value = (object)customer.UpdatedAt ?? DBNull.Value;
            cmd.Parameters.Add("@customer_first_name", SqlDbType.NVarChar, 150).Value = (object)customer.FirstName ?? DBNull.Value;

            cmd.Parameters.Add("@customer_last_name", SqlDbType.NVarChar, 150).Value = (object)customer.LastName ?? DBNull.Value;
            cmd.Parameters.Add("@customer_address_1", SqlDbType.NVarChar, 500).Value = (object)defaultAddr?.Address1 ?? DBNull.Value;
            cmd.Parameters.Add("@customer_address_2", SqlDbType.NVarChar, 500).Value = (object)defaultAddr?.Address2 ?? DBNull.Value;
            cmd.Parameters.Add("@customer_city", SqlDbType.NVarChar, 100).Value = (object)defaultAddr?.City ?? DBNull.Value;

            cmd.Parameters.Add("@customer_email", SqlDbType.NVarChar, 200).Value = (object)customer.Email ?? DBNull.Value;
            cmd.Parameters.Add("@customer_phone", SqlDbType.NVarChar, 50).Value = (object)customer.Phone ?? DBNull.Value;
            cmd.Parameters.Add("@customer_country_name", SqlDbType.NVarChar, 200).Value = (object)defaultAddr?.Country ?? DBNull.Value;

            // Parametri Shipping
            cmd.Parameters.Add("@shipping_address_full_name", SqlDbType.NVarChar, 150).Value = (object)shippingAddr.Name ?? DBNull.Value;
            cmd.Parameters.Add("@shipping_address_phone", SqlDbType.NVarChar, 50).Value = (object)shippingAddr.Phone ?? DBNull.Value;
            cmd.Parameters.Add("@shipping_address_city", SqlDbType.NVarChar, 100).Value = (object)shippingAddr.City ?? DBNull.Value;

            cmd.Parameters.Add("@shipping_address_country", SqlDbType.NVarChar, 200).Value = (object)shippingAddr.Country ?? DBNull.Value;
            cmd.Parameters.Add("@shipping_lines_code", SqlDbType.NVarChar, 50).Value = (object)shippingLine?.Code ?? DBNull.Value;
            AddDecimalParameter(cmd, "@shipping_lines_discounted_price", shippingLine?.DiscountedPrice);

            // Parametri Billing
            cmd.Parameters.Add("@billing_address_address1", SqlDbType.NVarChar, 500).Value = (object)billingAddr.Address1 ?? DBNull.Value;
            cmd.Parameters.Add("@billing_address_name", SqlDbType.NVarChar, 300).Value = (object)billingAddr.Name ?? DBNull.Value;
            cmd.Parameters.Add("@billing_address_city", SqlDbType.NVarChar, 100).Value = (object)billingAddr.City ?? DBNull.Value;

            cmd.Parameters.Add("@billing_address_country", SqlDbType.NVarChar, 200).Value = (object)billingAddr.Country ?? DBNull.Value;

            // Parametri Pharmacy (opțional)
            cmd.Parameters.Add("@order_pharmacy_pharmacy_id", SqlDbType.BigInt).Value = (object)order.Pharmacy?.PharmacyId ?? DBNull.Value;

            return Convert.ToInt64(await cmd.ExecuteScalarAsync());
        }

        private async Task SaveWebOrderDetails(List<LineItemModel> lineItems, long webOrderId, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"
        INSERT INTO [dbo].[web_order_details] (
            [web_order_id],
            [current_quantity],
            [grams],
            [price], 
            [quantity],
            [product_id],
            [total_discount],
            [product_exists]
        ) VALUES (
            @web_order_id,
            @current_quantity,
            @grams,
            @price, 
            @quantity,
            @product_id,
            @total_discount,
            @product_exists
        );";
            foreach (var item in lineItems)
            {
                using SqlCommand cmd = new(sql, connection, transaction);

                cmd.Parameters.Add("@web_order_id", SqlDbType.BigInt).Value = webOrderId;
                cmd.Parameters.Add("@current_quantity", SqlDbType.Int).Value = (object)item.CurrentQuantity ?? DBNull.Value;
                cmd.Parameters.Add("@grams", SqlDbType.Int).Value = (object)item.Grams ?? DBNull.Value;

                AddDecimalParameter(cmd, "@price", item.Price);

                cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = (object)item.Quantity ?? DBNull.Value;
                cmd.Parameters.Add("@product_id", SqlDbType.BigInt).Value = (object)item.Sku ?? DBNull.Value;
                cmd.Parameters.Add("@total_discount", SqlDbType.NVarChar, 50).Value = (object)item.TotalDiscount ?? DBNull.Value;
                cmd.Parameters.Add("@product_exists", SqlDbType.BigInt).Value = (object)item.ProductExists ?? DBNull.Value;

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private void AddDecimalParameter(SqlCommand cmd, string paramName, decimal? value)
        {
            SqlParameter param = new SqlParameter(paramName, SqlDbType.Decimal)
            {
                Precision = 18,
                Scale = 2,
                Value = (object)value ?? DBNull.Value
            };
            cmd.Parameters.Add(param);
        }

        private void ValidateOrderModel(WebOrderModel orderModel)
        {
            if (orderModel?.Order == null)
                throw new WebOrderValidationException("orderModel", "Date comandă lipsesc.");

            if (orderModel.Order.Customer == null)
                throw new WebOrderValidationException("Customer", "Informații client lipsesc.");

            if (orderModel.Order.ShippingAddress == null)
                throw new WebOrderValidationException("ShippingAddress", "Adresa de livrare lipsește.");

            if (orderModel.Order.BillingAddress == null)
                throw new WebOrderValidationException("BillingAddress", "Adresa de facturare lipsește.");

            if (orderModel.Order.LineItems == null || !orderModel.Order.LineItems.Any())
                throw new WebOrderValidationException("LineItems", "Comanda nu conține produse.");


        }

        #endregion
    }

    public class WebOrderValidationException : Exception
    {
        public string Field { get; }

        public WebOrderValidationException(string field, string message) : base(message)
        {
            Field = field;
        }
    }


    public static class SqlParameterExtensions
    {
        /// <summary>
        /// Wrapper pentru SqlParameter care capturează linia și validează la atribuire
        /// </summary>
        public class SafeSqlParameter
        {
            private readonly SqlParameter _parameter;
            private readonly string _fileName;

            private readonly int _lineNumber;
            private readonly string _memberName;

            internal SafeSqlParameter(SqlParameter parameter, string fileName, int lineNumber, string memberName)
            {
                _parameter = parameter;
                _fileName = fileName;
                _lineNumber = lineNumber;
                _memberName = memberName;
            }

            /// <summary>
            /// Setează valoarea cu validare automată
            /// </summary>
            public object Value
            {
                set
                {
                    try
                    {
                        ValidateValue(value, _parameter.SqlDbType);
                        _parameter.Value = value;
                    }
                    catch (Exception ex)
                    {
               
                        throw new InvalidOperationException(

                            $"\n------ EROARE PARAMETRU SQL ------\n" +
                            $"Fișier: {_fileName}\n" +
                            $"Metodă: {_memberName}\n" +
                            $"LINIA: {_lineNumber}\n" +
                            $"Parametru: {_parameter.ParameterName}\n" +
                            $"Tip așteptat: {_parameter.SqlDbType}\n" +
                            $"Tip primit: {value?.GetType().Name ?? "null"}\n" +
                            $"Valoare: {(value == null ? "NULL" : $"'{value}'")}\n" +
                            $"Eroare: {ex.Message}\n" +
                            $"-------------------------------------",

                            ex);
                    }
                }
            }

            private void ValidateValue(object value, SqlDbType type)
            {
                if (value == null || value == DBNull.Value)
                    return;

                bool isValid = type switch
                {
                    SqlDbType.BigInt => value is long || value is int || value is short || value is byte,
                    SqlDbType.Int => value is int || value is short || value is byte,
                    SqlDbType.SmallInt => value is short || value is byte,
                    SqlDbType.TinyInt => value is byte,
                    SqlDbType.Bit => value is bool || value is byte,
                    SqlDbType.Decimal => value is decimal || value is double || value is float || value is int || value is long,
                    SqlDbType.Money => value is decimal || value is double || value is float,
                    SqlDbType.Float => value is double || value is float,
                    SqlDbType.Real => value is float,
                    SqlDbType.DateTime => value is DateTime,
                    SqlDbType.DateTime2 => value is DateTime,
                    SqlDbType.Date => value is DateTime,
                    SqlDbType.Time => value is TimeSpan || value is DateTime,
                    SqlDbType.SmallDateTime => value is DateTime,
                    SqlDbType.Char => value is string || value is char,
                    SqlDbType.VarChar => value is string,
                    SqlDbType.Text => value is string,
                    SqlDbType.NChar => value is string || value is char,
                    SqlDbType.NVarChar => value is string,
                    SqlDbType.NText => value is string,
                    SqlDbType.Binary => value is byte[],
                    SqlDbType.VarBinary => value is byte[],
                    SqlDbType.Image => value is byte[],
                    SqlDbType.UniqueIdentifier => value is Guid || (value is string && Guid.TryParse((string)value, out _)),
                    _ => true // Pentru tipuri necunoscute, permite
                };

                if (!isValid)
                {
                    throw new InvalidCastException(
                        $"Tipul {value.GetType().Name} nu este compatibil cu SqlDbType.{type}");
                }
            }
        }

        /// <summary>
        /// Extensie pentru SqlParameterCollection - sintaxă identică cu Add() normal
        /// </summary>
        public static SafeSqlParameter AddSafe(this SqlParameterCollection parameters, string parameterName, SqlDbType sqlDbType, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
        {
            SqlParameter param = parameters.Add(parameterName, sqlDbType);
            string fileName = Path.GetFileName(filePath);

            return new SafeSqlParameter(param, fileName, lineNumber, memberName);
        }

        /// <summary>
        /// Extensie cu Size - pentru varchar, nvarchar etc.
        /// </summary>
        public static SafeSqlParameter AddSafe(this SqlParameterCollection parameters, string parameterName, SqlDbType sqlDbType, int size, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
        {
            SqlParameter param = parameters.Add(parameterName, sqlDbType, size);
            string fileName = Path.GetFileName(filePath);

            return new SafeSqlParameter(param, fileName, lineNumber, memberName);
        }
    }
}

