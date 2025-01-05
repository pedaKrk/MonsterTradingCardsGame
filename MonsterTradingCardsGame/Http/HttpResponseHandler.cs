using MonsterTradingCardsGame.Http.Interfaces;
using System.Text.Json;

namespace MonsterTradingCardsGame.Http
{
    public class HttpResponseHandler(StreamWriter writer) : IHttpResponseHandler
    {
        private readonly StreamWriter _writer = writer;

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

        public async Task SendCreatedAsync()
        {
            await SendResponseAsync("201 Created", null, null);
        }

        public async Task SendNoContentAsync()
        {
            await SendResponseAsync("204 No Content", null, null);
        }

        public async Task SendBadRequestAsync(object? responseBody = null)
        {
            if (responseBody != null)
            {
                var jsonResponse = JsonSerializer.Serialize(new { error = responseBody });
                await SendResponseAsync("400 Bad Request", "application/json", jsonResponse);
                return;
            }

            await SendResponseAsync("400 Bad Request", null, null);
        }

        public async Task SendUnauthorizedAsync(object? responseBody = null)
        {
            if (responseBody != null)
            {
                var jsonResponse = JsonSerializer.Serialize(new { error = responseBody });
                await SendResponseAsync("401 Unauthorized", "application/json", jsonResponse);
                return;
            }

            await SendResponseAsync("401 Unauthorized", null, null);
        }

        public async Task SendForbiddenAsync(object? responseBody = null)
        {
            if (responseBody != null)
            {
                var jsonResponse = JsonSerializer.Serialize(new { error = responseBody });
                await SendResponseAsync("403 Forbidden", "application/json", jsonResponse);
                return;
            }

            await SendResponseAsync("403 Forbidden", null, null);
        }

        public async Task SendNotFoundAsync(object? responseBody = null)
        {
            if (responseBody != null)
            {
                var jsonResponse = JsonSerializer.Serialize(new { error = responseBody });
                await SendResponseAsync("404 Not Found", "application/json", jsonResponse);
                return;
            }

            await SendResponseAsync("404 Not Found", null, null);
        }

        public async Task SendConflictAsync(object? responseBody = null)
        {
            if (responseBody != null)
            {
                var jsonResponse = JsonSerializer.Serialize(new { error = responseBody });
                await SendResponseAsync("409 Conflict", "application/json", jsonResponse);
                return;
            }

            await SendResponseAsync("409 Conflict", null, null);
        }

        public async Task SendInternalServerErrorAsync()
        {
            await SendResponseAsync("500 Internal Server Error", null, null);
        }
    }
}
