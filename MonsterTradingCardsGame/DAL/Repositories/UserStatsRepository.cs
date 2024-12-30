using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class UserStatsRepository : IUserStatsRepository
    {
        private readonly DataLayer dal;

        public UserStatsRepository()
        {
            dal = DataLayer.Instance;
        }

        public void AddUserStats(int userId, UserStats userStats)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO UserStats (UserId, Elo, Wins, Losses)
                VALUES (@UserId, @Elo, @Wins, @Losses)
                RETURNING Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@Elo", DbType.Int32, userStats.Elo);
            DataLayer.AddParameterWithValue(dbCommand, "@Wins", DbType.Int32, userStats.Wins);
            DataLayer.AddParameterWithValue(dbCommand, "@Losses", DbType.Int32, userStats.Losses);

            dbCommand.ExecuteNonQuery();
        }

        public List<UserStats> GetAllUserStats()
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, UserId, Elo, Wins, Losses 
                FROM UserStats
            """);

            List<UserStats> userStatsList = new List<UserStats>();

            using IDataReader reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                string? name = reader["Name"].ToString();

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                var userStats = new UserStats(name)
                {
                    Elo = Convert.ToInt32(reader["Elo"]),
                    Wins = Convert.ToInt32(reader["Wins"]),
                    Losses = Convert.ToInt32(reader["Losses"])
                };

                userStatsList.Add(userStats);
            }

            return userStatsList;
        }

        public UserStats? GetUserStats(int userId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, UserId, Elo, Wins, Losses
                FROM UserStats
                WHERE UserId = @UserId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);

            using IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                string? name = reader["Name"].ToString();

                if (string.IsNullOrEmpty(name))
                {
                    return null;
                }

                return new UserStats(name)
                {
                    Elo = Convert.ToInt32(reader["Elo"]),
                    Wins = Convert.ToInt32(reader["Wins"]),
                    Losses = Convert.ToInt32(reader["Losses"])
                };
            }

            return null;
        }

        public void UpdateUserStats(int userId, UserStats userStats)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                UPDATE UserStats
                SET Elo = @Elo, Wins = @Wins, Losses = @Losses
                WHERE UserId = @UserId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@Elo", DbType.Int32, userStats.Elo);
            DataLayer.AddParameterWithValue(dbCommand, "@Wins", DbType.Int32, userStats.Wins);
            DataLayer.AddParameterWithValue(dbCommand, "@Losses", DbType.Int32, userStats.Losses);

            dbCommand.ExecuteNonQuery();
        }
    }
}
