using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.Models
{
    public class Freight
    {
        public int Freight_id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Miktar { get; set; }

        public DateTime CreateAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; }
        public int? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public int User_id { get; set; }
        public User User { get; set; }

        [Required]
        public int ListingStatus_id { get; set; }
        public ListingStatus Listing_Status { get; set; }

        [Required]
        public int Cargo_id { get; set; }
        public CargoType Cargo_Type { get; set; }

        [Required]
        public int Category_id { get; set; }
        public CategoryType Category_Type { get; set; }

        [Required]
        public int VehicleType_id { get; set; }
        public VehicleType Vehicle_Type { get; set; }

        [Required]
        public int Yuklemeil_id { get; set; }
        public Province Yukleme_il { get; set; }

        [Required]
        public int Yuklemeilce_id { get; set; }
        public District Yukleme_ilce { get; set; }

        [Required]
        public int Varisil_id { get; set; }
        public Province Varis_il { get; set; }

        [Required]
        public int Varisilce_id { get; set; }
        public District Varis_ilce { get; set; }

    }



    public class FreightConfiguration : IEntityTypeConfiguration<Freight>
    {
        public void Configure(EntityTypeBuilder<Freight> builder)
        {
            builder.ToTable("Freight");

            builder.HasKey(e => e.Freight_id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .HasMaxLength(500);

            builder.Property(e => e.Miktar)
                .IsRequired();

            builder.Property(e => e.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.UpdateAt)
                .IsRequired(false);

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasQueryFilter(e => !e.IsDeleted);  

     
            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.User_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Listing_Status)
                .WithMany()
                .HasForeignKey(e => e.ListingStatus_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Cargo_Type)
                .WithMany()
                .HasForeignKey(e => e.Cargo_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Category_Type)
                .WithMany()
                .HasForeignKey(e => e.Category_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Vehicle_Type)
                .WithMany()
                .HasForeignKey(e => e.VehicleType_id)
                .OnDelete(DeleteBehavior.Restrict);

         
            builder.HasOne(e => e.Yukleme_il) 
                .WithMany(p => p.Yukleme_il)  
                .HasForeignKey(e => e.Yuklemeil_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Yukleme_ilce)
                .WithMany(d => d.Yukleme_ilce)
                .HasForeignKey(e => e.Yuklemeilce_id)
                .OnDelete(DeleteBehavior.Restrict);

          
            builder.HasOne(e => e.Varis_il) 
                .WithMany(p => p.Varis_il)  
                .HasForeignKey(e => e.Varisil_id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Varis_ilce) 
                .WithMany(d => d.Varis_ilce) 
                .HasForeignKey(e => e.Varisilce_id)
                .OnDelete(DeleteBehavior.Restrict);
      
            builder.HasIndex(e => e.Freight_id)
                .IsUnique()
                .HasDatabaseName("UK_Freight_id");

        }
    }






}
