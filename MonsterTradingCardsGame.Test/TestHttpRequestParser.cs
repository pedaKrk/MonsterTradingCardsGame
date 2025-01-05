using MonsterTradingCardsGame.BusinessLogic.Exceptions;
using MonsterTradingCardsGame.BusinessLogic.Services;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Http.Models;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Models;
using NSubstitute;
using NUnit.Framework;

namespace MonsterTradingCardsGame.Test
{
    public class TestHttpRequestParser
    {
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = Substitute.For<UserRepository>();
        }

        [Test]
        public void AuthenticateAndGetUser_ShouldThrowBadRequest_WhenAuthorizationTokenIsNull()
        {
            // Arrange
            var headers = new Headers();
            TokenService.GetUsernameByToken(Arg.Any<string>()).Returns((string?)null);

            // Act & Assert
            Assert.Throws<BadRequestException>(() =>
                HttpRequestParser.AuthenticateAndGetUser(headers));
        }

        [Test]
        public void AuthenticateAndGetUser_ShouldThrowUnauthorized_WhenUserNotFoundByToken()
        {
            // Arrange
            var headers = new Headers();
            string authorizationToken = "invalid-token";
            TokenService.GetUsernameByToken(authorizationToken).Returns((string?)null);
            headers.AddHeader("Authorization", authorizationToken);

            // Act & Assert
            Assert.Throws<UnauthorizedException>(() =>
                HttpRequestParser.AuthenticateAndGetUser(headers));
        }

        [Test]
        public void AuthenticateAndGetUser_ShouldThrowUnauthorized_WhenUserNotFoundByUsername()
        {
            // Arrange
            var headers = new Headers();
            string authorizationToken = "valid-token";
            string username = "validuser";

            TokenService.GetUsernameByToken(authorizationToken).Returns(username);
            _userRepository.GetUserByUsername(username).Returns((User?)null);
            headers.AddHeader("Authorization", authorizationToken);

            // Act & Assert
            Assert.Throws<UnauthorizedException>(() =>
                HttpRequestParser.AuthenticateAndGetUser(headers));
        }

        [Test]
        public void AuthenticateAndGetUser_ShouldReturnUser_WhenTokenAndUserAreValid()
        {
            // Arrange
            var headers = new Headers();
            string authorizationToken = "valid-token";
            string username = "validuser";

            var expectedUser = new User(username, "password");
            TokenService.GetUsernameByToken(authorizationToken).Returns(username);
            _userRepository.GetUserByUsername(username).Returns(expectedUser);
            headers.AddHeader("Authorization", authorizationToken);

            // Act
            var user = HttpRequestParser.AuthenticateAndGetUser(headers);

            // Assert
            Assert.That(user, Is.EqualTo(expectedUser));
        }
    }
}
