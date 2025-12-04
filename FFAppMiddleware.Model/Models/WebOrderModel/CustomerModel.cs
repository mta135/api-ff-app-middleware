using System.Text.Json.Serialization;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class CustomerModel
    {
        //  [JsonPropertyName("id")]
        //   public long Id { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        //   [JsonPropertyName("state")]
        //   public string? State { get; set; }

        //   [JsonPropertyName("note")]
        //   public string? Note { get; set; }

        //    [JsonPropertyName("verified_email")]
        //    public bool VerifiedEmail { get; set; }

        //   [JsonPropertyName("multipass_identifier")]
        //    public string? MultipassIdentifier { get; set; }

        //    [JsonPropertyName("tax_exempt")]
        //    public bool TaxExempt { get; set; }

        //    [JsonPropertyName("email_marketing_consent")]
        //    public MarketingConsent? EmailMarketingConsent { get; set; }

        //    [JsonPropertyName("sms_marketing_consent")]
        //   public MarketingConsent? SmsMarketingConsent { get; set; }

        //    [JsonPropertyName("tags")]
        //    public string? Tags { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        //    [JsonPropertyName("currency")]
        //    public string? Currency { get; set; }

        //    [JsonPropertyName("tax_exemptions")]
        //   public List<object>? TaxExemptions { get; set; }

        //   [JsonPropertyName("admin_graphql_api_id")]
        //    public string? AdminGraphqlApiId { get; set; }

        [JsonPropertyName("default_address")]
        public AddressModel? DefaultAddress { get; set; }
    }
}
