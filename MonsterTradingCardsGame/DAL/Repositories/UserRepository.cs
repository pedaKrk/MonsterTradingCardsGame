using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DataLayer dal = new();
        private readonly UserDataRepository userDataRepository = new();
        private readonly UserStatsRepository userStatsRepository = new();

        public void AddUser(User user)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO Users (Username, Password, Coins, Role)
                VALUES (@Username, @Password, @Coins, @Role)
                RETURNING Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Username", DbType.String, user.Username);
            DataLayer.AddParameterWithValue(dbCommand, "@Password", DbType.String, user.Password);
            DataLayer.AddParameterWithValue(dbCommand, "@Coins", DbType.Double, user.Coins);
            DataLayer.AddParameterWithValue(dbCommand, "@Role", DbType.String, user.Role.ToString());

            user.Id = (int)(dbCommand.ExecuteScalar() ?? 0);

            userDataRepository.AddUserData(user.Id, user.Data);
            userStatsRepository.AddUserStats(user.Id, user.Stats);
        }

        public List<User> GetAllUsers()
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, Username, Password, Coins, Role 
                FROM Users
            """);

            List<User> users = [];

            using IDataReader reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                string? username = reader["Username"].ToString();
                string? password = reader["Password"].ToString();
                string? role = reader["Role"].ToString();

                if(String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(role))
                {
                    continue;
                }

                var user = new User(username, password)
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Coins = Convert.ToDouble(reader["Coins"]),
                    Role = Enum.Parse<Role>(role)
                };

                UserData? userData = userDataRepository.GetUserData(user.Id);
                UserStats? userStats = userStatsRepository.GetUserStats(user.Id);

                if (userData == null || userStats == null)
                {
                    continue;
                }

                user.Data = userData;
                user.Stats = userStats;

                users.Add(user);
            }

            return users;
        }

        public User? GetUserByUsername(string name)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, Username, Password, Coins, Role
                FROM Users 
                WHERE Username = @Username
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Username", DbType.String, name);

            using IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                string? username = reader["Username"].ToString();
                string? password = reader["Password"].ToString();
                string? role = reader["Role"].ToString();

                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(role))
                {
                    return null;
                }

                var user = new User(username, password)
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Coins = Convert.ToDouble(reader["Coins"]),
                    Role = Enum.Parse<Role>(role)
                };

                UserData? userData = userDataRepository.GetUserData(user.Id);
                UserStats? userStats = userStatsRepository.GetUserStats(user.Id);

                if(userData == null || userStats == null)
                {
                    return null;
                }

                user.Data = userData;
                user.Stats = userStats;

                return user;
            }

            return null;
        }

        public void UpdateUser(User user)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                UPDATE Users
                SET Password = @Password, Coins = @Coins, Role = @Role
                WHERE Id = @Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.Int32, user.Id);
            DataLayer.AddParameterWithValue(dbCommand, "@Password", DbType.String, user.Password);
            DataLayer.AddParameterWithValue(dbCommand, "@Coins", DbType.Double, user.Coins);
            DataLayer.AddParameterWithValue(dbCommand, "@Role", DbType.String, user.Role.ToString());

            dbCommand.ExecuteNonQuery();

            userDataRepository.UpdateUserData(user.Id, user.Data);
            userStatsRepository.UpdateUserStats(user.Id, user.Stats);
        }

        public bool UserExists(string username)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT COUNT(*) 
                FROM Users 
                WHERE Username = @Username
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Username", DbType.String, username);
            var result = dbCommand.ExecuteScalar();
            return Convert.ToInt32(result) > 0;
        }
    }
}
