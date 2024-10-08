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
        private readonly HttpResponseHandler _responseHandler;

        public UserHandler(HttpResponseHandler responseHandler)
        {
            _responseHandler = responseHandler;
        }

        public async Task HandleUserRegistrationAsync(string requestBody)
        {
            try
            {
                var newUser = JsonSerializer.Deserialize<User>(requestBody);

                if (string.IsNullOrWhiteSpace(newUser?.Username) || string.IsNullOrWhiteSpace(newUser?.Password))
                {
                    await _responseHandler.SendBadRequestAsync();
                    return;
                }

                Console.WriteLine($"username: {newUser.Username}, password: {newUser.Password}");
                await _responseHandler.SendCreatedResponseAsync(newUser);


                // Check if the user already exists
                // response: 409 Conflict

                //Register new user
                //response: 201 Created

            }
            catch (JsonException)
            {
                await _responseHandler.SendBadRequestAsync();  // 400 Bad Request
            }
        }
    }
}
