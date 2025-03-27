namespace Kargo_İlan.Models
{
    public class Country
    {
        public int CountryID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public String CountryName { get; set; }




        public ICollection<Listing> YuklemeCountry { get; set; }  // Alış ilanlarına ait liste
        public ICollection<Listing> VarisCountry { get; set; }  // Varış ilanlarına ait liste


    }
}
