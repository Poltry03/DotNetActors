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

            var passwordHash = PasswordHelper.HashPassword(password, foundUser.Salt);

            if (foundUser.PasswordHash == passwordHash)
                return foundUser;

            return null;
        }

        internal static User GetUserByUsername(string username)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    var sql = "SELECT * FROM user " +
                        "WHERE username = @Username";
                    var foundUser = connection.QueryFirst<User>(sql, new { Username = username });
                    return foundUser;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static int Insert(User user)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    var sql = "INSERT INTO user (Username, PasswordHash, Salt)" +
                        " VALUES (@Username, @PasswordHash, @Salt); " +
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
