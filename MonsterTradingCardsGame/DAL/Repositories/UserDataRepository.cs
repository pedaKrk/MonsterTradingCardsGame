using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class UserDataRepository : IUserDataRepository
    {
        private readonly DataLayer dal;

        public UserDataRepository()
        {
            dal = DataLayer.Instance;
        }

        public void AddUserData(int userId, UserData userData)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO UserData (UserId, Name, Bio, Image)
                VALUES (@UserId, @Name, @Bio, @Image)
                RETURNING Id
            """);
            
            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@Name", DbType.String, userData.Name);
            DataLayer.AddParameterWithValue(dbCommand, "@Bio", DbType.String, userData.Bio);
            DataLayer.AddParameterWithValue(dbCommand, "@Image", DbType.String, userData.Image);

            dbCommand.ExecuteNonQuery();
        }

        public UserData? GetUserData(int userId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
            SELECT Id, UserId, Name, Bio, Image
            FROM UserData
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

                return new UserData(name)
                {
                    Bio = reader["Bio"].ToString(),
                    Image = reader["Image"].ToString()
                };   
            }
            
            return null;
        }

        public void UpdateUserData(int userId, UserData userData)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
            UPDATE UserData
            SET Name = @Name, Bio = @Bio, Image = @Image
            WHERE UserId = @UserId
            """);
            
            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@Name", DbType.String, userData.Name);
            DataLayer.AddParameterWithValue(dbCommand, "@Bio", DbType.String, userData.Bio ?? string.Empty);
            DataLayer.AddParameterWithValue(dbCommand, "@Image", DbType.String, userData.Image ?? string.Empty);
            
            dbCommand.ExecuteNonQuery();       
        }
    }
}
