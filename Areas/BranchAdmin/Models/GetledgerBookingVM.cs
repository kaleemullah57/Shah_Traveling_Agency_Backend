using System.ComponentModel.DataAnnotations;

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
        public float AlreadyPaid { get; set; }
    }




















    // Add ledger Payments


    public class TicketPaymentAddModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int BookingId { get; set; }

        /// <summary>
        /// 1 = Single Passenger
        /// 2 = Equal Distribution
        /// 3 = Custom Distribution
        /// </summary>
        [Required]
        [Range(1, 3)]
        public int PaymentMethodId { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "9999999999999999")]
        public decimal TotalAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(100)]
        public string? PaymentReference { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        [Required]
        [MinLength(1)]
        public List<TicketPaymentAllocationModel> Allocations { get; set; } = new();
    }

    public class TicketPaymentAllocationModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int BookingPassengerId { get; set; }

        public decimal? AllocatedAmount { get; set; }
    }

    public sealed class TicketPaymentAddResponse
    {
        public long TicketPaymentId { get; set; }
        public int CustomerId { get; set; }
        public int BookingId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public sealed class TicketPaymentAddedEvent
    {
        public long TicketPaymentId { get; set; }
        public int CustomerId { get; set; }
        public int BookingId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public List<int> BookingPassengerIds { get; set; } = new();
    }




















    // Get full ledger
    public class LedgerFullDetailsRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int AmountStatus { get; set; } = 0;
    }

    public class LedgerFullDetailsModel
    {
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public int BookingId { get; set; }
        public string? BookingReference { get; set; }

        public int BookingPassengerId { get; set; }
        public string? PassengerName { get; set; }

        public decimal TicketPrice { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
        public string? PaymentStatus { get; set; }

        public int? TicketPaymentId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }

        public decimal? PaidAmount { get; set; }
        public DateTime? PaidDate { get; set; }
    }


}
