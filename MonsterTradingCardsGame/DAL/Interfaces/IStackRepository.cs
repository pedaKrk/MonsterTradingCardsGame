using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    public interface IStackRepository
    {
        void AddCard(int userId, Guid cardId);
        Card? GetCardFromUser(int userId, Guid cardId);
        List<Card> GetAllCardsFromUser(int userId);
        bool UserHasCard(int userId, Guid cardId);
        void RemoveCard(int userId, Guid cardId);
    }
}
