using WebAPIActors.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace WebAPIActors.Helper
{
    public class DatabaseHelper
    {
        public static string ConnectionString { get; set; }

        public static List<Actor> GetAllActors()
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = "SELECT * FROM Actor";
                var actor = connection.Query<Actor>(sql);
                return actor.ToList();
            }
        }

        internal static bool Delete(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    var sql = "DELETE FROM actor WHERE Id = @Id";
                    connection.Execute(sql, new { Id = id });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        internal static List<Actor> FindActor(string research)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {   
                research = "%" + research + "%";
                var sql = "SELECT * " +
                    "FROM Actor WHERE Name LIKE @research " +
                    "OR  Surname LIKE @research " +
                    "OR Nation LIKE @research";
                var foundActors = connection.Query<Actor>(sql, new {Research = research}).ToList();
                return foundActors;
            }
        }

        internal static Actor GetActorById(int id)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = "SELECT * FROM Actor WHERE Id = @Id";
                var attore = connection.QueryFirstOrDefault<Actor>(sql, new { Id = id });
                return attore;
            }
        }

        internal static int Insert(Actor attore)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    var sql = "INSERT INTO actor (Name, Surname, nation, filmquantity, filmcachet, imgurl)" +
                        " VALUES (@Name, @Surname, @Nation, @FilmQuantity, @FilmCachet, @ImgUrl; " +
                        "SELECT LAST_INSERT_ID();";
                    var id = connection.ExecuteScalar<int>(sql, attore);
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

        internal static bool Update(Actor attore)
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    var sql = "UPDATE Actor SET Name = @Name, Surname = @Surname, Nation = @Nation, FilmQuantity = @FilmQuantity, FilmCachet = @FilmCachet, ImgUrl = @ImgUrl WHERE Id = @Id";
                    connection.Execute(sql, attore);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
