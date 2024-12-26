using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Exceptions;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using System.Text.Json;

namespace MonsterTradingCardsGame.BusinessLogic.Handlers
{
    internal class CardHandler
    {
        public static async Task HandleGetAllCardsAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                List<Card> cards = user.Stack.GetAllCards();
                cards.AddRange(user.Deck.GetCards());

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
            catch (UnauthorizedException ex)
            {
                await responseHandler.SendUnauthorizedAsync(ex.Message);
            }
        }
    }
}
