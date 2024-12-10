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
        public static List<Package> Packages { get; } = new List<Package>();
        public static List<TradingDeal> TradingDeals { get; } = new List<TradingDeal>();

        public static void AddUser(User user)
        {
            Users.Add(user);
        }

        public static User? GetUser(string username)
        {
            foreach (User user in Users)
            {
                if (user.Username == username)
                {
                    return user;
                }
            }
            return null;
        }

        public static bool UserExists(string username)
        {
            return Users.Any(u => u.Username == username);
        }

        public static void AddPackage(Package package)
        {
            Packages.Add(package);
        }

        public static Package? AcquirePackage()
        {
            Package? package = Packages.FirstOrDefault();
            if (package == null) 
            {
                return null;
            }

            Packages.RemoveAt(0);

            return package;
        }

        public static bool TradingDealExists(string id)
        {
            return TradingDeals.Any(d => d.Id == id);
        }

        public static void AddTradingDeal(TradingDeal deal)
        {
            TradingDeals.Add(deal);
        }

        public static bool DeleteTradingDeal(string id)
        {
            var deal = TradingDeals.FirstOrDefault(d => d.Id == id);
            if (deal == null)
            {
                return false;
            }

            TradingDeals.Remove(deal);
            return true;   
        }
    }
}
