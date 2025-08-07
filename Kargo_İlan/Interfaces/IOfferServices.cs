using Kargo_İlan.DTOs.Offer;
using Kargo_İlan.DTOs.Offers;
using Kargo_İlan.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Kargo_İlan.Interfaces
{
    public interface IOfferServices
    {
        // 1. Listeleme
        Task<List<Offers>> GetValidUserOffersAsync(int userId);

        // 2. Teklif Detayları
        Task<Offers> GetOfferDetailsAsync(int id);
        Task<Offer> GetOfferByIdAsync(int offerId);

        // 3. Oluşturma Akışı
        Task<bool> HasAcceptedOffer(int freightId);
        Task<Create> Create(int freightId);
        Task<Offer> CreateOfferAsync(Create offerDTO, int userId);

        // 4. Güncelleme Akışı
        Task<UpdateView> GetOfferUpdateDetailsAsync(int offerId);
        Task<Offer> UpdateOfferAsync(UpdateView offerDTO, User user);

        // 5. Silme
        Task DeleteOfferAsync(int offerId, int userId);

        // 6. Onay / Reddetme
        Task<bool> ConfirmOfferAsync(int offerId, int userId);
        Task<bool> RejectOfferAsync(int offerId, int userId);

        // 7. Yardımcı Metotlar
        Task<Freight> GetFreightByIdAsync(int freightId);
        void SetFreightDetails(ViewDataDictionary viewData, Freight freight);
        Task<bool> IsUserCompanyAsync(int userId);
        Task<string> GetCompanyNameAsync(int userId);

        Task<int?> GetCompanyIdForUserAsync(int userId);
    }
}
