using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class User(string username, string password)
    {
        public string Username { get; } = username;
        public string Password { get; } = password;
        public int Id { get; set; }
        public int Coins { get; set; } = 20;
        public UserProfile Profile { get; set; } = new UserProfile();
        public Role Role { get; set; } = Role.User;
        public Stack Stack { get; } = new Stack();
        public Deck Deck { get; } = new Deck();
    }
}
