using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal sealed class SpellCard : Card
    {
        public SpellCard(int damage, string name, CardType element)
            : base(damage, name, element) { }

        public override string ToString()
        {
            return $"Spell: name: {_name}, damage: {_damage}, element {_element}";
        }
    }
}
