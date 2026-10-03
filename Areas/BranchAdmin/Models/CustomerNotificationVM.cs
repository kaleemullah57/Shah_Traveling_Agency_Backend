namespace Shah_Traveling_Agency_API.Areas.BranchAdmin.Models
{
    public class CustomerNotificationVM
    {
    }


    public class CustomerNotification
    {
        public int NotificationId { get; set; }
        public int CustomerId { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public long? BookingId { get; set; }
        public long? BookingPassengerId { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ReadDate { get; set; }
    }



    // Read Notification 
    public class MarkNotificationReadRequest
    {
        public int NotificationId { get; set; }
    }
}
