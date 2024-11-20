using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    internal class PackageHandler
    {
        public static async Task HandleCreatePackageAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
        {
            try
            {
                string? authorizationToken = HttpRequestParser.ReadAuthorizationHeader(headers);

                Console.WriteLine($"token: {authorizationToken}");
                if (authorizationToken == null)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                if (!TokenService.HasToken(authorizationToken)) 
                {
                    await responseHandler.SendUnauthorizedAsync();
                    return;
                }

                var cards = JsonSerializer.Deserialize<List<Card>>(requestBody);
                if (cards == null || cards.Count == 0)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                Package package = new Package(cards);
                InMemoryDatabase.AddPackage(package);

                await responseHandler.SendCreatedResponseAsync();
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleAcquirePackageAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
        {
            try
            {
                string? authorizationToken = HttpRequestParser.ReadAuthorizationHeader(headers);

                if (authorizationToken == null)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                if (!TokenService.HasToken(authorizationToken))
                {
                    await responseHandler.SendUnauthorizedAsync();
                    return;
                }

                string? username = TokenService.GetUsernameByToken(authorizationToken);

                if (username == null)
                {
                    await responseHandler.SendUnauthorizedAsync();
                    return;
                }

                var user = InMemoryDatabase.GetUser(username);

                if (user == null) 
                { 
                    await responseHandler.SendUnauthorizedAsync();
                    return;
                }

                if(user.Coins - 5 < 0)
                {
                    //#403 response not enough money
                    return;
                }
                
                Package? package = InMemoryDatabase.AcquirePackage();
                if (package == null)
                {
                    //#5xx no packages
                    return;
                }

                user.Coins -= 5;
                user.Stack.AddCards(package.Open());

                await responseHandler.SendOkAsync();
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }

        }

        public static async Task HandleAddPackageAsync(HttpResponseHandler responseHandler, string requestBody)
        {

        }


    }
}
