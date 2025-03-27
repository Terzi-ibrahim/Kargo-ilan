namespace Kargo_İlan.Models
{
    public class Listing
    {
        public int ListingId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Weight { get; set; }
        public string Image { get; set; }

        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

           

        //User ile İlişkili
        public int UserId { get; set; }//yabancı anahar 
        public User User { get; set; }

        // CargoType ile ilişki
        public int CargoId { get; set; }  // Yabancı anahtar
        public CargoType CargoType { get; set; }  // Navigation property



        //categoryType ile ilişkili
        public int CategoryId { get; set; }//yabancı anahtar 
        public CategoryType CategoryType { get; set; }
   



        //VehicleType ile ilişki
        public int VehicleTypeId { get; set; }//yabanıc anahtar
        public VehicleType VehicleType { get; set; }//navigation

        //------------------------Yükleme Adres İlişki------------------------------------------------------

        // Yükleme il ile ilişki
        public int YuklemeIlId { get; set; }  // Yabancı anahtar (Province)
        public Province YuklemeIl { get; set; }  // Yükleme il

        // Yükleme ilçe ile ilişki
        public int YuklemeIlceId { get; set; }  // Yabancı anahtar (District)
        public District YuklemeIlce { get; set; }  // Yükleme ilçe

        // Yükleme ülke bilgisi
        public int YuklemeUlkeId { get; set; }
        public Country YuklemeUlke { get; set; } // Yükleme ülke ilişkisi

        //------------------------Varış Adres İlişki------------------------------------------------------


        // Varış il ile ilişki
        public int VarisIlId { get; set; }  // Yabancı anahtar (Province)
        public Province VarisIl { get; set; }  // Varış il

        // Varış ilçe ile ilişki
        public int VarisIlceId { get; set; }  // Yabancı anahtar (District)
        public District VarisIlce { get; set; }  // Varış ilçe

        // Varış ülke bilgisi
        public int VarisUlkeId { get; set; }
        public Country VarisUlke { get; set; } // Varış ülke ilişkisi




    }
}
