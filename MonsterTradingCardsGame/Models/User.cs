using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterTradingCardsGame.Authentication;

namespace MonsterTradingCardsGame.Models
{
    internal class User
    {
        private readonly Credentials _credentials;

        private readonly Stack _stack;

        private readonly Deck _deck;

        private int _coins;

        public User(string username, string password)
        {
            _credentials = new Credentials(username, password);
            _stack = new Stack();
            _deck = new Deck();

            _coins = 20;
        }
        public int Coins { get { return _coins; } }

        public void RemoveCoins(int amount)
        {
            _coins -= amount;
        }

        public void BuyPack()
        {
            if (Coins - Package.Price < 0)
            {
                Console.WriteLine("Not enough Coins to purchase Package!");
                return;
            }

            //ToDo: logic
        }

        public void AddCardToDeck(Card card)
        {
            _deck.AddCard(card);
        }

        public void RemoveCardFromDeck(Card card)
        {
            _deck.RemoveCard(card);
        }

        public void RemoveCardFromDeckAt(int index)
        {
            _deck.RemoveCard(index);
        }

        public void SwapCardInDeck(Card oldCard, Card newCard)
        {
            _deck.SwapCard(oldCard, newCard);
        }
    }
}
