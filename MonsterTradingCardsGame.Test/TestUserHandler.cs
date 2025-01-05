using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.BusinessLogic.Handlers;
using MonsterTradingCardsGame.BusinessLogic.Services;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Http.Models;
using MonsterTradingCardsGame.Models;
using NSubstitute;
using NUnit.Framework;

namespace MonsterTradingCardsGame.Test
{
    [TestFixture]
    public class TestUserHandler
    {

        private HttpResponseHandler _responseHandler;
        private UserRepository _userRepository;
        private UserDataRepository _userDataRepository;
        private UserStatsRepository _userStatsRepository;

        [SetUp]
        public void Setup()
        {
            _responseHandler = Substitute.For<HttpResponseHandler>();
            _userRepository = Substitute.For<UserRepository>();
            _userDataRepository = Substitute.For<UserDataRepository>();
            _userStatsRepository = Substitute.For<UserStatsRepository>();
        }

        [Test]
        public async Task HandleUserRegistrationAsync_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var requestBody = "{\"Username\":\"newuser\", \"Password\":\"password\"}";
            _userRepository.UserExists(Arg.Any<string>()).Returns(false);
            _userRepository.AddUser(Arg.Any<User>()).Returns(1);

            // Act
            await UserHandler.HandleUserRegistrationAsync(_responseHandler, requestBody);

            // Assert
            await _responseHandler.Received(1).SendCreatedAsync();
        }

        [Test]
        public void HandleUserRegistrationAsync_UserAlreadyExists_ThrowsConflict()
        {
            // Arrange
            var requestBody = "{\"Username\":\"existinguser\", \"Password\":\"password\"}";
            _userRepository.UserExists(Arg.Any<string>()).Returns(true);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ConflictException>(async () =>
                await UserHandler.HandleUserRegistrationAsync(_responseHandler, requestBody));

            Assert.That(exception.Message, Is.EqualTo("user already exists."));
        }

        [Test]
        public async Task HandleUserLoginAsync_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var requestBody = "{\"Username\":\"validuser\", \"Password\":\"validpassword\"}";
            var user = new User("validuser", "validpassword");
            _userRepository.GetUserByUsername("validuser").Returns(user);

            TokenService.GetTokenByUsername(user.Username).Returns((string?)null);
            TokenService.GenerateToken(user.Username).Returns($"{user.Username}-mtcgToken");

            // Act
            await UserHandler.HandleUserLoginAsync(_responseHandler, requestBody);

            // Assert
            await _responseHandler.Received(1).SendOkAsync();
        }

        [Test]
        public void HandleUserLoginAsync_InvalidCredentials_ThrowsUnauthorized()
        {
            // Arrange
            var requestBody = "{\"Username\":\"invaliduser\", \"Password\":\"wrongpassword\"}";
            _userRepository.GetUserByUsername("invaliduser").Returns((User?)null);

            // Act & Assert
            var exception = Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await UserHandler.HandleUserLoginAsync(_responseHandler, requestBody));

            Assert.That(exception.Message, Is.EqualTo("wrong credentials."));
        }

        [Test]
        public async Task HandleGetUserDataAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var headers = new Headers();  // Mock headers if necessary
            var username = "validuser";
            var user = new User("validuser", "validpassword") { Id = 1};
            _userRepository.GetUserByUsername(username).Returns(user);

            var userData = new UserData("John Doe");
            _userDataRepository.GetUserData(1).Returns(userData);

            // Act
            await UserHandler.HandleGetUserDataAsync(_responseHandler, headers, username);

            // Assert
            await _responseHandler.Received(1).SendOkAsync();
        }

        [Test]
        public async Task HandleChangeUserDataAsync_UnauthorizedAccess_ReturnsUnauthorized()
        {
            // Arrange
            var headers = new Headers();  // Mock headers for authentication
            var requestBody = "{\"Name\":\"New Name\"}";
            var username = "user1";

            // Mock authentication
            var authorizedUser = new User("admin", "istrator") {Role = Role.Admin };
            HttpRequestParser.AuthenticateAndGetUser(headers).Returns(authorizedUser);

            var user = new User("user2", "password123") { Id = 1, Role = Role.User };
            _userRepository.GetUserByUsername(username).Returns(user);

            // Act
            await UserHandler.HandleChangeUserDataAsync(_responseHandler, headers, requestBody, username);

            // Assert
            await _responseHandler.Received(1).SendUnauthorizedAsync();
        }

        [Test]
        public async Task HandleGetUserStatsAsync_ValidRequest_ReturnsOk()
        {
            // Arrange
            var headers = new Headers();  // Mock headers
            var user = new User("validuser", "validpassword") { Id = 1};
            _userRepository.GetUserByUsername("validuser").Returns(user);

            var userStats = new UserStats(user.Username) { Elo = 1100, Wins = 10, Losses = 0 };
            _userStatsRepository.GetUserStats(1).Returns(userStats);

            // Act
            await UserHandler.HandleGetUserStatsAsync(_responseHandler, headers);

            // Assert
            await _responseHandler.Received(1).SendOkAsync();
        }
    }
}
