using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Http.Models
{
    public class Headers
    {
        private readonly Dictionary<string, string> _headers = [];

        public void AddHeader(string key, string value)
        {
            _headers[key] = value;
        }
        public bool HasKey(string key)
        {
            return _headers.ContainsKey(key);
        }
        public string? TryGetValue(string key)
        {
            return _headers.TryGetValue(key, out var value) ? value : null;
        }
        public string GetValue(string key)
        {
            return _headers[key];
        }
    }
}
