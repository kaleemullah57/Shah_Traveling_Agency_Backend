namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class PurchasedInvoiceVM
    {
    }





    public class PurchasedInvoiceSearchRequest
    {
        public string? Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class PurchaseInvoicePaymentHistoryModel
    {
        public decimal PaymentAmount { get; set; }

        public DateTime? PaymentDate { get; set; }

        public int PaymentMethodId { get; set; }

        public string? MethodName { get; set; }

        public string? PaymentReference { get; set; }

        public string? PaymentRemarks { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal RemainingAmount { get; set; }
    }
    public class PurchasedInvoiceModel
    {
        public int PurchaseInvoiceId { get; set; }

        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public string? InvoiceNumber { get; set; }
        public string? PurchasedFrom { get; set; }
        public string? PurchaseReference { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public int PaymentStatusId { get; set; }
        public string? PaymentStatus { get; set; }

        public int StatusId { get; set; }
        public string? InventoryStatus { get; set; }

        public int CreatedById { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }


        // Purchase Invoice Item

        public int AirlineId { get; set; }
        public string? AirlineName { get; set; }
        public string? AirlineCode { get; set; }

        public int FromAirportId { get; set; }
        public string? FromAirport { get; set; }

        public int ToAirportId { get; set; }
        public string? ToAirport { get; set; }

        public DateTime DepartureDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }

        public int TicketQuantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal? CheckedBaggageKg { get; set; }
        public decimal? HandBaggageKg { get; set; }
        public decimal? PersonalItemKg { get; set; }

        public DateTime? ValidFrom { get; set; }
        public string? ValidUntil { get; set; }
        public int? TicketTypeId { get; set; }
        public string? TicketTypeName { get; set; }

        public string? StopsJson { get; set; }

        public List<PurchaseInvoiceStopModel> Stops { get; set; } = new();
        // Payment History
        public string? PaymentHistoryJson { get; set; }

        public List<PurchaseInvoicePaymentHistoryModel> PaymentHistory { get; set; }
            = new();
    }

    public class PurchaseInvoiceStopModel
    {
        public int StopNumber { get; set; }
        public string? StopAirport { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? DepartureDateTime { get; set; }
    }














    // Update Payment Invoice
    public class UpdatePurchasedInvoicePaymentRequest
    {
        public int PurchaseInvoiceId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public string? PaymentReference { get; set; }
        public string? Remarks { get; set; }
    }

    public class PurchasedInvoicePaymentResponse
    {
        public int PurchaseInvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
