using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FFAppMiddleware.Model.Models.SaleChart.PreCheckModel;

namespace FFAppMiddleware.Model.Models.SaleChart
{
    public class PreCheckApiModel
    {
        public Guid ClientId { get; set; }

        public long CardId { get; set; }

        public List<CartItem> Details { get; set; }
        
    }
}
