using Dapper;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using WebAPIActors.Models;

namespace WebAPIActors.Helper
{
    public static class UserHelper
    {
        public static string ConnectionString
        {
            get { return DatabaseHelper.ConnectionString; }
        }

        public static User LoginTry(string username, string password)
        {
            User foundUser = new User();
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = "SELECT * FROM user " +
                    "WHERE username = @Username";
                foundUser = connection.QueryFirst<User>(sql, new { Username = username});
            }

            if (foundUser.Password == password)
                return foundUser;
            return null;
        }

        internal static User GetUserByUsername(string username)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = "SELECT * FROM user " +
                    "WHERE username = @Username";
                var foundUser = connection.QueryFirst<User>(sql, new { Username = username });
                return foundUser;
            }
        }

        internal static int Insert(User user)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    var sql = "INSERT INTO user (Usernameame, Password)" +
                        " VALUES (@Username, @Password); " +
                        "SELECT LAST_INSERT_ID();";
                    var id = connection.ExecuteScalar<int>(sql, user);
                    return id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //log error
                return 0;
            }
        }
    }
}
