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






    // Get Airports
    public class AirportListRequest
    {
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }

    public class AirportModel
    {
        public int AirportId { get; set; }

        public string AirportName { get; set; } = string.Empty;

        public string IataCode { get; set; } = string.Empty;

        public string? IcaoCode { get; set; }

        public int CountryId { get; set; }

        public string? CountryName { get; set; }

        public int? ProvinceId { get; set; }

        public string? ProvinceName { get; set; }

        public int? CityId { get; set; }

        public string? CityName { get; set; }

        public bool IsInternational { get; set; }

        public bool IsActive { get; set; }

        public int CreatedById { get; set; }

        public string? UserName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
    public class AirportListResponse
    {
        public List<AirportModel> Data { get; set; } = new();

        public int TotalCount { get; set; }

        public int FilterCount { get; set; }
    }
}
