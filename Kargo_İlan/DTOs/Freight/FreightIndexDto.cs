namespace Kargo_İlan.DTOs.Freight
{
    public class FreightIndexDto
    {    
        public int Freight_id { get; set; }
        public string Title { get; set; }
        public decimal Miktar { get; set; } 
        public DateTime OlusturulmaTarihi { get; set; }
        public ProvinceDto YuklemeIl { get; set; } 
        public DistrictsDto Yuklemeilce { get; set; }
        public ProvinceDto VarisIl { get; set; }
        public DistrictsDto Varisilce { get; set; }
        public VehicleTypeDto VehicleType { get; set; }
        public CategoryTypeDto CategoryType { get; set; }
        public CargoTypeDto CargoType { get; set; }
        public string descreption { get; set; }

        public string OwnerFullName { get; set; }  
        public string? CompanyName { get; set; }    



    }

    public class ProvinceDto
    {
        public int Province_id { get; set; }
        public string ProvinceName { get; set; }
    }
    public class DistrictsDto
    {
        public int District_id { get; set; }
        public string  DistrictName { get; set; }
    }
    public class VehicleTypeDto
    {
        public int VehicleType_id { get; set; }
        public string VehicleName { get; set; }
    }

    public class CategoryTypeDto
    {
        public int Category_id { get; set; }
        public string CategoryName { get; set; }
    }
    public class CargoTypeDto
    {
        public int CargoType_id { get; set; }
        public string CargoName { get; set; }
    }
}
