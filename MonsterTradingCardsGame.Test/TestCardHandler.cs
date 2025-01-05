using MonsterTradingCardsGame.BusinessLogic.Handlers;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Http.Models;
using MonsterTradingCardsGame.Models;
using NSubstitute;
using NUnit.Framework;

namespace MonsterTradingCardsGame.Test
{
    public class TestCardHandler
    {
        private StackRepository _stackRepository;
        private DeckRepository _deckRepository;
        private HttpResponseHandler _responseHandler;
        private Headers _headers;
        private User _user;

        [SetUp]
        public void Setup()
        {
            _stackRepository = Substitute.For<StackRepository>();
            _deckRepository = Substitute.For<DeckRepository>();
            _responseHandler = Substitute.For<HttpResponseHandler>();

            _user = new User("testuser", "password") { Id = 1, Role = Role.User };
            _headers = new Headers();
        }

        [Test]
        public async Task HandleGetAllCardsAsync_ShouldReturnCards_WhenUserHasCards()
        {
            // Arrange
            var stackCard = new Card(Guid.NewGuid(), "WaterGoblin", 10.0, Element.Water, CardType.Monster);
            var deckCard = new Card(Guid.NewGuid(), "Dragon", 50.0, Element.Fire, CardType.Monster);

            _stackRepository.GetAllCardsFromUser(_user.Id).Returns([stackCard]);
            _deckRepository.GetDeckFromUser(_user.Id).Returns([deckCard]);

            // Act
            await CardHandler.HandleGetAllCardsAsync(_responseHandler, _headers);

            // Assert
            await _responseHandler.Received().SendOkAsync();
        }

        [Test]
        public async Task HandleGetAllCardsAsync_ShouldReturnNoContent_WhenUserHasNoCards()
        {
            // Arrange
            _stackRepository.GetAllCardsFromUser(_user.Id).Returns([]);
            _deckRepository.GetDeckFromUser(_user.Id).Returns([]);

            // Act
            await CardHandler.HandleGetAllCardsAsync(_responseHandler, _headers);

            // Assert
            await _responseHandler.Received().SendNoContentAsync();
        }
    }
}
