using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFappMiddleware.DataBase.Query
{
    public class SqlQueries
    {
        public const string GetProductCategories = @"select *FROM product_categories";

        public const string RetrieveProducts = @"SELECT TOP (10) * FROM products p";

    }
}
