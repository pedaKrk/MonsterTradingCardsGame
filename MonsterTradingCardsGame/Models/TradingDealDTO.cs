using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDealDTO(string id, string cardId, double price)
    {
        public string Id { get; } = id;
        public string CardId { get; } = cardId;
        public double Price { get; } = price;
    }
}
