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

                var tradingDeal = JsonSerializer.Deserialize<TradingDeal>(requestbody);
                if(tradingDeal == null) 
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                var card = user.Stack.GetCardById(tradingDeal.CardId);
                if(card == null)
                {
                    await responseHandler.SendForbiddenAsync();
                    return;
                }

                if (InMemoryDatabase.TradingDealExists(tradingDeal.Id))
                {
                    await responseHandler.SendConflictResponseAsync();
                    return;
                }

                user.Stack.RemoveCard(card);
                InMemoryDatabase.AddTradingDeal(tradingDeal);

                await responseHandler.SendCreatedResponseAsync();
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleGetTradesAsync(HttpResponseHandler responseHandler, Headers headers, string requestbody)
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
    }
}
