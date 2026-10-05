namespace Shah_Traveling_Agency_API.Areas.PublicArea.Models
{
    public class BookingPassengerVM
    {
    }

    public class BookingPassengerRequest
    {
        public int PassengerTypeId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;

        public string? PassportNumber { get; set; }
        public string PassportIssueDate { get; set; }
        public string PassportExpireDate { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? ContactNumber { get; set; }

        public string? Email { get; set; }
    }

    public class CreateBookingRequest
    {
        public int PurchaseInvoiceItemId { get; set; }

        public List<BookingPassengerRequest> Passengers { get; set; }
            = new List<BookingPassengerRequest>();
    }

    public class CreateBookingResponse
    {
        public long BookingId { get; set; }
        public long BookingPassengerId { get; set; }
        public int CustomerID { get; set; }

        public string BookingReference { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime HoldUntil { get; set; }
    }
}
