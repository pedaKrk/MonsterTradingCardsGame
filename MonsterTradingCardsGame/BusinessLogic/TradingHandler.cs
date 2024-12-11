using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
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

                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                    PropertyNameCaseInsensitive = true
                };

                var tradingDealDTO = JsonSerializer.Deserialize<TradingDealDTO>(requestbody, options);
                if (tradingDealDTO == null)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                var tradingDeal = new TradingDeal(tradingDealDTO.Id, tradingDealDTO.CardId, user.Username ,tradingDealDTO.Price);

                var card = user.Stack.GetCardById(tradingDeal.CardId);
                if(card == null)
                {
                    await responseHandler.SendForbiddenAsync(new {error = "the user doesn't own this card."});
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
                if(tradingDeals == null)
                {
                    await responseHandler.SendInternalServerErrorAsync();
                    return;
                }

                if (tradingDeals.Count == 0)
                {
                    await responseHandler.SendNoContentAsync();
                    return;
                }

                await responseHandler.SendOkAsync(new {tradingDeals});
                                
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

                if(tradingDealId == null)
                {
                    await responseHandler.SendBadRequestAsync( new { error = "tradingDealId is required."});
                    return;
                }

                var tradingDeal = InMemoryDatabase.GetTradingDeal(tradingDealId);
                if (tradingDeal == null)
                {
                    await responseHandler.SendNotFoundAsync();
                    return;
                }

                if (!user.Stack.HasCard(tradingDeal.CardId)) {
                    await responseHandler.SendForbiddenAsync(new {error = "The deal contains a card that is not owned by the user." });
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
        public static async Task HandleAcceptTradingDealAsync(HttpResponseHandler responseHandler, Headers headers, string requestbody, string? tradingDealId)
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
    }
}
