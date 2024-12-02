using System.Text.Json.Serialization;

namespace FlightSearch.Server.Models
{
    public class SearchParameter
    {
        public int Id { get; set; }
        public string? SearchParameterHash { get; set; }
        public string Origin { get; set; } 
        public string Destination { get; set; } 
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public int Passengers { get; set; }
        public string Currency { get; set; }

        [JsonIgnore]
        public ICollection<Flight>? FlightResults { get; set; }
    }
}

