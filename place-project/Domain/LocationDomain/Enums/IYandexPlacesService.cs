using place_project.Domain.LocationDomain.Data;

namespace place_project.Domain.LocationDomain.Enums
{
    public interface IYandexPlacesService
    {
        public Task<PlaceSearchResultYandex?> FindPlaceAsync(BusinessPlace place);
    }
}
