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

        private static readonly Dictionary<string, Func<string, Headers, string, HttpResponseHandler, string?, Task>> _postRoutes = new()
        {
            { "/users", async (path, headers, body, responseHandler, value) => await UserHandler.HandleUserRegistrationAsync(responseHandler, body) },
            { "/sessions", async (path, headers, body, responseHandler, value) => await UserHandler.HandleUserLoginAsync(responseHandler, body) },
            { "/packages", async (path, headers, body, responseHandler, value) => await PackageHandler.HandleCreatePackageAsync(responseHandler, headers, body) },
            { "/transactions/packages", async (path, headers, body, responseHandler, value) => await PackageHandler.HandleAcquirePackageAsync(responseHandler, headers, body) }
        };

        private static readonly Dictionary<string, Func<string, Headers, string, HttpResponseHandler, string?, Task>> _getRoutes = new()
        {
            { "/cards", async (path, headers, body, responseHandler, value) => await CardHandler.HandleGetAllCardsAsync(responseHandler, headers) },
            { "/deck", async (path, headers, body, responseHandler, value) => await DeckHandler.HandleGetDeckAsync(responseHandler, headers) },
            { "/users", async (path, headers, body, responseHandler, value) => await UserHandler.HandleGetUserAsync(responseHandler, headers, value) } 
        };

        private static readonly Dictionary<string, Func<string, Headers, string, HttpResponseHandler, string?, Task>> _putRoutes = new()
        {
            { "/deck", async (path, headers, body, responseHandler, value) => await DeckHandler.HandleConfigureDeckAsync(responseHandler, headers, body) }
        };

        private static readonly Dictionary<string, Func<string, Headers, string, HttpResponseHandler, string?, Task>> _deleteRoutes = new()
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

            Dictionary<string, Func<string, Headers, string, HttpResponseHandler, string?, Task>>? methodRoutes = method switch
            {
                "POST" => _postRoutes,
                "GET" => _getRoutes,
                "PUT" => _putRoutes,
                "DELETE" => _deleteRoutes,
                _ => null
            };

            (path, string? value) = GetDynamicPath(path);

            Console.WriteLine("path: " + path);
            Console.WriteLine("value: " + value);

            if (methodRoutes != null && methodRoutes.TryGetValue(path, out Func<string, Headers, string, HttpResponseHandler, string?, Task>? handler)) 
            {
                Console.WriteLine("method found");
                await handler(path, headers, requestBody, responseHandler, value);                
                return;
            }

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

