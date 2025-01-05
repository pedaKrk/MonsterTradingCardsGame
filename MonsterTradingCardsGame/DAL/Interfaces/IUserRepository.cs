using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.DAL.Interfaces
{
    public interface IUserRepository
    {
        int AddUser(User user);
        User? GetUserByUsername(string username);
        List<User> GetAllUsers();
        void UpdateUser(User user);
        bool UserExists(string username);
    }
}
