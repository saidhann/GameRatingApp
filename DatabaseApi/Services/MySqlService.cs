using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
using System.Data;
using System.Data.Common;
using System.Net.Mail;

namespace DatabaseApi.Services
{
    public class MySqlService : IMySqlService
    {
        private readonly ILogger<MySqlService> _logger;
        private IConfiguration _configuration { get; set; }
        public MySqlService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnectionString()
        {
            return this._configuration.GetConnectionString("DefaultConnection");
        }
        public void AddUser(LoginPasswordItem logpas)
        {
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();

                // Retrieve the next ID_uzytkownika
                int nextUserId = GetNextUserId(con);

                // Insert the new user into Uzytkownicy table
                string query = "INSERT INTO Uzytkownicy (ID_uzytkownika, Login, Haslo, Data_rejestracji) " +
                               "VALUES (@UserId, @Login, @Password, NOW())";

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", nextUserId);
                    cmd.Parameters.AddWithValue("@Login", logpas.Login);
                    cmd.Parameters.AddWithValue("@Password", logpas.Password);
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
        private int GetNextUserId(MySqlConnection con)
        {
            int nextUserId = 0;
            string query = "SELECT IFNULL(MAX(ID_uzytkownika), 0) + 1 AS NextUserId FROM Uzytkownicy";

            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    nextUserId = rdr.GetInt32("NextUserId");
                }
                rdr.Close();
            }

