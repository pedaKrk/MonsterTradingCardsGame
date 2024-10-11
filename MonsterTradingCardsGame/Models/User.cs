using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class User
    {

        public string Username { get; }
        public string Password { get; }
        public int Coins { get; set; }

        private readonly Stack _stack;

        private readonly Deck _deck;

        public string? Token {  get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            _stack = new Stack();
            _deck = new Deck();

            Coins = 20;
        }

        public void RemoveCoins(int amount)
        {
            Coins -= amount;
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
