namespace Kargo_İlan.Models
{
    public class CategoryType
    {

        public int CategoryId { get; set; }
        public String CategoryName { get; set; }

        public ICollection<Listing> Listings { get; set; }  // One-to-Many (Bire-Çok) ilişki

    }
}
