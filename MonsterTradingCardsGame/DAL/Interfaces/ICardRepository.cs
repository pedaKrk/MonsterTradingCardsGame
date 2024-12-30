using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface ICardRepository
    {
        void CreateCard(Card card);
        Card? GetCard(string cardId);
    }
}
