using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.BusinessLogic.Services;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.Http
{
    internal class HttpRequestParser
    {
        public static async Task<(string method, string path, string version)> ReadRequestLineAsync(StreamReader reader)
        {
            var line = await reader.ReadLineAsync() ?? throw new Exception("Invalid HTTP request");

            var httpParts = line.Split(' ');
            return (httpParts[0], httpParts[1], httpParts[2]);
        }

        public static async Task<Headers> ReadHeadersAsync(StreamReader reader)
        {
            var headers = new Headers();
            string? line;

            while (!string.IsNullOrWhiteSpace(line = await reader.ReadLineAsync()))
            {
                var headerParts = line.Split(':');
                headers.AddHeader(headerParts[0], headerParts[1].Trim());
            }

            return headers;
        }

        public static async Task<string> ReadRequestBodyAsync(StreamReader reader, int contentLength)
        {
            if (contentLength == 0)
            {
                return string.Empty;
            }

            var buffer = new char[contentLength];
            await reader.ReadBlockAsync(buffer, 0, contentLength);
            return new string(buffer);
        }

        private static string? ReadAuthorizationHeader(Headers headers)
        {
            const string AuthorizationHeaderKey = "Authorization";
            const string BearerPrefix = "Bearer";

            string? authorizationHeaderValue = headers.GetValue(AuthorizationHeaderKey).Trim();

            if (authorizationHeaderValue == null)
            {
                return null;
            }

            if (!authorizationHeaderValue.StartsWith(BearerPrefix))
            {
                return null;
            }

            return authorizationHeaderValue[BearerPrefix.Length..].Trim();
        }

        public static User AuthenticateAndGetUser(Headers headers)
        {
            UserRepository userRepository = new();
            string? authorizationToken = ReadAuthorizationHeader(headers) ?? throw new BadRequestException("bad request");
            string? username = TokenService.GetUsernameByToken(authorizationToken) ?? throw new UnauthorizedException("user not found.");

            Console.WriteLine($"{username}: {authorizationToken}");
            return userRepository.GetUserByUsername(username) ?? throw new UnauthorizedException("user not found.");
        }
    }
}
