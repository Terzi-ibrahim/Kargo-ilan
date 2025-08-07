using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs
{
    public class ContactDTO
    {
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "İsminiz zorunludur.")]     
        public string FullName { get; set; }

        [Required(ErrorMessage = "Lütfen Mesajınzı Giriniz.")]
        public string Message { get; set; }
    }
}
