using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.Models
{
    public class User
    {


        public int User_id { get; set; }
        public int Person_id { get; set; }

        public Person Person { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public bool IsDeleted { get; set; } = false; // Varsayılan olarak false (aktif)
                                                     
        // Silinme tarihi (geri alma durumu)
        public DateTime? DeletedAt { get; set; }

        public DateTime CreateAt { get; set; }
        public int CreatedBy { get; set; }


        public DateTime? UpdateAt { get; set; } 
        public int? UpdatedBy { get; set; } 

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();

        public ICollection<UserNotificationPrefs> NotificationPrefs { get; set; }


    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(e => e.User_id);

            builder.Property(e => e.UserName)
                .IsRequired() 
                .HasMaxLength(150); 

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255); 

            builder.Property(e => e.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()"); 

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.UpdateAt)
                .IsRequired(false);

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);

            builder.Property(e => e.ResetToken)
                .IsRequired(false); 

            builder.Property(e => e.ResetTokenExpiry)
                .IsRequired(false);
                                    
  
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

    
            builder.Property(e => e.DeletedAt)
                .IsRequired(false);

            builder.HasOne(u => u.Person)
                   .WithMany()
                .HasForeignKey(u => u.Person_id)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserCompanies)
              .WithOne(uc => uc.User)
              .HasForeignKey(uc => uc.User_id)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.UserName).IsUnique().HasDatabaseName("UK_User_UserName");
        }
    }
}
