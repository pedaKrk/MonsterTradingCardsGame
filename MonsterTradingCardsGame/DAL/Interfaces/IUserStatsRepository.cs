using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IUserStatsRepository
    {
        void CreateUserStats(UserStats userStats);
        void UpdateUserStats(UserStats userStats);
        void GetUserStats(string userId);
    }
}
