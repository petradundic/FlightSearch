using FlightSearch.Server.Models;
using System.Security.Cryptography;
using System.Text;

namespace FlightSearch.Server.Utility
{
    public static class HashUtility
    {
        public static string GenerateSearchHash(SearchParameter searchParams)
        {
            using var sha256 = SHA256.Create();
            var input = $"{searchParams.Origin}-{searchParams.Destination}-{searchParams.DepartureDate}-{searchParams.ReturnDate}-{searchParams.Passengers}-{searchParams.Currency}";
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashBytes);
        }

    }
}
