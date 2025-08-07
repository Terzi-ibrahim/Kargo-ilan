using Kargo_İlan.Models;

namespace Kargo_İlan.DTOs.Freight
{
    public class FreightCreateViewDto
    {        
        public FreightCreate Freight { get; set; } // İlan verisi
        public List<CargoType> CargoTypes { get; set; } // Kargo tipleri
        public List<CategoryType> CategoryTypes { get; set; } // Kategori tipleri
        public List<VehicleType> VehicleTypes { get; set; } // Araç tipleri
        public List<Province> Provinces { get; set; } // İller
        public Dictionary<int, List<DistrictDto>> Districts { get; set; } // İllere ait ilçeler


    }
    
}
