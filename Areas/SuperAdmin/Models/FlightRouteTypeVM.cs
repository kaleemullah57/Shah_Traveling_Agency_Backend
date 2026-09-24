namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class FlightRouteTypeVM
    {
    }



    public class FlightRouteTypeGetRequest
    {
        public string? Search { get; set; }

        public bool? IsActive { get; set; }

        public int? FlightRouteTypeId { get; set; }
    }
    public class FlightRouteType
    {
        public int FlightRouteTypeId { get; set; }

        public string RouteTypeName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public int? CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }


    public class FlightRouteTypeAddRequest
    {
        public string RouteTypeName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }
    }


  


    public class FlightRouteTypeUpdateRequest
    {
        public int FlightRouteTypeId { get; set; }

        public string RouteTypeName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
