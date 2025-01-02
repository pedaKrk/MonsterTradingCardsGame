
namespace MonsterTradingCardsGame.Models
{
    internal class Card(Guid id, string name, double damage, Element element, CardType cardType)
    {
        public Guid Id { get; } = id;
        public double Damage { get; } = damage;
        public string Name { get; } = name;
        public Element Element { get; } = element;
        public CardType CardType { get; } = cardType;
    }
}
