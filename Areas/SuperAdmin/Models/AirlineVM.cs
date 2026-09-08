namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class AirlineVM
    {
    }



    // Add Airlines
    public class AddAirlineRequest
    {
        public string AirlineName { get; set; } = string.Empty;

        public string AirlineCode { get; set; } = string.Empty;

        public string? IATACode { get; set; }

        public string? ICAOCode { get; set; }

        public int? CountryId { get; set; }

        public string? LogoPath { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
