using place_project.Domain.LocationDomain.Data;

namespace place_project.Domain.LocationDomain.Interfaces
{
    public interface IYandexPlacesService
    {
        public Task<PlaceSearchResultYandex?> FindPlaceAsync(BusinessPlace place);
    }
}
