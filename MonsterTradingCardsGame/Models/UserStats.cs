using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class UserStats(string name)
    {
        public string Name { get; set; } = name;
        public int Elo { get; set; } = 1000;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;

    }
}
