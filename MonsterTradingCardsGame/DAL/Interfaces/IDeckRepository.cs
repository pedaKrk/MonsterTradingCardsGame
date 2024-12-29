
namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IDeckRepository
    {
        void AddCard(string userId, string cardId);
        void RemoveCard(string userId, string cardId);
    }
}
