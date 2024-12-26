using MonsterTradingCardsGame.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class PlayerSession(User user, HttpResponseHandler responseHandler)
    {
        public User User { get; } = user;
        public HttpResponseHandler ResponseHandler { get; } = responseHandler;
    }
}
