using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Exceptions;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using System.Text.Json;

namespace MonsterTradingCardsGame.BusinessLogic.Handlers
{
    internal class ScoreboardHandler
    {
        public static async Task HandleGetScoreboardAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var authorizedUser = HttpRequestParser.AuthenticateAndGetUser(headers);

                List<UserStats> scoreboard = [];

                var users = InMemoryDatabase.GetAllUsers();
                foreach (var user in users)
                {
                    scoreboard.Add(user.Stats);
                }

                scoreboard.Sort((x, y) => y.Elo.CompareTo(x.Elo));

                await responseHandler.SendOkAsync(new { scoreboard });
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
