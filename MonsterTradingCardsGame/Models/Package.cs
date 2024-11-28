using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Package(List<Card> cards)
    {
        public static readonly int Price = 5;

        private readonly List<Card> _pack = new(cards);

        public List<Card> Open()
        {
            return new List<Card>(_pack);
        }
    }
}
