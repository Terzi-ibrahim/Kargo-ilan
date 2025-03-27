using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore;

namespace Kargo_İlan.Data
{
    public class KargoDbContext(IConfiguration configuration) : DbContext
    {
        private readonly IConfiguration _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("KargoDb"));

        }

        public DbSet<CargoType> CargoTypes { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Listing> Listings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            //-------------------------------------- Listing modelindeki Özellik ve  ilişkileri konfigüre etme------------------------------------


            modelBuilder.Entity<Listing>(entity =>
            {
                // ---------------------- Temel Özellikler ----------------------
                entity.Property(l => l.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Baslik");  // Türkçe kolon adı

                entity.Property(l => l.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("Aciklama");

                entity.Property(l => l.Weight)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("Agirlik");

                entity.Property(l => l.Image)
                    .HasMaxLength(500)
                    .HasColumnName("ResimYolu");

                entity.Property(l => l.CreatedAt)
                    .HasColumnName("OlusturulmaTarihi")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(l => l.UpdatedAt)
                    .HasColumnName("GuncellemeTarihi");

                // ---------------------- Index Tanımları ----------------------
                entity.HasIndex(l => l.UserId, "IX_Listing_Kullanici");
                entity.HasIndex(l => l.CreatedAt, "IX_Listing_OlusturulmaTarihi");
                entity.HasIndex(l => new { l.YuklemeIlId, l.VarisIlId }, "IX_Listing_Rota");
                entity.HasIndex(l => l.VehicleTypeId, "IX_Listing_AracTipi");

                // ---------------------- İlişki Konfigürasyonları ----------------------

                // Kullanıcı İlişkisi
                entity.HasOne(l => l.User)
                    .WithMany(u => u.Listings)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Listing_Kullanici");

                // Kargo Tipi İlişkisi (Restrict daha güvenli)
                entity.HasOne(l => l.CargoType)
                    .WithMany(c => c.Listings)
                    .HasForeignKey(l => l.CargoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_KargoTipi");

                // Kategori İlişkisi
                entity.HasOne(l => l.CategoryType)
                    .WithMany(c => c.Listings)
                    .HasForeignKey(l => l.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_Kategori");

                // Araç Tipi İlişkisi
                entity.HasOne(l => l.VehicleType)
                    .WithMany(v => v.Listings)
                    .HasForeignKey(l => l.VehicleTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_AracTipi");

                // ---------------------- Adres İlişkileri ----------------------

                // Yükleme Adresi
                entity.HasOne(l => l.YuklemeIl)
                    .WithMany(p => p.YuklemeIl)
                    .HasForeignKey(l => l.YuklemeIlId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_YuklemeIli");

                entity.HasOne(l => l.YuklemeIlce)
                    .WithMany(d => d.YuklemeIlce)
                    .HasForeignKey(l => l.YuklemeIlceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_YuklemeIlcesi");

                entity.HasOne(l => l.YuklemeUlke)
                    .WithMany(c => c.YuklemeCountry)
                    .HasForeignKey(l => l.YuklemeUlkeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_YuklemeUlkesi");

                // Varış Adresi
                entity.HasOne(l => l.VarisIl)
                    .WithMany(p => p.VarisIl)
                    .HasForeignKey(l => l.VarisIlId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_VarisIli");

                entity.HasOne(l => l.VarisIlce)
                    .WithMany(d => d.VarisIlce)
                    .HasForeignKey(l => l.VarisIlceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_VarisIlcesi");

                entity.HasOne(l => l.VarisUlke)
                    .WithMany(c => c.VarisCountry)
                    .HasForeignKey(l => l.VarisUlkeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Listing_VarisUlkesi");
            });
            //--------------------------------------------------CargoType özellikleri--------------------------------------

            modelBuilder.Entity<CargoType>(entity =>
            {
                entity.HasKey(c => c.CargoId)
                      .HasName("PK_KargoTipleri");

                entity.Property(c => c.CargoName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("KargoAdi")
                    .HasComment("Kargo tipinin adını belirtir");

                entity.HasMany(c => c.Listings)
                    .WithOne(l => l.CargoType)
                    .HasForeignKey(l => l.CargoId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_KargoTipi");
            });

            //--------------------------------------------------CategoryType özellikleri--------------------------------------

            modelBuilder.Entity<CategoryType>(entity =>
            {
                entity.HasKey(c => c.CategoryId)
                      .HasName("PK_Kategoriler");

                entity.Property(c => c.CategoryName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("KategoriAdi")
                    .HasComment("Kategori adını belirtir");

                entity.HasMany(c => c.Listings)
                    .WithOne(l => l.CategoryType)
                    .HasForeignKey(l => l.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_Kategori");
            });
            //--------------------------------------------------VehicleType özellikleri--------------------------------------

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasKey(v => v.VehicleTypeId)
                      .HasName("PK_AracTipleri");

                entity.Property(v => v.VehicleName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("AracAdi")
                    .HasComment("Araç tipinin adını belirtir");

                entity.HasMany(v => v.Listings)
                    .WithOne(l => l.VehicleType)
                    .HasForeignKey(l => l.VehicleTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_AracTipi");
            });
            //--------------------------------------------------Country özellikleri--------------------------------------

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(c => c.CountryID)
                      .HasName("PK_Ulkeler");

                entity.Property(c => c.CountryName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("UlkeAdi");

                entity.Property(c => c.Latitude)
                    .HasColumnType("decimal(9,6)")
                    .HasColumnName("Enlem");

                entity.Property(c => c.Longitude)
                    .HasColumnType("decimal(9,6)")
                    .HasColumnName("Boylam");

                // Yükleme ilişkileri
                entity.HasMany(c => c.YuklemeCountry)
                    .WithOne(l => l.YuklemeUlke)
                    .HasForeignKey(l => l.YuklemeUlkeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_YuklemeUlkesi");

                // Varış ilişkileri
                entity.HasMany(c => c.VarisCountry)
                    .WithOne(l => l.VarisUlke)
                    .HasForeignKey(l => l.VarisUlkeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_VarisUlkesi");
            });
            //--------------------------------------------------District özellikleri--------------------------------------

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(d => d.DistrictId)
                      .HasName("PK_Ilceler");

                entity.Property(d => d.DistrictName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("IlceAdi");

                entity.Property(d => d.Latitude)
                    .HasColumnType("decimal(9,6)")
                    .HasColumnName("Enlem");

                entity.Property(d => d.Longitude)
                    .HasColumnType("decimal(9,6)")
                    .HasColumnName("Boylam");

                // Yükleme ilişkileri
                entity.HasMany(d => d.YuklemeIlce)
                    .WithOne(l => l.YuklemeIlce)
                    .HasForeignKey(l => l.YuklemeIlceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_YuklemeIlcesi");

                // Varış ilişkileri
                entity.HasMany(d => d.VarisIlce)
                    .WithOne(l => l.VarisIlce)
                    .HasForeignKey(l => l.VarisIlceId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_VarisIlcesi");
            });
            //--------------------------------------------------Province özellikleri--------------------------------------

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(p => p.ProvinceId)
                      .HasName("PK_Iller");

                entity.Property(p => p.ProvinceName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("IlAdi");

                entity.Property(p => p.Latitude)
                    .HasColumnType("decimal(9,6)")
                    .HasColumnName("Enlem");

                entity.Property(p => p.Longitude)
                    .HasColumnType("decimal(9,6)")
                    .HasColumnName("Boylam");

                // Yükleme ilişkileri
                entity.HasMany(p => p.YuklemeIl)
                    .WithOne(l => l.YuklemeIl)
                    .HasForeignKey(l => l.YuklemeIlId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_YuklemeIli");

                // Varış ilişkileri
                entity.HasMany(p => p.VarisIl)
                    .WithOne(l => l.VarisIl)
                    .HasForeignKey(l => l.VarisIlId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Ilanlar_VarisIli");
            });



            //--------------------------------------------------User özellikleri--------------------------------------

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId)
                      .HasName("PK_Kullanicilar");

                entity.Property(u => u.RoleId)
                    .IsRequired()
                    .HasColumnName("RolID");

                // Role ilişkisi
                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Kullanicilar_Roller");

                // Listings ilişkisi
                entity.HasMany(u => u.Listings)
                    .WithOne(l => l.User)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Ilanlar_Kullanicilar");
            });

            //--------------------------------------------------Role özellikleri--------------------------------------

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleId)
                      .HasName("PK_Roller");

                entity.Property(r => r.RoleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("RolAdi")
                    .HasComment("Rolün adını belirtir");

                // Kullanıcı ilişkileri
                entity.HasMany(r => r.Users)
                    .WithOne(u => u.Role)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Kullanicilar_Roller");
            });





        }




    }
}
