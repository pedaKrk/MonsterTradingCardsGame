using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic.Handler
{
    internal class ScoreboardHandler
    {
        public static async Task HandleGetScoreboardAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var authorizedUser = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (authorizedUser == null)
                {
                    return;
                }

                List<UserStats> scoreboard = [];

                var users = InMemoryDatabase.GetAllUsers();
                foreach (var user in users)
                {
                    scoreboard.Add(user.Stats);
                }

                scoreboard.Sort((x, y) => y.Elo.CompareTo(x.Elo));

                await responseHandler.SendOkAsync(new { scoreboard });
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }
    }
}
