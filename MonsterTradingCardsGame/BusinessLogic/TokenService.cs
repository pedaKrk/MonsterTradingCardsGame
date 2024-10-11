using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic
{
    internal class TokenService
    {
        private static readonly Dictionary<string, string> _userTokens = new Dictionary<string, string>();

        public static string GenerateToken(string username)
        {
            //StackOverflow
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            _userTokens[username] = token;

            return token;
        }

        public static bool IsValidToken(string username, string token)
        {
            return _userTokens.ContainsKey(username) && _userTokens[username] == token;
        }
    }
}
