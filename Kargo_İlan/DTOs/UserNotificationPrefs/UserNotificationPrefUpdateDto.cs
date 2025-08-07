namespace Kargo_İlan.DTOs.UserNotificationPrefs
{
    public class UserNotificationPrefUpdateDto
    {
        public int Id { get; set; }   
        public int Yuklemeil_id { get; set; }
        public int? Yuklemeilce_id { get; set; }
        public int Varisil_id { get; set; }
        public int? Varisilce_id { get; set; }
        public bool IsActive { get; set; }
    }
}
