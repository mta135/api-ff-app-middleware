using FFappMiddleware.DataAcces.DataBaseConnection;
using FFappMiddleware.DataBase.Repositories.Abstract;
using FFAppMiddleware.Model.Models.Products;
using Microsoft.Data.SqlClient;

namespace FFappMiddleware.DataBase.Repositories.Real
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataBaseAccesConfig _db;

        public ProductRepository()
        {
            _db = new DataBaseAccesConfig();
        }

        public async Task<List<ProductApiModel>> RetrieveProducts()
        {
            try
            {
                List<ProductApiModel> products = new();
                SqlCommand command = new("SELECT TOP (10) * FROM products p", await _db.ConnectionAsync);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        ProductApiModel product = new ProductApiModel
                        {
                            Id = Convert.ToInt64(reader["product_id"]),
                            ProductName = Convert.ToString(reader["product_name"]),
                            ProductDescprition = Convert.ToString(reader["product_description"]),
                            ProducerId = reader["producer_id"] == DBNull.Value ? null : (long?)reader["producer_id"]
                        };
                        products.Add(product);
                    }
                }
                return products;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving products: {ex.Message}");
            }
            finally
            {
                await _db.DisposeAsync();
            }
        }
    }
}
