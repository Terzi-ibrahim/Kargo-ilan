using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Models
{
    public class UserRole
    {
        public int User_id { get; set; }
        public int Role_id { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");

            builder.HasKey(e => new { e.User_id, e.Role_id });

            builder.Property(e => e.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.UpdateAt)
                .IsRequired(false);

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            builder.HasOne(e => e.User)
                 .WithMany(u => u.UserRoles)
                 .HasForeignKey(e => e.User_id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.Role_id)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasIndex(e => new { e.User_id, e.Role_id }).IsUnique().HasDatabaseName("UK_UserRole_User_Role");
        }
    }


}
