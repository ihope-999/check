using System.Text.Json.Serialization;

namespace place_project.Domain.LocationDomain.Data
{
    public record GooglePlaceDetailsCandidate
    {
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("formatted_phone_number")]
        public string? FormattedPhoneNumber { get; set; }

        public string PhoneNumber { get; set; }
        [JsonPropertyName("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

    }
    public record GoogleFindDetailsResponse
    {

        public GooglePlaceDetailsCandidate Result { get; set; }

    }

    public record AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; init; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; init; }

        [JsonPropertyName("types")]
        public List<string> Types { get; init; }
    }
}
