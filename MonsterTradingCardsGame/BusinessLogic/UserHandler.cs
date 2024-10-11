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

                if (!string.IsNullOrWhiteSpace(user.Token) && TokenService.IsValidToken(user.Username, user.Token))
                {
                    Console.WriteLine($"User {loginUser.Username} is already logged in with a valid token.");
                    await responseHandler.SendOkAsync(new { user.Token });
                    return;
                }

                user.Token = TokenService.GenerateToken(user.Username);

                Console.WriteLine($"User {loginUser.Username} logged in successfully.");
                await responseHandler.SendOkAsync(new { user.Token });

            }
            catch (JsonException)
            {
                await responseHandler.SendBadRequestAsync();
            }

        }
    }
}
