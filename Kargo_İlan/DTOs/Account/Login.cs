using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Account
{
    public class Login
    {
        [Required(ErrorMessage = "Bu alan zorunludur.")]    
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
