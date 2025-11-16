using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.WebOrderModel
{
    public class MarketingConsent
    {
        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("opt_in_level")]
        public string? OptInLevel { get; set; }

        [JsonPropertyName("consent_updated_at")]
        public DateTimeOffset? ConsentUpdatedAt { get; set; }

        [JsonPropertyName("consent_collected_from")]
        public string? ConsentCollectedFrom { get; set; } // Doar pentru SMS
    }
}
