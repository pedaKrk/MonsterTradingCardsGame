using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.BusinessLogic.Token
{
    internal class TokenService
    {
        private static readonly Dictionary<string, string> _userTokens = [];
        private static readonly Dictionary<string, string> _tokenToUser = [];
        private const string TokenSuffix = "-mtcgToken";

        public static string GenerateToken(string username)
        {
            var token = $"{username}{TokenSuffix}";
            _userTokens[username] = token;
            _tokenToUser[token] = username;

            return token;
        }

        public static string? GetTokenByUsername(string username)
        {
            return _userTokens.TryGetValue(username, out var token) ? token : null;
        }

        public static string? GetUsernameByToken(string token)
        {
            return _tokenToUser.TryGetValue(token, out var username) ? username : null;
        }
    }
}
