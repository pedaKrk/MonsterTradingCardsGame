using MonsterTradingCardsGame.BusinessLogic.Handlers;
using MonsterTradingCardsGame.BusinessLogic.Services;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Http.Interfaces;
using MonsterTradingCardsGame.Models;
using NSubstitute;
using System.Text.Json;

namespace MonsterTradingCardsGame.Test
{
    public class TestUserHandler
    {

        private readonly IHttpResponseHandler _responseHandler;
        private readonly IUserRepository _userRepository;
        private readonly IUserDataRepository _userDataRepository;
        private readonly IUserStatsRepository _userStatsRepository;

        public TestUserHandler()
        {
            _responseHandler = Substitute.For<IHttpResponseHandler>();
            _userRepository = Substitute.For<IUserRepository>();
            _userDataRepository = Substitute.For<IUserDataRepository>();
            _userStatsRepository = Substitute.For<IUserStatsRepository>();
        }

        [Test]
        public async Task HandleUserRegistrationAsync_ValidInput_UserCreated()
        {
            // Arrange
            string requestBody = JsonSerializer.Serialize(new User("testuser", "password123"));

            _userRepository.UserExists("testuser").Returns(false);
            _userRepository.AddUser(Arg.Any<User>()).Returns(1); // Mocking a created user ID

            var handler = new UserHandler();

            // Act
            await handler.HandleUserRegistrationAsync(_responseHandler, requestBody);

            // Assert
            await _responseHandler.Received(1).SendCreatedAsync();
        }

        [Test]
        public async Task HandleUserRegistrationAsync_UserAlreadyExists_ConflictException()
        {
            // Arrange
            string requestBody = JsonSerializer.Serialize(new User("existinguser", "password123"));

            _userRepository.UserExists("existinguser").Returns(true);

            var handler = new UserHandler();

            // Act
            await handler.HandleUserRegistrationAsync(_responseHandler, requestBody);

            // Assert
            await _responseHandler.Received(1).SendConflictAsync("user already exists.");
        }

        [Test]
        public async Task HandleUserLoginAsync_ValidCredentials_TokenReturned()
        {
            // Arrange
            string requestBody = JsonSerializer.Serialize(new User("testuser", "password123"));

            string user = JsonSerializer.Serialize(new User("testuser", "password123"));

            _userRepository.GetUserByUsername("testuser").Returns(user);

            TokenService.GetTokenByUsername("testuser").Returns((string?)null);
            TokenService.GenerateToken("testuser").Returns("newtoken123");

            var handler = new UserHandler();

            // Act
            await handler.HandleUserLoginAsync(_responseHandler, requestBody);

            // Assert
            await _responseHandler.Received(1).SendOkAsync(Arg.Is<object>(o => ((dynamic)o).newToken == "newtoken123"));
        }

        [Test]
        public async Task HandleUserLoginAsync_InvalidCredentials_UnauthorizedException()
        {
            // Arrange
            string requestBody = JsonSerializer.Serialize(new User("testuser", "wrongpassword"));

            string user = JsonSerializer.Serialize(new User("testuser", "wrongpassword"));

            _userRepository.GetUserByUsername("testuser").Returns(user);

            var handler = new UserHandler();

            // Act
            await handler.HandleUserLoginAsync(_responseHandler, requestBody);

            // Assert
            await _responseHandler.Received(1).SendUnauthorizedAsync("wrong credentials.");
        }

        [Test]
        public async Task HandleGetUserDataAsync_ValidUser_UserDataReturned()
        {
            // Arrange
            var headers = new Headers();
            var user = new User("testuser", "password123") { Id = 1 };

            _userRepository.GetUserByUsername("testuser").Returns(user);

            _userDataRepository.GetUserData(1).Returns(new UserData(user.Username));

            var handler = new UserHandler();

            // Act
            await handler.HandleGetUserDataAsync(_responseHandler, headers, "testuser");

            // Assert
            await _responseHandler.Received(1).SendOkAsync(Arg.Any<object>());
        }

    }
}
