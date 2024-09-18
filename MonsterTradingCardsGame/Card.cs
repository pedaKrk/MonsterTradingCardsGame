using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    public enum CardType
    {
        Water,
        Fire,
        Normal
    }
    internal class Card
    {
        private readonly int _damage;

        private readonly string _name;

        private readonly CardType _element;
     
        public Card(string name, int damage, CardType element /*später ein enum zb*/) 
        { 
            _name = name;
            _damage = damage;
            _element = element;
        }

        public string Name { get { return _name; } }
        public int Damage { get { return _damage; } }
        public CardType Element { get { return _element; } }
    }
}
