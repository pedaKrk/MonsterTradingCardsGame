namespace MonsterTradingCardsGame.DAL.Configurations
{
    internal class DatabaseConfig(string host, string database, string username, string password)
    {
        public string Host { get; set; } = host;
        public string Database { get; set; } = database;
        public string Username { get; set; } = username;
        public string Password { get; set; } = password;
    }
}
