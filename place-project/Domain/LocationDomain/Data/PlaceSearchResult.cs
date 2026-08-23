using System.ComponentModel.DataAnnotations;

namespace place_project.Domain.LocationDomain.Data
{
    public class PlaceSearchResult
    {
        [Key]
        public int Id { get; set; }
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string CommentMsg { get; set; } = "";
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public double OverallScore { get; set; }

        public string NormalizePhoneNumber(string phoneNumber)
        {
            PhoneNumber = new string((phoneNumber ?? string.Empty).Where(char.IsDigit).ToArray());
            return PhoneNumber;
        }
    }
}
