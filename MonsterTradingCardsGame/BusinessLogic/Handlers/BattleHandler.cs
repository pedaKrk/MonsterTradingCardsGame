using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.BusinessLogic.Handlers
{
    internal class BattleHandler
    {
        private static readonly Queue<PlayerSession> WaitingPlayers = new();

        public static async Task JoinBattleAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            //funktioniert nicht
            var user = HttpRequestParser.AuthenticateAndGetUser(headers);

            var playerSession = new PlayerSession(user, responseHandler);

            lock (WaitingPlayers)
            {
                if (WaitingPlayers.Count == 0)
                {
                    WaitingPlayers.Enqueue(playerSession);
                }
                while(WaitingPlayers.Count == 0)
                {
                    Console.WriteLine("Player waiting for a battle...");
                }

                var opponentSession = WaitingPlayers.Dequeue();
                Console.WriteLine("Starting battle...");

                // Check if the opponent's session and response handler are still valid
                if (opponentSession.ResponseHandler == null || opponentSession.User == null)
                {
                    Console.WriteLine("Opponent's session or response handler has been disposed.");
                    return;
                }

                // Proceed to start the battle in a separate task
                Task.Run(() => StartBattleAsync(playerSession, opponentSession));
            }
        }

        private static async Task StartBattleAsync(PlayerSession player1, PlayerSession player2)
        {
            if (player1.ResponseHandler == null || player2.ResponseHandler == null)
            {
                Console.WriteLine("One of the players has a disposed ResponseHandler.");
                return;
            }

            Console.WriteLine($"Battle started between {player1.User.Username} and {player2.User.Username}!");

            var (player1Message, player2Message) = SimulateBattle(player1.User, player2.User);

            // Safely check and send responses
            if (player1.ResponseHandler != null)
            {
                await player1.ResponseHandler.SendOkAsync(new { message = player1Message });
            }

            if (player2.ResponseHandler != null)
            {
                await player2.ResponseHandler.SendOkAsync(new { message = player2Message });
            }
        }

        private static (string player1Message, string player2Message) SimulateBattle(User player1, User player2)
        {
            // Placeholder for actual battle logic
            var random = new Random();
            var player1Wins = random.Next(0, 2) == 0;

            return player1Wins
                ? ("You won!", "You lost!")
                : ("You lost!", "You won!");
        }
    }
}
