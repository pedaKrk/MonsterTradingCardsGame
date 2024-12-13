﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal(string id, string cardId, string username, double price)
    {
        public string Id { get; } = id;
        public string CardId { get; } = cardId;
        public string Username { get; } = username;
        public double Price { get; } = price;
    }
}
