using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Models;
using System.Text.Json;
using MonsterTradingCardsGame.BusinessLogic;

namespace MonsterTradingCardsGame.Server
{
    internal class Server
    {

        private static readonly int _port = 10001;

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, string?, Task>> _postRoutes = new()
        {
            { "/users", async (responseHandler, headers, requestBody, parameter) => await UserHandler.HandleUserRegistrationAsync(responseHandler, requestBody) },
            { "/sessions", async (responseHandler, headers, requestBody, parameter) => await UserHandler.HandleUserLoginAsync(responseHandler, requestBody) },
            { "/packages", async (responseHandler, headers, requestBody, parameter) => await PackageHandler.HandleCreatePackageAsync(responseHandler, headers, requestBody) },
            { "/transactions/packages", async (responseHandler, headers, requestBody, parameter) => await PackageHandler.HandleAcquirePackageAsync(responseHandler, headers, requestBody) },
            { "/trades", async (responseHandler, headers, requestBody, parameter) => await TradingHandler.HandleCreateTradeAsync(responseHandler, headers, requestBody) }
        };

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, string?, Task>> _getRoutes = new()
        {
            { "/cards", async (responseHandler, headers, requestBody, parameter) => await CardHandler.HandleGetAllCardsAsync(responseHandler, headers) },
            { "/deck", async (responseHandler, headers, requestBody, parameter) => await DeckHandler.HandleGetDeckAsync(responseHandler, headers) },
            { "/users", async (responseHandler, headers, requestBody, parameter) => await UserHandler.HandleGetUserDataAsync(responseHandler, headers, parameter) },
            { "/trades", async (responseHandler, headers, requestBody, parameter) => await TradingHandler.HandleGetTradesAsync(responseHandler, headers, requestBody) }
        };

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, string?, Task>> _putRoutes = new()
        {
            { "/deck", async (responseHandler, headers, requestBody, parameter) => await DeckHandler.HandleConfigureDeckAsync(responseHandler, headers, requestBody) },
            { "/users", async (responseHandler, headers, requestBody, parameter) => await UserHandler.HandleChangeUserDataAsync(responseHandler, headers, requestBody, parameter) }
        };

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, string?, Task>> _deleteRoutes = new()
        {
        };

        public static void Run()
        {
            Console.WriteLine($"HttpServer-Demo: use http://localhost:{_port}/");

            var server = new TcpListener(IPAddress.Any, _port);
            server.Start();

            while (true)
            {
                Task.Run(() => HandleClientAsync(server.AcceptTcpClient()));
            }
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                using var reader = new StreamReader(client.GetStream());

                var responseHandler = new HttpResponseHandler(writer);

                var (method, path, version) = await HttpRequestParser.ReadRequestLineAsync(reader);
                var headers = await HttpRequestParser.ReadHeadersAsync(reader);
                var contentLength = headers.HasKey("Content-Length") ? int.Parse(headers.GetValue("Content-Length")) : 0;
                var requestBody = await HttpRequestParser.ReadRequestBodyAsync(reader, contentLength);

                Console.WriteLine($"Method: {method}, Path: {path}, Version: {version}");

                await RouteRequestAsync(method, path, headers, requestBody, responseHandler);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        private static async Task RouteRequestAsync(string method, string path, Headers headers, string requestBody, HttpResponseHandler responseHandler)
        {

            Dictionary<string, Func<HttpResponseHandler, Headers, string, string?, Task>>? methodRoutes = method switch
            {
                "POST" => _postRoutes,
                "GET" => _getRoutes,
                "PUT" => _putRoutes,
                "DELETE" => _deleteRoutes,
                _ => null
            };

            //fehler mit dynamischen pfaden (transactions/packages) wird als dynamischer pfad bearbeitet
            (path, string? parameter) = GetDynamicPath(path);

            if (methodRoutes != null && methodRoutes.TryGetValue(path, out Func<HttpResponseHandler, Headers, string, string?, Task>? handler)) 
            {
                await handler(responseHandler, headers, requestBody, parameter);                
                return;
            }

            Console.WriteLine("Method not found");
            await responseHandler.SendNotFoundAsync();
        }

        
        private static (string, string?) GetDynamicPath(string path)
        {
            var parts = path.Split('/');

            //parts[0] is always an empty string
            //since the path starts with a '/'!

            if (parts.Length > 2)
            {
                return ($"/{parts[1]}", parts[2]);
            }

            return (path, null);
        }
        
    }
}

