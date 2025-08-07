
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace Kargo_İlan.Models
{
    public class Notification
    {
        [Key]
        public int Notification_id { get; set; }

        [Required]
        public string Message { get; set; } 

        public bool IsRead { get; set; } = false; 

        public DateTime CreatedAt { get; set; }


        public int? Offer_id { get; set; } // Nullable yap

        [ForeignKey("Offer_id")]
        public Offer Offer { get; set; }

        [Required]
        public int User_id { get; set; }
        public User User { get; set; }

        public int? FreightId { get; set; }  
        public Freight Freight { get; set; }


        public int? UserNotificationPrefId { get; set; }
        public UserNotificationPrefs UserNotificationPref { get; set; }


    }
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            // Primary Key
            builder.HasKey(n => n.Notification_id);

            // User ile ilişki
            builder.HasOne(n => n.User)
                  .WithMany()
                  .HasForeignKey(n => n.User_id)  // Bildirimi alan kullanıcı
                  .OnDelete(DeleteBehavior.Cascade);

            // Offer ile ilişki
            builder.HasOne(n => n.Offer)
                  .WithMany()
                  .HasForeignKey(n => n.Offer_id)
                  .OnDelete(DeleteBehavior.Restrict); // Teklif silindiğinde bildirim silinmez

            // Message alanı zorunlu ve 1000 karakter sınırına sahip
            builder.Property(n => n.Message)
                  .IsRequired()
                  .HasMaxLength(1000);

            // CreatedDate alanı varsayılan olarak GETDATE()
            builder.Property(n => n.CreatedAt)
                  .HasDefaultValueSql("GETDATE()");

            // IsRead alanı varsayılan olarak false
            builder.Property(n => n.IsRead)
                  .HasDefaultValue(false);

            builder.HasOne(n => n.UserNotificationPref)
            .WithMany()
            .HasForeignKey(n => n.UserNotificationPrefId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(n => n.Freight)
                .WithMany()
                .HasForeignKey(n => n.FreightId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }

}
