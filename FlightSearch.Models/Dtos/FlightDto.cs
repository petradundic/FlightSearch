namespace FlightSearch.Server.Dtos
{
    public class FlightDto
    {
        public int OutboundStops { get; set; }
        public int ReturnStops { get; set; }
        public string OutboundDuration { get; set; } // e.g., "PT1H50M"
        public string ReturnDuration { get; set; }   // e.g., "PT45M"
        public DateTime? OutboundDepartureTime { get; set; } // Converted to DateTime
        public DateTime? OutboundArrivalTime { get; set; }   // Converted to DateTime
        public DateTime? ReturnDepartureTime { get; set; }   // Converted to DateTime
        public DateTime? ReturnArrivalTime { get; set; }     // Converted to DateTime
        public string? TotalPrice { get; set; }
        public string? Currency { get; set; }
    }
}
