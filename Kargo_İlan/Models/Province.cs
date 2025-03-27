namespace Kargo_İlan.Models
{
        public class Province
        {
            public int ProvinceId { get; set; }
            public decimal Latitude { get; set; }//Enlem
            public decimal Longitude { get; set; }//Boylam
            public String ProvinceName { get; set; }


              
        
            public ICollection<Listing> YuklemeIl { get; set; }  // Alış ilanlarına ait liste
            public ICollection<Listing> VarisIl { get; set; }  // Varış ilanlarına ait liste
        }   
}
