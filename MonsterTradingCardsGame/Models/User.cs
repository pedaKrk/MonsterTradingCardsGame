using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class User
    {

        private string _username;

        private string _password;

        private readonly Stack _stack;

        private readonly Deck _deck;

        private int _coins;

        public User(string username, string password)
        {
            _username = username;
            _password = password;
            _stack = new Stack();
            _deck = new Deck();

            _coins = 20;
        }
        public int Coins { get { return _coins; } }

        public string Username { get { return _username; } }
        public string Password { get { return _password; } }

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

            Package package = new Package();

            _stack.AddCards(package.Open());
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

        public string StackToString()
        {
            return _stack.ToString();
        }

        public string DeckToString()
        {
            return _deck.ToString();
        }
    }
}
