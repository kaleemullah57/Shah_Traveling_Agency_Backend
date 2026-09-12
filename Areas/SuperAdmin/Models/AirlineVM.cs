namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class AirlineVM
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    // Get Airlines
    public class AirlineModel
    {
        public int AirlineId { get; set; }

        public string AirlineName { get; set; } = string.Empty;

        public string AirlineCode { get; set; } = string.Empty;

        public string? IATACode { get; set; }

        public string? ICAOCode { get; set; }

        public string? LogoPath { get; set; }

        public bool IsActive { get; set; }

        public int? CountryId { get; set; }

        public string? CountryName { get; set; }

        public int CreatedById { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
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





    // Edit Airline
    public class EditAirlineRequest
    {
        public int AirlineId { get; set; }

        public string AirlineName { get; set; } = string.Empty;

        public string AirlineCode { get; set; } = string.Empty;

        public string IATACode { get; set; } = string.Empty;

        public string ICAOCode { get; set; } = string.Empty;

        public int CountryId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
