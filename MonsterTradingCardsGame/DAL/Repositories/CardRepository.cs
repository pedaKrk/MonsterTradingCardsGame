using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class CardRepository : ICardRepository
    {
        private readonly DataLayer dal = DataLayer.Instance;

        public void CreateCard(Card card)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO Cards (Id, Name, Damage, Element, CardType)
                VALUES (@Id, @Name, @Damage, @Element, @CardType)
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.Guid, card.Id);
            DataLayer.AddParameterWithValue(dbCommand, "@Name", DbType.String, card.Name);
            DataLayer.AddParameterWithValue(dbCommand, "@Damage", DbType.Double, card.Damage);
            DataLayer.AddParameterWithValue(dbCommand, "@Element", DbType.String, card.Element.ToString());
            DataLayer.AddParameterWithValue(dbCommand, "@CardType", DbType.String, card.CardType.ToString());

            dbCommand.ExecuteNonQuery();
        }

        public Card? GetCard(Guid cardId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, Name, Damage, Element, CardType
                FROM Cards
                WHERE Id = @CardId
            """);

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
    }
}
