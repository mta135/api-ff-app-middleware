using FFappMiddleware.DataAcces.DataBaseConnection;
using FFappMiddleware.DataBase.Logger;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.Products;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace FFappMiddleware.DataBase.Repositories.Real
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseAccesConfig _db;

        public ProductRepository()
        {
            _db = new DataBaseAccesConfig();
        }

        public async Task<List<ProductCategoriesModel>> ProductsCategories(string lang)
        {
            try
            {
                List<ProductCategoriesModel> products = new();
                string query = @"

SELECT [id_category] Id,
    CASE 
        WHEN @key = 'ru' THEN category_nameRu
        WHEN @key = 'en' THEN category_nameEn		
        ELSE category_name
    END AS Name,
    parent_id ParentId
    
FROM product_therapeutic_groups_site";

                SqlCommand command = new(query, await _db.PharmaFFConnectionAsync);
                command.Parameters.Add("@key", SqlDbType.NVarChar, 3).Value = lang;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ProductCategoriesModel productCategoriesModel = new ProductCategoriesModel
                        {
                            Id = Convert.ToInt64(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            ParentId = reader["ParentId"] == DBNull.Value ? null : (long?)reader["ParentId"]                           
                        };

                        products.Add(productCategoriesModel);
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    WriteLog.DB.Error("GetProductsCategories method. SqlException: ", ex);
                }
                else
                {
                    throw new Exception($"GetProductsCategories method. General Exception: ", ex);
                }

                return new List<ProductCategoriesModel>();
            }
        }

        private SqlCommand CommandGetAllProducts(ProductRequest filter, SqlConnection sqlConnection)
        {
            SqlCommand command = new SqlCommand("[dbo].[GetAllProducts]", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PageNumber", filter.PageNumber);
            command.Parameters.AddWithValue("@PageSize", filter.PageSize);

            // Filter
            command.Parameters.Add("@ProductId", SqlDbType.BigInt).Value =
                (object)filter.Filter.ProductId ?? DBNull.Value;
            command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 200).Value =
                (object)filter.Filter.ProductName ?? DBNull.Value;
            command.Parameters.Add("@ProducerName", SqlDbType.NVarChar, 200).Value =
                (object)filter.Filter.ProducerName ?? DBNull.Value;

            // Sort
            command.Parameters.Add("@SortColumn", SqlDbType.NVarChar, 100).Value =
                (object)filter.Sort.Column ?? DBNull.Value;
            command.Parameters.Add("@SortDirection", SqlDbType.NVarChar, 4).Value =
                (object)filter.Sort.Direction ?? DBNull.Value;

            // OUTPUT
            command.Parameters.Add("@_PageNumber", SqlDbType.Int).Direction = ParameterDirection.Output;
            command.Parameters.Add("@_PageSize", SqlDbType.Int).Direction = ParameterDirection.Output;
            command.Parameters.Add("@TotalCount", SqlDbType.BigInt).Direction = ParameterDirection.Output;
            command.Parameters.Add("@TotalPages", SqlDbType.Int).Direction = ParameterDirection.Output;
            command.Parameters.Add("@HasNextPage", SqlDbType.Bit).Direction = ParameterDirection.Output;
            command.Parameters.Add("@HasPreviousPage", SqlDbType.Bit).Direction = ParameterDirection.Output;

            return command;
        }

        public async Task<ProductResponse> GetAllProducts(ProductRequest filter)
        {
            SqlCommand command = null;
            try
            {
                ProductResponse response = new ProductResponse();

                List<ProductModel> products = new();

                command = CommandGetAllProducts(filter, await _db.PharmaFFConnectionAsync);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ProductModel product = new ProductModel
                        {
                            ProductCategoryId = Convert.ToInt32(reader["group_id"]),

                            ProductId = Convert.ToInt64(reader["product_id"]),
                            ProductName = Convert.ToString(reader["name"]),
                            ProductDescription = Convert.ToString(reader["description"]),
                            Status = 1,
                            StockSalePrice = Convert.ToDecimal(reader["price"]),
                            MaxCalculationBonus = Convert.ToDecimal(reader["max_calculation_bonus"]),

                            MaxCalculationBonusUnit = Convert.ToDecimal(reader["max_calculation_bonus_unit"]),
                            MaxPaymentBonus = Convert.ToDecimal(reader["max_payment_bonus"]),

                            MaxPaymentBonusUnit = Convert.ToDecimal(reader["max_payment_bonus_unit"]),
                            BarCodes = Convert.ToString(reader["barcodes"]),
                            ProducerName = Convert.ToString(reader["manufacturer"]),
                            ProductCategoryName = Convert.ToString(reader["product_category_name"])
                        };

                        products.Add(product);
                    }
                }

                response.PageNumber = Convert.ToInt32(command.Parameters["@_PageNumber"].Value.ToString());
                response.PageSize = Convert.ToInt32(command.Parameters["@_PageSize"].Value.ToString());
                response.TotalCount = Convert.ToInt64(command.Parameters["@TotalCount"].Value.ToString());

                response.TotalPages = Convert.ToInt32(command.Parameters["@TotalPages"].Value.ToString());
                response.HasNextPage = Convert.ToBoolean(command.Parameters["@HasNextPage"].Value);
                response.HasPreviousPage = Convert.ToBoolean(command.Parameters["@HasPreviousPage"].Value);

                response.Products = products;

                return response;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                {
                    WriteLog.DB.Error("GetAllProducts method. SqlException: ", ex);
                }
                else
                {
                    WriteLog.DB.Error("GetAllProducts method. Exception: ", ex);
                }

                return null;
            }

            finally
            {
                command.Dispose();
            }
        }

        public async Task<List<ProductModel>> GetPromotionsForProductId(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<ProductModel>();

            string productIds = string.Join(",", ids);

            string query = @"
DECLARE @product_id VARCHAR(MAX);
SET @product_id = @Ids;

DECLARE @products TABLE (product_id BIGINT);

DECLARE @xml XML;
SET @xml = CAST('<i>' + REPLACE(@product_id, ',', '</i><i>') + '</i>' AS XML);

INSERT INTO @products (product_id)
SELECT TRY_CAST(s.i.value('.', 'varchar(50)') AS BIGINT)
FROM @xml.nodes('/i') AS s(i)
WHERE TRY_CAST(s.i.value('.', 'varchar(50)') AS BIGINT) IS NOT NULL;

SELECT 
    p.product_id,
    p.product_name,
     pr.producer_name,
    pr.producer_id,
    pc.product_category_name,
   
    pro.promotion_id,
    pro.promotion_name,
    pro.promotion_begin_date,
    pro.promotion_end_date,
    CAST(pro.promotion_use_supplier AS BIT) AS promotion_use_supplier,
    CAST(pro.promotion_is_valability_term_oriented AS BIT) AS promotion_is_valability_term_oriented,
    CAST(pro.is_stock_age_oriented AS BIT) AS is_stock_age_oriented,
    CAST(pro.promotion_use_producers AS BIT) AS promotion_use_producers,
    CAST(pro.promotion_use_product_categories AS BIT) AS promotion_use_product_categories,
    CAST(pro.promotion_use_products AS BIT) AS promotion_use_products
FROM promotions AS pro
INNER JOIN promotions_products_units AS ppu ON pro.promotion_id = ppu.promotion_id
LEFT JOIN products_units AS pu ON pu.product_unit_id = ppu.product_unit_id
LEFT JOIN products AS p ON p.product_id = pu.product_id
LEFT JOIN producers AS pr ON p.producer_id = pr.producer_id
LEFT JOIN product_categories AS pc ON p.product_category_id = pc.product_category_id
WHERE 
    pro.promotion_is_validated = 1
    AND p.product_id IN (SELECT product_id FROM @products)
    AND pro.promotion_begin_date <= GETDATE()
    AND pro.promotion_end_date >= GETDATE()

UNION ALL

SELECT 
    p.product_id,
    p.product_name,
   
    pr.producer_name,
    pr.producer_id,
    pc.product_category_name,
  
    pro.promotion_id,
    pro.promotion_name,
    pro.promotion_begin_date,
    pro.promotion_end_date,
    CAST(pro.promotion_use_supplier AS BIT),
    CAST(pro.promotion_is_valability_term_oriented AS BIT),
    CAST(pro.is_stock_age_oriented AS BIT),
    CAST(pro.promotion_use_producers AS BIT),
    CAST(pro.promotion_use_product_categories AS BIT),
    CAST(pro.promotion_use_products AS BIT)
FROM promotions AS pro
INNER JOIN promotions_producers AS ppr ON pro.promotion_id = ppr.promotion_id
INNER JOIN products AS p ON p.producer_id = ppr.producer_id
INNER JOIN producers AS pr ON p.producer_id = pr.producer_id
LEFT JOIN product_categories AS pc ON p.product_category_id = pc.product_category_id
WHERE 
    pro.promotion_is_validated = 1
    AND p.product_id IN (SELECT product_id FROM @products)
    AND pro.promotion_begin_date <= GETDATE()
    AND pro.promotion_end_date >= GETDATE()
    AND ppr.promotion_producer_id IN (
        SELECT producer_id FROM products ppp WHERE ppp.product_id IN (SELECT product_id FROM @products)
    )

UNION ALL

SELECT 
    p.product_id,
    p.product_name,
   
    pr.producer_name,
    pr.producer_id,
    pc.product_category_name,    
    pro.promotion_id,
    pro.promotion_name,
    pro.promotion_begin_date,
    pro.promotion_end_date,
    CAST(pro.promotion_use_supplier AS BIT),
    CAST(pro.promotion_is_valability_term_oriented AS BIT),
    CAST(pro.is_stock_age_oriented AS BIT),
    CAST(pro.promotion_use_producers AS BIT),
    CAST(pro.promotion_use_product_categories AS BIT),
    CAST(pro.promotion_use_products AS BIT)
FROM promotions AS pro
INNER JOIN promotions_product_categories AS ppc ON pro.promotion_id = ppc.promotion_id
INNER JOIN products AS p ON p.product_category_id = ppc.product_category_id
LEFT JOIN producers AS pr ON p.producer_id = pr.producer_id
LEFT JOIN product_categories AS pc ON p.product_category_id = pc.product_category_id
WHERE 
    pro.promotion_is_validated = 1
    AND p.product_id IN (SELECT product_id FROM @products)
    AND pro.promotion_begin_date <= GETDATE()
    AND pro.promotion_end_date >= GETDATE()
    AND ppc.product_category_id IN (
        SELECT product_category_id FROM products pppc WHERE pppc.product_id IN (SELECT product_id FROM @products)
    );";

            try
            {
                var connection = await _db.PharmaFFConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = new SqlCommand(query, connection);
                command.Parameters.Add("@Ids", SqlDbType.VarChar).Value = productIds ?? string.Empty;

                var productDict = new Dictionary<long, ProductModel>();

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    long productId = Convert.ToInt64(reader["product_id"]);

                    if (!productDict.TryGetValue(productId, out var product))
                    {
                        product = new ProductModel
                        {
                            ProductId = productId,
                            ProductName = Convert.ToString(reader["product_name"])
                            
                        };

                        productDict[productId] = product;
                    }

                    var promo = new PromotionResponse
                    {
                        PromotionId = Convert.ToInt64(reader["promotion_id"]),
                        PromotionName = Convert.ToString(reader["promotion_name"]),
                        PromotionBeginDate = reader["promotion_begin_date"] == DBNull.Value
                            ? null
                            : (DateTime?)reader["promotion_begin_date"],
                        PromotionEndDate = reader["promotion_end_date"] == DBNull.Value
                            ? null
                            : (DateTime?)reader["promotion_end_date"],
                        PromotionUseSupplier = Convert.ToBoolean(reader["promotion_use_supplier"]),
                        PromotionIsValabilityTermOriented =
                            Convert.ToBoolean(reader["promotion_is_valability_term_oriented"]),
                        IsStockAgeOriented = Convert.ToBoolean(reader["is_stock_age_oriented"]),
                        PromotionUseProducers = Convert.ToBoolean(reader["promotion_use_producers"]),
                        PromotionUseProductCategories = Convert.ToBoolean(reader["promotion_use_product_categories"]),
                        PromotionUseProducts = Convert.ToBoolean(reader["promotion_use_products"])
                    };

                    product.Promotions.Add(promo);
                }

                return productDict.Values.ToList();
            }
            catch (SqlException ex)
            {
                WriteLog.DB.Error("GetPromotionsForProductId method. SqlException: ", ex);
                return new List<ProductModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("GetPromotionsForProductId method. General Exception: ", ex);
            }




        }


        public async Task<List<long>> GetBestSellingProducts()
        {
            string query = @"SELECT DISTINCT 
                         product_id     
                     FROM [crm_best_selling_products]
                     WHERE date_from <= GETDATE() AND date_to >= GETDATE();";

            try
            {
                var connection = await _db.PharmaFFConnectionAsync;
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = new SqlCommand(query, connection);
                var productIds = new List<long>();

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    productIds.Add(Convert.ToInt64(reader["product_id"]));
                }

                return productIds;
            }
            catch (SqlException ex)
            {
                WriteLog.DB.Error("GetBestSellingProducts method. SqlException: ", ex);
                return new List<long>();
            }
            catch (Exception ex)
            {
                throw new Exception("GetBestSellingProducts method. General Exception: ", ex);
            }
        }

    }
}
