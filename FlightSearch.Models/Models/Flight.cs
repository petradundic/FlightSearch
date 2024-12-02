using System.Text.Json.Serialization;

namespace FlightSearch.Server.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public int SearchParameterId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? OutboundDepartureTime { get; set; } 
        public DateTime? OutboundArrivalTime { get; set; }   
        public DateTime? ReturnDepartureTime { get; set; }  
        public DateTime? ReturnArrivalTime { get; set; }
        public int OutboundStops { get; set; }
        public int ReturnStops { get; set; }
        public int Passengers { get; set; }
        public string Currency { get; set; }
        public decimal TotalPrice { get; set; }

        [JsonIgnore]
        public SearchParameter? SearchParameter { get; set; }
    }
}
