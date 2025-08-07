using Kargo_İlan.Data;
using Kargo_İlan.DTOs.Freight;
using Kargo_İlan.DTOs.Notification;
using Kargo_İlan.DTOs.UserNotificationPrefs;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore;


namespace Kargo_İlan.Services
{
    public class UserNotificationPrefsService : IUserNotificationPrefsService
    {
        private readonly KargoDbContext _context;

        public UserNotificationPrefsService(KargoDbContext context )
        {
            _context = context;      
            
        }
        public async Task<UserNotificationPrefs?> GetByIdAsync(int id)
        {
            return await _context.UserNotificationPrefs
                .Include(p => p.Yukleme_il)
                .Include(p => p.Yukleme_ilce)
                .Include(p => p.Varis_il)
                .Include(p => p.Varis_ilce)
                .FirstOrDefaultAsync(p => p.UserNotificationPrefId == id && !p.IsDeleted);
        }

        public async Task<IEnumerable<UserNotificationPrefs>> GetByUserIdAsync(int userId)
        {
            var usrid = await _context.UserNotificationPrefs
                    .Where(p => p.User_id == userId && !p.IsDeleted)                    
                    .Include(p => p.Yukleme_il)
                    .Include(p => p.Yukleme_ilce)
                    .Include(p => p.Varis_il)
                    .Include(p => p.Varis_ilce)
                    .ToListAsync();

            return usrid;
        }
        public async Task<UserNotificationPrefCreateViewModel> GetCreateViewModelAsync()
        {
            var provinces = await _context.Province.ToListAsync();

            var districts = await _context.District
                .GroupBy(d => d.Province_id)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Select(d => new District
                    {
                        District_id = d.District_id,
                        DistrictName = d.DistrictName
                    }).ToList()
                );

            return new UserNotificationPrefCreateViewModel
            {
                Provinces = provinces,
                Districts = districts,
                UserNotificationPref = new UserNotificationPrefCreateDto() 
            };
        }

