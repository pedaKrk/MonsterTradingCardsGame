using MonsterTradingCardsGame.BusinessLogic.Handlers;
using MonsterTradingCardsGame.DAL.Repositories;
using MonsterTradingCardsGame.Http;
using MonsterTradingCardsGame.Http.Models;
using MonsterTradingCardsGame.Models;
using NSubstitute;
using NUnit.Framework;
using System.Text.Json;

namespace MonsterTradingCardsGame.Test
{
    internal class TestDeckHandler
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
        public async Task HandleConfigureDeckAsync_ShouldReturnOk_WhenValidDeckIsConfigured()
        {
            // Arrange
            var cardId1 = Guid.NewGuid();
            var cardId2 = Guid.NewGuid();
            var cardId3 = Guid.NewGuid();
            var cardId4 = Guid.NewGuid();

            var cardList = new List<Guid> { cardId1, cardId2, cardId3, cardId4 };

            var card1 = new Card(cardId1, "WaterGoblin", 10.0, Element.Water, CardType.Monster);
            var card2 = new Card(cardId2, "Dragon", 50.0, Element.Fire, CardType.Monster);
            var card3 = new Card(cardId3, "FirePhoenix", 60.0, Element.Fire, CardType.Monster);
            var card4 = new Card(cardId4, "WaterTitan", 100.0, Element.Water, CardType.Monster);

            _stackRepository.GetCardFromUser(_user.Id, cardId1).Returns(card1);
            _stackRepository.GetCardFromUser(_user.Id, cardId2).Returns(card2);
            _stackRepository.GetCardFromUser(_user.Id, cardId3).Returns(card3);
            _stackRepository.GetCardFromUser(_user.Id, cardId4).Returns(card4);

            // Act
            await DeckHandler.HandleConfigureDeckAsync(_responseHandler, _headers, JsonSerializer.Serialize(cardList));

            // Assert
            await _responseHandler.Received().SendOkAsync();
        }

        [Test]
        public async Task HandleConfigureDeckAsync_ShouldReturnBadRequest_WhenInvalidCardCount()
        {
            // Arrange
            var cardList = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            // Act
            await DeckHandler.HandleConfigureDeckAsync(_responseHandler, _headers, JsonSerializer.Serialize(cardList));

            // Assert
            await _responseHandler.Received().SendBadRequestAsync();
        }

        [Test]
        public async Task HandleConfigureDeckAsync_ShouldReturnBadRequest_WhenCardDoesNotBelongToUser()
        {
            // Arrange
            var cardId1 = Guid.NewGuid();
            var cardId2 = Guid.NewGuid();
            var cardList = new List<Guid> { cardId1, cardId2 };

            _stackRepository.GetCardFromUser(_user.Id, cardId1).Returns((Card?)null);

            // Act
            await DeckHandler.HandleConfigureDeckAsync(_responseHandler, _headers, JsonSerializer.Serialize(cardList));

            // Assert
            await _responseHandler.Received().SendBadRequestAsync();
        }

        [Test]
        public async Task HandleGetDeckAsync_ShouldReturnDeck_WhenUserHasDeck()
        {
            // Arrange
            var card1 = new Card(Guid.NewGuid(), "WaterGoblin", 10.0, Element.Water, CardType.Monster);
            var card2 = new Card(Guid.NewGuid(), "Dragon", 50.0, Element.Fire, CardType.Monster);

            _deckRepository.GetDeckFromUser(_user.Id).Returns([card1, card2]);

            // Act
            await DeckHandler.HandleGetDeckAsync(_responseHandler, _headers);

            // Assert
            await _responseHandler.Received().SendOkAsync();
        }

        [Test]
        public async Task HandleGetDeckAsync_ShouldReturnNoContent_WhenUserHasNoDeck()
        {
            // Arrange
            _deckRepository.GetDeckFromUser(_user.Id).Returns([]);

            // Act
            await DeckHandler.HandleGetDeckAsync(_responseHandler, _headers);

            // Assert
            await _responseHandler.Received().SendNoContentAsync();
        }
    }
}
