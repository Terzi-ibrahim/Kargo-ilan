using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kargo_İlan.Models
{
    public class Province
    {
        public int Province_id { get; set; }
        public decimal Latitude { get; set; } // Enlem
        public decimal Longitude { get; set; } 
        [Column("IlAdi")] 
        public string ProvinceName { get; set; }

        public ICollection<District> District { get; set; } 
        public ICollection<Freight> Yukleme_il { get; set; } 
        public ICollection<Freight> Varis_il { get; set; } 



        public ICollection<UserNotificationPrefs> YuklemeIlanlari { get; set; }
        public ICollection<UserNotificationPrefs> VarisIlanlari { get; set; }
    }

    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.HasKey(p => p.Province_id).HasName("PK_Iller");

            builder.Property(p => p.ProvinceName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("IlAdi");

            builder.Property(p => p.Latitude)
                .HasColumnType("decimal(9,6)")
                .HasColumnName("Enlem");

            builder.Property(p => p.Longitude)
                .HasColumnType("decimal(9,6)")
                .HasColumnName("Boylam");

            // Alış ilanlarına ait liste için ilişki
            builder.HasMany(p => p.Yukleme_il)
                .WithOne(f => f.Yukleme_il) // `Freight` ile ilişki
                .HasForeignKey(f => f.Yuklemeil_id) // `Foreign Key`
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Freight_Yukleme_il");

            // Varış ilanlarına ait liste için ilişki
            builder.HasMany(p => p.Varis_il)
                .WithOne(f => f.Varis_il) // `Freight` ile ilişki
                .HasForeignKey(f => f.Varisil_id) // `Foreign Key`
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Freight_Varis_il");
        }
    }

}
