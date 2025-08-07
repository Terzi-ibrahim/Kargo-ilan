using System.ComponentModel.DataAnnotations.Schema;

namespace Kargo_İlan.DTOs
{
   
    public class Geometry
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Components
    {
        public string country { get; set; }
        public string city { get; set; }
        public string town { get; set; }
        public string state { get; set; }
    }

    public class Result
    {
        public string formatted { get; set; }
        public Geometry geometry { get; set; }
        public Components components { get; set; }
    }

    public class LocationResponse
    {
        public List<Result> results { get; set; }
    }

   
}
