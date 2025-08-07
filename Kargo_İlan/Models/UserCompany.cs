using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Models
{
    public class UserCompany
    {
        public int UserCompany_id { get; set; }
        public int User_id { get; set; }
        public int Company_id { get; set; }
      
        public DateTime CreateAt { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; } 

        public int? UpdatedBy { get; set; }

        // Soft delete alanı
        public bool IsDeleted { get; set; } = false; // Varsayılan olarak false (aktif)

        // Silinme tarihi (geri alma durumu)
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Company Company { get; set; }
    }
    public class UserCompanyConfiguration : IEntityTypeConfiguration<UserCompany>
    {
        public void Configure(EntityTypeBuilder<UserCompany> builder)
        {
            builder.ToTable("UserCompany");

            builder.HasKey(e => e.UserCompany_id);

            builder.Property(e => e.User_id)
                .IsRequired();

            builder.Property(e => e.Company_id)
                .IsRequired();

            builder.Property(e => e.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.UpdateAt)
                .IsRequired(false);

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            // Foreign Key ve ilişkiler
            builder.HasOne(e => e.User)
                .WithMany(u => u.UserCompanies)
                .HasForeignKey(e => e.User_id)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde ilişkili UserCompany'yi silme

            builder.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.Company_id)
                .OnDelete(DeleteBehavior.Restrict); // Şirket silindiğinde ilişkili UserCompany'yi silme

            builder.HasIndex(e => new { e.User_id, e.Company_id }).IsUnique().HasDatabaseName("UK_UserCompany_User_Company");
        }
    }

}
