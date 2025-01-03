
using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class DeckRepository : IDeckRepository
    {
        private readonly DataLayer dal = DataLayer.Instance;
        public void AddCard(int userId, Guid cardId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO Decks (UserId, CardId)
                VALUES (@UserId, @CardId)
            """
            );

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, cardId);

            dbCommand.ExecuteNonQuery();
        }

        public void AddCardsToUser(int userId, List<Card> cards)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO Decks (UserId, CardId)
                VALUES (@UserId, @CardId)
            """);

            foreach (var card in cards)
            {
                DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);
                DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, card.Id);
                dbCommand.ExecuteNonQuery();
            }
        }


        public List<Card> GetDeckFromUser(int userId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT c.Id, c.Name, c.Damage, c.Element, c.CardType
                FROM Cards c
                JOIN Decks d ON c.Id = d.CardId
                WHERE d.UserId = @UserId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@UserId", DbType.Int32, userId);

            List<Card> deck = [];

            using IDataReader reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                string? id = reader["Id"].ToString();
                string? name = reader["Name"].ToString();
                string? element = reader["Element"].ToString();
                string? cardType = reader["CardType"].ToString();

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(element) || string.IsNullOrEmpty(cardType))
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

                deck.Add(card);
            }

            return deck;
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
    }
}
