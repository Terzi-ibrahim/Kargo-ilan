using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Kargo_İlan.Models;
using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.Models
{
    public class Contact
    {
        public int Contact_id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contact");

        // Anahtar olarak ContactId kullanılır
        builder.HasKey(c => c.Contact_id);


        // FullName alanını zorunlu ve maksimum uzunluk 200 karakter olarak ayarlıyoruz
        builder.Property(c => c.FullName)
              .IsRequired()
              .HasMaxLength(200);

        // Email alanını zorunlu ve maksimum uzunluk 200 karakter olarak ayarlıyoruz
        builder.Property(c => c.Email)
              .IsRequired()
              .HasMaxLength(200);

        // Email alanına indeks eklemek, sorgu hızını artırabilir
        builder.HasIndex(c => c.Email)
              .HasDatabaseName("IX_Contact_Email")
              .IsUnique(false);  // Email'in benzersiz olması gerekmiyor ancak hız için indeks ekliyoruz

        // Message alanını zorunlu ve maksimum uzunluk 2000 karakter olarak ayarlıyoruz
        builder.Property(c => c.Message)
              .IsRequired()
              .HasMaxLength(2000);

        // CreatedDate alanına varsayılan tarih ekliyoruz
        builder.Property(c => c.CreatedDate)
              .HasDefaultValueSql("GETDATE()");
    }
}
