using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Kargo_İlan.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace Kargo_İlan.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
       

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        private int GetUserId()
        {
            return Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);


            var notificationDTOs = notifications
                .Where(n => n.UserNotificationPrefId == null)
                .Select(n => new Kargo_İlan.DTOs.Notification.NotificationDto
                {
                    NotificationId = n.Notification_id,
                    UserId = n.User_id,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedDate = n.CreatedAt,
                    OfferId = n.Offer_id,
                    FreightId = n.FreightId
                }).ToList();

            return View(notificationDTOs);
        }



        public async Task<IActionResult> ViewNotification(int notificationId)
        {
            int userId = GetUserId();
            var notification = await _notificationService.MarkAsReadAsync(notificationId, userId);

            if (notification != null)
            {
                TempData["Error"] = "Bildirim bulunamadı.";
                if (notification.Offer_id != null)
                    return RedirectToAction("Details", "Offer", new { id = notification.Offer_id, from = "Notification" });

                if (notification.FreightId != null)
                    return RedirectToAction("Details", "Freight", new { id = notification.FreightId, from = "Notification" });
            }

            TempData["Error"] = "Bildirim içeriği bulunamadı.";
            return RedirectToAction("Index");
        }

      


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int notificationId)
        {
            int userId = GetUserId();
            await _notificationService.DeleteNotificationAsync(notificationId, userId);

            TempData["Success"] = "Bildirim silindi.";
            return RedirectToAction("Index");
        }




    }
}
