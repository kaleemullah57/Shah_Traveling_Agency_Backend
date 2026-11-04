namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class UpdateTicketSellingPriceVM
    {
    }


    public class UpdateTicketSellingPriceRequest
    {
        public int PurchaseInvoiceItemId { get; set; }
        public decimal? SellingPrice { get; set; }
    }






    // Avable Tickets
    public class AvailableTicketsRequest
    {
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public decimal? FromSellingPrice { get; set; }
        public decimal? ToSellingPrice { get; set; }
    }
    public class AvailableTicketModel
    {
        public int PurchaseInvoiceItemId { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public string InvoiceNumber { get; set; }

        public DateTime? DepartureDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }

        public int Quantity { get; set; }
        public int? SharedQuantity { get; set; }
        public int? SoldQuantity { get; set; }
        public int? RemainingInventory { get; set; }
        public int? AvailableToCustomer { get; set; }
        public int? AvailableToShare { get; set; }

        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }

        public decimal? CheckedBaggageKg { get; set; }
        public decimal? HandBaggageKg { get; set; }
        public decimal? PersonalItemKg { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }

        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public string? AirlineName { get; set; }
        public string? AirlineCode { get; set; }

        public string? FromAirport { get; set; }
        public string? ToAirport { get; set; }

        public string? FromCountry { get; set; }
        public string? ToCountry { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }
        public string? PNRNo { get; set; }

        // Outbound flight stops
        public string? StopsJson { get; set; }
        public List<GetAvailablePurchaseInvoiceStopModel> Stops { get; set; } = new();

        // Return flight details JSON
        public string? ReturnAiroplanDetailJson { get; set; }

        // Deserialized return flight details
        public AvailableTicketReturnFlightModel? ReturnFlightDetails { get; set; }

        // Return flight stops JSON
        public string? ReturnStopsJSON { get; set; }

        // Deserialized return stops
        public List<AvailableTicketReturnStopModel> ReturnStops { get; set; } = new();
    }

    public class GetAvailablePurchaseInvoiceStopModel
    {
        public int StopNumber { get; set; }
        public string? StopAirport { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? DepartureDateTime { get; set; }
    }

    public class AvailableTicketReturnFlightModel
    {
        public int? ReturnAirlineId { get; set; }
        public string? ReturnAirline { get; set; }

        public int? ReturnFromAirportId { get; set; }
        public string? ReturnFromAirport { get; set; }

        public int? ReturnToAirportId { get; set; }
        public string? ReturnToAirport { get; set; }

        public DateTime? ReturnDepartureDateTime { get; set; }
        public DateTime? ReturnArrivalDateTime { get; set; }

        public int? ReturnFlightRouteTypeId { get; set; }
        public string? ReturnFlightRoute { get; set; }
    }

    public class AvailableTicketReturnStopModel
    {
        public int StopNumber { get; set; }
        public int PurchaseInvoiceItemId { get; set; }
        public int AirportId { get; set; }

        public string? AirportName { get; set; }

        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? DepartureDateTime { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
















    // Share Tickets
    public class ShareTicketRequest
    {
        public int PurchaseInvoiceItemId { get; set; }
        public int Quantity { get; set; }
    }






    // Reduce Shared Ticket Quantity
    public class ReduceSharedTicketQuantityRequest
    {
        public int PurchaseInvoiceItemId { get; set; }
        public int Quantity { get; set; }
    }

}
