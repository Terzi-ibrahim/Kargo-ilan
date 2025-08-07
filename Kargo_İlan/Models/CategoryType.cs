using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Models
{
    public class CategoryType
    {

        public int Category_id { get; set; }
        public String CategoryName { get; set; }

        public ICollection<Freight> Freight { get; set; }  // One-to-Many (Bire-Çok) ilişki
        
    }
    public class CategoryTypeConfiguration : IEntityTypeConfiguration<CategoryType>
    {
        public void Configure(EntityTypeBuilder<CategoryType> builder)
        {
            // Tablo ismini ve primary key'i belirliyoruz
            builder.ToTable("CategoryType");
            builder.HasKey(c => c.Category_id).HasName("PK_Kategoriler");

            // Kategori adı için özellikler
            builder.Property(c => c.CategoryName)
                .IsRequired() // Zorunlu
                .HasMaxLength(255); // Maksimum uzunluk 255           
             

            // İlanlar (Freight) ile ilişkili
            builder.HasMany(c => c.Freight)
                .WithOne(l => l.Category_Type) // Bir CategoryType, birden fazla Freight ile ilişkilidir
                .HasForeignKey(l => l.Category_id) 
                .OnDelete(DeleteBehavior.Restrict) // Silme davranışı: silinen kategori, ilişkili ilanları silmez
                .HasConstraintName("FK_Ilanlar_Kategori"); // Kısıtlama adı
        }
    }
}
