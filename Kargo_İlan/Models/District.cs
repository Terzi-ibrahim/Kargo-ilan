namespace Kargo_İlan.Models
{
    public class District
    {
        public int DistrictId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public String DistrictName { get; set; }


       


        public ICollection<Listing> YuklemeIlce { get; set; }  // Alış ilanlarına ait liste
        public ICollection<Listing> VarisIlce { get; set; }  // Varış ilanlarına ait liste

    }
}
