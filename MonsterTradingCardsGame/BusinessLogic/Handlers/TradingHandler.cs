using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using System.Text.Json;

namespace MonsterTradingCardsGame.BusinessLogic.Handlers
{
    internal class TradingHandler
    {
        public static async Task HandleCreateTradeAsync(HttpResponseHandler responseHandler, Headers headers, string requestbody)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                var tradingDealDTO = JsonSerializer.Deserialize<TradingDealDTO>(requestbody) ?? throw new BadRequestException("bad json.");

                var tradingDeal = new TradingDeal(tradingDealDTO, user.Username);

                var card = user.Stack.GetCardById(tradingDeal.CardId) ?? throw new ForbiddenException("the user doesn't own this card.");

                if (InMemoryDatabase.TradingDealExists(tradingDeal.Id))
                {
                    throw new ConflictException("tradingdeal already exists");
                }

                InMemoryDatabase.AddTradingDeal(tradingDeal);

                await responseHandler.SendCreatedAsync();
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
            catch (ForbiddenException ex)
            {
                await responseHandler.SendForbiddenAsync(ex.Message);
            }
            catch (ConflictException ex)
            {
                await responseHandler.SendConflictAsync(ex.Message);
            }
        }

        public static async Task HandleGetTradesAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                var tradingDeals = InMemoryDatabase.TradingDeals ?? throw new InternalServerException();
                if (tradingDeals.Count == 0)
                {
                    await responseHandler.SendNoContentAsync();
                    return;
                }

                await responseHandler.SendOkAsync(new { tradingDeals });
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
            catch (InternalServerException)
            {
                await responseHandler.SendInternalServerErrorAsync();
            }
        }

        public static async Task HandleDeleteTradingDealAsync(HttpResponseHandler responseHandler, Headers headers, string requestbody, string? tradingDealId)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                if (tradingDealId == null)
                {
                    throw new BadRequestException("tradingDealId is required.");
                }

                var tradingDeal = InMemoryDatabase.GetTradingDeal(tradingDealId) ?? throw new NotFoundException("no tradingDeal with this id found.");
                if (!user.Stack.HasCard(tradingDeal.CardId))
                {
                    throw new ForbiddenException("The deal contains a card that is not owned by the user.");
                }

                InMemoryDatabase.DeleteTradingDeal(tradingDealId);

                await responseHandler.SendOkAsync();
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
            catch (NotFoundException ex)
            {
                await responseHandler.SendNotFoundAsync(ex.Message);
            }
        }

        public static async Task HandleAcceptTradingDealAsync(HttpResponseHandler responseHandler, Headers headers, string? tradingDealId)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                if (tradingDealId == null)
                {
                    throw new BadRequestException("tradingDealId is required.");
                }

                var tradingDeal = InMemoryDatabase.GetTradingDeal(tradingDealId) ?? throw new NotFoundException("no tradingDeal with this id found.");

                if (user.Username == tradingDeal.Username)
                {
                    throw new ForbiddenException("user can't trade with self.");
                }

                var offerer = InMemoryDatabase.GetUser(tradingDeal.Username) ?? throw new InternalServerException();
                var card = offerer.Stack.GetCardById(tradingDeal.CardId) ?? throw new ForbiddenException("offerer doesn't own this card.");

                if (user.Coins - tradingDeal.Price < 0)
                {
                    throw new ForbiddenException("user doens't have enough coins to purchse this card.");
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
            catch (NotFoundException ex)
            {
                await responseHandler.SendNotFoundAsync(ex.Message);
            }
            catch (ForbiddenException ex)
            {
                await responseHandler.SendForbiddenAsync(ex.Message);
            }
            catch (InternalServerException)
            {
                await responseHandler.SendInternalServerErrorAsync();
            }
        }
    }
}
