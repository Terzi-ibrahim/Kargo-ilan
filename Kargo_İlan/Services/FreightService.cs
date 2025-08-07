
using Kargo_İlan.Data;
using Kargo_İlan.DTOs.Freight;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Services
{
    public class FreightService :IFreightService
    {

        private readonly KargoDbContext _context;
        private readonly IUserNotificationPrefsService _userNotificationPrefsService;

        public FreightService(KargoDbContext context, IUserNotificationPrefsService userNotificationPrefsService)
        {
            _context = context;
            _userNotificationPrefsService = userNotificationPrefsService;
        }

        public async Task<(List<int> UserOfferedFreightIds, List<int> AcceptedOfferFreightIds)> GetUserOfferStatusAsync(int userId)
        {
            var userOffers = await _context.Offer
                .Where(o => o.User_id == userId && !o.IsDeleted)
                .Select(o => o.Freight_id)
                .ToListAsync();

            var acceptedOffers = await _context.Offer
                .Where(o => o.ConfirmDate !=null && !o.IsDeleted)
                .Select(o => o.Freight_id)
                .ToListAsync();

            return (userOffers, acceptedOffers);
        }

        public async Task<List<FreightIndexDto>> GetAllFreightsAsync()
        {
            var listings = await _context.Freight
                .Where(l => !l.IsDeleted)
                .Include(l => l.Yukleme_il)
                .Include(l => l.Varis_il)
                .Include(l => l.Varis_ilce)
                .Include(l => l.Yukleme_ilce)
                .Include(l => l.Vehicle_Type)
                .Include(l => l.Cargo_Type)
                .Include(l => l.Category_Type)
                .ToListAsync();

            return listings.Select(l => new FreightIndexDto
            {
                Freight_id = l.Freight_id,
                Title = l.Title,
                Miktar = l.Miktar,
                OlusturulmaTarihi = l.CreateAt,
                YuklemeIl = new ProvinceDto
                {
                    Province_id = l.Yukleme_il.Province_id,
                    ProvinceName = l.Yukleme_il.ProvinceName
                },
                Yuklemeilce = new DistrictsDto
                {
                    District_id = l.Yukleme_ilce.District_id,
                    DistrictName = l.Yukleme_ilce.DistrictName
                },
                VarisIl = new ProvinceDto
                {
                    Province_id = l.Varis_il.Province_id,
                    ProvinceName = l.Varis_il.ProvinceName
                },
                Varisilce = new DistrictsDto
                {
                    District_id = l.Yukleme_ilce.District_id,
                    DistrictName = l.Yukleme_ilce.DistrictName
                },
                VehicleType = new VehicleTypeDto
                {
                    VehicleType_id = l.Vehicle_Type.VehicleType_id,
                    VehicleName = l.Vehicle_Type.VehicleName
                },
                CategoryType = new CategoryTypeDto
                {
                    Category_id = l.Category_Type.Category_id,
                    CategoryName = l.Category_Type.CategoryName
                },
                CargoType= new CargoTypeDto
                {
                    CargoType_id=l.Cargo_Type.Cargo_id,
                    CargoName=l.Cargo_Type.CargoName
                }


            }).ToList();
        }

        public async Task<List<FreightIndexDto>> GetFilteredFreightsAsync(
            string searchText,
            int? yukilId,
            int? yukilceId,
            int? varisilId,
            int? varisilceId,
            int? vehicleId,
            int? categoryId,
            int? cargoid)
        {
            var query = _context.Freight
                .Where(l => !l.IsDeleted)
                .Include(l => l.Yukleme_il)
                .Include(l => l.Yukleme_ilce)
                .Include(l => l.Varis_il)
                .Include(l=>l.Varis_ilce)
                .Include(l => l.Vehicle_Type)
                .Include(l => l.Category_Type)
                .Include(l=> l.Cargo_Type)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(l =>
                    l.Title.Contains(searchText) ||
                    l.Freight_id.ToString().Contains(searchText));
            }

            if (yukilId.HasValue)
            {
                query = query.Where(l => l.Yukleme_il.Province_id == yukilId.Value);
            }

            if (yukilceId.HasValue)
            {
                query = query.Where(l => l.Yukleme_ilce.District_id == yukilceId.Value);
            }
            if (varisilId.HasValue)
            {
                query = query.Where(l => l.Varis_il.Province_id == varisilId.Value);
            }

            if (varisilceId.HasValue)
            {
                query = query.Where(l => l.Varis_ilce.District_id == varisilceId.Value);
            }

            if (vehicleId.HasValue)
            {
                query = query.Where(l => l.VehicleType_id == vehicleId.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(l => l.Category_id == categoryId.Value);
            }
            if (cargoid.HasValue) {
                query = query.Where(l => l.Cargo_id == cargoid.Value);
            }

            var listings = await query.ToListAsync();

            return listings.Select(l => new FreightIndexDto
            {
                Freight_id = l.Freight_id,
                Title = l.Title,
                Miktar = l.Miktar,
                OlusturulmaTarihi = l.CreateAt,
                YuklemeIl = new ProvinceDto
                {
                    Province_id = l.Yukleme_il.Province_id,
                    ProvinceName = l.Yukleme_il.ProvinceName
                },
                Yuklemeilce =new DistrictsDto
                {
                    District_id = l.Yukleme_ilce.District_id,
                    DistrictName =l.Yukleme_ilce.DistrictName,
                },
                VarisIl = new ProvinceDto
                {
                    Province_id = l.Varis_il.Province_id,
                    ProvinceName = l.Varis_il.ProvinceName
                },
                Varisilce = new DistrictsDto
                {
                    District_id = l.Varis_ilce.District_id,
                    DistrictName = l.Varis_ilce.DistrictName
                },
                VehicleType = new VehicleTypeDto
                {
                    VehicleType_id = l.Vehicle_Type.VehicleType_id,
                    VehicleName = l.Vehicle_Type.VehicleName
                },
                CategoryType = new CategoryTypeDto
                {
                    Category_id = l.Category_Type.Category_id,
                    CategoryName = l.Category_Type.CategoryName
                },
                CargoType = new CargoTypeDto
                {
                    CargoType_id =l.Cargo_Type.Cargo_id,
                    CargoName = l.Cargo_Type.CargoName
                }
            }).ToList();
        }
        public async Task<FilterViewModel> GetFilterOptionsAsync()
        {
            // İller
            var provinces = await _context.Province.ToListAsync();

            // Araç Türleri
            var vehicles = await _context.VehicleType.ToListAsync();

            // Kategoriler
            var categories = await _context.CategoryType.ToListAsync();

            // Kargo Türleri
            var cargotype = await _context.CargoType.ToListAsync();

            // Tüm ilçeleri bir seferde al ve grupla
            var allDistricts = await _context.District
                .Select(d => new DistrictDto
                {
                    Id = d.District_id,
                    Name = d.DistrictName,
                    ProvinceId = d.Province_id
                })
                .ToListAsync();

            var districtsByProvince = allDistricts
                .GroupBy(d => d.ProvinceId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return new FilterViewModel
            {
                Provinces = provinces.Select(p => new ProvinceDto
                {
                    Province_id = p.Province_id,
                    ProvinceName = p.ProvinceName
                }).ToList(),

                Vehicles = vehicles.Select(v => new VehicleTypeDto
                {
                    VehicleType_id = v.VehicleType_id,
                    VehicleName = v.VehicleName
                }).ToList(),

                Categories = categories.Select(c => new CategoryTypeDto
                {
                    Category_id = c.Category_id,
                    CategoryName = c.CategoryName
                }).ToList(),

                CargoType = cargotype.Select(k => new CargoTypeDto
                {
                    CargoType_id = k.Cargo_id,
                    CargoName = k.CargoName
                }).ToList(),

                Districts = districtsByProvince
            };
        }


        public async Task<FreightCreateViewDto> GetFreightCreateViewModelAsync()
        {
            // Veritabanından gerekli verileri çekiyoruz
            var cargoTypes = await _context.CargoType.ToListAsync();
            var categoryTypes = await _context.CategoryType.ToListAsync();
            var vehicleTypes = await _context.VehicleType.ToListAsync();

            // Province modelini kullanarak il verilerini alıyoruz
            var provinces = await _context.Province.ToListAsync();

            // İlçeleri Province'a göre gruplayarak Dictionary yapısına alıyoruz        
            var provinceDistrictDictionary = new Dictionary<int, List<DistrictDto>>();
            int totalDistrictCount = 0;

            foreach (var province in provinces)
            {
                var districts = await _context.District
                    .Where(d => d.Province_id == province.Province_id)
                    .Select(d => new DistrictDto
                    {
                        Id = d.District_id,
                        Name = d.DistrictName,
                        ProvinceId = d.Province_id
                    })
                    .ToListAsync();

                totalDistrictCount += districts.Count;

                provinceDistrictDictionary.Add(province.Province_id, districts);
            }         

            // Veri kontrolleri
            if (cargoTypes == null)
                throw new Exception("Kargo türleri verisi bulunamadı.");
            if (categoryTypes == null)
                throw new Exception("Kategori türleri verisi bulunamadı.");
            if (vehicleTypes == null)
                throw new Exception("Araç türleri verisi bulunamadı.");
            if (provinces == null)
                throw new Exception("İl verisi bulunamadı.");

            var model = new FreightCreateViewDto
            {
                CargoTypes = cargoTypes,
                CategoryTypes = categoryTypes,
                VehicleTypes = vehicleTypes,
                Provinces = provinces,
                Districts = provinceDistrictDictionary,
                Freight = new FreightCreate()
            };

            return model;
        }

        public async Task CreateFreightAsync(FreightCreate model, int userId)
        {
            ArgumentNullException.ThrowIfNull(model);

            var freight = new Freight
            {
                Title = model.Title,
                Description = model.Description,
                Miktar = model.Miktar,
                Cargo_id = model.Cargo_id,
                Category_id = model.Category_id,
                VehicleType_id = model.VehicleType_id,
                Yuklemeil_id = model.YuklemeIlId,
                Yuklemeilce_id = model.YuklemeIlceId,
                Varisil_id = model.VarisIlId,
                Varisilce_id = model.VarisIlceId,
                User_id = userId,
                CreatedBy = userId,
                CreateAt = DateTime.Now,
                ListingStatus_id = 20 
            };

            _context.Freight.Add(freight);
            await _context.SaveChangesAsync();



            var matchedPrefs = await _userNotificationPrefsService.GetMatchingPreferencesAsync(
                freight.Yuklemeil_id,
                freight.Yuklemeilce_id,
                freight.Varisil_id,
                freight.Varisilce_id
                );
         
            if (matchedPrefs?.Any() == true)
            {
                foreach (var pref in matchedPrefs)
                {
                    await _userNotificationPrefsService.SendNotificationAsync(pref.User_id, freight, pref.UserNotificationPrefId);
                }
            }



        }
        public async Task<List<FreightIndexDto>> MyFreight(int user)
        {
            var userfreightListings = await _context.Freight
                .Where(l => l.User_id == user && !l.IsDeleted)
                
                .Select(l => new FreightIndexDto
                {
                    Freight_id = l.Freight_id,
                    Title = l.Title,
                    Miktar = l.Miktar,
                    OlusturulmaTarihi = l.CreateAt,
                    YuklemeIl = new ProvinceDto
                    {
                        Province_id = l.Yukleme_il.Province_id,
                        ProvinceName = l.Yukleme_il.ProvinceName
                    },
                    VarisIl = new ProvinceDto
                    {
                        Province_id = l.Varis_il.Province_id,
                        ProvinceName = l.Varis_il.ProvinceName
                    },
                    VehicleType = new VehicleTypeDto
                    {
                        VehicleType_id = l.Vehicle_Type.VehicleType_id,
                        VehicleName = l.Vehicle_Type.VehicleName
                    },
                    CategoryType = new CategoryTypeDto
                    {
                        Category_id = l.Category_Type.Category_id,
                        CategoryName = l.Category_Type.CategoryName
                    },
                    CargoType = new CargoTypeDto 
                    {
                        CargoType_id = l.Cargo_Type.Cargo_id,
                        CargoName = l.Cargo_Type.CargoName
                    }
                })
                .ToListAsync();

            return userfreightListings;
        }      

        public async Task<FreightCreateViewDto> GetFreightEditViewModelAsync()
        {
            // Veritabanı sorgularını burada servis içinde yapıyoruz.
            var cargoTypes = await _context.CargoType.ToListAsync();
            var categoryTypes = await _context.CategoryType.ToListAsync();
            var vehicleTypes = await _context.VehicleType.ToListAsync();
            var provinces = await _context.Province.ToListAsync();

            // District'leri her bir province için yüklemek
            var provinceDistrictDictionary = await _context.Province
                .Include(p => p.District) // İlçe bilgilerini de dahil ediyoruz
                .Select(p => new
                {
                    Province = p,
                    Districts = p.District.Select(d => new DistrictDto
                    {
                        Id = d.District_id,
                        Name = d.DistrictName,
                        ProvinceId = d.Province_id
                    }).ToList()
                })
                .ToDictionaryAsync(p => p.Province.Province_id, p => p.Districts);

            return new FreightCreateViewDto
            {
                CargoTypes = cargoTypes,
                CategoryTypes = categoryTypes,
                VehicleTypes = vehicleTypes,
                Provinces = provinces,
                Districts = provinceDistrictDictionary
            };
        }

        public async Task<Freight> GetFreightByIdAsync(int id)
        {
            var freight = await _context.Freight
                .Include(l => l.Yukleme_il)
                .Include(l => l.Varis_il)
                .FirstOrDefaultAsync(l => l.Freight_id == id && !l.IsDeleted);

            return freight;
        }
        public async Task<bool> EditFreightAsync(int id, FreightCreate model, int userId)
        {
            var freight = await _context.Freight
                .FirstOrDefaultAsync(l => l.Freight_id == id && !l.IsDeleted);

            if (freight == null)
            {
                return false; // İlan bulunamadı
            }

            // Modeldeki verileri Freight nesnesine aktar
            freight.Title = model.Title;
            freight.Description = model.Description;
            freight.Miktar = model.Miktar;
            freight.Cargo_id = model.Cargo_id;
            freight.Category_id = model.Category_id;
            freight.VehicleType_id = model.VehicleType_id;
            freight.Yuklemeil_id = model.YuklemeIlId;
            freight.Yuklemeilce_id = model.YuklemeIlceId;
            freight.Varisil_id = model.VarisIlId;
            freight.Varisilce_id = model.VarisIlceId;
            freight.UpdateAt = DateTime.Now;
            freight.UpdatedBy = userId;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                
                return false;
            }
        }
        public async Task<FreightIndexDto?> GetFreightDetailAsync(int id)
        {
            // Freight nesnesini al ve ilişkili verileri yükle
            var freight = await _context.Freight
                  .Include(l => l.Yukleme_il)
                  .Include(l => l.Yukleme_ilce)
                  .Include(l => l.Varis_il)
                  .Include(l => l.Varis_ilce)
                  .Include(l => l.Vehicle_Type)
                  .Include(l => l.Category_Type)   
                  .Include(l=> l.Cargo_Type)                 
                  .FirstOrDefaultAsync(l => l.Freight_id == id && !l.IsDeleted);

            if (freight == null)
                return null;

            // Kullanıcı bilgisi
            var user = await _context.User
                .Include(u => u.Person)
                .FirstOrDefaultAsync(u => u.User_id == freight.User_id);

            string ownerFullName = user?.Person != null
                ? $"{user.Person.Name} {user.Person.SurName}"
                : "Bilinmiyor";

            // Şirket bilgisi
            string? companyName = null;
            var userCompany = await _context.UserCompany
                .FirstOrDefaultAsync(uc => uc.User_id == freight.User_id && !uc.IsDeleted);

            if (userCompany != null)
            {
                var company = await _context.Company
                    .FirstOrDefaultAsync(c => c.Company_id == userCompany.Company_id);

                if (company != null)
                {
                    companyName = company.CompanyName;
                }
            }        

            // Freight nesnesini DTO'ya dönüştür
            return new FreightIndexDto
            {
                Freight_id = freight.Freight_id,
                Title = freight.Title,
                Miktar = freight.Miktar,
                descreption = freight.Description,
                OlusturulmaTarihi = freight.CreateAt,
                OwnerFullName = ownerFullName,
                CompanyName = companyName,
                
                YuklemeIl = new ProvinceDto
                {
                    Province_id = freight.Yukleme_il.Province_id,
                    ProvinceName = freight.Yukleme_il.ProvinceName
                },
                Yuklemeilce = new DistrictsDto
                {
                    District_id = freight.Yukleme_ilce.District_id,
                    DistrictName = freight.Yukleme_ilce.DistrictName
                },
                VarisIl = new ProvinceDto
                {
                    Province_id = freight.Varis_il.Province_id,
                    ProvinceName = freight.Varis_il.ProvinceName
                },
                Varisilce = new DistrictsDto
                {
                    District_id = freight.Varis_ilce.District_id,
                    DistrictName = freight.Varis_ilce.DistrictName
                },
                VehicleType = new VehicleTypeDto
                {
                    VehicleType_id = freight.Vehicle_Type.VehicleType_id,
                    VehicleName = freight.Vehicle_Type.VehicleName
                },
                CategoryType = new CategoryTypeDto
                {
                    Category_id = freight.Category_Type.Category_id,
                    CategoryName = freight.Category_Type.CategoryName
                },
                CargoType=new CargoTypeDto
                {
                    CargoName=freight.Cargo_Type.CargoName,
                    CargoType_id= freight.Cargo_id

                }
            };
        }

        public async Task<Freight?> GetFreightForDeleteAsync(int id, int userId)
        {
            return await _context.Freight
                .Include(l=>l.Vehicle_Type)
                .Include(l=>l.Category_Type)
                .Include(l=>l.Cargo_Type)
                .FirstOrDefaultAsync(l => l.Freight_id == id && l.User_id == userId && !l.IsDeleted);
        }

        public async Task<bool> SoftDeleteFreightAsync(int id, int userId)
        {
            var listing = await GetFreightForDeleteAsync(id, userId);
            if (listing == null) return false;

            listing.IsDeleted = true;
            listing.UpdateAt = DateTime.Now;
            listing.UpdatedBy = userId;

            _context.Freight.Update(listing);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
