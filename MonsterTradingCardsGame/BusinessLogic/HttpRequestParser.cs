using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    internal class HttpRequestParser
    {
        public static async Task<(string method, string path, string version)> ReadRequestLineAsync(StreamReader reader)
        {
            var line = await reader.ReadLineAsync() ?? throw new Exception("Invalid HTTP request");

            var httpParts = line.Split(' ');
            return (httpParts[0], httpParts[1], httpParts[2]);
        }

        public static async Task<Dictionary<string, string>> ReadHeadersAsync(StreamReader reader)
        {
            var headers = new Dictionary<string, string>();
            string? line;

            while (!string.IsNullOrWhiteSpace(line = await reader.ReadLineAsync()))
            {
                var headerParts = line.Split(':');
                headers[headerParts[0]] = headerParts[1].Trim();
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
    }
}
