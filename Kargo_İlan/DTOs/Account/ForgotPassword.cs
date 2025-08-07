using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Account
{
    public class ForgotPassword
    {

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
    }
}
