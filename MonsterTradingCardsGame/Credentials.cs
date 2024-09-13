using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame
{
    internal class Credentials
    {
        private string _username;

        public Credentials(string username, string password)
        {
            this._username = username;
            Password = password;
        }

        public string Password { get; set; }
    }
}
