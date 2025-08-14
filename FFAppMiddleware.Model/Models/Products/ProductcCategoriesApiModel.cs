using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.Products
{
    public class ProductcCategoriesApiModel
    {
        public long Id { get; set; }

        public string ProductCategoryName { get; set; }

        public string SuperCategoryName { get; set; }
    }
}
