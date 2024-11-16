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
    internal class CardHandler
    {
        public static async Task HandleGetAllCardsAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
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

                var user = InMemoryDatabase.Users.FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    Console.WriteLine("user == null");
                    await responseHandler.SendUnauthorizedAsync();
                    return;
                }

                List<Card> cards = user.GetStack();

                if(cards.Count == 0)
                {
                    //#204 user has no cards
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
