﻿using MonsterTradingCardsGame.BusinessLogic.Http;
using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic.Handler
{
    internal class TradingHandler
    {
        public static async Task HandleCreateTradeAsync(HttpResponseHandler responseHandler, Headers headers, string requestbody)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                var tradingDealDTO = JsonSerializer.Deserialize<TradingDealDTO>(requestbody);
                if (tradingDealDTO == null)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                var tradingDeal = new TradingDeal(tradingDealDTO, user.Username);

                var card = user.Stack.GetCardById(tradingDeal.CardId);
                if (card == null)
                {
                    await responseHandler.SendForbiddenAsync(new { error = "the user doesn't own this card." });
                    return;
                }

                if (InMemoryDatabase.TradingDealExists(tradingDeal.Id))
                {
                    await responseHandler.SendConflictResponseAsync();
                    return;
                }

                InMemoryDatabase.AddTradingDeal(tradingDeal);

                await responseHandler.SendCreatedResponseAsync();
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleGetTradesAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                var tradingDeals = InMemoryDatabase.TradingDeals;
                if (tradingDeals == null)
                {
                    await responseHandler.SendInternalServerErrorAsync();
                    return;
                }

                if (tradingDeals.Count == 0)
                {
                    await responseHandler.SendNoContentAsync();
                    return;
                }

                await responseHandler.SendOkAsync(new { tradingDeals });

            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleDeleteTradingDealAsync(HttpResponseHandler responseHandler, Headers headers, string requestbody, string? tradingDealId)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                if (tradingDealId == null)
                {
                    await responseHandler.SendBadRequestAsync(new { error = "tradingDealId is required." });
                    return;
                }

                var tradingDeal = InMemoryDatabase.GetTradingDeal(tradingDealId);
                if (tradingDeal == null)
                {
                    await responseHandler.SendNotFoundAsync();
                    return;
                }

                if (!user.Stack.HasCard(tradingDeal.CardId))
                {
                    await responseHandler.SendForbiddenAsync(new { error = "The deal contains a card that is not owned by the user." });
                    return;
                }

                InMemoryDatabase.DeleteTradingDeal(tradingDealId);

                await responseHandler.SendOkAsync();
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                await responseHandler.SendBadRequestAsync();
            }
        }
        public static async Task HandleAcceptTradingDealAsync(HttpResponseHandler responseHandler, Headers headers, string? tradingDealId)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                if (tradingDealId == null)
                {
                    await responseHandler.SendBadRequestAsync(new { error = "tradingDealId is required." });
                    return;
                }

                var tradingDeal = InMemoryDatabase.GetTradingDeal(tradingDealId);
                if (tradingDeal == null)
                {
                    await responseHandler.SendNotFoundAsync();
                    return;
                }

                if (user.Username == tradingDeal.Username)
                {
                    await responseHandler.SendForbiddenAsync(new { error = "user can't trade with self." });
                    return;
                }

                var offerer = InMemoryDatabase.GetUser(tradingDeal.Username);
                if (offerer == null)
                {
                    await responseHandler.SendInternalServerErrorAsync();
                    return;
                }

                var card = offerer.Stack.GetCardById(tradingDeal.CardId);
                if (card == null)
                {
                    await responseHandler.SendForbiddenAsync(new { error = "offerer doesn't own this card." });
                    return;
                }

                if (user.Coins - tradingDeal.Price < 0)
                {
                    await responseHandler.SendForbiddenAsync(new { error = "user doens't have enough coins to purchse this card." });
                    return;
                }

                user.Coins -= tradingDeal.Price;
                offerer.Coins += tradingDeal.Price;
                offerer.Stack.RemoveCard(card);
                user.Stack.AddCard(card);

                InMemoryDatabase.DeleteTradingDeal(tradingDealId);

                await responseHandler.SendOkAsync();
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                await responseHandler.SendBadRequestAsync();
            }
        }
    }
}
