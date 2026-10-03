namespace Shah_Traveling_Agency_API.Areas.PublicArea.Models
{
    public class CancelBookingPassengerVM
    {
    }

    public class CancelBookingPassengerRequest
    {
        public long BookingPassengerId { get; set; }
        public string? CancellationReason { get; set; }
    }

    public class CancelBookingPassengerModel
    {
        public long BookingId { get; set; }
        public long BookingPassengerId { get; set; }

        public int CustomerId { get; set; }

        public int PurchaseInvoiceItemId { get; set; }
        public int BookingStatusId { get; set; }
        public string? BookingStatus { get; set; }
        public int CancellationTypeId { get; set; }
        public string? CancellationTypeName { get; set; }
    }
}
