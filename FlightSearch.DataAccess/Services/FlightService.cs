using FlightSearch.DataAccess.Data;
using FlightSearch.Server.Dtos;
using FlightSearch.Server.Models;
using FlightSearch.Server.Utility;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using FlightSearch.DataAccess.Utility;
using Microsoft.IdentityModel.Tokens;

namespace FlightSearch.Server.Services
{
    public class FlightService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public FlightService(HttpClient httpClient, ApplicationDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<FlightDto>> FetchFlightsFromAmadeus(SearchParameter searchParams)
        {
            var token = await AmadeusHelper.GetAccessToken(_httpClient);

            var url = $"https://test.api.amadeus.com/v2/shopping/flight-offers?" +
              $"originLocationCode={searchParams.Origin}&destinationLocationCode={searchParams.Destination}" +
              $"&departureDate={searchParams.DepartureDate:yyyy-MM-dd}" +
              (searchParams.ReturnDate != null ? $"&returnDate={searchParams.ReturnDate:yyyy-MM-dd}" : "") +
              $"&adults={searchParams.Passengers}&currencyCode={searchParams.Currency}";

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch flights from Amadeus API. Status: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var flightData = AmadeusHelper.ExtractFlightDetails(content);

            return flightData ?? new List<FlightDto>();
        }


        public async Task AddFlightsAsync(IEnumerable<Flight> flights)
        {
            if (flights == null || !flights.Any())
            {
                throw new ArgumentNullException(nameof(flights));
            }

            await _dbContext.Set<Flight>().AddRangeAsync(flights);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddSearchParameterAsync(SearchParameter searchParameter)
        {
            if (searchParameter == null)
            {
                throw new ArgumentNullException(nameof(searchParameter));
            }

            await _dbContext.Set<SearchParameter>().AddAsync(searchParameter);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<SearchParameter?> GetSearchParameterByHashAsync(string hashValue)
        {
            return await _dbContext.SearchParameters.FirstOrDefaultAsync(sp => sp.SearchParameterHash == hashValue);
        }

        public async Task<List<Flight>> GetFlightsBySearchParameterIdAsync(int searchParameterId)
        {
            return await _dbContext.Flights
                .Where(f => f.SearchParameterId == searchParameterId)
                .ToListAsync();
        }

        public async Task<List<Flight>> GetFlightsBySearchParametersAsync(SearchParameter searchParameter)
        {
            string parametersHashValue = HashUtility.GenerateSearchHash(searchParameter);
            var existingSearchParam = await GetSearchParameterByHashAsync(parametersHashValue);

            if (existingSearchParam == null)
            {
                var flightDtos = await FetchFlightsFromAmadeus(searchParameter);
                searchParameter.SearchParameterHash = parametersHashValue;
                await AddSearchParameterAsync(searchParameter);
                var savedSearchParam = await GetSearchParameterByHashAsync(parametersHashValue);

                if (savedSearchParam == null)
                {
                    throw new Exception("Failed to save search parameters.");
                }
                var flights = flightDtos.Select(dto =>
                {
                    var flight = _mapper.Map<Flight>(dto);
                    flight.SearchParameterId = savedSearchParam.Id;
                    flight.Origin = savedSearchParam.Origin;
                    flight.Destination = savedSearchParam.Destination;
                    flight.Passengers = savedSearchParam.Passengers;
                    flight.Currency = savedSearchParam.Currency;
                    return flight;
                });

                await AddFlightsAsync(flights);
                return flights.ToList();
            }
            else
            {
                var flightsFromDb = await GetFlightsBySearchParameterIdAsync(existingSearchParam.Id);
                return flightsFromDb;
            }
        }
    }
}
