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
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                if (user.Role != Role.Admin)
                {
                    await responseHandler.SendForbiddenAsync(new {message = "provided user is not admin!" });
                    return;
                }

                var cards = JsonSerializer.Deserialize<List<Card>>(requestBody);
                if (cards == null || cards.Count == 0)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                //--error--
                //'409':
                //description: At least one card in the packages already exists

                var package = new Package(cards);
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
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                if (user.Coins - Package.Price < 0)
                {
                    await responseHandler.SendForbiddenAsync(new { error = "not enough money to acquire a package!" });
                    return;
                }
                
                Package? package = InMemoryDatabase.AcquirePackage();
                if (package == null)
                {
                    await responseHandler.SendNotFoundAsync();
                    return;
                }

                user.Coins -= Package.Price;
                user.Stack.AddCards(package.Open());

                await responseHandler.SendOkAsync();
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }
    }
}
