using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    public interface ICardRepository
    {
        void CreateCard(Card card);
        Card? GetCard(Guid cardId);
    }
}
