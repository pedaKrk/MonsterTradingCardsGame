using MonsterTradingCardsGame.DAL.Services;
using Npgsql;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Connections
{
    internal class DataLayer : IDisposable
    {
        #region Singleton-Pattern
        private static DataLayer? instance;
        public static DataLayer Instance
        {
            get
            {
                return instance ??= new DataLayer();
            }
        }
        #endregion

        private readonly IDbConnection connection;

        public DataLayer()
        {
            var dbConfig = DatabaseConfigService.GetDatabaseConfig();
            connection = new NpgsqlConnection(DatabaseConfigService.GetConnectionString(dbConfig));
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
