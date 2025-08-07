using System;
using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Offer
{
    public class UpdateView
    {
        [Required]
        public int OfferId { get; set; }

        // Sadece güncellenecek alanlar:
        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Range(1, double.MaxValue, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Description { get; set; }

        // Güvenlik için POST action’da override edeceğiz.
        public int User_id { get; set; }

        // Aşağıdakiler formda gönderilmeyecek, yalnızca read-only gösterim için doldurulacak:
        public string FreightTitle { get; set; }
        public int Freight_id { get; set; }
        public string AlisYuk { get; set; }
        public string VarisYuk { get; set; }
        public string CargoTur { get; set; }
        public decimal Miktari { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
