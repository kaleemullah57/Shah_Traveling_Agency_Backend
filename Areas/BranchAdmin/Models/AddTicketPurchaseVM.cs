namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class AddTicketPurchaseVM
    {
    }




    // Add Tickets To Inventory
    public class AddTicketPurchaseRequest
    {
        public string PurchasedFrom { get; set; } = string.Empty;
        public string? PurchaseReference { get; set; }
        public DateTime? InvoiceDate { get; set; }

        public int AirlineId { get; set; }
        public int FromAirportId { get; set; }
        public int ToAirportId { get; set; }

        public DateTime DepartureDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }

        public decimal? CheckedBaggageKg { get; set; }
        public decimal? HandBaggageKg { get; set; }
        public decimal? PersonalItemKg { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }

        public decimal PaidAmount { get; set; } = 0;

        public int? PaymentMethodId { get; set; }
        public string? PaymentReference { get; set; }
        public string? Remarks { get; set; }
    }

    public class AddTicketPurchaseResponse
    {
        public int PurchaseInvoiceId { get; set; }
        public int PurchaseInvoiceItemId { get; set; }
        public int InventoryId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public int PaymentStatusId { get; set; }
    }
}
