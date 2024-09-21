using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Enums;

namespace MonsterTradingCardsGame.Models
{
    internal class Package
    {
        public static readonly int Price = 5;

        private readonly List<Card> _pack;

        private const int PackSize = 5;

        public Package()
        {
            _pack = new List<Card>(PackSize);

            _pack.Add(new MonsterCard(50, "Monster1", CardType.Fire));
            _pack.Add(new MonsterCard(50, "Monster2", CardType.Water));
            _pack.Add(new MonsterCard(50, "Monster1", CardType.Normal));
            _pack.Add(new SpellCard(50, "Spell1", CardType.Fire));
            _pack.Add(new SpellCard(50, "Spell2", CardType.Water));
        }

        public List<Card> Open()
        {
            return new List<Card>(_pack);
        }
    }
}
