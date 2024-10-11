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
                _writer.WriteLine();
                await _writer.WriteLineAsync(responseBody);
            }
            else
            {
                _writer.WriteLine();
            }
        }

        public async Task SendOkAsync(object? responseBody = null)
        {
            if (responseBody != null)
            {
                var jsonResponse = JsonSerializer.Serialize(responseBody);
                await SendResponseAsync("200 OK", "application/json", jsonResponse);
                return;
            }
            
            await SendResponseAsync("200 OK", null, null);          
        }

        public async Task SendCreatedResponseAsync()
        {
            await SendResponseAsync("201 Created", null, null);
        }

        public async Task SendConflictResponseAsync()
        {
            await SendResponseAsync("409 Conflict", null, null);
        }

        public async Task SendBadRequestAsync()
        {
            await SendResponseAsync("400 Bad Request", null, null);
        }

        public async Task SendUnauthorizedAsync()
        {
            await SendResponseAsync("401 Unauthorized", null, null);
        }

        public async Task SendNotFoundAsync()
        {
            await SendResponseAsync("404 Not Found", null, null);
        }

        public async Task SendInternalServerErrorAsync()
        {
            await SendResponseAsync("500 Internal Server Error", null, null);
        }
    }
}