            return nextUserId;
        }
        public IEnumerable<TableItem> ShowTables()
        {
            List<TableItem> tableList = new List<TableItem>();

            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using(MySqlCommand cmd = new MySqlCommand("show tables", con))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        TableItem tempTableName = new TableItem(rdr.GetString(0));
                        tableList.Add(tempTableName);
                    }
                }
            }
            return tableList;
        }
        public IEnumerable<string> GetAllGenres()
        {
            List<string> genreList = new List<string>();
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("select Nazwa_gatunku from Gatunki", con))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        string tempGenre = rdr.GetString("Nazwa_gatunku");
                        genreList.Add(tempGenre);
                    }
                }
                con.Close();
            }
            return genreList;
        }
        public IEnumerable<CommentItem> GetComments(string gameName)
        {
            List<CommentItem> genreList = new List<CommentItem>();
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT Komentarze.Tresc_komentarza, " +
                    "Komentarze.Data_dodania, " +
                    "Uzytkownicy.Login " +
                    "FROM Komentarze " +
                    "JOIN Gry ON Komentarze.ID_gry = Gry.ID_gry " +
                    "JOIN Uzytkownicy ON Komentarze.ID_uzytkownika = Uzytkownicy.ID_uzytkownika " +
                    $"WHERE Gry.Tytul = '{gameName}'", con))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        CommentItem tempGenre = new CommentItem(rdr.GetString("Login"),rdr.GetString("Tresc_komentarza"), rdr.GetDateTime("Data_dodania"));
                        genreList.Add(tempGenre);
                    }
                }
                con.Close();
            }
            return genreList;
        }
        public IEnumerable<RatingItem> GetRatings(string gameName)
        {
            List<RatingItem> genreList = new List<RatingItem>();
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT Uzytkownicy.Login, " +
                    "Oceny.Ocena, " +
                    "Oceny.Data_oceny " +
                    "FROM Oceny " +
                    "JOIN Gry ON Oceny.ID_gry = Gry.ID_gry " +
                    "JOIN Uzytkownicy ON Oceny.ID_uzytkownika = Uzytkownicy.ID_uzytkownika " +
                    $"WHERE Gry.Tytul = '{gameName}'", con))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        RatingItem tempGenre = new RatingItem(rdr.GetString("Login"),rdr.GetInt32("Ocena"), rdr.GetDateTime("Data_oceny"));
                        genreList.Add(tempGenre);
                    }
                }
                con.Close();
            }
            return genreList;
        }
        public bool Login(LoginPasswordItem logpas)
        {
            bool answer = false;
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();

                // Retrieve the password for the given login
                string query = "SELECT Haslo FROM Uzytkownicy WHERE Login = @Login";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Login", logpas.Login);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            string storedPassword = rdr.GetString("Haslo");

                            // Compare the stored password with the provided password
                            if (storedPassword.Equals(logpas.Password))
                            {
                                answer = true;
                            }
                        }
                    }
                }

                con.Close();
            }
            return answer;
        }
        public bool Register(LoginPasswordItem logpas)
        {
            bool answer = false;
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM Uzytkownicy WHERE Login = @Login;", con))
                {
                    cmd.Parameters.AddWithValue("@Login", logpas.Login);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        answer = !Convert.ToBoolean(rdr.GetInt32(0));
                        if(answer) AddUser(logpas);
                    }
                }
                con.Close();
            }
            return answer;
        }
        public IEnumerable<GameItem> SearchGame(GameSearchItem gameSearchItem)
        {
            string command = "SELECT Gry.Tytul, Gry.Data_wydania, Gry.Opis " +
                             "FROM Gry " +
                             "JOIN Kategorie_wiekowe ON Gry.ID_kategorii_wiekowej = Kategorie_wiekowe.ID_kategorii_wiekowej " +
                             "JOIN Gatunki ON Gry.ID_gatunku = Gatunki.ID_gatunku " +
                             "JOIN Wydawcy ON Gry.ID_wydawcy = Wydawcy.ID_wydawcy " +
                             "LEFT JOIN Oceny ON Gry.ID_gry = Oceny.ID_gry " +
                             "WHERE Gry.Tytul LIKE @Title";

            List<MySqlParameter> parameters = new List<MySqlParameter>
    {
        new MySqlParameter("@Title", gameSearchItem.Title + "%")
    };

            if (gameSearchItem.Pegi != "any")
            {
                command += " AND Kategorie_wiekowe.Nazwa_kategorii = @Pegi";
                parameters.Add(new MySqlParameter("@Pegi", gameSearchItem.Pegi));
            }

            if (gameSearchItem.Genre != "any")
            {
                command += " AND Gatunki.Nazwa_gatunku = @Genre";
                parameters.Add(new MySqlParameter("@Genre", gameSearchItem.Genre));
            }

            command += " GROUP BY Gry.ID_gry";

            switch (gameSearchItem.HowToSort)
            {
                case HowToSortEnum.AuthorAscending:
                    command += " ORDER BY Wydawcy.Nazwa_wydawcy ASC";
                    break;
                case HowToSortEnum.AuthorDescending:
                    command += " ORDER BY Wydawcy.Nazwa_wydawcy DESC";
                    break;
                case HowToSortEnum.RatingAscending:
                    command += " ORDER BY AVG(Oceny.Ocena) ASC";
                    break;
                case HowToSortEnum.RatingDescending:
                    command += " ORDER BY AVG(Oceny.Ocena) DESC";
                    break;
                case HowToSortEnum.NotSort:
                default:
                    break;
            }

            List<GameItem> tableList = new List<GameItem>();
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(command, con))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            GameItem tempTableName = new GameItem(rdr.GetString("Tytul"), rdr.GetDateTime("Data_wydania"), rdr.GetString("Opis"));
                            tableList.Add(tempTableName);
                        }
                    }
                }
                con.Close();
            }

            return tableList;
        }
        public bool AddComment(AddCommentItem comment)
        {
            int gameId = -1;
            int userId = -1;
            int commentId = -1;
            bool answer = false;

            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();

                // Retrieve gameId based on comment.GameName
                string queryGameId = "SELECT ID_gry FROM Gry WHERE Tytul = @GameName";
                using (MySqlCommand cmd = new MySqlCommand(queryGameId, con))
                {
                    cmd.Parameters.AddWithValue("@GameName", comment.GameName);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        gameId = rdr.GetInt32("ID_gry");
                    }
                    rdr.Close();
                }

                // Retrieve userId based on comment.Login
                string queryUserId = "SELECT ID_uzytkownika FROM Uzytkownicy WHERE Login = @Login";
                using (MySqlCommand cmd = new MySqlCommand(queryUserId, con))
                {
                    cmd.Parameters.AddWithValue("@Login", comment.Login);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        userId = rdr.GetInt32("ID_uzytkownika");
                    }
                    rdr.Close();
                }

                // Retrieve next commentId
                string queryMaxCommentId = "SELECT IFNULL(MAX(ID_komentarza), 0) AS MaxCommentId FROM Komentarze";
                using (MySqlCommand cmd = new MySqlCommand(queryMaxCommentId, con))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        commentId = rdr.GetInt32("MaxCommentId");
                    }
                    commentId++;
                    rdr.Close();
                }

                // Insert comment into Komentarze table
                string queryInsertComment = "INSERT INTO Komentarze (ID_komentarza, ID_uzytkownika, ID_gry, Tresc_komentarza, Data_dodania) " +
                                            "VALUES (@CommentId, @UserId, @GameId, @CommentText, CURDATE())";
                using (MySqlCommand cmd = new MySqlCommand(queryInsertComment, con))
                {
                    cmd.Parameters.AddWithValue("@CommentId", commentId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@GameId", gameId);
                    cmd.Parameters.AddWithValue("@CommentText", comment.Comment);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        answer = true;
                    }
                }

                con.Close();
            }

            return answer;
        }

        public bool AddRating(AddRatingItem rating)
        {
            int gameId = -1;
            int userId = -1;
            int ratingId = -1;
            bool answer = false;
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT ID_gry FROM Gry WHERE Tytul = '{rating.GameName}'", con);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read()) gameId = rdr.GetInt32("ID_gry");
                con.Close();

                con.Open();
                cmd = new MySqlCommand($"SELECT ID_uzytkownika FROM Uzytkownicy WHERE Login = '{rating.Login}'", con);
                rdr = cmd.ExecuteReader();
                if (rdr.Read()) userId = rdr.GetInt32("ID_uzytkownika");
                con.Close();

                con.Open();
                cmd = new MySqlCommand("SELECT IFNULL(MAX(ID_oceny), 0) FROM Oceny", con);
                rdr = cmd.ExecuteReader();
                if (rdr.Read()) ratingId = rdr.GetInt32(0);
                ratingId++;
                con.Close();

                if (userId < 0 || gameId < 0 || ratingId < 0) return false;

                con.Open();
                cmd = new MySqlCommand($"DELETE FROM Oceny WHERE ID_gry = '{gameId}' AND ID_uzytkownika = '{userId}'", con);
                rdr = cmd.ExecuteReader();
                con.Close();

                con.Open();
                cmd = new MySqlCommand($"INSERT INTO Oceny (ID_oceny, ID_uzytkownika, ID_gry, Ocena, Data_oceny) VALUES ('{ratingId}','{userId}', '{gameId}', '{rating.Rating}', CURDATE());", con);
                rdr = cmd.ExecuteReader();
                answer = true;

                con.Close();
            }
            return answer;
        }

        public bool AddGame(AddGameItem game)
        {
            bool isAdded = false;
            int gameId = -1;

            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                try
                {
                    con.Open();

                    // Step 1: Insert into Gry table
                    MySqlCommand cmd = new MySqlCommand(@"INSERT INTO Gry 
                        (Tytul, Data_wydania, ID_wydawcy, ID_gatunku, ID_kategorii_wiekowej, Opis) 
                        VALUES (@Title, @Date, @DeveloperId, @GenreId, @AgeRatingId, @Description);
                        SELECT LAST_INSERT_ID();", con);

                    cmd.Parameters.AddWithValue("@Title", game.Title);
                    cmd.Parameters.AddWithValue("@Date", game.Date);
                    cmd.Parameters.AddWithValue("@DeveloperId", GetDeveloperIdByName(game.Developer));
                    cmd.Parameters.AddWithValue("@GenreId", GetGenreIdByName(game.Genre));
                    cmd.Parameters.AddWithValue("@AgeRatingId", GetAgeRatingIdByName(game.Pegi));
                    cmd.Parameters.AddWithValue("@Description", game.Description);

                    gameId = Convert.ToInt32(cmd.ExecuteScalar());

                    if (gameId <= 0)
                    {
                        throw new Exception("Game ID not generated.");
                    }

                    isAdded = true;
                }
                catch (Exception ex)
                {
                    // Handle exceptions (logging, throwing, etc.)
                    Console.WriteLine("Error adding game: " + ex.Message);
                    isAdded = false;
                }
                finally
                {
                    con.Close();
                }
            }

            return isAdded;
        }

        public int GetGenreIdByName(string genreName)
        {
            int genreId = -1;

            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                try
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT ID_gatunku FROM Gatunki WHERE Nazwa_gatunku = @GenreName", con);
                    cmd.Parameters.AddWithValue("@GenreName", genreName);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        genreId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (logging, throwing, etc.)
                    Console.WriteLine("Error getting genre ID: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return genreId;
        }

        public int GetAgeRatingIdByName(string ageRating)
        {
            int ageRatingId = -1;

            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                try
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT ID_kategorii_wiekowej FROM Kategorie_wiekowe WHERE Nazwa_kategorii = @AgeRating", con);
                    cmd.Parameters.AddWithValue("@AgeRating", ageRating);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        ageRatingId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (logging, throwing, etc.)
                    Console.WriteLine("Error getting age rating ID: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return ageRatingId;
        }

        public int GetDeveloperIdByName(string developerName)
        {
            int developerId = -1;

            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                try
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand("SELECT ID_wydawcy FROM Wydawcy WHERE Nazwa_wydawcy = @DeveloperName", con);
                    cmd.Parameters.AddWithValue("@DeveloperName", developerName);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        developerId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions (logging, throwing, etc.)
                    Console.WriteLine("Error getting developer ID: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return developerId;
        }
        // Other database operations (CRUD)
    }
}
