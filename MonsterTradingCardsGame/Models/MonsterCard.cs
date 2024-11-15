using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MonsterTradingCardsGame.Models
{
    internal sealed class MonsterCard : Card
    {
        public MonsterCard(int damage, string name, int health, CardType element)
            : base(damage, name, element) { }
 
        public override string ToString()
        {
            return $"Monster: name: {_name}, damage: {_damage}, element {_element}";
        }
    }
}
