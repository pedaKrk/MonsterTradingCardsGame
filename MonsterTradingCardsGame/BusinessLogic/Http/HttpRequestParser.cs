using MonsterTradingCardsGame.BusinessLogic.Token;
using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic.Http
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
            if (contentLength == 0) return string.Empty;

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

        public static async Task<User?> AuthenticateAndGetUserAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            string? authorizationToken = ReadAuthorizationHeader(headers);
            if (authorizationToken == null)
            {
                await responseHandler.SendBadRequestAsync();
                return null;
            }

            if (!TokenService.HasToken(authorizationToken))
            {
                await responseHandler.SendUnauthorizedAsync();
                return null;
            }

            string? username = TokenService.GetUsernameByToken(authorizationToken);
            if (username == null)
            {
                await responseHandler.SendUnauthorizedAsync();
                return null;
            }

            var user = InMemoryDatabase.GetUser(username);
            if (user == null)
            {
                await responseHandler.SendUnauthorizedAsync();
                return null;
            }

            return user;
        }
    }
}
