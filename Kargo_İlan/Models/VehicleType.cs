namespace Kargo_İlan.Models
{
    public class VehicleType
    {
        public int VehicleTypeId { get; set; }
        public String VehicleName { get; set; }

        public ICollection<Listing> Listings { get; set; }  // One-to-Many (Bire-Çok) ilişki
    }
}
