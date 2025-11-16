using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class ShopMoney
    {
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

        [JsonPropertyName("currency_code")]
        public string? CurrencyCode { get; set; }
    }
}
