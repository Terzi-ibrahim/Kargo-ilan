namespace Kargo_İlan.DTOs.UserNotificationPrefs
{
    public class UserNotificationPrefCreateDto
    {
        public int YuklemeIlId { get; set; }
        public int? YuklemeIlceId { get; set; } 
        public int VarisIlId { get; set; }
        public int? VarisIlceId { get; set; }  
        public bool IsActive { get; set; } = true;
    }
}
