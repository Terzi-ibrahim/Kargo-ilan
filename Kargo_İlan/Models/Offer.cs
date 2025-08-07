using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kargo_İlan.Models
{
    public class Offer
    {
        [Key]
        public int Offer_id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }    
        
        [Required]
        public DateTime ExpiryDate { get; set; }

        public DateTime? ConfirmDate { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; }
        public int? UpdatedBy { get; set; }   

        public bool IsDeleted { get; set; } = false;

        public int User_id { get; set; }
        public User User { get; set; }

        public int Freight_id { get; set; }
        public Freight Freight { get; set; }



        [Required]
        [Column("OfferStatusStatusId")]
        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public OfferStatus Status { get; set; } = null!;

        public Offer()
        {
            ExpiryDate = DateTime.Now.AddDays(2);
        }

    }
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> entity)
        {
            entity.ToTable("Offer");

            entity.HasKey(o => o.Offer_id);

            entity.Property(o => o.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(o => o.Description)
                .HasMaxLength(1000);

            entity.Property(o => o.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(o => o.CreatedBy)
                .IsRequired();

            entity.Property(o => o.UpdateAt)
                .IsRequired(false);

            entity.Property(o => o.UpdatedBy)
                .IsRequired(false);

            entity.Property(o => o.ConfirmDate)
                .IsRequired(false);

            entity.Property(o => o.ExpiryDate)
                .IsRequired();

            entity.Property(o => o.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasQueryFilter(o => !o.IsDeleted);

            entity.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.User_id)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.Freight) 
                .WithMany()
                .HasForeignKey(o => o.Freight_id)
                .OnDelete(DeleteBehavior.Restrict);


            // --- Durum ilişkisi ---
            entity.HasOne(o => o.Status)
                  .WithMany(s => s.Offers)
                  .HasForeignKey(o => o.StatusId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }


}
