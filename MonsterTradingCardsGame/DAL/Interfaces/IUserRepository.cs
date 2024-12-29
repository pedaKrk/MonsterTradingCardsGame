using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    internal interface IUserRepository
    {
        void AddUser(User user);
        User? GetUserByUsername(string username);
        List<User> GetAllUsers();
        void UpdateUser(User user);
        bool UserExists(string username);
    }
}
