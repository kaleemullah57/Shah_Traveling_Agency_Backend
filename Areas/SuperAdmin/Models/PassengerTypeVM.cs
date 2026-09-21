namespace Shah_Traveling_Agency_API.Areas.SuperAdmin.Models
{
    public class PassengerTypeVM
    {
    }



    // Get Passenger Types
    public class PassengerTypeModel
    {
        public int PassengerTypeId { get; set; }
        public string PassengerTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
    }





    // Add Passenger Types

    public class AddPassengerTypeRequest
    {
        public string PassengerTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }





    // Update Passenger Types
    public class UpdatePassengerTypeRequest
    {
        public int PassengerTypeId { get; set; }
        public string PassengerTypeName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
