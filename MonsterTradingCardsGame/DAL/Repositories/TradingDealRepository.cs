using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class TradingDealRepository : ITradingDealRepository
    {
        private readonly DataLayer dal = new();

        public bool TradingDealExists(Guid tradingDealId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT COUNT(*) FROM TradingDeals WHERE Id = @Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.Guid, tradingDealId);

            var result = dbCommand.ExecuteScalar();
            return Convert.ToInt32(result) > 0;
        }

        public void AddTradingDeal(TradingDeal tradingDeal)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO TradingDeals (Id, CardId, Price, Username)
                VALUES (@Id, @CardId, @Price, @Username)
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.Guid, tradingDeal.Id);
            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.Guid, tradingDeal.CardId);
            DataLayer.AddParameterWithValue(dbCommand, "@Price", DbType.Double, tradingDeal.Price);
            DataLayer.AddParameterWithValue(dbCommand, "@Username", DbType.String, tradingDeal.Username);

            dbCommand.ExecuteNonQuery();
        }

        public TradingDeal? GetTradingDeal(Guid tradingDealId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, CardId, Price, Username
                FROM TradingDeals
                WHERE Id = @Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.Guid, tradingDealId);

            using IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                string? id = reader["Id"].ToString();
                string? cardId = reader["CardId"].ToString();
                string? username = reader["Username"].ToString();

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(cardId) || string.IsNullOrEmpty(username))
                {
                    return null;
                }

                return new TradingDeal(
                    Guid.Parse(id),
                    Guid.Parse(cardId),
                    Convert.ToDouble(reader["Price"]),
                    username
                );
            }

            return null;
        }

        public List<TradingDeal> GetAllTradingDeals()
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, CardId, Price, Username
                FROM TradingDeals
            """);

            List<TradingDeal> tradingDeals = [];

            using IDataReader reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                string? id = reader["Id"].ToString();
                string? cardId = reader["CardId"].ToString();
                string? username = reader["Username"].ToString();

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(cardId) || string.IsNullOrEmpty(username))
                {
                    continue;
                }

                tradingDeals.Add(new TradingDeal(
                    Guid.Parse(id),
                    Guid.Parse(cardId),
                    Convert.ToDouble(reader["Price"]),
                    username
                ));

            }

            return tradingDeals;
        }

        public void DeleteTradingDeal(Guid tradingDealId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                DELETE FROM TradingDeals WHERE Id = @Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.Guid, tradingDealId);

            dbCommand.ExecuteNonQuery();
        }
    }
}
