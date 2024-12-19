using MonsterTradingCardsGame.BusinessLogic.Http;
using MonsterTradingCardsGame.BusinessLogic.Token;
using MonsterTradingCardsGame.Database;
using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic.Handler
{
    internal class UserHandler
    {
        public static async Task HandleUserRegistrationAsync(HttpResponseHandler responseHandler, string requestBody)
        {
            try
            {
                var newUser = JsonSerializer.Deserialize<User>(requestBody);

                if (string.IsNullOrWhiteSpace(newUser?.Username) || string.IsNullOrWhiteSpace(newUser?.Password))
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                if (InMemoryDatabase.UserExists(newUser.Username))
                {
                    await responseHandler.SendConflictResponseAsync();
                    return;
                }

                Console.WriteLine($"username: {newUser.Username}, password: {newUser.Password}");
                InMemoryDatabase.AddUser(newUser);

                await responseHandler.SendCreatedResponseAsync();
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleUserLoginAsync(HttpResponseHandler responseHandler, string requestBody)
        {
            try
            {
                Console.WriteLine("Login:\n");
                var loginUser = JsonSerializer.Deserialize<User>(requestBody);

                Console.WriteLine($"Username: {loginUser?.Username}, Password: {loginUser?.Password}");

                if (string.IsNullOrWhiteSpace(loginUser?.Username) || string.IsNullOrWhiteSpace(loginUser?.Password))
                {
                    Console.WriteLine("Not Username or Password provided.");
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                var user = InMemoryDatabase.Users.FirstOrDefault(u => u.Username == loginUser.Username);

                if (user == null || loginUser.Password != user.Password)
                {
                    Console.WriteLine("User not found or wrong credentials.");
                    await responseHandler.SendUnauthorizedAsync();
                    return;
                }

                if (TokenService.HasUserToken(user.Username))
                {
                    string token = TokenService.GetTokenByUsername(user.Username);

                    Console.WriteLine($"User {loginUser.Username} is already logged in with a valid token.");
                    await responseHandler.SendOkAsync(new { token });
                    return;
                }

                TokenService.GenerateToken(user.Username);
                string newToken = TokenService.GetTokenByUsername(user.Username);

                Console.WriteLine($"User {loginUser.Username} logged in successfully.");
                await responseHandler.SendOkAsync(new { newToken });

            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }

        }

        public static async Task HandleGetUserDataAsync(HttpResponseHandler responseHandler, Headers headers, string? username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                var authorizedUser = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (authorizedUser == null)
                {
                    return;
                }

                var user = InMemoryDatabase.GetUser(username);
                if (user == null)
                {
                    await responseHandler.SendNotFoundAsync();
                    return;
                }

                var userData = user.Profile;

                await responseHandler.SendOkAsync(new { userData });
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleChangeUserDataAsync(HttpResponseHandler responseHandler, Headers headers, string requestBody, string? username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                var authorizedUser = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (authorizedUser == null)
                {
                    return;
                }

                var user = InMemoryDatabase.GetUser(username);
                if (user == null)
                {
                    await responseHandler.SendNotFoundAsync();
                    return;
                }

                //implement RoleService
                /*
                if(authorizedUser.Role != Role.Admin && authorizedUser.Username != user.Username)
                {
                    await responseHandler.SendUnauthorizedAsync();
                    return;
                }
                */
                var newUserProfile = JsonSerializer.Deserialize<UserProfile>(requestBody);
                if (newUserProfile == null)
                {
                    await responseHandler.SendBadRequestAsync();
                    return;
                }

                user.Profile.Update(newUserProfile);

                await responseHandler.SendOkAsync();
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }

        public static async Task HandleGetUserStatsAsync(HttpResponseHandler responseHandler, Headers headers)
        {
            try
            {
                var user = await HttpRequestParser.AuthenticateAndGetUserAsync(responseHandler, headers);
                if (user == null)
                {
                    return;
                }

                await responseHandler.SendOkAsync(new { user.Stats });
            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }
        }
    }
}
