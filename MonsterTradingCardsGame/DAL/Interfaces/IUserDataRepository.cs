using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IUserDataRepository
    {
        void CreateUserData(UserData userData);
        void UpdateUserData(UserData userData);
        UserData GetUserData(string userId);
    }
}
