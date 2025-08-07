using Kargo_İlan.Data;
using Kargo_İlan.DTOs.Offer;
using Kargo_İlan.DTOs.Offers;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Services
{
    public class OfferServices : IOfferServices
    {
        private readonly KargoDbContext _context;
        private readonly IOfferNotificationService _notificationService;

        // Status sabitleri
        private const int PendingStatus = 1;
        private const int ConfirmedStatus = 2;
        private const int RejectedStatus = 3;

        public OfferServices(KargoDbContext context, IOfferNotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // 1. Listeleme
        public async Task<List<Offers>> GetValidUserOffersAsync(int userId)
        {
            var offers = await _context.Offer
                .Include(o => o.Freight)
                .Include(o => o.User).ThenInclude(u => u.Person)
                .Include(o => o.Status)
                // ➡️ hem kullanıcı ilişkili teklifleri al, hem de IsDeleted filtresi uygula
                .Where(o => !o.IsDeleted
                         && (o.User_id == userId
                             || o.Freight.User_id == userId))
                .ToListAsync();

            return offers.Select(o => new Offers
            {
                Offer_id = o.Offer_id,
                Freight_id = o.Freight_id,
                FreightTitle = o.Freight.Title,
                Price = o.Price,
                Description = o.Description,
                CreateAt = o.CreateAt,
                ExpiryDate = o.ExpiryDate,
                RemainingTime = GetRemainingTime(o.ExpiryDate),
                User_id = o.User_id,
                FullName = o.User.UserName,
                Name = o.User.Person.Name,
                SurName = o.User.Person.SurName,
                Email = o.User.Person.Email,
                PhoneNumber = o.User.Person.PhoneNumber,
                FreightOwnerId = o.Freight.User_id,
                StatusId = o.StatusId,
                StatusName = o.Status.Name,
                IsDeleted = o.IsDeleted        // ➡️ DTO’ya da bu bilgiyi al
            }).ToList();
        }


        // 2. Oluşturma akışı
        public async Task<bool> HasAcceptedOffer(int freightId)
        {
            return await _context.Offer
                .AnyAsync(o => o.Freight_id == freightId && o.StatusId == ConfirmedStatus && !o.IsDeleted);
        }

        public async Task<Create> Create(int freightId)
        {
            var freight = await _context.Freight
                .FirstOrDefaultAsync(f => f.Freight_id == freightId && !f.IsDeleted);
            if (freight == null)
                throw new InvalidOperationException("İlan bulunamadı.");

            return new Create { Freight_id = freightId };
        }

        public async Task<Offer> CreateOfferAsync(Create dto, int userId)
        {
            var freight = await _context.Freight
                .FirstOrDefaultAsync(f => f.Freight_id == dto.Freight_id && !f.IsDeleted);
            if (freight == null)
                throw new InvalidOperationException("İlan bulunamadı.");
            if (freight.User_id == userId)
                throw new InvalidOperationException("Kendi ilanınıza teklif veremezsiniz.");
            if (dto.Price <= 0)
                throw new InvalidOperationException("Teklif fiyatı sıfırdan büyük olmalıdır.");

            bool hasConfirmed = await _context.Offer
                .AnyAsync(o => o.Freight_id == dto.Freight_id && o.StatusId == ConfirmedStatus);
            if (hasConfirmed)
                throw new InvalidOperationException("Bu ilana zaten bir teklif kabul edilmiş.");

            bool hasOffered = await _context.Offer
                .AnyAsync(o => o.Freight_id == dto.Freight_id && o.User_id == userId && o.StatusId == PendingStatus);
            if (hasOffered)
                throw new InvalidOperationException("Bu ilana zaten teklif verdiniz.");

            var offer = new Offer
            {
                Freight_id = dto.Freight_id,
                User_id = userId,
                Price = dto.Price,
                Description = dto.Description,
                CreateAt = DateTime.UtcNow,
                CreatedBy = userId,
                StatusId = PendingStatus
            };

            _context.Offer.Add(offer);
            await _context.SaveChangesAsync();

            var companyName = await GetCompanyNameAsync(userId);
            await _notificationService.SendOfferCreatedNotificationAsync(offer, companyName);

            return offer;
        }

        // 3. Detay / Güncelleme akışı
        public async Task<Offers> GetOfferDetailsAsync(int id)
        {
            var o = await _context.Offer
                .IgnoreQueryFilters()
                .Include(x => x.Freight)
                .Include(x => x.User).ThenInclude(u => u.Person)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Offer_id == id);
            if (o == null) return null;

            return new Offers
            {
                Offer_id = o.Offer_id,
                Price = o.Price,
                Description = o.Description,
                CreateAt = o.CreateAt,
                ExpiryDate = o.ExpiryDate,
                Freight_id = o.Freight_id,
                FreightTitle = o.Freight.Title,
                User_id = o.User_id,
                FullName = o.User.UserName,
                Name = o.User.Person.Name,
                SurName = o.User.Person.SurName,
                Email = o.User.Person.Email,
                PhoneNumber = o.User.Person.PhoneNumber,
                FreightOwnerId = o.Freight.User_id,
                StatusId = o.StatusId,
                StatusName = o.Status.Name
            };
        }
        public async Task<UpdateView> GetOfferUpdateDetailsAsync(int offerId)
        {
            var o = await _context.Offer
                .Include(x => x.Freight).ThenInclude(f => f.Yukleme_il)
                .Include(x => x.Freight).ThenInclude(f => f.Varis_il)
                .Include(x => x.Freight).ThenInclude(f => f.Cargo_Type)
                .FirstOrDefaultAsync(x => x.Offer_id == offerId && !x.IsDeleted);
            if (o == null) return null;

            return new UpdateView
            {
                User_id = o.User_id,
                OfferId = o.Offer_id,
                Price = o.Price,
                Description = o.Description,
                CreateAt = o.CreateAt,
                Freight_id = o.Freight_id,
                FreightTitle = o.Freight.Title,
                AlisYuk = o.Freight.Yukleme_il.ProvinceName,
                VarisYuk = o.Freight.Varis_il.ProvinceName,
                CargoTur = o.Freight.Cargo_Type.CargoName,
                Miktari = o.Freight.Miktar
            };
        }

        public async Task<Offer> UpdateOfferAsync(UpdateView dto, User user)
        {
            var o = await _context.Offer
                .FirstOrDefaultAsync(x => x.Offer_id == dto.OfferId && x.User_id == user.User_id && !x.IsDeleted);
            if (o == null)
                throw new InvalidOperationException("Teklif bulunamadı.");

            o.Price = dto.Price;
            o.Description = dto.Description;
            o.ExpiryDate = DateTime.UtcNow.AddDays(2);
            o.UpdateAt = DateTime.UtcNow;
            o.UpdatedBy = user.User_id;

            _context.Offer.Update(o);
            await _context.SaveChangesAsync();

            var comp = await GetCompanyNameAsync(user.User_id);
            await _notificationService.SendOfferUpdatedNotificationAsync(o, comp);

            return o;
        }


      
        // 4. Silme
        public async Task DeleteOfferAsync(int offerId, int userId)
        {
            var o = await _context.Offer
                .IgnoreQueryFilters()    // soft-deleted’lar da dahil
                .FirstOrDefaultAsync(x => x.Offer_id == offerId);
            if (o == null || o.IsDeleted)
                throw new InvalidOperationException("Teklif bulunamadı veya zaten silinmiş.");

            if (o.User_id != userId)
                throw new UnauthorizedAccessException("Bu teklifi silme yetkiniz yok.");

            o.IsDeleted = true;
            o.UpdateAt = DateTime.UtcNow;
            o.UpdatedBy = userId;

            _context.Offer.Update(o);
            await _context.SaveChangesAsync();
        }


        // 5. Onay / Reddetme
        public async Task<bool> ConfirmOfferAsync(int offerId, int userId)
        {
            var o = await _context.Offer.FindAsync(offerId);
            if (o == null || o.StatusId != PendingStatus)
                return false;

            var freight = await _context.Freight.FindAsync(o.Freight_id);
            if (freight?.User_id != userId)
                return false;

            o.StatusId = ConfirmedStatus;
            o.ConfirmDate = DateTime.UtcNow;
            o.UpdateAt = DateTime.UtcNow;
            o.UpdatedBy = userId;

            var others = await _context.Offer
                .Where(x => x.Freight_id == o.Freight_id && x.Offer_id != offerId)
                .ToListAsync();
            others.ForEach(x => x.StatusId = RejectedStatus);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectOfferAsync(int offerId, int userId)
        {
            var o = await _context.Offer.FindAsync(offerId);
            if (o == null || o.StatusId != PendingStatus)
                return false;

            var freight = await _context.Freight.FindAsync(o.Freight_id);
            if (freight?.User_id != userId)
                return false;

            o.StatusId = RejectedStatus;
            o.UpdateAt = DateTime.UtcNow;
            o.UpdatedBy = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        // 6. Yardımcılar
        public async Task<Offer> GetOfferByIdAsync(int offerId)
            => await _context.Offer.FindAsync(offerId);

        public async Task<Freight> GetFreightByIdAsync(int freightId)
            => await _context.Freight
                             .Include(f => f.Cargo_Type)
                             .Include(f => f.Vehicle_Type)
                             .Include(f => f.Yukleme_il)
                             .Include(f => f.Varis_il)
                             .FirstOrDefaultAsync(f => f.Freight_id == freightId && !f.IsDeleted);

        public void SetFreightDetails(ViewDataDictionary viewData, Freight freight)
        {
            viewData["Freight_id"] = freight.Freight_id;
            viewData["FreightTitle"] = freight.Title ?? "Bilinmiyor";
            viewData["FreightDescription"] = freight.Description ?? "Bilinmiyor";
            viewData["FreightCategory"] = freight.Cargo_Type?.CargoName ?? "Bilinmiyor";
            viewData["FreightCargoType"] = freight.Category_Type?.CategoryName ?? "Bilinmiyor";
            viewData["FreightVehicleType"] = freight.Vehicle_Type?.VehicleName ?? "Bilinmiyor";
            viewData["FreightLoadLocation"] = freight.Yukleme_il?.ProvinceName ?? "Bilinmiyor";
            viewData["FreightDestination"] = freight.Varis_il?.ProvinceName ?? "Bilinmiyor";
        }

        public async Task<bool> IsUserCompanyAsync(int userId)
        {
            return await _context.UserRole
                .Include(ur => ur.Role)
                .AnyAsync(ur => ur.User_id == userId && ur.Role.RoleName == "Company");
        }

        public async Task<string> GetCompanyNameAsync(int userId)
        {
            var user = await _context.User
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.User_id == userId);
            if (user == null)
                return "Bilinmiyor";

            bool isCompany = await IsUserCompanyAsync(userId);
            if (!isCompany)
                return user.UserName;

            var uc = await _context.UserCompany
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.User_id == userId);
            return uc?.Company?.CompanyName ?? user.UserName;
        }

        private string GetRemainingTime(DateTime expiryDate)
        {
            var span = expiryDate - DateTime.UtcNow;
            if (span.TotalSeconds < 0) return "Süresi doldu";
            if (span.TotalDays >= 1) return $"{(int)span.TotalDays} gün {span.Hours} saat kaldı";
            if (span.TotalHours >= 1) return $"{(int)span.TotalHours} saat {span.Minutes} dakika kaldı";
            return $"{span.Minutes} dakika {span.Seconds} saniye kaldı";
        }

        public async Task<int?> GetCompanyIdForUserAsync(int userId)
        {
            var uc = await _context.UserCompany
                .FirstOrDefaultAsync(x => x.User_id == userId);
            if (uc == null) return null;
            return uc.Company_id; 
        }


    }
}
