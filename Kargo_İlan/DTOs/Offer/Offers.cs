namespace Kargo_İlan.DTOs.Offers
{
    public class Offers
    {
        public int Offer_id { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Freight_id { get; set; }
        public string FreightTitle { get; set; } = string.Empty;

        // Teklif veren
        public int User_id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // İlan sahibi (sadece View'da kontrol için)
        public int FreightOwnerId { get; set; }

        // Status bilgisi
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;

        // Opsiyonel: kalan süre
        public string RemainingTime { get; set; } = string.Empty;

        // ✅ Eklenmesi gerekenler:
        public bool IsConfirmed => StatusId == 2;
        public bool IsRejected => StatusId == 3;

        // ✔ Soft-delete kontrolü için
        public bool IsDeleted { get; set; }
    }
}
