using Kargo_İlan.Data;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Services
{
    public class OfferNotificationService : IOfferNotificationService
    {
        private readonly KargoDbContext _context;

        public OfferNotificationService(KargoDbContext context)
        {
            _context = context;
        }


        public async Task SendOfferCreatedNotificationAsync(Offer offer, string companyName)
        {
        
            string freightNotification = $"{companyName} sizin ilanınıza teklif verdi.";

       
            string companyNotification = $"İlan numarası ({offer.Freight.Freight_id}) olan ilana teklif başarıyla oluşturuldu.";

           
            await CreateOfferNotificationAsync(offer.Freight.User_id, offer.Offer_id, freightNotification);
            await CreateOfferNotificationAsync(offer.User_id, offer.Offer_id, companyNotification);
        }

        public async Task SendOfferUpdatedNotificationAsync(Offer offer, string companyName)
        {
            string freightNotification = $"{companyName} şirketi, ilanınıza verdiği teklifi güncelledi.";
            string companyNotification = $"{offer.Offer_id} numaralı teklif başarıyla güncellendi.";

            await CreateOfferNotificationAsync(offer.Freight.User_id, offer.Offer_id, freightNotification);
            await CreateOfferNotificationAsync(offer.User_id, offer.Offer_id, companyNotification);
        }
        public async Task SendOfferDeletedNotificationAsync(Offer offer, string companyName)
        {
            string message = $"{offer.Offer_id} numaralı teklif {companyName} tarafından silindi.";
            await CreateOfferNotificationAsync(offer.Freight.User_id, offer.Offer_id, message);
        }
        public async Task SendOfferApprovalNotificationAsync(int approverUserId, Offer offer)
        {
            string confirmMessage = $"{offer.Offer_id} numaralı teklifi onayladınız.";
            string companyMessage = "Teklifiniz onaylandı. İlan sahibinin iletişim bilgilerine erişebilirsiniz.";

            await CreateOfferNotificationAsync(approverUserId, offer.Offer_id, confirmMessage);
            await CreateOfferNotificationAsync(offer.User_id, offer.Offer_id, companyMessage);
        }
        public async Task SendOfferConfirmationNotificationsAsync(Offer offer, int userid, ITempDataDictionary tempData)
        {
            if (userid==null)
            {
                tempData["Error"] = "Kullanıcı bilgisi alınamadı.";
                return;
            }

            var currentUser = await _context.User.FirstOrDefaultAsync(u => u.User_id == userid);
            if (currentUser == null)
            {
                tempData["Error"] = "Kullanıcı bulunamadı.";
                return;
            }

            await SendOfferApprovalNotificationAsync(currentUser.User_id, offer);
        }
        private async Task CreateOfferNotificationAsync(int userId, int offerId, string message)
        {
            var notification = new Notification
            {
                User_id = userId,
                Offer_id = offerId,                
                Message = message,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();
        }




    }
}
