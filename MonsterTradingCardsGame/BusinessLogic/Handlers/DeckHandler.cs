using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using System.Text.Json;


namespace MonsterTradingCardsGame.BusinessLogic.Handlers
{
    internal class DeckHandler
    {
        public static async Task HandleConfigureDeckAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                var cardIds = JsonSerializer.Deserialize<List<string>>(requestBody) ?? throw new BadRequestException("bad json.");

                if (cardIds.Count != Deck.DeckSize)
                {
                    throw new BadRequestException("The provided deck did not include the required amount of cards");
                }

                var cards = new List<Card>(Deck.DeckSize);
                foreach (var cardId in cardIds)
                {
                    Card? card = user.Stack.GetCardById(cardId) ?? throw new BadRequestException("At least one of the provided cards does not belong to the user or is not available.");
                    cards.Add(card);
                }

                user.Deck.AddCards(cards);

                await responseHandler.SendOkAsync();
            }
            catch (JsonException ex)
            {
                await responseHandler.SendBadRequestAsync(ex.Message);
            }
            catch (BadRequestException ex)
            {
                await responseHandler.SendBadRequestAsync(ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                await responseHandler.SendUnauthorizedAsync(ex.Message);
            }
        }

        public static async Task HandleGetDeckAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                var cards = user.Deck.GetCards();

                if (cards.Count == 0)
                {
                    await responseHandler.SendNoContentAsync();
                    return;
                }

                await responseHandler.SendOkAsync(new { cards });
            }
            catch (JsonException ex)
            {
                await responseHandler.SendBadRequestAsync(ex.Message);
            }
            catch (BadRequestException ex)
            {
                await responseHandler.SendBadRequestAsync(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await responseHandler.SendUnauthorizedAsync(ex.Message);
            }
        }
    }
}
