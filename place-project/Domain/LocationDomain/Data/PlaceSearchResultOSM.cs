using System.Text.Json.Serialization;

namespace place_project.Domain.LocationDomain.Data
{
    public record OSMPlaceCandidate
    {
        [JsonPropertyName("display_name")]
        public string? Name { get; set; }

        [JsonPropertyName("lat")]
        public string? Lat { get; set; }

        [JsonPropertyName("lon")]
        public string? Lon { get; set; }

        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        [JsonPropertyName("extratags")]
        public ExtraTags? ExtraTags { get; set; }
    }

    public record Address
    {
        public string? completeAddress { get; set; }
        public string? Street { get; set; }
        [JsonPropertyName("road")] public string? Road { get; set; }
        [JsonPropertyName("city")] public string? City { get; set; }
        [JsonPropertyName("town")] public string? Town { get; set; }
        [JsonPropertyName("village")] public string? Village { get; set; }
        [JsonPropertyName("hamlet")] public string? Hamlet { get; set; }
        [JsonPropertyName("state")] public string? State { get; set; }
        [JsonPropertyName("postcode")] public string? Postcode { get; set; }
        [JsonPropertyName("country")] public string? Country { get; set; }
    }

    public class ExtraTags
    {
        [JsonPropertyName("phone")] public string? PhoneNumber { get; set; }
        [JsonPropertyName("website")] public string? Website { get; set; }
    }

}
