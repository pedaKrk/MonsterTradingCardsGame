﻿using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic.Handler
{
    internal class BattleHandler
    {
        private static readonly Queue<PlayerSession> WaitingPlayers = new();

        public static async Task JoinBattleAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
            if (user == null)
            {
                return;
            }

            var playerSession = new PlayerSession(user, responseHandler);

            lock (WaitingPlayers)
            {

                if (WaitingPlayers.Count == 0)
                {
                    Console.WriteLine("Player waiting for a battle...");
                    WaitingPlayers.Enqueue(playerSession);
                    return;
                }

                var opponentSession = WaitingPlayers.Dequeue();
                Task.Run(() => StartBattleAsync(playerSession, opponentSession));
            }
        }

        private static async Task StartBattleAsync(PlayerSession player1, PlayerSession player2)
        {
            Console.WriteLine($"Battle started between {player1.User.Username} and {player2.User.Username}!");

            var (player1Message, player2Message) = SimulateBattle(player1.User, player2.User);

            await player1.ResponseHandler.SendOkAsync(new { message = player1Message });
            await player2.ResponseHandler.SendOkAsync(new { message = player2Message });
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