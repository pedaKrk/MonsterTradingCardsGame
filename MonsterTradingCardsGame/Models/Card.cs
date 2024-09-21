using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Enums;

namespace MonsterTradingCardsGame.Models
{
    internal abstract class Card
    {
        private readonly int _damage;

        private readonly string _name;

        private readonly CardType _element;

        public Card(int damage, string name, CardType element)
        {
            _damage = damage;
            _name = name;
            _element = element;
        }

        public int Damage { get { return _damage; } }
        public string Name { get { return _name; } }
        public CardType Element { get { return _element; } }

        public override string ToString()
        {
            return $"name: {_name}, damage: {Damage}, element {_element}";
        }
    }
}
