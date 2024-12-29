using Npgsql;
using System.Data;

namespace MonsterTradingCardsGame.DAL.Connections
{
    internal class DataLayer : IDisposable
    {
        #region Singleton-Pattern
        private static DataLayer instance;
        public static DataLayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataLayer("Host=localhost;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True");
                }
                return instance;
            }
        }
        #endregion


        private readonly string connectionString;
        private IDbConnection connection;

        public DataLayer(string connectionString)
        {
            this.connectionString = connectionString;
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
