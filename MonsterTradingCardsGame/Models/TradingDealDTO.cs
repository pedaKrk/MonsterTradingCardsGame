
namespace MonsterTradingCardsGame.Models
{
    internal class TradingDealDTO(Guid id, Guid cardId, double price)
    {
        public Guid Id { get; } = id;
        public Guid CardId { get; } = cardId;
        public double Price { get; } = price;
    }
}
