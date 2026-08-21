using System.ComponentModel.DataAnnotations;

namespace place_project.Domain.LocationDomain.Data
{
    public class BusinessPlace
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field must be entered")]
        public string? Name { get; init; }

        public string? PlaceId { get; init; }
        public string? Lat { get; set; } = "";
        public string? Lon { get; set; } = "";

        [Required(ErrorMessage = "Address must be entered")]
        public string Address { get; set; } = "";

        public string? Road { get; init; }
        public string? City { get; init; }
        public string? State { get; init; } = "";
        public string? PostalCode { get; init; }
        public string? Country { get; init; }

        public string? PhoneNumber { get; init; }
    }
}
