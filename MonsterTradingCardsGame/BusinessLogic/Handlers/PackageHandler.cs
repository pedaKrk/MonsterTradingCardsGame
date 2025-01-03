using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.BusinessLogic.Handlers
{
    internal class PackageHandler
    {
        public static async Task HandleCreatePackageAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                /*
                 * erst wieder einfuegen, wenn Role System implementiert ist
                 * 
                if (user.Role != Role.Admin)
                {
                    await responseHandler.SendForbiddenAsync(new {message = "provided user is not admin!" });
                    return;
                }
                */

                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                    PropertyNameCaseInsensitive = true
                };

                var cards = JsonSerializer.Deserialize<List<Card>>(requestBody, options);
                if (cards == null || cards.Count != Package.Size)
                {
                    throw new BadRequestException("bad json.");
                }

                //--error--
                //'409':
                //description: At least one card in the packages already exists

                CardRepository cardRepository = new();

                foreach (var card in cards) {
                    cardRepository.CreateCard(card);
                }

                PackageRepository packageRepository = new();

                var package = new Package(cards);
                packageRepository.CreatePackage(package);

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
        }

        public static async Task HandleAcquirePackageAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                if (user.Coins - Package.Price < 0)
                {
                    throw new ForbiddenException("not enough money to acquire a package!");
                }

                PackageRepository packageRepository = new();
                Package? package = packageRepository.AcquirePackage();
                if (package == null)
                {
                    await responseHandler.SendNotFoundAsync();
                    return;
                }

                UserRepository userRepository = new();
                StackRepository stackRepository = new();

                user.Coins -= Package.Price;
                userRepository.UpdateUser(user);

                foreach (var card in package.Pack)
                {
                    stackRepository.AddCard(user.Id, card.Id);
                }

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
            catch (ForbiddenException ex)
            {
                await responseHandler.SendForbiddenAsync(ex.Message);
            }
        }
    }
}
