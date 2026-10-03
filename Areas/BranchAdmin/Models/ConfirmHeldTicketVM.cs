namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class ConfirmHeldTicketVM
    {
    }


    public class ConfirmHeldTicketRequest
    {
        public int BookingPassengerId { get; set; }
    }

    public class ConfirmHeldTicketResponse
    {
        public int BookingPassengerId { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int BookingStatusId { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedById { get; set; }
        public string? ApprovedBy { get; set; }
    }



}
