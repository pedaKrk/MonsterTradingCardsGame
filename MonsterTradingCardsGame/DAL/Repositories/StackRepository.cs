using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class StackRepository : IStackRepository
    {
        private readonly DataLayer dal = DataLayer.Instance;
        public void AddCard(int userId, Guid cardId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO Stacks (UserId, CardId)
                VALUES (@UserId, @CardId)
            """
            );

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, cardId);

            dbCommand.ExecuteNonQuery();
        }

        public List<Card> GetAllCardsFromUser(int userId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT c.Id, c.Name, c.Damage, c.Element, c.CardType
                FROM Cards c
                JOIN Stacks s ON c.Id = s.CardId
                WHERE s.UserId = @UserId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);

            List<Card> cards = [];

            using IDataReader reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                string? id = reader["Id"].ToString();
                string? name = reader["Name"].ToString();
                string? element = reader["Element"].ToString();
                string? cardType = reader["CardType"].ToString();

                if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(element) || string.IsNullOrEmpty(cardType))
                {
                    continue;
                }

                var card = new Card(
                    Guid.Parse(id),
                    name,
                    Convert.ToDouble(reader["Damage"]),
                    Enum.Parse<Element>(element),
                    Enum.Parse<CardType>(cardType)
                );

                cards.Add(card);
            }

            return cards;
        }

        public Card? GetCardFromUser(int userId, Guid cardId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT c.Id, c.Name, c.Damage, c.Element, c.CardType
                FROM Cards c
                JOIN Stacks s ON c.Id = s.CardId
                WHERE s.UserId = @UserId AND c.Id = @CardId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, cardId);

            using IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                string? id = reader["Id"].ToString();
                string? name = reader["Name"].ToString();
                string? element = reader["Element"].ToString();
                string? cardType = reader["CardType"].ToString();

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(element) || string.IsNullOrEmpty(cardType))
                {
                    return null;
                }

                return new Card(
                    Guid.Parse(id),
                    name,
                    Convert.ToDouble(reader["Damage"]),
                    Enum.Parse<Element>(element),
                    Enum.Parse<CardType>(cardType)
                );
            }

            return null;
        }

        public void RemoveCard(int userId, Guid cardId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                DELETE FROM Stacks 
                WHERE UserId = @UserId AND CardId = @CardId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, cardId);

            dbCommand.ExecuteNonQuery();
        }

        public bool UserHasCard(int userId, Guid cardId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT COUNT(*) 
                FROM Stacks 
                WHERE UserId = @UserId AND CardId = @CardId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, cardId);

            var result = dbCommand.ExecuteScalar();
            return Convert.ToInt32(result) > 0;
        }
    }
}
