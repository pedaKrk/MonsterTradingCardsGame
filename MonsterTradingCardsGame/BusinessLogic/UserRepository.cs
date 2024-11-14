using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonsterTradingCardsGame.Models;

namespace MonsterTradingCardsGame.BusinessLogic
{
    internal class UserRepository
    {
        private readonly DataLayer dal;

        public UserRepository()
        {
            dal = DataLayer.Instance;
        }

        public void Add(User user)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
            INSERT INTO person (name, age, description)
            VALUES (@name, @age, @description)
            RETURNING id
            """);
            DataLayer.AddParameterWithValue(dbCommand, "@name", DbType.String, user.Username);

            //dbCommand.ExecuteNonQuery(); // does not return the id
            user.Id = (int)(dbCommand.ExecuteScalar() ?? 0);
        }

        public List<User> GetAll()
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
            SELECT id, name, age, description 
            FROM person 
            """);

            List<User> result = [];   // new List<Person>()
            using IDataReader reader = dbCommand.ExecuteReader();
            while (reader.Read())
            {
                //result.Add(new User());
            }
            return result;
        }

        public User? Get(int id)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
            SELECT id, name, age, description 
            FROM person 
            WHERE id = @id
            """);
            DataLayer.AddParameterWithValue(dbCommand, "@id", DbType.Int32, id);

            using IDataReader reader = dbCommand.ExecuteReader();
            if (reader.Read())
            {
                //return new User()
            }
            return null;
        }

        public void Update(User user)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
            UPDATE person 
            SET name = @name, age = @age, description = @description
            WHERE id = @id
            """);
            DataLayer.AddParameterWithValue(dbCommand, "@name", DbType.String, user.Username);
            DataLayer.AddParameterWithValue(dbCommand, "@id", DbType.Int32, user.Id);
            dbCommand.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using IDbCommand dbCommand = dal.CreateCommand("""
            DELETE FROM person 
            WHERE id = @id
            """);
            DataLayer.AddParameterWithValue(dbCommand, "@id", DbType.Int32, id);
            dbCommand.ExecuteNonQuery();
        }
    }
}
