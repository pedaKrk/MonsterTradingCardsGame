using MonsterTradingCardsGame.DAL.Connections;
using MonsterTradingCardsGame.DAL.Interfaces;
using MonsterTradingCardsGame.Models;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Repositories
{
    internal class TradingDealRepository : ITradingDealRepository
    {
        private readonly DataLayer dal;

        public TradingDealRepository()
        {
            dal = DataLayer.Instance;
        }

        public bool TradingDealExists(string tradingDealId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT COUNT(*) FROM TradingDeals WHERE Id = @Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.String, tradingDealId);

            var result = dbCommand.ExecuteScalar();
            return Convert.ToInt32(result) > 0;
        }

        public void AddTradingDeal(TradingDeal tradingDeal)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                INSERT INTO TradingDeals (Id, CardId, Price, Username)
                VALUES (@Id, @CardId, @Price, @Username)
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.String, tradingDeal.Id);
            DataLayer.AddParameterWithValue(dbCommand, "@CardId", DbType.String, tradingDeal.CardId);
            DataLayer.AddParameterWithValue(dbCommand, "@Price", DbType.Double, tradingDeal.Price);
            DataLayer.AddParameterWithValue(dbCommand, "@Username", DbType.String, tradingDeal.Username);

            dbCommand.ExecuteNonQuery();
        }

        public TradingDeal? GetTradingDeal(string tradingDealId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                SELECT Id, CardId, Price, Username
                FROM TradingDeals
                WHERE Id = @Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.String, tradingDealId);

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
                    id,
                    cardId,
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

            List<TradingDeal> tradingDeals = new List<TradingDeal>();

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
                    id,
                    cardId,
                    Convert.ToDouble(reader["Price"]),
                    username
                ));

            }

            return tradingDeals;
        }

        public void DeleteTradingDeal(string tradingDealId)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
                DELETE FROM TradingDeals WHERE Id = @Id
            """);

            DataLayer.AddParameterWithValue(dbCommand, "@Id", DbType.String, tradingDealId);

            dbCommand.ExecuteNonQuery();
        }
    }
}
