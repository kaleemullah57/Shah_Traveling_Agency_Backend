namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class GetledgerBookingVM
    {
    }



    public class GetLedgerCustomerBookingsRequest
    {
        public string? Search { get; set; }
        public int CustomerId { get; set; }
    }
    public class LedgerCustomerBooking
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; } = string.Empty;
    }








    // Get Passenger Confirm Tickets
    public class GetLedgerConfirmedBookingPassengersRequest
    {
        public int CustomerId { get; set; }
        public int BookingId { get; set; }
    }
    public class LedgerConfirmedBookingPassenger
    {
        public int BookingPassengerId { get; set; }
        public int BookingId { get; set; }

        // Passenger
        public int PassengerTypeId { get; set; }
        public string PassengerTypeName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? ContactNumber { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportExpireDate { get; set; }
        public string? Email { get; set; }

        // Airline
        public int AirlineId { get; set; }
        public string Airline { get; set; } = string.Empty;

        // Route
        public int FromAirportId { get; set; }
        public string FromAirport { get; set; } = string.Empty;

        public int ToAirportId { get; set; }
        public string ToAirport { get; set; } = string.Empty;

        // Country
        public int CountryId { get; set; }
        public string Country { get; set; } = string.Empty;

        // Status
        public int BookingStatusId { get; set; }
        public float UnitPrice { get; set; }
    }
}
