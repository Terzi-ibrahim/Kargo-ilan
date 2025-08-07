namespace Kargo_İlan.DTOs.Freight
{
    public class FilterViewModel
    {
        public List<ProvinceDto> Provinces { get; set; }

        public List<VehicleTypeDto> Vehicles { get; set; }
        public List<CategoryTypeDto> Categories { get; set; }
        public List<CargoTypeDto> CargoType { get; set; }
        public Dictionary<int, List<DistrictDto>> Districts { get; set; }
    }
}
