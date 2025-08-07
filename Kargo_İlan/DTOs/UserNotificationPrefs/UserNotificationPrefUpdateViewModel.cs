using Kargo_İlan.DTOs.Freight;
using Kargo_İlan.Models;

namespace Kargo_İlan.DTOs.UserNotificationPrefs
{
    public class UserNotificationPrefUpdateViewModel
    {
        public UserNotificationPrefUpdateDto UserNotificationPref { get; set; }
        public List<Province> Provinces { get; set; }
        public Dictionary<int, List<DistrictDto>> Districts { get; set; }
    }
}
