using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Package
    {
        public static readonly int Price = 5;

        private readonly List<Card> _pack;

        private const int PackSize = 5;

        public Package(List<Card> cards)
        {
            _pack = new List<Card>(cards);
        }

        public List<Card> Open()
        {
            return new List<Card>(_pack);
        }
    }
}
