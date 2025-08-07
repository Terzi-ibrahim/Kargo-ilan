namespace Kargo_İlan.DTOs.Company
{
    public class CompanyUserDetail
    {   
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }     
        public List<FreightIndex> Listings { get; set; }           
        public List<OfferIndex> Offers { get; set; }
    }
    public class FreightIndex
    {
        public int Freight_id { get; set; }
        public string Title { get; set; }
        public string VehicleType { get; set; }
        public string Category { get; set; }
        public string ListingStatus { get; set; }
        public string CargoType{ get; set; }        
        public DateTime OlusturulmaTarihi { get; set; }
        public string YuklemeIl { get; set; }
        public string VarisIl { get; set; }
        public string YuklemeIlce { get; set; }
        public string VarisIlce { get; set; }
        public List<OfferIndex> Offers { get; set; } = new();
    }
    public class OfferIndex
    {
        public int Offer_id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public string OfferUserName { get; set; }
        public string OfferUserCompany { get; set; }
        public string FreightTitle { get; set; }
        public int Freight_id { get; set; }
    }

 }
