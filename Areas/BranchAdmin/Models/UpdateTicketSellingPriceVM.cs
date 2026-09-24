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
        public string? StopsJson { get; set; }
        public List<GetAvailablePurchaseInvoiceStopModel> Stops { get; set; } = new();
    }


    public class GetAvailablePurchaseInvoiceStopModel
    {
        public int StopNumber { get; set; }
        public string? StopAirport { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? DepartureDateTime { get; set; }
    }

















    // Share Tickets
    public class ShareTicketRequest
    {
        public int PurchaseInvoiceItemId { get; set; }
        public int Quantity { get; set; }
    }
}
