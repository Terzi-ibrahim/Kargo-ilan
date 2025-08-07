using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Offers
{
    public class Create
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat sıfırdan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [MinLength(5, ErrorMessage = "Açıklama en az 5 karakter olmalıdır.")]
        public string Description { get; set; }

       
        public int Freight_id { get; set; }
    }

}
