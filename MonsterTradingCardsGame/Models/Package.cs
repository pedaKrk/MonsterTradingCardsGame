
namespace MonsterTradingCardsGame.Models
{
    internal class Package(List<Card> cards)
    {
        public static readonly int Price = 5;
        public static readonly int Size = 5;

        public readonly List<Card> Pack = new(cards);
    }
}
