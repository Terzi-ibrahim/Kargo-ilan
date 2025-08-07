using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Account
{
 
    public class ProfileViewModel
    {
        public Profile Profile { get; set; }
        public UserChangePassword UserChangePassword { get; set; }
    }

    public class UserChangePassword
    {
        [Required(ErrorMessage = "Mevcut şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre zorunludur.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrar zorunludur.")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }


    public class Profile
    {
        public int UserId { get; internal set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Adres zorunludur.")]
        [StringLength(500)]
        public string Address { get; set; }

        public int ActiveFreight { get; set; }
        public int OfferedFreight { get; set; }
    }

}
