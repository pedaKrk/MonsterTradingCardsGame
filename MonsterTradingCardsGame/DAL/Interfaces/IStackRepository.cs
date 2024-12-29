
namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IStackRepository
    {
        void AddCard(string userId, string cardId);
        void RemoveCard(string userId, string cardId);
    }
}
