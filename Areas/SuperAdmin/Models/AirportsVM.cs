namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class AirportsVM
    {
    }





    // Add Airports
    public class AddAirportRequest
    {
        public string AirportName { get; set; } = string.Empty;

        public string IataCode { get; set; } = string.Empty;

        public string? IcaoCode { get; set; }

        public int CountryId { get; set; }

        public int? ProvinceId { get; set; }

        public int? CityId { get; set; }

        public int IsInternational { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
