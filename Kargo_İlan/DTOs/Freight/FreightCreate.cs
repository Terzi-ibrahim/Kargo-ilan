using System.ComponentModel.DataAnnotations;

namespace Kargo_İlan.DTOs.Freight
{
    public class FreightCreate
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Miktar alanı zorunludur.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Miktar 0.1'den büyük olmalıdır.")]
        public decimal Miktar { get; set; }

        [Required(ErrorMessage = "Kargo türü seçilmelidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kargo türü seçin.")]
        public int Cargo_id { get; set; }

        [Required(ErrorMessage = "Kategori seçilmelidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçin.")]
        public int Category_id { get; set; }

        [Required(ErrorMessage = "Araç türü seçilmelidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir araç türü seçin.")]
        public int VehicleType_id { get; set; }

        [Required(ErrorMessage = "Yükleme ili seçilmelidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir il seçin.")]
        public int YuklemeIlId { get; set; }

        [Required(ErrorMessage = "Yükleme ilçesi seçilmelidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir ilçe seçin.")]
        public int YuklemeIlceId { get; set; }

        [Required(ErrorMessage = "Varış ili seçilmelidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir il seçin.")]
        public int VarisIlId { get; set; }

        [Required(ErrorMessage = "Varış ilçesi seçilmelidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir ilçe seçin.")]
        public int VarisIlceId { get; set; }
    }
}
