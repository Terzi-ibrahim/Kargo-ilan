using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Company
{
    public class AddCompanyUser
    {
 
        [Required(ErrorMessage = "Ad zorunludur.")]
        public string Name { get; set; }

    
        [Required(ErrorMessage = "Soyad zorunludur.")]
        public string Surname { get; set; }

      
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string Email { get; set; }


        [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "Telefon numarası geçerli değil.")]
        public string PhoneNumber { get; set; }



        public string Address { get; set; }

  
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]       
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]    
        public string ConfirmPassword { get; set; }
      
        [Required]
        public int CompanyId { get; set; }
      
        [Required]
        public int CompanyUserId { get; set; }
    }
}
