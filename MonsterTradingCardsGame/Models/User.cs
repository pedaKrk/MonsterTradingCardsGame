using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class User
    {

        public string Username { get; private set;  }
        public string Password { get; private set; }
        public int Id { get; set; }
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
    }
}
