using System.Text.Json.Serialization;

namespace place_project.Domain.LocationDomain.Data
{
    public class PlaceSearchResultYandex
    {
        [JsonPropertyName("features")]
        public List<YandexFeature> Features { get; set; }

        public double OverallScore { get; set; }
    }

    public class YandexFeature
    {
        [JsonPropertyName("properties")]
        public YandexProperties Properties { get; set; }
    }

    public class YandexProperties
    {
        [JsonPropertyName("CompanyMetaData")]
        public YandexCompanyMetaData CompanyMetaData { get; set; }
    }

    public class YandexCompanyMetaData
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("address")]
        public string? FullAddress { get; set; }  // Raw string address

        [JsonPropertyName("Address")]
        public YandexAddress? StructuredAddress { get; set; }

        [JsonPropertyName("Phones")]
        public List<YandexPhone>? Phones { get; set; }
        public double OverallScore { get; internal set; }
    }

    public class YandexAddress
    {
        [JsonPropertyName("formatted")]
        public string? Formatted { get; set; }

        [JsonPropertyName("postal_code")]
        public string? PostalCode { get; set; }

        [JsonPropertyName("Components")]
        public List<YandexAddressComponent>? Components { get; set; }

        public string GetCountry() => Components?.FirstOrDefault(c => c.Kind == "country")?.Name ?? string.Empty;
        public string GetCity() => Components?.FirstOrDefault(c => c.Kind == "locality")?.Name
                                   ?? Components?.FirstOrDefault(c => c.Kind == "province")?.Name
                                   ?? string.Empty;
        public string GetRegion() => Components?.FirstOrDefault(c => c.Kind == "province")?.Name
                                     ?? Components?.FirstOrDefault(c => c.Kind == "area")?.Name
                                     ?? string.Empty;
        public string GetStreet() => Components?.FirstOrDefault(c => c.Kind == "street")?.Name ?? string.Empty;
    }

    public class YandexAddressComponent
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }  // e.g., country, province, street, house

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class YandexPhone
    {
        [JsonPropertyName("formatted")]
        public string Formatted { get; set; }
    }

    public record SuggestRoot
    {
        [JsonPropertyName("results")]
        public List<SuggestItem> Results { get; set; } = [];
    }

    public record SuggestItem
    {
        [JsonPropertyName("title")]
        public SuggestText? Title { get; set; }

        [JsonPropertyName("subtitle")]
        public SuggestText? Subtitle { get; set; }
    }

    public record SuggestText
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
