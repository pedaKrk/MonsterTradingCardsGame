using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MonsterTradingCardsGame.Enums;

namespace MonsterTradingCardsGame.Models
{
    internal sealed class MonsterCard : Card
    {
        private readonly int _health;
        public MonsterCard(int damage, string name, int health, CardType element)
            : base(damage, name, element)
        {
            _health = health;
        }

        public int Health { get { return _health; } }

        public override string ToString()
        {
            return $"name: {_name}, damage: {_damage}, health: {_health}, element {_element}";
        }
    }
}
