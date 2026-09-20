namespace Shah_Traveling_Agency_API.Areas.PublicArea.Models
{
    public class SharedTicketsVm
    {
    }



    // Get Shared Tickets
    public class SharedTicketsRequest
    {
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public decimal? FromSellingPrice { get; set; }

        public decimal? ToSellingPrice { get; set; }
    }
    public class SharedTicketModel
    {
        public int? PurchaseInvoiceItemId { get; set; }

        public string? AirlineName { get; set; }

        public string? AirlineCode { get; set; }

        public string? FromAirport { get; set; }

        public string? ToAirport { get; set; }

        public string? FromCountry { get; set; }

        public string? ToCountry { get; set; }

        public DateTime? DepartureDateTime { get; set; }

        public DateTime? ArrivalDateTime { get; set; }

        public int? AvailableQuantity { get; set; }

        public decimal? SellingPrice { get; set; }

        public decimal? CheckedBaggageKg { get; set; }

        public decimal? HandBaggageKg { get; set; }

        public decimal? PersonalItemKg { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidUntil { get; set; }

        public int? BranchId { get; set; }

        public string? BranchName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }
    }

}
