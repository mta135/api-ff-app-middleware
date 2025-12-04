using System.Text.Json.Serialization;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class AddressModel
    {
        //   [JsonPropertyName("first_name")]
        //  public string? FirstName { get; set; }

        [JsonPropertyName("address1")]
        public string? Address1 { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        //    [JsonPropertyName("zip")]
        //    public string? Zip { get; set; }

        //   [JsonPropertyName("province")]
        //    public string? Province { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

    //    [JsonPropertyName("last_name")]
    //    public string? LastName { get; set; }

        [JsonPropertyName("address2")]
        public string? Address2 { get; set; }

        //    [JsonPropertyName("company")]
        //   public string? Company { get; set; }

        //    [JsonPropertyName("latitude")]
        //    public decimal? Latitude { get; set; }

        //    [JsonPropertyName("longitude")]
        //    public decimal? Longitude { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        //    [JsonPropertyName("country_code")]
        //    public string? CountryCode { get; set; }

        //    [JsonPropertyName("province_code")]
        //    public string? ProvinceCode { get; set; }

        //    [JsonPropertyName("id")]
        //   public long? Id { get; set; } // Doar pentru default_address

        //   [JsonPropertyName("customer_id")]
        //   public long? CustomerId { get; set; } // Doar pentru default_address

        //    [JsonPropertyName("country_name")]
        //    public string? CountryName { get; set; } // Doar pentru default_address

        //    [JsonPropertyName("default")]
        //   public bool? Default { get; set; } // Doar pentru default_address
    }
}
