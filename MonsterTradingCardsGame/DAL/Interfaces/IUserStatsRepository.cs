using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IUserStatsRepository
    {
        void AddUserStats(int userId, UserStats userStats);
        void UpdateUserStats(int userId, UserStats userStats);
        UserStats? GetUserStats(int userId);
    }
}
