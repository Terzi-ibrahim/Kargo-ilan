using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kargo_İlan.Models
{
    public class CargoType
    {
        public int Cargo_id { get; set; }

        public string CargoName { get; set; }

        public ICollection<Freight> Freight { get; set; }
    }

    public class CargoTypeConfiguration : IEntityTypeConfiguration<CargoType>
    {
        public void Configure(EntityTypeBuilder<CargoType> builder)
        {
          
            builder.ToTable("CargoType");
            builder.HasKey(c => c.Cargo_id).HasName("PK_KargoTipleri");


            builder.Property(c => c.CargoName)
                .IsRequired()
                .HasMaxLength(255);       

            // İlanlar (Freight) ile ilişkili
            builder.HasMany(c => c.Freight)
                .WithOne(l => l.Cargo_Type)
                .HasForeignKey(l => l.Cargo_id)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ilanlar_KargoTipi");
        }
    }
}
