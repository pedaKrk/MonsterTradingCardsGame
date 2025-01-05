using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.BusinessLogic.Services;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Http.Models;
using MonsterTradingCardsGame.Models;
using System.Text.Json;

namespace MonsterTradingCardsGame.BusinessLogic.Handlers
{
    public static class UserHandler
    {
        public static async Task HandleUserRegistrationAsync(HttpResponseHandler responseHandler, string requestBody)
        {
            try
            {
                var newUser = JsonSerializer.Deserialize<User>(requestBody);

                if (string.IsNullOrWhiteSpace(newUser?.Username) || string.IsNullOrWhiteSpace(newUser?.Password))
                {
                    throw new BadRequestException("bad json.");
                }

                UserRepository userRepository = new();
                if (userRepository.UserExists(newUser.Username))
                {
                    throw new ConflictException("user already exists.");
                }

                if(newUser.Username == "admin")
                {
                    newUser.Role = Role.Admin;
                }

                var userId = userRepository.AddUser(newUser);

                UserDataRepository userDataRepository = new();
                userDataRepository.AddUserData(userId, newUser.Data);

                UserStatsRepository userStatsaRepository = new();
                userStatsaRepository.AddUserStats(userId, newUser.Stats);

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
            catch (ConflictException ex)
            {
                await responseHandler.SendConflictAsync(ex.Message);
            }
        }

        public static async Task HandleUserLoginAsync(HttpResponseHandler responseHandler, string requestBody)
        {
            try
            {
                var loginUser = JsonSerializer.Deserialize<User>(requestBody);

                if (string.IsNullOrWhiteSpace(loginUser?.Username) || string.IsNullOrWhiteSpace(loginUser?.Password))
                {
                    throw new BadRequestException("no username or password provided.");
                }

                UserRepository userRepository = new();
                var user = userRepository.GetUserByUsername(loginUser.Username);
                if (user == null || loginUser.Password != user.Password)
                {
                    throw new UnauthorizedException("wrong credentials.");
                }

                string? token = TokenService.GetTokenByUsername(user.Username);
                if (token == null)
                {
                    var newToken = TokenService.GenerateToken(user.Username);
                    await responseHandler.SendOkAsync(new { newToken });
                    return;
                }

                await responseHandler.SendOkAsync(new { token });
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

        public static async Task HandleGetUserDataAsync(HttpResponseHandler responseHandler, Headers headers, string? username)
        {
            try
            {
                var authorizedUser = HttpRequestParser.AuthenticateAndGetUser(headers);

                if (string.IsNullOrEmpty(username))
                {
                    throw new BadRequestException("username is required.");
                }
                
                UserRepository userRepository = new();
                var user = userRepository.GetUserByUsername(username) ?? throw new NotFoundException("user not found.");

                UserDataRepository userDataRepository = new();
                var userData = userDataRepository.GetUserData(user.Id);

                await responseHandler.SendOkAsync(new { userData });
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
            catch(NotFoundException ex)
            {
                await responseHandler.SendNotFoundAsync(ex.Message);
            }
        }

        public static async Task HandleChangeUserDataAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody, string? username)
        {
            try
            {
                var authorizedUser = HttpRequestParser.AuthenticateAndGetUser(headers);

                if (string.IsNullOrEmpty(username))
                {
                    throw new BadRequestException("username is required.");
                }

                UserRepository userRepository = new();
                var user = userRepository.GetUserByUsername(username) ?? throw new NotFoundException("user not found.");


                if(authorizedUser.Role != Role.Admin && authorizedUser.Username != user.Username)
                {
                    throw new UnauthorizedException("User doesn't own this account or is not admin");
                }

                var newUserData = JsonSerializer.Deserialize<UserData>(requestBody) ?? throw new BadRequestException("bad json.");

                user.Data.Update(newUserData);
                user.Stats.Name = user.Data.Name;

                UserDataRepository userDataRepository = new();
                userDataRepository.UpdateUserData(user.Id, user.Data);

                UserStatsRepository userStatsRepository = new();
                userStatsRepository.UpdateUserStats(user.Id, user.Stats);

                await responseHandler.SendOkAsync(new {user.Data});
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

        public static async Task HandleGetUserStatsAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = HttpRequestParser.AuthenticateAndGetUser(headers);

                UserStatsRepository userStatsRepository = new();
                var userStats = userStatsRepository.GetUserStats(user.Id);

                await responseHandler.SendOkAsync(new { userStats });
            }
            catch(JsonException ex)
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
