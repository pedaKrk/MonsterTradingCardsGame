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

        private static int _port = 10001;
        
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

                switch (method.ToUpper())
                {
                    case "GET":
                        await HandleGetAsync(responseHandler, path, headers, requestBody);
                        break;
                    case "POST":
                        await HandlePostAsync(responseHandler, path, headers, requestBody);
                        break;
                    //case "PUT":
                    //    await HandlePutAsync(writer, path, requestBody);
                    //    break;
                    //case "DELETE":
                    //    await HandleDeleteAsync(writer, path);
                    //    break;
                    default:
                        await responseHandler.SendNotFoundAsync();
                        break;
                }

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

        private static async Task HandlePostAsync(HttpResponseHandler responseHandler, string path, Headers headers, string requestBody)
        {
            // ToDo: Dictionary für paths
            Console.WriteLine(path);

            switch (path)
            {
                case "/users":
                    await UserHandler.HandleUserRegistrationAsync(responseHandler, requestBody);
                    break;

                case "/sessions":
                    await UserHandler.HandleUserLoginAsync(responseHandler, requestBody);
                    break;

                case "/packages":
                    await PackageHandler.HandleCreatePackageAsync(responseHandler, headers, requestBody);
                    break;

                case "/transactions/packages":
                    await PackageHandler.HandleAcquirePackageAsync(responseHandler, headers, requestBody);
                    break;

                case "/cards":
                    break;

                case "/deck":
                    break;

                case "/stats":
                    break;

                case "/scoreboard":
                    break;

                case "/tradings":
                    break;

                default:
                    await responseHandler.SendNotFoundAsync();
                    break;
            }       
        }

        private static async Task HandleGetAsync(HttpResponseHandler responseHandler, string path, Headers headers, string requestBody)
        {
            switch (path)
            {
                case "/cards":
                    await CardHandler.HandleGetAllCardsAsync(responseHandler, headers, requestBody);
                    break;

                default:
                    await responseHandler.SendNotFoundAsync();
                    break;
            }
        }

        }
}

