using FlightSearch.Server.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightSearch.DataAccess.Utility
{
    public static class AmadeusHelper
    {
        public static async Task<string> GetAccessToken(HttpClient httpClient)
        {
            var tokenUrl = "https://test.api.amadeus.com/v1/security/oauth2/token";
            var requestBody = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("client_id",Environment.GetEnvironmentVariable("AMADEUS_KEY")),
            new KeyValuePair<string, string>("client_secret", Environment.GetEnvironmentVariable("AMADEUS_SECRET"))
        });

            var response = await httpClient.PostAsync(tokenUrl, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch access token from Amadeus API.");
            }
            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<AmadeusTokenResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return tokenResponse?.AccessToken ?? throw new Exception("Invalid token response from Amadeus API.");
        }

        public static List<FlightDto> ExtractFlightDetails(string content)
        {
            var flightDetailsList = new List<FlightDto>();

            var jsonDocument = JsonDocument.Parse(content);
            JsonElement root = jsonDocument.RootElement;
            JsonElement data = root.GetProperty("data");

            foreach (JsonElement flightOffer in data.EnumerateArray())
            {
                JsonElement itineraries = flightOffer.GetProperty("itineraries");

                int outboundStops = 0;
                int returnStops = 0;
                string outboundDuration = "";
                string returnDuration = "";
                DateTime? outboundDeparture = null;
                DateTime? outboundArrival = null;
                DateTime? returnDeparture = null;
                DateTime? returnArrival = null;

                if (itineraries.GetArrayLength() > 0)
                {
                    JsonElement outboundItinerary = itineraries[0];
                    JsonElement outboundSegments = outboundItinerary.GetProperty("segments");

                    outboundStops = outboundSegments.GetArrayLength() - 1;
                    outboundDuration = outboundItinerary.GetProperty("duration").GetString();

                    if (outboundSegments.GetArrayLength() > 0)
                    {
                        JsonElement firstSegment = outboundSegments[0];
                        JsonElement lastSegment = outboundSegments[outboundSegments.GetArrayLength() - 1];
                        outboundDeparture = DateTime.Parse(firstSegment.GetProperty("departure").GetProperty("at").GetString());
                        outboundArrival = DateTime.Parse(lastSegment.GetProperty("arrival").GetProperty("at").GetString());
                    }
                }

                if (itineraries.GetArrayLength() > 1)
                {
                    JsonElement returnItinerary = itineraries[1];
                    JsonElement returnSegments = returnItinerary.GetProperty("segments");

                    returnStops = returnSegments.GetArrayLength() - 1;
                    returnDuration = returnItinerary.GetProperty("duration").GetString();

                    if (returnSegments.GetArrayLength() > 0)
                    {
                        JsonElement firstSegment = returnSegments[0];
                        JsonElement lastSegment = returnSegments[returnSegments.GetArrayLength() - 1];
                        returnDeparture = DateTime.Parse(firstSegment.GetProperty("departure").GetProperty("at").GetString());
                        returnArrival = DateTime.Parse(lastSegment.GetProperty("arrival").GetProperty("at").GetString());
                    }
                }

                string? totalPrice = flightOffer.GetProperty("price").GetProperty("total").GetString();
                string? currency = flightOffer.GetProperty("price").GetProperty("currency").GetString();

                flightDetailsList.Add(new FlightDto
                {
                    OutboundStops = outboundStops,
                    ReturnStops = returnStops,
                    OutboundDuration = outboundDuration,
                    ReturnDuration = returnDuration,
                    OutboundDepartureTime = outboundDeparture,
                    OutboundArrivalTime = outboundArrival,
                    ReturnDepartureTime = returnDeparture,
                    ReturnArrivalTime = returnArrival,
                    TotalPrice = totalPrice,
                    Currency = currency
                });
            }

            return flightDetailsList;
        }

    }

    public class AmadeusTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
