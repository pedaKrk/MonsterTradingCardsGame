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
        public string Name { get; set; } = string.Empty;
        public string Bio {  get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
        public Stack Stack { get; } = new Stack();
        public Deck Deck { get; } = new Deck();
    }
}
