using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Models
{
    public class Role
    {
        public int Role_id { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

    }
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role"); 

            builder.HasKey(e => e.Role_id); 

            builder.Property(e => e.RoleName)
                .IsRequired() 
                .HasMaxLength(50); 

            // Benzersizlik (unique) index tanımlamaları
            builder.HasIndex(e => e.RoleName).IsUnique().HasDatabaseName("UK_Role_RoleName");
        }
    }
}
