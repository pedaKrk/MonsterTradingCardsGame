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

            foreach(Card card in cards)
            {
                Console.WriteLine($"card: {card.ToString()}");
            }

        }

        public static async Task HandleAcquirePackageAsync(HttpResponseHandler responseHandler, string requestBody)
        {

        }

        public static async Task HandleAddPackageAsync(HttpResponseHandler responseHandler, string requestBody)
        {

        }


    }
}
