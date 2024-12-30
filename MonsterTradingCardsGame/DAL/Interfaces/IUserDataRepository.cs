using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IUserDataRepository
    {
        void AddUserData(int userId, UserData userData);
        void UpdateUserData(int userId, UserData userData);
        UserData? GetUserData(int userId);
    }
}
