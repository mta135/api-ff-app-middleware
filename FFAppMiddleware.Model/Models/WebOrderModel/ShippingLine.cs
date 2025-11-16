using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class ShippingLine
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("carrier_identifier")]
        public string? CarrierIdentifier { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("discounted_price")]
        public string? DiscountedPrice { get; set; }

        [JsonPropertyName("discounted_price_set")]
        public MoneySet? DiscountedPriceSet { get; set; }

        [JsonPropertyName("is_removed")]
        public bool IsRemoved { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("price")]
        public string? Price { get; set; }

        [JsonPropertyName("price_set")]
        public MoneySet? PriceSet { get; set; }

        [JsonPropertyName("requested_fulfillment_service_id")]
        public string? RequestedFulfillmentServiceId { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("tax_lines")]
        public List<object>? TaxLines { get; set; } // Specifică un tip mai bun

        [JsonPropertyName("discount_allocations")]
        public List<object>? DiscountAllocations { get; set; } // Specifică un tip mai bun
    }
}
