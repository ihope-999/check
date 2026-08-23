using Microsoft.EntityFrameworkCore;
using place_project.Domain.LocationDomain.Data;

namespace place_project.Domain.LocationDomain.Database
{
    public class LocationDBContext : DbContext
    {
        public LocationDBContext(DbContextOptions<LocationDBContext> options)
        : base(options)
        {
        }
        public DbSet<PlaceSearchResult> Locations { get; set; }
    }
}
