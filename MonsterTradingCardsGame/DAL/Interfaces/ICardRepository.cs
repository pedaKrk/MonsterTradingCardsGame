using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface ICardRepository
    {
        void CreateCard(Card card);
        void GetCard(string cardId);
    }
}
