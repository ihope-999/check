using place_project.Domain.LocationDomain.Data;

namespace place_project.Domain.LocationDomain.Interfaces
{
    public interface IWikiLocationService
    {
        Task<PlaceSearchResult?> FindPlaceAsync(BusinessPlace place);
    }
}
