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

        public DbSet<Freight> Freight { get; set; }
        public DbSet<CargoType> CargoType { get; set; }
        public DbSet<CategoryType> CategoryType { get; set; }      
        public DbSet<Province> Province { get; set; }
        public DbSet<District> District { get; set; } 
        public DbSet<VehicleType> VehicleType { get; set; }     
        public DbSet<ListingStatus> ListingStatus { get; set; }              
        public DbSet<Person> Person { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }     
        public DbSet<Offer> Offer { get; set; }           
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<UserCompany> UserCompany { get; set; }

        public DbSet<UserNotificationPrefs> UserNotificationPrefs { get; set; }
        public DbSet<OfferStatus> OfferStatus { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //İl İlçe
            modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
            modelBuilder.ApplyConfiguration(new DistrictConfiguration());
            //Yük 
            modelBuilder.ApplyConfiguration(new FreightConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CargoTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ListingStatusConfiguration());

            //ayar         
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new UserNotificationPrefsConfiguration());


            //Kullanıcı
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new UserCompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            //teklif
            modelBuilder.ApplyConfiguration(new OfferConfiguration());      
            modelBuilder.ApplyConfiguration(new OfferStatusConfiguration());

            SeedData(modelBuilder);
        }       
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryType>().HasData(
                new CategoryType { Category_id = 10, CategoryName = "Kırılabilir Ürün" },
                new CategoryType { Category_id = 20, CategoryName = "Yanıcı Madde" },
                new CategoryType { Category_id = 30, CategoryName = "Soğuk Zincir" },
                new CategoryType { Category_id = 40, CategoryName = "Kimyasal Madde" },
                new CategoryType { Category_id = 50, CategoryName = "Gıda" },
                new CategoryType { Category_id = 70, CategoryName = "Elektronik Eşya" },
                new CategoryType { Category_id = 80, CategoryName = "Mobilya" },
                new CategoryType { Category_id = 90, CategoryName = "Diğer" }
            );

            modelBuilder.Entity<CargoType>().HasData(
                new CargoType { Cargo_id = 10, CargoName = "Palet" },
                new CargoType { Cargo_id = 20, CargoName = "Çuval" },
                new CargoType { Cargo_id = 30, CargoName = "Koli" },
                new CargoType { Cargo_id = 40, CargoName = "Varil" },
                new CargoType { Cargo_id = 50, CargoName = "Bidon" },
                new CargoType { Cargo_id = 60, CargoName = "Sıvı Tankı" },
                new CargoType { Cargo_id = 70, CargoName = "Cam Şişe" },
                new CargoType { Cargo_id = 80, CargoName = "Diğer" }
            );

            modelBuilder.Entity<VehicleType>().HasData(
                new VehicleType { VehicleType_id = 10, VehicleName = "Kamyonet" },
                new VehicleType { VehicleType_id = 20, VehicleName = "Tır" },
                new VehicleType { VehicleType_id = 30, VehicleName = "Kamyon" },
                new VehicleType { VehicleType_id = 40, VehicleName = "Frigo" },
                new VehicleType { VehicleType_id = 50, VehicleName = "Panelvan" },
                new VehicleType { VehicleType_id = 60, VehicleName = "Minivan" },
                new VehicleType { VehicleType_id = 70, VehicleName = "Açık Kasa Tır" },
                new VehicleType { VehicleType_id = 80, VehicleName = "Kapalı Kasa Kamyon" },
                new VehicleType { VehicleType_id = 90, VehicleName = "Diğer" }
            );

            modelBuilder.Entity<ListingStatus>().HasData(
                new ListingStatus { ListingStatus_id = 10, Status = "Aktif" },
                new ListingStatus { ListingStatus_id = 20, Status = "Beklemede" },
                new ListingStatus { ListingStatus_id = 30, Status = "Tamamlandı" },
                new ListingStatus { ListingStatus_id = 40, Status = "İptal Edildi" }
            );

            modelBuilder.Entity<Role>().HasData(
             new Role { Role_id = 10, RoleName = "User" },
             new Role { Role_id = 20, RoleName = "CompanyUser" },
             new Role { Role_id = 30, RoleName = "Company" },
             new Role { Role_id = 40, RoleName = "Admin" }
             );
        }
    }
}
