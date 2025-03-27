namespace Kargo_İlan.Models
{
    public class CargoType
    {

        public int CargoId { get; set; }

        public string CargoName { get; set; }

        public ICollection<Listing> Listings { get; set; }
    }
}
