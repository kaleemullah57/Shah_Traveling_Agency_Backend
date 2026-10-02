namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class PendingHoldBookingVM
    {
        public int BookingId { get; set; }
        public string? BookingReference { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public int BookingStatusId { get; set; }
        public string? BookingStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CancelledOn { get; set; }

        public int PurchaseInvoiceItemId { get; set; }
        public string? AirlineName { get; set; }

        public DateTime? TicketValidUntil { get; set; }
        public string? PNRNUMBER { get; set; }

        public string? Passengers { get; set; }

        public int FromAirportId { get; set; }
        public string? FromAirport { get; set; }

        public int ToAirportId { get; set; }
        public string? ToAirport { get; set; }

        public string? Stops { get; set; }

        // Deserialized properties
        public List<PendingHoldBookingPassengerVM> PassengerList { get; set; }
            = new();

        public List<PendingHoldBookingStopVM> StopList { get; set; }
            = new();
    }


    public class PendingHoldBookingPassengerVM
    {
        public int BookingPassengerId { get; set; }
        public int BookingId { get; set; }
        public int PassengerTypeId { get; set; }
        public string? PassengerType { get; set; }
        public string? PassengerName { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? Email { get; set; }

        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedById { get; set; }
        public string? ApprovedBy { get; set; }

        public string? CancellationReason { get; set; }

        public DateTime? RejectedDate { get; set; }
        public int? RejectedById { get; set; }
        public string? RejectedBy { get; set; }

        public string? RejectionReason { get; set; }

        public int? BookingStatusId { get; set; }
        public string? BookingStatus { get; set; }

        public DateTime? HoldUntil { get; set; }

        public DateTime? CancelledDate { get; set; }

        public int? CancellationTypeId { get; set; }
        public string? CancellationType { get; set; }

        public int? CancelledByuserId { get; set; }
        public int? CancelledBy { get; set; }
    }


    public class PendingHoldBookingStopVM
    {
        public int PurchaseInvoiceItemStopId { get; set; }
        public int PurchaseInvoiceItemId { get; set; }
        public int StopNumber { get; set; }
        public int AirportId { get; set; }
        public string? StopAirport { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? DepartureDateTime { get; set; }
    }


    public class PendingHoldBookingRequestVM
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int BookingStatus { get; set; }
    }
}
