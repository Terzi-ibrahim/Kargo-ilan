using Kargo_İlan.Models;

namespace Kargo_İlan.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUserNotificationsAsync(int userId);    
        Task<Notification?> MarkAsReadAsync(int notificationId, int userId);
        Task DeleteNotificationAsync(int notificationId, int userId);

    }
}
