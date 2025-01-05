using MonsterTradingCardsGame.BusinessLogic.Handlers;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Http.Models;
using MonsterTradingCardsGame.Models;
using NSubstitute;

namespace MonsterTradingCardsGame.Test
{
    [TestFixture]
    public class TestPackageHandler
    {
        private HttpResponseHandler _responseHandler;
        private CardRepository _cardRepository;
        private PackageRepository _packageRepository;

        [SetUp]
        public void Setup()
        {
            _responseHandler = Substitute.For<HttpResponseHandler>();
            _cardRepository = Substitute.For<CardRepository>();
            _packageRepository = Substitute.For<PackageRepository>();
        }

        [Test]
        public async Task HandleCreatePackageAsync_ValidRequest_CreatesPackage()
        {
            // Arrange
            var headers = new Headers();
            var requestBody = @"
                [
                    {""Id"": ""845f0dc7-37d0-426e-994e-43fc3ac83c08"", ""Name"": ""WaterGoblin"", ""Damage"": 10.0, ""Element"": ""Water"", ""CardType"": ""Monster""},
                    {""Id"": ""99f8f8dc-e25e-4a95-aa2c-782823f36e2a"", ""Name"": ""Dragon"", ""Damage"": 50.0, ""Element"": ""Fire"", ""CardType"": ""Monster""},
                    {""Id"": ""e85e3976-7c86-4d06-9a80-641c2019a79f"", ""Name"": ""WaterSpell"", ""Damage"": 20.0, ""Element"": ""Water"", ""CardType"": ""Spell""},
                    {""Id"": ""1cb6ab86-bdb2-47e5-b6e4-68c5ab389334"", ""Name"": ""Ork"", ""Damage"": 45.0, ""Element"": ""Normal"", ""CardType"": ""Monster""},
                    {""Id"": ""dfdd758f-649c-40f9-ba3a-8657f4b3439f"", ""Name"": ""FireSpell"", ""Damage"": 25.0, ""Element"": ""Water"", ""CardType"": ""Spell""}
                ]";

            var user = new User("admin", "adminpassword") { Role = Role.Admin };  // Admin user
            HttpRequestParser.AuthenticateAndGetUser(headers).Returns(user);

            // Act
            await PackageHandler.HandleCreatePackageAsync(_responseHandler, headers, requestBody);

            // Assert
            await _responseHandler.Received(1).SendCreatedAsync();
            _cardRepository.Received(4).CreateCard(Arg.Any<Card>());
            _packageRepository.Received(1).CreatePackage(Arg.Any<Package>());
        }

        [Test]
        public async Task HandleCreatePackageAsync_NonAdminUser_ReturnsForbidden()
        {
            // Arrange
            var headers = new Headers();
            var requestBody = @"
                [
                    {""Id"": ""845f0dc7-37d0-426e-994e-43fc3ac83c08"", ""Name"": ""WaterGoblin"", ""Damage"": 10.0, ""Element"": ""Water"", ""CardType"": ""Monster""},
                    {""Id"": ""99f8f8dc-e25e-4a95-aa2c-782823f36e2a"", ""Name"": ""Dragon"", ""Damage"": 50.0, ""Element"": ""Fire"", ""CardType"": ""Monster""},
                    {""Id"": ""e85e3976-7c86-4d06-9a80-641c2019a79f"", ""Name"": ""WaterSpell"", ""Damage"": 20.0, ""Element"": ""Water"", ""CardType"": ""Spell""},
                    {""Id"": ""1cb6ab86-bdb2-47e5-b6e4-68c5ab389334"", ""Name"": ""Ork"", ""Damage"": 45.0, ""Element"": ""Normal"", ""CardType"": ""Monster""},
                    {""Id"": ""dfdd758f-649c-40f9-ba3a-8657f4b3439f"", ""Name"": ""FireSpell"", ""Damage"": 25.0, ""Element"": ""Water"", ""CardType"": ""Spell""}
                ]";

            var user = new User("nonadmin", "password") { Role = Role.User };  // Non-admin user
            HttpRequestParser.AuthenticateAndGetUser(headers).Returns(user);

            // Act
            await PackageHandler.HandleCreatePackageAsync(_responseHandler, headers, requestBody);

            // Assert
            await _responseHandler.Received(1).SendForbiddenAsync();
        }

        [Test]
        public async Task HandleCreatePackageAsync_InvalidCardCount_ReturnsBadRequest()
        {
            // Arrange
            var headers = new Headers();
            var invalidRequestBody = @"
                [
                    {""Id"": ""845f0dc7-37d0-426e-994e-43fc3ac83c08"", ""Name"": ""WaterGoblin"", ""Damage"": 10.0, ""Element"": ""Water"", ""CardType"": ""Monster""}
                ]";
            var user = new User("admin", "adminpassword") { Role = Role.Admin };
            HttpRequestParser.AuthenticateAndGetUser(headers).Returns(user);

            // Act
            await PackageHandler.HandleCreatePackageAsync(_responseHandler, headers, invalidRequestBody);

            // Assert
            await _responseHandler.Received(1).SendBadRequestAsync();
        }

        [Test]
        public async Task HandleAcquirePackageAsync_ValidRequest_AcquiresPackage()
        {
            // Arrange
            var headers = new Headers();
            var user = new User("validuser", "password") { Coins = 50 };  // User with enough coins
            HttpRequestParser.AuthenticateAndGetUser(headers).Returns(user);

            var card1 = new Card(Guid.NewGuid(), "WaterGoblin", 10.0, Element.Water, CardType.Monster);
            var card2 = new Card(Guid.NewGuid(), "Dragon", 50.0, Element.Fire, CardType.Monster);
            var card3 = new Card(Guid.NewGuid(), "WaterSpell", 20.0, Element.Water, CardType.Spell);
            var card4 = new Card(Guid.NewGuid(), "Ork", 45.0, Element.Normal, CardType.Monster);
            var card5 = new Card(Guid.NewGuid(), "FireSpell", 25.0, Element.Water, CardType.Spell);

            var package = new Package([card1, card2, card3, card4, card5]);

            _packageRepository.AcquirePackage().Returns(package);

            var userRepository = Substitute.For<UserRepository>();
            var stackRepository = Substitute.For<StackRepository>();

            // Act
            await PackageHandler.HandleAcquirePackageAsync(_responseHandler, headers);

            // Assert
            await _responseHandler.Received(1).SendOkAsync();
            userRepository.Received(1).UpdateUser(user);
            stackRepository.Received(2).AddCard(Arg.Any<int>(), Arg.Any<Guid>());
        }

        [Test]
        public async Task HandleAcquirePackageAsync_InsufficientFunds_ReturnsForbidden()
        {
            // Arrange
            var headers = new Headers();
            var user = new User("validuser", "password") { Coins = 0 };  // User with insufficient coins
            HttpRequestParser.AuthenticateAndGetUser(headers).Returns(user);

            // Act
            await PackageHandler.HandleAcquirePackageAsync(_responseHandler, headers);

            // Assert
            await _responseHandler.Received(1).SendForbiddenAsync();
        }

        [Test]
        public async Task HandleAcquirePackageAsync_PackageNotFound_ReturnsNotFound()
        {
            // Arrange
            var headers = new Headers();
            var user = new User("validuser", "password");
            HttpRequestParser.AuthenticateAndGetUser(headers).Returns(user);

            _packageRepository.AcquirePackage().Returns((Package?)null);  // No package available

            // Act
            await PackageHandler.HandleAcquirePackageAsync(_responseHandler, headers);

            // Assert
            await _responseHandler.Received(1).SendNotFoundAsync();
        }
    }
}

