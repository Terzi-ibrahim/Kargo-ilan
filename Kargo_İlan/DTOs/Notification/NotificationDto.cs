namespace Kargo_İlan.DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? OfferId { get; set; }
        public int? FreightId { get; set; }

        public string Type
        {
            get
            {
                if (OfferId.HasValue)
                    return "teklif";
                else if (FreightId.HasValue)
                    return "ilan";
                else
                    return "genel";
            }
        }
    }
}