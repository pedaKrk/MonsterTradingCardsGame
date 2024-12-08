using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class TradingDeal(string id, string cardId, CardType cardType, float minDamage)
    {
        public string Id { get; } = id;
        public string CardId { get; } = cardId;
        public CardType CardType { get; } = cardType;
        public float MinDamage { get; } = minDamage;
    }
}
