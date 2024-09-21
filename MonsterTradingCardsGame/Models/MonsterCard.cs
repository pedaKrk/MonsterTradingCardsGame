using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Enums;

namespace MonsterTradingCardsGame.Models
{
    internal sealed class MonsterCard : Card
    {
        public MonsterCard(int damage, string name, CardType element)
            : base(damage, name, element)
        {

        }
    }
}
