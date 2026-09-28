namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class ExpiredBookingModel
    {
        public long BookingId { get; set; }
        public int PurchaseInvoiceItemId { get; set; }
        public int Quantity { get; set; }
    }
}
