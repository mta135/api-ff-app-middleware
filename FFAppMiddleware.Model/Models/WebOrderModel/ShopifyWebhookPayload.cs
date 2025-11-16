using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class ShopifyWebhookPayload
    {
        [JsonPropertyName("order")]
        public ShopifyOrderModel? Order { get; set; }
    }
}
