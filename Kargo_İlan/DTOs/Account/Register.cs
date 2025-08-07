using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Account
{
    public class Register
    {
        [Required(ErrorMessage = "Ad  zorunludur")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [RegularExpression("^[a-zA-Z0-9._$*]+$", ErrorMessage = "Kullanıcı adı yalnızca harf, rakam ve . _ $ * karakterlerini içerebilir.")]

        public string UserName { get; set; }


        [Display(Name = "Adres")]
        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        [RegularExpression(@"\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}", ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; }
    

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        [Display(Name = "Şifre Tekrar")]
        public string ConfirmPassword { get; set; }    

      

    }

}
