using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Headers(Dictionary<string, string> headers)
    {
        private readonly Dictionary<string, string> _headers = new(headers);

        public void AddHeader(string key, string value) 
        { 
            _headers[key] = value;
        }
        public bool HasKey(string key)
        {
            return _headers.ContainsKey(key);
        }
        public string GetValue(string key)
        {
            return _headers[key];
        }
    }
}
