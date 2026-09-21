namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class PassengerTypeVM
    {
    }





    // Add Passenger Types
    public class PassengerTypeModel
    {
        public int PassengerTypeId { get; set; }
        public string PassengerTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class AddPassengerTypeRequest
    {
        public string PassengerTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
