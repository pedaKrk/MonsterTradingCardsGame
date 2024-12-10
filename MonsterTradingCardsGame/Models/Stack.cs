using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Stack
    {
        private readonly List<Card> _stack = [];

        public void AddCard(Card card)
        {
            _stack.Add(card);
        }

        public void AddCards(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                _stack.Add(card);
            }
        }

        public void RemoveCard(Card card)
        {
            _stack.Remove(card);
        }

        public Card GetCardAt(int index)
        {
            return _stack[index];
        }

        public Card? GetCardById(string id)
        {
            foreach (Card card in _stack)
            {
                Console.WriteLine($"{id}: {card.Id}");
                if (card.Id == id)
                {
                    return card;
                }
            }
            return null;
        }

        public List<Card> GetAllCards()
        {
            return new List<Card>(_stack);
        }

        public override string ToString()
        {
            string str = string.Empty;
            foreach (Card card in _stack) 
            {
                str += card.ToString() + "\n";
            }

            return str;
        }
    }
}
