using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class WebOrderModel
    {
        [JsonPropertyName("order")]
        public OrderModel? Order { get; set; }
    }
}
