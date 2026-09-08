using place_project.Domain.LocationDomain.Data;
using place_project.Domain.LocationDomain.Interfaces;

namespace place_project.Domain.LocationDomain.Service
{
    public class GooglePlacesService : IGooglePlacesService
    {
        private readonly HttpClient _http;
        private readonly ILogger<GooglePlacesService> _logger;
        private readonly string? _apiKey;

        public GooglePlacesService(HttpClient http, IConfiguration configuration, ILogger<GooglePlacesService> logger)
        {
            _logger = logger;
            _http = http;
            _apiKey = configuration["Google:ApiKey"];

            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                _logger.LogWarning("Google API key is not configured. Google lookup will be disabled.");
            }
        }

        public async Task<PlaceSearchResult?> FindPlaceAsync(BusinessPlace place)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                _logger.LogWarning("Skipping Google lookup because API key is missing.");
                return null;
            }

            var input = $"{place.Name} {place.Address} {place.City} {place.State} {place.Country}".Trim();
            var url = $"https://maps.googleapis.com/maps/api/place/findplacefromtext/json" +
                      $"?input={Uri.EscapeDataString(input)}" +
                      $"&inputtype=textquery" +
                      $"&fields=place_id,name,formatted_address" +
                      $"&key={Uri.EscapeDataString(_apiKey)}";

            var response = await _http.GetFromJsonAsync<GoogleFindPlaceResponse>(url);
            var candidate = response?.Candidates?.FirstOrDefault();
            if (candidate is null)
            {
                _logger.LogWarning("Google FindPlace returned no candidates.");
                return null;
            }

            var detailsUrl = $"https://maps.googleapis.com/maps/api/place/details/json" +
                     $"?place_id={Uri.EscapeDataString(candidate.PlaceId)}" +
                     $"&fields=name,formatted_address,address_components,formatted_phone_number" +
                     $"&key={Uri.EscapeDataString(_apiKey)}";

            var detailsResponse = await _http.GetFromJsonAsync<GoogleFindDetailsResponse>(detailsUrl);
            var result = detailsResponse?.Result;
            if (result == null)
            {
                _logger.LogWarning("Google Place Details response was empty.");
                return null;
            }

            string? GetComponent(string type) =>
                result.AddressComponents?.FirstOrDefault(c => c.Types.Contains(type))?.LongName;

            var citationCheck = new CitationCheck
            {
                givenName = place.Name ?? string.Empty,
                givenAddress = place.Address ?? string.Empty,
                givenPhoneNumber = place.PhoneNumber ?? string.Empty,
                givenCity = place.City ?? string.Empty
            };

            citationCheck.CalculateNameScore(result.Name);
            citationCheck.CalculateAddressScore(result.FormattedAddress);
            citationCheck.CalculatePhoneNumberScore(result.FormattedPhoneNumber ?? string.Empty);
            citationCheck.CalculateCityScore(GetComponent("locality") ?? string.Empty);

            return new PlaceSearchResult
            {
                PlaceId = result.PlaceId,
                Name = result.Name,
                Address = result.FormattedAddress,
                PhoneNumber = result.FormattedPhoneNumber,
                City = GetComponent("locality") ?? string.Empty,
                Region = GetComponent("administrative_area_level_1") ?? string.Empty,
                PostalCode = GetComponent("postal_code") ?? string.Empty,
                Country = GetComponent("country") ?? string.Empty,
                OverallScore = citationCheck.CalculateOverallScore()
            };
        }
    }
}
