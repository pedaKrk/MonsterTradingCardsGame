namespace MonsterTradingCardsGame.DAL.Configurations
{
    public class DatabaseConfig(string host, string database, string username, string password)
    {
        public string Host { get; } = host;
        public string Database { get; } = database;
        public string Username { get; } = username;
        public string Password { get; } = password;
    }
}
