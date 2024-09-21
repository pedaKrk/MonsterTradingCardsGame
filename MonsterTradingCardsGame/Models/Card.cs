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
        protected readonly int _damage;

        protected readonly string _name;

        protected readonly CardType _element;

        public Card(int damage, string name, CardType element)
        {
            _damage = damage;
            _name = name;
            _element = element;
        }

        public int Damage { get { return _damage; } }
        public string Name { get { return _name; } }
        public CardType Element { get { return _element; } }

        public abstract string ToString();
    }
}
