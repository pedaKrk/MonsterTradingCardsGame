
using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IDeckRepository
    {
        void AddCard(int userId, Guid cardId);
        List<Card> GetDeckFromUser(int userId);
        void AddCardsToUser(int userId, List<Card> cards);
        void RemoveCard(int userId, Guid cardId);
    }
}
