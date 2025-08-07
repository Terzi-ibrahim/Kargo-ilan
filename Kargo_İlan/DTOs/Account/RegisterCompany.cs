using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Account
{
    public class RegisterCompany
    {
        [Required(ErrorMessage = "Vergi numarası zorunludur.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Vergi numarası tam olarak 11 rakamdan oluşmalıdır.")]
        [Display(Name = "Vergi Numarası")]
        public string TaxNumber { get; set; }

        [Required(ErrorMessage = "Şirket adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Şirket adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Şirket Adı")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [RegularExpression(@"^\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$", ErrorMessage = "Geçerli bir telefon numarası giriniz (örn: (500) 555-5555).")]
        [Display(Name = "Şirket Telefonu")]
        public string CompanyPhone { get; set; }

        [Required(ErrorMessage = "Şirket e-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "Şirket E-posta")]
        public string CompanyEmail { get; set; }

        [Display(Name = "Şirket Adresi")]
        public string CompanyAdress { get; set; }
    }
}
