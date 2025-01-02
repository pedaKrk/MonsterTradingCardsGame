
namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal(Guid id, Guid cardId, double price, string username)
    {
        public Guid Id { get; } = id;
        public Guid CardId { get; } = cardId;
        public double Price { get; } = price;
        public string Username { get; } = username;
    }
}
