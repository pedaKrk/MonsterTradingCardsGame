using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Card
    {
        private readonly int _damage;
        public Card(string name, int damage, string element /*später ein enum zb*/) 
        { 
            Name = name;
            _damage = damage;
            Element = element;
        }

        public string Name { get; }
        public string Element { get; }
    }
}
