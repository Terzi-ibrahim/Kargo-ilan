using Kargo_İlan.DTOs.Freight;
using Kargo_İlan.DTOs.Notification;
using Kargo_İlan.DTOs.UserNotificationPrefs;
using Kargo_İlan.Models;

namespace Kargo_İlan.Interfaces
{
    public interface IUserNotificationPrefsService
    {
        Task<IEnumerable<UserNotificationPrefs>> GetByUserIdAsync(int userId);
        Task<UserNotificationPrefCreateViewModel> GetCreateViewModelAsync();
        Task<UserNotificationPrefs?> GetByIdAsync(int id);
        Task ToggleActiveStatusAsync(int userId);
        Task<List<UserNotificationPrefDto>> GetUserNotificationPrefsDtoByUserIdAsync(int userId);
        Task CreateAsync(UserNotificationPrefCreateDto dto, int userId);
        Task SendNotificationAsync(int userId, Freight freight, int? userNotificationPrefId = null);
        Task<List<UserNotificationPrefs>> GetMatchingPreferencesAsync(int yuklemeIlId, int? yuklemeIlceId, int varisIlId, int? varisIlceId);
        Task<List<NotificationDto>> GetNotificationsByPreferenceAsync(UserNotificationPrefs pref);   
        Task<UserNotificationPrefUpdateViewModel> GetEditViewModelAsync(int id);
        Task<Dictionary<int, List<DistrictDto>>> GetDistrictsGroupedByProvinceAsync();        
        Task<bool> UpdateAsync(UserNotificationPrefUpdateDto dto, int updatedBy);
        Task<List<Province>> GetProvincesAsync();
        Task DeleteAsync(int prefId, int userId);
    }
}
