using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class LineItemModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("admin_graphql_api_id")]
        public string? AdminGraphqlApiId { get; set; }

        [JsonPropertyName("current_quantity")]
        public int CurrentQuantity { get; set; }

        [JsonPropertyName("fulfillable_quantity")]
        public int FulfillableQuantity { get; set; }

        [JsonPropertyName("fulfillment_service")]
        public string? FulfillmentService { get; set; }

        [JsonPropertyName("fulfillment_status")]
        public string? FulfillmentStatus { get; set; }

        [JsonPropertyName("gift_card")]
        public bool GiftCard { get; set; }

        [JsonPropertyName("grams")]
        public int Grams { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("price")]
        public string? Price { get; set; }

        [JsonPropertyName("price_set")]
        public MoneySet? PriceSet { get; set; }

        [JsonPropertyName("product_exists")]
        public bool ProductExists { get; set; }

        [JsonPropertyName("product_id")]
        public long ProductId { get; set; }

        [JsonPropertyName("properties")]
        public List<object>? Properties { get; set; } // Specifică un tip mai bun

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("requires_shipping")]
        public bool RequiresShipping { get; set; }

        [JsonPropertyName("sku")]
        public string? Sku { get; set; }

        [JsonPropertyName("taxable")]
        public bool Taxable { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("total_discount")]
        public string? TotalDiscount { get; set; }

        [JsonPropertyName("total_discount_set")]
        public MoneySet? TotalDiscountSet { get; set; }

        [JsonPropertyName("variant_id")]
        public long VariantId { get; set; }

        [JsonPropertyName("variant_inventory_management")]
        public string? VariantInventoryManagement { get; set; }

        [JsonPropertyName("variant_title")]
        public string? VariantTitle { get; set; }

        [JsonPropertyName("vendor")]
        public string? Vendor { get; set; }

        [JsonPropertyName("tax_lines")]
        public List<TaxLine>? TaxLines { get; set; }

        [JsonPropertyName("duties")]
        public List<object>? Duties { get; set; } // Specifică un tip mai bun

        [JsonPropertyName("discount_allocations")]
        public List<object>? DiscountAllocations { get; set; } // Specifică un tip mai bun
    }
}

