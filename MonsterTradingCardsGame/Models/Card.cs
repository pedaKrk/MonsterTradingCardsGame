using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Card
    {
        private readonly int _id;

        private readonly int _damage;

        private readonly string _name;

        private readonly Element _element;

        private readonly CardType _cardType;

        public Card(int id, int damage, string name, Element element)
        {
            _id = id;
            _damage = damage;
            _name = name;
            _element = element;
            _cardType = DetermineCardType(name);
        }

        public int Id { get { return _id; } }
        public int Damage { get { return _damage; } }
        public string Name { get { return _name; } }
        public Element Element { get { return _element; } }

        private CardType DetermineCardType(string name)
        {
            return name.Contains("Spell", StringComparison.OrdinalIgnoreCase)
                ? CardType.SpellCard
                : CardType.MonsterCard;
        }

    }
}
