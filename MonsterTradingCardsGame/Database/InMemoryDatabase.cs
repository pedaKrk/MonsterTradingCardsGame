using MonsterTradingCardsGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Database
{
    internal class InMemoryDatabase
    {
        public static List<User> Users { get; } = new List<User>();
        public static List<Card> Cards { get; } = new List<Card>();

        public static void AddUser(User user)
        {
            Users.Add(user);
        }

        public static bool UserExists(string username)
        {
            return Users.Any(u => u.Username == username);
        }
    }
}
