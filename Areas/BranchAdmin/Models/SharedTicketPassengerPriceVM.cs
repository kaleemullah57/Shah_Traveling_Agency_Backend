namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class SharedTicketPassengerPriceVM
    {
    }



    public class AddSharedTicketPassengerPriceRequest
    {
        public int PurchaseInvoiceItemId { get; set; }
        public int PassengerTypeId { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateSharedTicketPassengerPriceRequest
    {
        public int SharedTicketPassengerPriceId { get; set; }
        public decimal Price { get; set; }
    }

    public class SharedTicketPassengerPrice
    {
        public int SharedTicketPassengerPriceId { get; set; }
        public int PurchaseInvoiceItemId { get; set; }
        public int PassengerTypeId { get; set; }
        public string? PassengerTypeName { get; set; }
        public decimal Price { get; set; }
        public int BranchId { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
