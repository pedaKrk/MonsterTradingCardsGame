using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class User
    {
        private Credentials _credentials;

        private Stack _stack;

        private Deck _deck;

        public User(string username, string password)
        {
            _credentials = new Credentials(username, password);
            _stack = new Stack();
            _deck = new Deck();

            Coins = 20;
        }

        public int Coins {  get; set; }

        public void BuyPack()
        {
            if ((Coins - Package.Price) < 0)
            {
                Console.WriteLine("Not enough Coins to purchase Package!");
                return;
            }

            //ToDo: logic
        }
    }
}
