using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterTradingCardsGame.Models
{
    internal class UserProfile
    {
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public UserProfile() { }

        public UserProfile(UserProfile userProfile)
        {
            Name = userProfile.Name ?? Name;
            Bio = userProfile.Bio ?? Bio;
            Image = userProfile.Image ?? Image;
        }
    }
}
