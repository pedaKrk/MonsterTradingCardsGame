using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    internal class DeckHandler
    {
        public static async Task HandleConfigureDeckAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                var cardIds = JsonSerializer.Deserialize<List<string>>(requestBody);
                if(cardIds == null)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                if (cardIds.Count != Deck.DeckSize)
                {
                    await responseHandler.SendBadRequestAsync(new { error = "The provided deck did not include the required amount of cards" });
                    return;
                }

                var cards = new List<Card>(Deck.DeckSize);
                foreach (var cardId in cardIds) 
                {
                    Card? card = user.Stack.GetCardById(cardId);
                    if(card == null)
                    {
                        await responseHandler.SendForbiddenAsync(new {error = "At least one of the provided cards does not belong to the user or is not available." });
                        return;
                    }

                    cards.Add(card);
                }

                user.Deck.AddCards(cards);

                await responseHandler.SendOkAsync();
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleGetDeckAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                var cards = user.Deck.GetCards();

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
