using Kargo_İlan.DTOs.Notification;

namespace Kargo_İlan.DTOs.UserNotificationPrefs
{
    public class UserNotificationPrefDto
    {
        public int Id { get; set; }

        public int YuklemeilId { get; set; }
        public string YuklemeIlName { get; set; }

        public int? YuklemeilceId { get; set; }
        public string YuklemeIlceName { get; set; }

        public int VarisilId { get; set; }
        public string VarisIlName { get; set; }

        public int? VarisilceId { get; set; }
        public string VarisIlceName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<NotificationDto> Notifications { get; set; } = new();
    }   

}
