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

        public Card? GetCardById(string cardId)
        {
            return _stack.FirstOrDefault(card => card.Id == cardId);
        }

        public bool HasCard(string cardId)
        {
            return _stack.Any(card => card.Id == cardId);
        }

        public List<Card> GetAllCards()
        {
            return new List<Card>(_stack);
        }
    }
}
