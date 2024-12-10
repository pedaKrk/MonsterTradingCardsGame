using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Deck
    {
        private List<Card> _cards = [];

        public static readonly int DeckSize = 4;

        public void AddCards(List<Card> cards)
        {
            _cards = new List<Card>(cards);
        }

        public List<Card> GetCards()
        {
            return new List<Card>(_cards);
        }
    }
}
