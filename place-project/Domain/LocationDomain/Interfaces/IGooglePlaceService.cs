using place_project.Domain.LocationDomain.Data;

namespace place_project.Domain.LocationDomain.Interfaces
{

    public interface IGooglePlacesService
    {
        Task<PlaceSearchResult?> FindPlaceAsync(BusinessPlace query);
    }
}
