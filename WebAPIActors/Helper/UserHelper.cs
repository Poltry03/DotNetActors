using Dapper;
using MySql.Data.MySqlClient;
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
    }
}
