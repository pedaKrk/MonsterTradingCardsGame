using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Deck
    {
        private readonly List<Card> _cards;

        public static readonly int DeckSize = 4;

        public Deck()
        {
            _cards = new List<Card>(DeckSize);
        }

        public int Count { get { return _cards.Count; } }

        public void AddCard(Card card)
        {
            if(_cards.Count > DeckSize)
            {
                Console.WriteLine("Deck is full!");
                return;
            }
 
            _cards.Add(card);
        }

        public Card? GetCard(int index)
        {
            if (index < 0 || index >= _cards.Count)
            {
                Console.WriteLine("Illegal index!");
                return null;
            }

            return _cards[index];
        }
    }
}
