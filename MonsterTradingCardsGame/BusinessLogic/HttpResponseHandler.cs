using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    internal class HttpResponseHandler
    {
        private readonly StreamWriter _writer;

        public HttpResponseHandler(StreamWriter writer) 
        {
            _writer = writer;
        }

        private async Task SendResponseAsync(string statusCode, string? contentType, string? responseBody)
        {
            _writer.WriteLine($"HTTP/1.0 {statusCode}");
            if (!string.IsNullOrEmpty(contentType))
            {
                _writer.WriteLine($"Content-Type: {contentType}");
            }

            if (!string.IsNullOrEmpty(responseBody))
            {
                _writer.WriteLine($"Content-Length: {responseBody.Length}");
                _writer.WriteLine(); // Empty line indicates the end of headers
                await _writer.WriteLineAsync(responseBody);
            }
            else
            {
                _writer.WriteLine(); // Only headers, no body
            }
        }

        public async Task SendCreatedResponseAsync(User user)
        {
            var responseBody = JsonSerializer.Serialize(user);
            await SendResponseAsync("201 Created", "application/json", responseBody);
        }

        public async Task SendConflictResponseAsync()
        {
            await SendResponseAsync("409 Conflict", null, null);
        }

        public async Task SendBadRequestAsync()
        {
            await SendResponseAsync("400 Bad Request", null, null);
        }

        public async Task SendNotFoundAsync()
        {
            await SendResponseAsync("404 Not Found", null, null);
        }
    }
}
