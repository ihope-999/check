using place_project.Domain.LocationDomain.Data;

namespace place_project.Domain.LocationDomain.Interfaces
{
    public interface IOpenStreetMapService
    {
        Task<PlaceSearchResult?> FindPlaceAsyncOSM(BusinessPlace place);
    }
}
