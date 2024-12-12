using MonsterTradingCardsGame.BusinessLogic.Http;
using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic.Handler
{
    internal class CardHandler
    {
        public static async Task HandleGetAllCardsAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                List<Card> cards = user.Stack.GetAllCards();
                cards.AddRange(user.Deck.GetCards());

                if (cards.Count == 0)
                {
                    await responseHandler.SendNoContentAsync();
                    return;
                }

                await responseHandler.SendOkAsync(new { cards });
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }
    }
}
