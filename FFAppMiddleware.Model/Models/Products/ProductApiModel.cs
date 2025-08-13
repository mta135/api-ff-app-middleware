using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.Products
{
    public class ProductApiModel
    {
        public long Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductDescprition { get; set; } = string.Empty;

        public long? ProducerId { get; set; }
    }
}
