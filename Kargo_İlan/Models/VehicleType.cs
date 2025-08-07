using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Models
{
    public class VehicleType
    {
        public int VehicleType_id { get; set; }
        public string VehicleName { get; set; }
        public ICollection<Freight> Freight { get; set; }  
    }

    public class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
    {
        public void Configure(EntityTypeBuilder<VehicleType> builder)
        {
            builder.ToTable("VehicleType");

            builder.HasKey(v => v.VehicleType_id);

            builder.Property(v => v.VehicleName)
                .IsRequired()
                .HasMaxLength(100);

            // One-to-Many ilişkiyi tanımlıyoruz
            builder.HasMany(v => v.Freight)
                .WithOne(f => f.Vehicle_Type)
                .HasForeignKey(f => f.VehicleType_id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
