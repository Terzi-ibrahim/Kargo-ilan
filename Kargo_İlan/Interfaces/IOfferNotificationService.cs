using Kargo_İlan.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Kargo_İlan.Interfaces
{
    public interface IOfferNotificationService
    {

        Task SendOfferCreatedNotificationAsync(Offer offer, string companyName);
        Task SendOfferUpdatedNotificationAsync(Offer offer, string companyName);
        Task SendOfferDeletedNotificationAsync(Offer offer, string companyName);
        Task SendOfferApprovalNotificationAsync(int approverUserId, Offer offer);
        Task SendOfferConfirmationNotificationsAsync(Offer offer, int userid, ITempDataDictionary tempData);       
 
    }

}
