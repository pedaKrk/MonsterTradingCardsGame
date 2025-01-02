using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.DAL.Repositories;
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
                var tradingDeal = new TradingDeal(tradingDealDTO.Id, tradingDealDTO.CardId, tradingDealDTO.Price, user.Username);

                StackRepository stackRepository = new();
                var card = stackRepository.GetCardFromUser(user.Id, tradingDeal.CardId) ?? throw new ForbiddenException("the user doesn't own this card.");

                TradingDealRepository tradingDealRepository = new();
                if (tradingDealRepository.TradingDealExists(tradingDeal.Id))
                {
                    throw new ConflictException("tradingdeal already exists");
                }

                tradingDealRepository.AddTradingDeal(tradingDeal);

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

                TradingDealRepository tradingDealRepository = new();
                var tradingDeals = tradingDealRepository.GetAllTradingDeals();
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

        public static async Task HandleDeleteTradingDealAsync(HttpResponseHandler responseHandler, Headers headers, string? tradingDealIdString)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                if (tradingDealIdString == null)
                {
                    throw new BadRequestException("tradingDealId is required.");
                }

                var tradingDealId = Guid.Parse(tradingDealIdString);

                TradingDealRepository tradingDealRepository = new();
                var tradingDeal = tradingDealRepository.GetTradingDeal(tradingDealId) ?? throw new NotFoundException("no tradingDeal with this id found.");

                StackRepository stackRepository = new();
                if (!stackRepository.UserHasCard(user.Id, tradingDeal.CardId))
                {
                    throw new ForbiddenException("The deal contains a card that is not owned by the user.");
                }

                tradingDealRepository.DeleteTradingDeal(tradingDealId);

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

        public static async Task HandleAcceptTradingDealAsync(HttpResponseHandler responseHandler, Headers headers, string? tradingDealIdString)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                if (tradingDealIdString == null)
                {
                    throw new BadRequestException("tradingDealId is required.");
                }

                var tradingDealId = Guid.Parse(tradingDealIdString);

                TradingDealRepository tradingDealRepository = new();
                var tradingDeal = tradingDealRepository.GetTradingDeal(tradingDealId) ?? throw new NotFoundException("no tradingDeal with this id found.");

                if (user.Username == tradingDeal.Username)
                {
                    throw new ForbiddenException("user can't trade with self.");
                }

                UserRepository userRepository = new();
                var offerer = userRepository.GetUserByUsername(tradingDeal.Username) ?? throw new InternalServerException();

                StackRepository stackRepository = new();
                var card = stackRepository.GetCardFromUser(offerer.Id, tradingDeal.CardId) ?? throw new ForbiddenException("offerer doesn't own this card.");

                if (user.Coins - tradingDeal.Price < 0)
                {
                    throw new ForbiddenException("user doens't have enough coins to purchse this card.");
                }

                user.Coins -= tradingDeal.Price;
                userRepository.UpdateUser(user);

                offerer.Coins += tradingDeal.Price;
                userRepository.UpdateUser(offerer);

                stackRepository.RemoveCard(offerer.Id, card.Id);
                stackRepository.AddCard(user.Id, card.Id);

                tradingDealRepository.DeleteTradingDeal(tradingDealId);

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
