using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal(TradingDealDTO tradingDealDTO, string username)
    {
        public string Id { get; } = tradingDealDTO.Id;
        public string CardId { get; } = tradingDealDTO.CardId;
        public double Price { get; } = tradingDealDTO.Price;
        public string Username { get; } = username;
    }
}
