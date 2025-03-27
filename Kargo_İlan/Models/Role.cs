namespace Kargo_İlan.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public String RoleName { get; set; }

        public ICollection<User> Users { get; set; } // Bir rol birden fazla kullanıcıya sahip olabilir

    }
}
