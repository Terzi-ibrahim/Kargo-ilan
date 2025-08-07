using Kargo_İlan.Models;

namespace Kargo_İlan.DTOs.Freight
{
    public class ProvinceWithDistrictsDto
    {
        public Province Province { get; set; }
        public List<DistrictDto> Districts { get; set; }
    }

}
