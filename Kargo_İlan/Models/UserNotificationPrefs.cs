using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Models
{
    public class UserNotificationPrefs
    {
  
        public int UserNotificationPrefId { get; set; }

        public int User_id { get; set; }
        public User User { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public int Yuklemeil_id { get; set; }
        public Province Yukleme_il { get; set; }

        public int? Yuklemeilce_id { get; set; }
        public District Yukleme_ilce { get; set; }


        public int Varisil_id { get; set; }
        public Province Varis_il { get; set; }

        public int? Varisilce_id { get; set; }
        public District Varis_ilce { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

    }
    public class UserNotificationPrefsConfiguration : IEntityTypeConfiguration<UserNotificationPrefs>
    {
        public void Configure(EntityTypeBuilder<UserNotificationPrefs> builder)
        {
            builder.ToTable("UserNotificationPrefs");

            builder.HasKey(x => x.UserNotificationPrefId);

            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired();
            builder.Property(x => x.UpdatedAt);
            builder.Property(x => x.UpdatedBy);

            // User ilişkisi
            builder.HasOne(x => x.User)
                   .WithMany(u => u.NotificationPrefs)
                   .HasForeignKey(x => x.User_id)
                   .OnDelete(DeleteBehavior.Restrict);

            // Province ilişkileri
            builder.HasOne(x => x.Yukleme_il)
                   .WithMany(p => p.YuklemeIlanlari)
                   .HasForeignKey(x => x.Yuklemeil_id)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Varis_il)
                   .WithMany(p => p.VarisIlanlari)
                   .HasForeignKey(x => x.Varisil_id)
                   .OnDelete(DeleteBehavior.Restrict);

            // District ilişkileri
            builder.HasOne(x => x.Yukleme_ilce)
                   .WithMany(d => d.YuklemeIlanlari)
                   .HasForeignKey(x => x.Yuklemeilce_id)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Varis_ilce)
                   .WithMany(d => d.VarisIlanlari)
                   .HasForeignKey(x => x.Varisilce_id)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }



}
