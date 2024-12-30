using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IStackRepository
    {
        void AddCard(int userId, string cardId);
        Card? GetCardFromUser(int userId, string cardId);
        List<Card> GetAllCardsFromUser(int userId);
        bool UserHasCard(int userId, string cardId);
        void RemoveCard(int userId, string cardId);
    }
}
