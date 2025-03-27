namespace Kargo_İlan.Models
{
    public class User
    {
        public int UserId { get; set; }


        public int RoleId { get; set; }
        public Role Role { get; set; }



        public ICollection<Listing> Listings { get; set; } // One-to-Many (Bire-Çok) ilişki

    }
}
