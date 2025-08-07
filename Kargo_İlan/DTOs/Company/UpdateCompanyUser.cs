using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Company
{
    public class UpdateCompanyUser
    {
        [Required(ErrorMessage = "Ad zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }

        [Required]
        public int Person_id { get; set; }

        [Required]
        public int CompanyUserId { get; set; }
    }
}
