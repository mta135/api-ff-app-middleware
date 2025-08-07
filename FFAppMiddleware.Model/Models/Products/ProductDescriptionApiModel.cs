using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.Products
{
    public class ProductDescriptionApiModel
    {
        public int ProductDescriptionId { get; set; }

        public string Description { get; set; } = null!;

        public Guid Rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
