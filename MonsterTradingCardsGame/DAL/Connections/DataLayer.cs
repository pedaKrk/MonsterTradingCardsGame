using Npgsql;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Connections
{
    internal class DataLayer : IDisposable
    {
        private readonly string connectionString = "Host=localhost;Database=monster_trading_cards_game;Username=postgres;Password=postgres;Persist Security Info=True";
        private readonly IDbConnection connection;

        public DataLayer()
        {
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public IDbCommand CreateCommand(string commandText)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        public static void AddParameterWithValue(IDbCommand command, string parameterName, DbType type, object? value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = type;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }
}