        public async Task ToggleActiveStatusAsync(int prefId)
        {
            var pref = await _context.UserNotificationPrefs
                .FirstOrDefaultAsync(p => p.UserNotificationPrefId == prefId && !p.IsDeleted);

            if (pref == null)
                throw new Exception("Kayıt bulunamadı.");

            pref.IsActive = !pref.IsActive;
            pref.UpdatedAt = DateTime.Now;

            _context.UserNotificationPrefs.Update(pref);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserNotificationPrefDto>> GetUserNotificationPrefsDtoByUserIdAsync(int userId)
        {
            var prefs = await _context.UserNotificationPrefs
                .Where(p => p.User_id == userId && !p.IsDeleted)
                .Include(p => p.Yukleme_il)
                .Include(p => p.Yukleme_ilce)
                .Include(p => p.Varis_il)
                .Include(p => p.Varis_ilce)
                .ToListAsync();

            var model = new List<UserNotificationPrefDto>();

            foreach (var p in prefs)
            {
                var notifications = await GetNotificationsByPreferenceAsync(p); 

                model.Add(new UserNotificationPrefDto
                {
                    Id = p.UserNotificationPrefId,
                    YuklemeilId = p.Yuklemeil_id,
                    YuklemeIlName = p.Yukleme_il?.ProvinceName,
                    YuklemeilceId = p.Yuklemeilce_id,
                    YuklemeIlceName = p.Yukleme_ilce?.DistrictName,
                    VarisilId = p.Varisil_id,
                    VarisIlName = p.Varis_il?.ProvinceName,
                    VarisilceId = p.Varisilce_id,
                    VarisIlceName = p.Varis_ilce?.DistrictName,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    Notifications = notifications 
                });
            }

            return model;
        }
        public async Task<List<NotificationDto>> GetNotificationsByPreferenceAsync(UserNotificationPrefs pref)
        {
            return await _context.Notification
                .Where(n => n.UserNotificationPrefId == pref.UserNotificationPrefId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    NotificationId = n.Notification_id,
                    UserId = n.User_id,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedDate = n.CreatedAt,
                    FreightId = n.FreightId,

                })
                .ToListAsync();
        }
        public async Task CreateAsync(UserNotificationPrefCreateDto dto, int userId)
        {
            var entity = new UserNotificationPrefs
            {
                User_id = userId,
                Yuklemeil_id = dto.YuklemeIlId,
                Yuklemeilce_id = dto.YuklemeIlceId ?? (int?)null,
                Varisil_id = dto.VarisIlId,
                Varisilce_id = dto.VarisIlceId ?? (int?)null,
                IsActive = dto.IsActive,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };


            _context.UserNotificationPrefs.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserNotificationPrefs>> GetMatchingPreferencesAsync(
            int yuklemeIlId, int? yuklemeIlceId, int varisIlId, int? varisIlceId)
        {
            return await _context.UserNotificationPrefs
                .Where(p => p.IsActive && !p.IsDeleted)
                .Where(p =>
                    p.Yuklemeil_id == yuklemeIlId &&
                    (!p.Yuklemeilce_id.HasValue || p.Yuklemeilce_id == yuklemeIlceId) &&
                    p.Varisil_id == varisIlId &&
                    (!p.Varisilce_id.HasValue || p.Varisilce_id == varisIlceId)
                )
                .ToListAsync();
        }
        public async Task SendNotificationAsync(int userId, Freight freight, int? userNotificationPrefId = null)
        {
            var yuklemeIl = await _context.Province.FindAsync(freight.Yuklemeil_id);
            var yuklemeIlce = await _context.District.FindAsync(freight.Yuklemeilce_id);
            var varisIl = await _context.Province.FindAsync(freight.Varisil_id);
            var varisIlce = await _context.District.FindAsync(freight.Varisilce_id);

            string yukleme = yuklemeIl?.ProvinceName ?? "Bilinmeyen İl";
            if (yuklemeIlce != null)
                yukleme += " / " + yuklemeIlce.DistrictName;

            string varis = varisIl?.ProvinceName ?? "Bilinmeyen İl";
            if (varisIlce != null)
                varis += " / " + varisIlce.DistrictName;

            var message = $"Yeni bir ilan yayınlandı: {freight.Title} - Yükleme: {yukleme} → Varış: {varis}";

            var notification = new Notification
            {
                User_id = userId,
                Message = message,
                CreatedAt = DateTime.Now,
                IsRead = false,
                UserNotificationPrefId = userNotificationPrefId ,
                FreightId = freight.Freight_id
            };

            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Province>> GetProvincesAsync()
        {
            return await _context.Province.ToListAsync();
        }

        public async Task<Dictionary<int, List<DistrictDto>>> GetDistrictsGroupedByProvinceAsync()
        {
            return await _context.Province
                .Include(p => p.District)
                .Select(p => new
                {
                    ProvinceId = p.Province_id,
                    Districts = p.District.Select(d => new DistrictDto
                    {
                        Id = d.District_id,
                        Name = d.DistrictName,
                        ProvinceId = d.Province_id
                    }).ToList()
                })
                .ToDictionaryAsync(p => p.ProvinceId, p => p.Districts);
        }

        public async Task<UserNotificationPrefUpdateViewModel> GetEditViewModelAsync(int id)
        {
            var entity = await _context.UserNotificationPrefs
                .Include(p => p.Yukleme_il)
                .Include(p => p.Yukleme_ilce)
                .Include(p => p.Varis_il)
                .Include(p => p.Varis_ilce)
                .FirstOrDefaultAsync(p => p.UserNotificationPrefId == id && !p.IsDeleted);

            if (entity == null) return null;

            var provinces = await GetProvincesAsync();
            var districts = await GetDistrictsGroupedByProvinceAsync();

            var vm = new UserNotificationPrefUpdateViewModel
            {
                UserNotificationPref = new UserNotificationPrefUpdateDto
                {
                    Id = entity.UserNotificationPrefId,
                    Yuklemeil_id = entity.Yuklemeil_id,
                    Yuklemeilce_id = entity.Yuklemeilce_id,
                    Varisil_id = entity.Varisil_id,
                    Varisilce_id = entity.Varisilce_id,
                    IsActive = entity.IsActive
                 
                },
                Provinces = provinces,
                Districts = districts
            };


            return vm;
        }
        public async Task<bool> UpdateAsync(UserNotificationPrefUpdateDto dto, int updatedBy)
        {
            var entity = await _context.UserNotificationPrefs.FindAsync(dto.Id);
            if (entity == null || entity.IsDeleted)
                return false;

            entity.Yuklemeil_id = dto.Yuklemeil_id;
            entity.Yuklemeilce_id = dto.Yuklemeilce_id;
            entity.Varisil_id = dto.Varisil_id;
            entity.Varisilce_id = dto.Varisilce_id;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(int prefId, int userId)
        {
            var pref = await _context.UserNotificationPrefs
                .FirstOrDefaultAsync(p => p.UserNotificationPrefId == prefId && !p.IsDeleted && p.User_id == userId);

            if (pref == null)
                throw new Exception("Bildirim tercihi bulunamadı.");

            pref.IsDeleted = true;
            pref.UpdatedAt = DateTime.UtcNow;
            pref.UpdatedBy = userId;

            _context.UserNotificationPrefs.Update(pref);
            await _context.SaveChangesAsync();
        }
    }
}
