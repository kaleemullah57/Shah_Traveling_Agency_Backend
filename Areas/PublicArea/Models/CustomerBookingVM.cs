namespace Shah_Traveling_Agency_API.Areas.PublicArea.Models
{
    public class CustomerBookingVM
    {
    }



    public class CustomerBookingModel
    {
        public long BookingId { get; set; }

        public string? BookingReference { get; set; }

        public int CustomerId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int PurchaseInvoiceItemId { get; set; }

        public int BookedTickets { get; set; }

        public int BookingStatusId { get; set; }

        public string? BookingStatus { get; set; }
        public string? PNRNo { get; set; }


        public List<CustomerBookingPassengerModel>       PassengerBookingDetails    { get; set; } = new();
    }


    public class CustomerBookingPassengerModel
    {
        public long BookingPassengerId { get; set; }

        public int PassengerTypeId { get; set; }

        public string? PassengerTypeName { get; set; }

        public string? PassengerName { get; set; }

        public string? PassportNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? ContactNumber { get; set; }

        public string? Email { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedDate { get; set; }


        // Passenger Status
        public int? PassengerBookingStatusId { get; set; }

        public string? PassengerBookingStatus { get; set; }


        // Hold
        public DateTime? HoldUntil { get; set; }


        // Approval
        public DateTime? ApprovedDate { get; set; }

        public int? ApprovedById { get; set; }

        public string? ApprovedBy { get; set; }


        // Cancellation
        public DateTime? CancelledDate { get; set; }

        public string? CancellationReason { get; set; }

        public int? CancelledByUserId { get; set; }

        public string? CancelledBy { get; set; }


        // Rejection
        public DateTime? RejectedDate { get; set; }

        public string? RejectionReason { get; set; }

        public int? RejectedById { get; set; }

        public string? RejectedBy { get; set; }


        // Cancellation Type
        public int? CancellationTypeId { get; set; }
        public string? CancellationTypeName { get; set; }

        public int? FlightRouteTypeId { get; set; }
        public string? FlightRouteType { get; set; }
        public int? FlightJourneyTypeId { get; set; }
        public string? FlightJourneyType { get; set; }


        public decimal? checkedBaggagekg { get; set; }
        public decimal? handBaggagekg { get; set; }
        public decimal? personalItemkg { get; set; }



        public int? FromAirportId { get; set; }
        public string? FromAirport { get; set; }
        public DateTime? DepartureDateTime { get; set; }
        public int? ToAirportId { get; set; }
        public string? ToAirport { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public int? airlineId { get; set; }
        public string? AirlineName { get; set; }


        public List<CustomerBookingStopModel> Stops { get; set; } = new();
    }


    public class CustomerBookingSearchRequest
    {
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }



    public class CustomerBookingDbModel
    {
        public long BookingId { get; set; }

        public string? BookingReference { get; set; }

        public int CustomerId { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int PurchaseInvoiceItemId { get; set; }

        public int BookedTickets { get; set; }

        public int BookingStatusId { get; set; }

        public string? BookingStatus { get; set; }
        public string? PNRNo { get; set; }

        public string? PassengerBookingDetails { get; set; }
    }


    public class CustomerBookingStopModel
    {
        public long PurchaseInvoiceItemStopId { get; set; }

        public int PurchaseInvoiceItemId { get; set; }

        public int StopNumber { get; set; }

        public int AirportId { get; set; }

        public string? StopAirport { get; set; }

        public DateTime? ArrivalDateTime { get; set; }

        public DateTime? DepartureDateTime { get; set; }
    }
}
