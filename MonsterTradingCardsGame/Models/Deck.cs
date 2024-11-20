using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
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
            if (_cards.Count >= DeckSize)
            {
                Console.WriteLine("Deck is full!");
                return;
            }

            _cards.Add(card);
        }

        public void RemoveCard(int index)
        {
            if (!IsValidIndex(index))
            {
                Console.WriteLine("Illegal index!");
                return;
            }

            _cards.RemoveAt(index);
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }

        public List<Card> GetCards()
        {
            return new List<Card>(_cards);
        }

        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < _cards.Count;
        }

        public override string ToString()
        {
            string str = "Deck:\n";
             
            for(int i = 0; i < DeckSize; i++)
            {
                if(i >= Count)
                {
                    str += $"{i}:\n";
                    continue;
                }

                str += $"{i}: {_cards[i].ToString()}\n";
            }

            str += "---------\n\n";

            return str;
        }
    }
}
