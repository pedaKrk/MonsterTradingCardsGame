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

                var CardIds = JsonSerializer.Deserialize<List<string>>(requestBody);

                if(CardIds == null)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                foreach (var CardId in CardIds) 
                {
                    Card? card = user.Stack.GetCardById(CardId);

                    if(card == null)
                    {
                        // wrong id in requestBody
                        return;
                    }
                    
                    user.Deck.AddCard(card);
                }


                await responseHandler.SendOkAsync();
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }

        }

        public static async Task HandleGetDeckAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
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

                var cards = user.Deck.GetCards();

                await responseHandler.SendOkAsync(new { cards });
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }

        }
    }
}
