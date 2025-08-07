using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kargo_İlan.Models
{
    public class District
    {
        public int District_id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        [Column("IlceAdi")] 
        public string DistrictName { get; set; }

        public int Province_id { get; set; } 
        public Province Province { get; set; }   

        public ICollection<Freight> Yukleme_ilce { get; set; }  
        public ICollection<Freight> Varis_ilce { get; set; }  




        public ICollection<UserNotificationPrefs> YuklemeIlanlari { get; set; }
        public ICollection<UserNotificationPrefs> VarisIlanlari { get; set; }
    }


    public class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("Districts");

            builder.HasKey(d => d.District_id).HasName("PK_Ilceler");

            builder.Property(d => d.DistrictName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("IlceAdi");

            builder.Property(d => d.Latitude)
                .HasColumnType("decimal(9,6)")
                .HasColumnName("Enlem");

            builder.Property(d => d.Longitude)
                .HasColumnType("decimal(9,6)")
                .HasColumnName("Boylam");

            // İlçenin yükleme ilçesi (YuklemeIlce) ile ilişkisini kuruyoruz
            builder.HasMany(d => d.Yukleme_ilce)
                .WithOne(f => f.Yukleme_ilce)  // Freight ile ilişki
                .HasForeignKey(f => f.Yuklemeilce_id)  // Foreign key
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ilanlar_YuklemeIlcesi");

            // İlçenin varış ilçesi (VarisIlce) ile ilişkisini kuruyoruz
            builder.HasMany(d => d.Varis_ilce)
                .WithOne(f => f.Varis_ilce)  // Freight ile ilişki
                .HasForeignKey(f => f.Varisilce_id)  // Foreign key
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ilanlar_VarisIlcesi");

            // İlçenin ait olduğu şehir (Province) ile ilişkisini kuruyoruz
            builder.HasOne(d => d.Province)
                .WithMany(p => p.District)
                .HasForeignKey(d => d.Province_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ilceler_Iller");
        }
    }

}