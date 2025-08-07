using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.Models
{
    public class OfferStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;   // Örn: "Pending", "Confirmed", "Rejected"

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<Offer>? Offers { get; set; }
    }
    public class OfferStatusConfiguration : IEntityTypeConfiguration<OfferStatus>
    {
        public void Configure(EntityTypeBuilder<OfferStatus> entity)
        {
            entity.ToTable("OfferStatus");
            entity.HasKey(s => s.StatusId);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(50);
            entity.Property(s => s.Description).HasMaxLength(200);

            // Başlangıç verisi (seed)
            entity.HasData(
                new OfferStatus { StatusId = 1, Name = "Pending", Description = "Teklif beklemede" },
                new OfferStatus { StatusId = 2, Name = "Confirmed", Description = "Teklif onaylandı" },
                new OfferStatus { StatusId = 3, Name = "Rejected", Description = "Teklif reddedildi" }
            );
        }
    }
}
