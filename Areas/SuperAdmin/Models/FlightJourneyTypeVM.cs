namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class FlightJourneyTypeVM
    {
    }




    public class FlightJourneyType
    {
        public int FlightJourneyTypeId { get; set; }
        public string JourneyTypeName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public int? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class FlightJourneyTypeGetRequest
    {
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
        public int? FlightJourneyTypeId { get; set; }
    }



    public class FlightJourneyTypeAddRequest
    {
        public string JourneyTypeName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class FlightJourneyTypeUpdateRequest
    {
        public int FlightJourneyTypeId { get; set; }
        public string JourneyTypeName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

}
