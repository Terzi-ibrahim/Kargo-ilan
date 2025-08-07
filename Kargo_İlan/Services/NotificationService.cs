using Kargo_İlan.Data;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Services
{
    public class NotificationService : INotificationService
    {
        private readonly KargoDbContext _context;

        public NotificationService(KargoDbContext context)
        {
            _context = context;
        }


        public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
        {
            return await _context.Notification
                .Where(n => n.User_id == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }      
    
        public async Task<Notification?> MarkAsReadAsync(int notificationId, int userId)
        {
            var notification = await _context.Notification
                .FirstOrDefaultAsync(n => n.Notification_id == notificationId && n.User_id == userId);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return notification;
        }

        public async Task DeleteNotificationAsync(int notificationId, int userId)
        {
            var notification = await _context.Notification
                .FirstOrDefaultAsync(n => n.Notification_id == notificationId && n.User_id == userId);

            if (notification != null)
            {
                _context.Notification.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
