using Microsoft.Extensions.Configuration;
using MonsterTradingCardsGame.DAL.Configurations;

namespace MonsterTradingCardsGame.DAL.Services
{
    internal class DatabaseConfigService
    {
        public static DatabaseConfig GetDatabaseConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            var host = configuration.GetSection("DatabaseSettings:Host").Value;
            var database = configuration.GetSection("DatabaseSettings:Database").Value;
            var username = configuration.GetSection("DatabaseSettings:Username").Value;
            var password = configuration.GetSection("DatabaseSettings:Password").Value;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(database) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("DatabaseSettings section is missing or invalid in the configuration.");
            }

            var dbConfig = new DatabaseConfig
            (
                host,
                database,
                username,
                password
            );

            return dbConfig;
        }

        public static string GetConnectionString(DatabaseConfig dbConfig)
        {
            return $"Host={dbConfig.Host};Database={dbConfig.Database};Username={dbConfig.Username};Password={dbConfig.Password};Persist Security Info=True";
        }
    }
}
