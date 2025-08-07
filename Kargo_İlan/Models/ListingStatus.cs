using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Models
{
    public class ListingStatus
    {
        public int ListingStatus_id { get; set; }
        public string Status { get; set; } // Örneğin "Aktif", "Pasif", "Beklemede"

        // Listing ile ilişki
        public ICollection<Freight> Freight { get; set; }
    }
}

public class ListingStatusConfiguration : IEntityTypeConfiguration<ListingStatus>
{
    public void Configure(EntityTypeBuilder<ListingStatus> builder)
    {
        builder.ToTable("ListingStatus");


        builder.HasKey(ls => ls.ListingStatus_id).HasName("PK_IlanDurumlari");


        builder.Property(ls => ls.Status)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(ls => ls.Freight)
            .WithOne(l => l.Listing_Status)
            .HasForeignKey(l => l.ListingStatus_id)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Ilanlar_IlanDurumu");
    }
}
