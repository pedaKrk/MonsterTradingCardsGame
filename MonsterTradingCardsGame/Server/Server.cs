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
using MonsterTradingCardsGame.BusinessLogic.Handler;
using MonsterTradingCardsGame.BusinessLogic.Http;

namespace MonsterTradingCardsGame.Server
{
    internal class Server
    {

        private static readonly int _port = 10001;

        /*
         * ToDo: - Battle implementieren
         *       - UserStats (name wird nicht geupdated)
         *       - Endpunkte testen
         *       - Unique feature marketplace wo cards coins wert sind !!
         *       - RoleSystem
         *       - DB
         *       - UnitTests
         */

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, Dictionary<string, string>?, Task>> _postRoutes = new()
        {
            { "/users", async (responseHandler, headers, requestBody, parameters) => await UserHandler.HandleUserRegistrationAsync(responseHandler, requestBody) },
            { "/sessions", async (responseHandler, headers, requestBody, parameters) => await UserHandler.HandleUserLoginAsync(responseHandler, requestBody) },
            { "/packages", async (responseHandler, headers, requestBody, parameters) => await PackageHandler.HandleCreatePackageAsync(responseHandler, headers, requestBody) },
            { "/transactions/packages", async (responseHandler, headers, requestBody, parameters) => await PackageHandler.HandleAcquirePackageAsync(responseHandler, headers, requestBody) },
            { "/tradings", async (responseHandler, headers, requestBody, parameters) => await TradingHandler.HandleCreateTradeAsync(responseHandler, headers, requestBody) },
            { "/tradings/{tradingdealid}", async (responseHandler, headers, requestBody, parameters) => await TradingHandler.HandleAcceptTradingDealAsync(responseHandler, headers, parameters?["tradingdealid"]) }
        };

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, Dictionary<string, string>?, Task>> _getRoutes = new()
        {
            { "/cards", async (responseHandler, headers, requestBody, parameters) => await CardHandler.HandleGetAllCardsAsync(responseHandler, headers) },
            { "/deck", async (responseHandler, headers, requestBody, parametes) => await DeckHandler.HandleGetDeckAsync(responseHandler, headers) },
            { "/users/{username}", async (responseHandler, headers, requestBody, parameters) => await UserHandler.HandleGetUserDataAsync(responseHandler, headers, parameters?["username"]) },
            { "/tradings", async (responseHandler, headers, requestBody, parameters) => await TradingHandler.HandleGetTradesAsync(responseHandler, headers) },
            { "/stats", async (responseHandler, headers, requestBody, parameters) => await UserHandler.HandleGetUserStatsAsync(responseHandler, headers) },
            { "/scoreboard", async (responseHandler, headers, requestBody, parameters) => await ScoreboardHandler.HandleGetScoreboardAsync(responseHandler, headers) }
        };

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, Dictionary<string, string>?, Task>> _putRoutes = new()
        {
            { "/deck", async (responseHandler, headers, requestBody, parameters) => await DeckHandler.HandleConfigureDeckAsync(responseHandler, headers, requestBody) },
            { "/users", async (responseHandler, headers, requestBody, parameters) => await UserHandler.HandleChangeUserDataAsync(responseHandler, headers, requestBody, parameters?["username"]) }
        };

        private static readonly Dictionary<string, Func<HttpResponseHandler, Headers, string, Dictionary<string, string>?, Task>> _deleteRoutes = new()
        {
            { "/tradings/{tradingdealid}", async (responseHandler, headers, requestBody, parameters) => await TradingHandler.HandleDeleteTradingDealAsync(responseHandler, headers, requestBody, parameters?["tradingdealid"]) }
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

            var methodRoutes = method switch
            {
                "POST" => _postRoutes,
                "GET" => _getRoutes,
                "PUT" => _putRoutes,
                "DELETE" => _deleteRoutes,
                _ => null
            };

            if (methodRoutes == null)
            {
                Console.WriteLine("Unsupported HTTP method");
                await responseHandler.SendNotFoundAsync();
                return;
            }

            var (matchedRoute, parameters) = MatchPath(path, methodRoutes);

            if (matchedRoute != null && methodRoutes.TryGetValue(matchedRoute, out var handler)) 
            {
                await handler(responseHandler, headers, requestBody, parameters);                
                return;
            }

            Console.WriteLine($"No route found for Path: {path}");
            await responseHandler.SendNotFoundAsync();
        }

        //code von chatgpt
        //aber es funktioniert
        private static (string?, Dictionary<string, string>?) MatchPath(
            string path,
            Dictionary<string, Func<HttpResponseHandler, Headers, string, Dictionary<string, string>?, Task>> routes)
        {
            foreach (var route in routes.Keys)
            {
                var routeParts = route.Split('/');
                var pathParts = path.Split('/');

                if (routeParts.Length != pathParts.Length)
                    continue;

                var parameters = new Dictionary<string, string>();
                var isMatch = true;

                for (int i = 0; i < routeParts.Length; i++)
                {
                    if (routeParts[i].StartsWith("{") && routeParts[i].EndsWith("}"))
                    {
                        var key = routeParts[i].Trim('{', '}');
                        parameters[key] = pathParts[i];
                    }
                    else if (routeParts[i] != pathParts[i])
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                    return (route, parameters);
            }

            return (null, null);
        }

    }
}

