using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal(string id, string cardId, double price, string username)
    {
        public string Id { get; } = id;
        public string CardId { get; } = cardId;
        public double Price { get; } = price;
        public string Username { get; } = username;
    }
}
