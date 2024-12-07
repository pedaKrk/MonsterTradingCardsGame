using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Card(string id, string name, double damage, Element element, CardType cardType)
    {
        public string Id { get; } = id;
        public double Damage { get; } = damage;
        public string Name { get; } = name;
        public Element Element { get; } = element;
        public CardType CardType { get; } = cardType;

        public override string ToString()
        {
            return $"[{CardType} Card] Id: {Id}, Name: {Name}, Damage: {Damage}, Element: {Element}";
        }
    }
}
