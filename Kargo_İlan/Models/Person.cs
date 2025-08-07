using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kargo_İlan.Models
{
    public class Person
    {
        public int Person_id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SurName { get; set; }

        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }
 
        public DateTime? DeletedAt { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }

        public DateTime? UpdateAt { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person");

            builder.HasKey(e => e.Person_id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.SurName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Address)
                .HasMaxLength(500);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.CreateAt)
                .HasDefaultValueSql("GETUTCDATE()"); 

            builder.Property(e => e.CreatedBy)
                .IsRequired();

            builder.Property(e => e.UpdateAt)
                .IsRequired(false); 

            builder.Property(e => e.UpdatedBy)
                .IsRequired(false);
         
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            
            builder.Property(e => e.DeletedAt)
                .IsRequired(false);

            // Benzersizlik (unique) index tanımlamaları
            builder.HasIndex(e => e.Email).IsUnique().HasDatabaseName("UK_Person_Email");
            builder.HasIndex(e => e.PhoneNumber).IsUnique().HasDatabaseName("UK_Person_PhoneNumber");
        }
    }
}
