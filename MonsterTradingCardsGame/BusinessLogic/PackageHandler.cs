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
        public static async Task HandleCreatePackageAsync(HttpResponseHandler responseHandler, string requestBody)
        {
            var cards = JsonSerializer.Deserialize<List<Card>>(requestBody);

            if (cards == null || cards.Count == 0) {
                await responseHandler.SendBadRequestAsync();
                return;
            }

            Package package = new Package(cards);
            InMemoryDatabase.AddPackage(package);

            await responseHandler.SendCreatedResponseAsync();
        }

        public static async Task HandleAcquirePackageAsync(HttpResponseHandler responseHandler, string requestBody)
        {

        }

        public static async Task HandleAddPackageAsync(HttpResponseHandler responseHandler, string requestBody)
        {

        }


    }
}
