namespace Kargo_İlan.DTOs.Offers
{
    public class Update
    {
        public int OfferId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }   
        public int Freight_id { get; set; }

    }
}
