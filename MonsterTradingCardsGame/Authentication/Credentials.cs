using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Authentication
{
    internal class Credentials
    {
        private readonly string _username;

        private string _password;

        public Credentials(string username, string password)
        {
            _username = username;
            _password = password;
        }
    }
}
