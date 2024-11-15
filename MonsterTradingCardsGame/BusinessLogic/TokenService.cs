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
        private const string TokenSuffix = "-mtcgToken";

        public static void GenerateToken(string username)
        {
            _userTokens[username] = $"{username}{TokenSuffix}";
        }

        public static bool IsValidToken(string token)
        {
            foreach (var value in _userTokens.Values) 
            {
                if (value == token)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasUserToken(string username)
        {
            return _userTokens.ContainsKey(username);
        }

        public static bool HasToken(string token)
        {
            foreach (var value in _userTokens.Values)
            {
                if (value == token)
                {
                    return true;
                }
            }

            return false;
        }
        

        public static string GetTokenByUsername(string username)
        {
            return _userTokens[username];
        }

        public static string? GetToken(string token)
        {
            foreach(var value in _userTokens.Values)
            {
                if(value == token)
                {
                    return value;
                }
            }

            return null;
        }
    }
}
