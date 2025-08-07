using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.Models
{
    public class Company
    {
        public int Company_id { get; set; }

        [Required]  // CompanyName zorunlu
        public string CompanyName { get; set; }

        [Required]
        [Phone]  // Telefon numarası doğrulaması
        public string CompanyPhone { get; set; }

        [Required]
        [EmailAddress]  // E-posta formatı doğrulaması
        public string CompanyEmail { get; set; }

        public string CompanyAdress { get; set; } // İsteğe bağlı

        [Required]  // TaxNumber zorunlu
        public string TaxNumber { get; set; }

        public DateTime CreateAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; } // Nullable

        public int? UpdatedBy { get; set; } // Nullable

        // Soft delete alanı
        public bool IsDeleted { get; set; } = false; // Varsayılan olarak false (aktif)

        // Silinme tarihi (geri alma durumu)
        public DateTime? DeletedAt { get; set; }
    }

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company"); // Tablo adı

            builder.HasKey(e => e.Company_id); // Primary key

            builder.Property(e => e.CompanyName)
                .IsRequired() // Zorunlu
                .HasMaxLength(150); // Maksimum uzunluk

            builder.Property(e => e.CompanyPhone)
                .IsRequired() // Zorunlu
                .HasMaxLength(20); // Maksimum uzunluk

            builder.Property(e => e.CompanyEmail)
                .IsRequired() // Zorunlu
                .HasMaxLength(100); // Maksimum uzunluk

            builder.Property(e => e.CompanyAdress)
                .HasMaxLength(250); // Maksimum uzunluk (isteğe bağlı)

            builder.Property(e => e.TaxNumber)
                .IsRequired(); // Zorunlu

            builder.Property(e => e.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()"); // UTC zaman, eğer veritabanında otomatik ayarlanacaksa

            builder.Property(e => e.CreatedBy)
                .IsRequired(); // CreatedBy zorunlu

            builder.Property(e => e.UpdateAt)
                .IsRequired(false); // Nullable

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false); // Nullable

            // Soft delete alanı
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            // Silinme tarihi (geri alma durumu)
            builder.Property(e => e.DeletedAt)
                .IsRequired(false); 

            // Benzersizlik (unique) index tanımlamaları
            builder.HasIndex(e => e.CompanyEmail).IsUnique().HasDatabaseName("UK_Company_Email");
            builder.HasIndex(e => e.TaxNumber).IsUnique().HasDatabaseName("UK_Company_TaxNumber");
        }
    }
}
