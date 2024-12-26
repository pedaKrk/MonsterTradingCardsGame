using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Exceptions
{
    internal class InternalServerException : Exception
    {
        public InternalServerException() : base() { }
        public InternalServerException(string message) : base(message) { }
        public InternalServerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
