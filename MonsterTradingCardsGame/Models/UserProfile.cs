using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class UserProfile
    {
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }

        public UserProfile() { }

        public void Update(UserProfile userProfile)
        {
            Name = userProfile.Name ?? Name;
            Bio = userProfile.Bio ?? Bio;
            Image = userProfile.Image ?? Image;
        }
    }
}
