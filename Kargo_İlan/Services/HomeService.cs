using Kargo_İlan.Data;
using Kargo_İlan.DTOs.Freight;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Services
{
    public class HomeService : IHomeService
    {
        private readonly KargoDbContext _context;

        public HomeService(KargoDbContext context)
        {
            _context = context;

        }
        public async Task<List<CategoryType>> GetAllCategoriesAsync()
        {
            return await _context.CategoryType
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }
        public async Task<List<FreightIndexDto>> GetLastSixFreightsAsync()
        {
            var freights = await _context.Freight
                .Include(x => x.Category_Type)
                .Include(x => x.Yukleme_il)
                .Include(x => x.Yukleme_ilce)
                .Include(x => x.Varis_il)
                .Include(x => x.Varis_ilce)
                .Include(x => x.Vehicle_Type)
                .Include(x => x.Cargo_Type)
                .OrderByDescending(x => x.CreateAt)
                .Take(6)
                .Select(x => new FreightIndexDto
                {
                    Freight_id = x.Freight_id,
                    Title = x.Title,
                    Miktar = x.Miktar,
                    OlusturulmaTarihi = x.CreateAt,
                    YuklemeIl = new ProvinceDto
                    {
                        Province_id = x.Yukleme_il.Province_id,
                        ProvinceName = x.Yukleme_il.ProvinceName
                    },
                    Yuklemeilce = new DistrictsDto
                    {
                        District_id = x.Yukleme_ilce.District_id,
                        DistrictName = x.Yukleme_ilce.DistrictName
                    },
                    VarisIl = new ProvinceDto
                    {
                        Province_id = x.Varis_il.Province_id,
                        ProvinceName = x.Varis_il.ProvinceName
                    },
                    Varisilce = new DistrictsDto
                    {
                        District_id = x.Varis_ilce.District_id,
                        DistrictName = x.Varis_ilce.DistrictName
                    },
                    VehicleType = new VehicleTypeDto
                    {
                        VehicleType_id = x.Vehicle_Type.VehicleType_id,
                        VehicleName = x.Vehicle_Type.VehicleName
                    },
                    CategoryType = new CategoryTypeDto
                    {
                        Category_id = x.Category_Type.Category_id,
                        CategoryName = x.Category_Type.CategoryName
                    },
                    CargoType = new CargoTypeDto
                    {
                        CargoType_id = x.Cargo_Type.Cargo_id,
                        CargoName = x.Cargo_Type.CargoName
                    }
                }).ToListAsync();

            return freights;
        }







    }
}
