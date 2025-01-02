using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class PackageRepository : IPackageRepository
    {
        private readonly DataLayer dal = new();
        private readonly CardRepository cardRepository = new();
        public void CreatePackage(Package package)
        {
            foreach (var card in package.Pack)
            {
                cardRepository.CreateCard(card);

                using IDbCommand dbCommand = dal.CreateCommand("""
                    INSERT INTO Packages (CardId)
                    VALUES (@CardId)
                """);

                DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, card.Id);

                dbCommand.ExecuteNonQuery();
            }
        }

        public void DeletePackage(Guid cardId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                DELETE FROM Packages WHERE CardId = @CardId
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, cardId);
            dbCommand.ExecuteNonQuery();
        }

        public Package? AcquirePackage()
        {
            List<Card> cards = new(Package.Size);

            using (IDbCommand dbCommand = dal.CreateCommand("""
                SELECT p.Id, c.Id AS CardId, c.Name, c.Damage, c.Element, c.CardType
                FROM Packages p
                JOIN Cards c ON p.CardId = c.Id
                ORDER BY p.Id DESC
                LIMIT 5
            """))
            using (IDataReader reader = dbCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    string? cardId = reader["CardId"].ToString();
                    string? name = reader["Name"].ToString();
                    string? element = reader["Element"].ToString();
                    string? cardType = reader["CardType"].ToString();

                    if (string.IsNullOrEmpty(cardId) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(element) || string.IsNullOrEmpty(cardType))
                    {
                        return null;
                    }

                    cards.Add(new Card(
                        Guid.Parse(cardId),
                        name,
                        Convert.ToDouble(reader["Damage"]),
                        Enum.Parse<Element>(element),
                        Enum.Parse<CardType>(cardType)
                    ));
                }
            }

            if (cards.Count != Package.Size)
            {
                return null;
            }

            foreach (Card card in cards)
            {
                DeletePackage(card.Id);
            }

            return new Package(cards);
        }
    }
}
