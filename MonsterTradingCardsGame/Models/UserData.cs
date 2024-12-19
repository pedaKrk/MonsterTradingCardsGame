using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class UserData
    {
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }

        public UserData(string name) 
        {
            Name = name;
        }

        public void Update(UserData userData)
        {
            Name = userData.Name ?? Name;
            Bio = userData.Bio ?? Bio;
            Image = userData.Image ?? Image;
        }
    }
}
