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
        public int Id { get; set; }
        public double Coins { get; set; } = 20;
        public UserData Data { get; set; }
        public UserStats Stats { get; set; }
        public Role Role { get; set; } = Role.User;
        public Stack Stack { get; } = new Stack();
        public Deck Deck { get; } = new Deck();

        public User(string username, string password)
        {
            Username = username;
            Password = password;

            Data = new UserData(Username);
            Stats = new UserStats(Username);
        }
    }
}
