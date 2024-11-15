using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class Headers
    {
        private Dictionary<string, string> _headers;

        public Headers() 
        {
            _headers = new Dictionary<string, string>();
        }
        public Headers(Dictionary<string, string> headers) 
        {
            _headers = new Dictionary<string, string>(headers);
        }

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
