using Kargo_İlan.Models;

namespace Kargo_İlan.DTOs.UserNotificationPrefs
{
    public class UserNotificationPrefCreateViewModel
    {
        public UserNotificationPrefCreateDto UserNotificationPref { get; set; }
        public List<Province> Provinces { get; set; }
        public Dictionary<int, List<District>> Districts { get; set; }
    }

}
