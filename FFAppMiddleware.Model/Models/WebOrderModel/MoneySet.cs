using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class MoneySet
    {
        [JsonPropertyName("shop_money")]
        public ShopMoney? ShopMoney { get; set; }

        [JsonPropertyName("presentment_money")]
        public PresentmentMoney? PresentmentMoney { get; set; }
    }
}
