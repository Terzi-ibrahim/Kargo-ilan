using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Company
{
    public class CompanyProfile
    {
        // Şirket profil bilgileri
        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string CompanyPhone { get; set; }

        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "Adres zorunludur.")]
        public string CompanyAdress { get; set; }

        [Required(ErrorMessage = "Vergi numarası zorunludur.")]
        public string TaxNumber { get; set; }

        // Görsel özet
        public int ActiveFreight { get; set; }
        public int OfferedFreight { get; set; }

        // Kullanıcılar
        public List<CompanyUserDto> CompanyUsers { get; set; } = new();

    }

    public class CompanyUserDto
    {
        public int User_id { get; set; } 
        public int Company_id { get; set; } 
        public string Name { get; set; } 
        public string SurName { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; } 
        public bool IsDeleted { get; set; } 
    }  
}
