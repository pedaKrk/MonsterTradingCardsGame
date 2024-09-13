using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Deck
    {
        private LinkedList<Card> cards;

        public Deck()
        {
            cards = new LinkedList<Card>();
        }

        public void Add(Card card)
        {
            if (cards.Count == 4)
            {
                Console.WriteLine("Your Deck is full!");
                return;
            }

            cards.AddLast(card);
        }

        public void Remove(Card card)
        {
            cards.Remove(card);
        }

    }
}
