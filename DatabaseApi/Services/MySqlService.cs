using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
using System.Data;
using System.Data.Common;

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
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO Uzytkownicy (ID_uzytkownika, Login, Haslo, Data_rejestracji) SELECT IFNULL(MAX(ID_uzytkownika), 0) + 1, '{logpas.Login}', '{logpas.Password}', NOW() FROM Uzytkownicy;",con);
                MySqlDataReader rdr = cmd.ExecuteReader();
                con.Close();
            }
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
                con.Close();
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

        public bool Login(LoginPasswordItem logpas)
        {
            bool answer = false;
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand($"SELECT Haslo FROM Uzytkownicy WHERE Login = '{logpas.Login}';", con))
                {

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        string passwordBuff = new string(rdr.GetString(0));
                        if(passwordBuff.Equals(logpas.Password)) answer = true;
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
                using (MySqlCommand cmd = new MySqlCommand($"SELECT COUNT(*) FROM Uzytkownicy WHERE Login = '{logpas.Login}';", con))
                {

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

            string command = "SELECT Gry.Tytul, Gry.Data_wydania, Gry.Opis\r\n" +
                    "FROM Gry\r\n" +
                    "JOIN Kategorie_wiekowe ON Gry.ID_kategorii_wiekowej = Kategorie_wiekowe.ID_kategorii_wiekowej\r\n" +
                    "JOIN Gatunki ON Gry.ID_gatunku = Gatunki.ID_gatunku\r\n" +
                    "JOIN Wydawcy ON Gry.ID_wydawcy = Wydawcy.ID_wydawcy\r\n" +
                    "LEFT JOIN Oceny ON Gry.ID_gry = Oceny.ID_gry\r\n" +
                    $"WHERE Gry.Tytul LIKE '{gameSearchItem.Title}%'\r\n";

            if (!(gameSearchItem.Pegi == "any")) command += $"AND Kategorie_wiekowe.Nazwa_kategorii = '{gameSearchItem.Pegi}'\r\n";
            if (!(gameSearchItem.Genre == "any")) command += $"AND Gatunki.Nazwa_gatunku = '{gameSearchItem.Genre}'\r\n";

            command+= "GROUP BY Gry.ID_gry\r\n";

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
                    command+= " ORDER BY AVG(Oceny.Ocena) DESC";
                    break;
                case HowToSortEnum.NotSort:
                    break;
            }

            List<GameItem> tableList = new List<GameItem>();
            using (MySqlConnection con = new MySqlConnection(GetConnectionString()))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(command, con))
                {
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        GameItem tempTableName = new GameItem(rdr.GetString("Tytul"),rdr.GetDateTime("Data_wydania"),rdr.GetString("Opis"));
                        tableList.Add(tempTableName);
                    }
                }
                con.Close();
            }
            return tableList;
        }


        // Other database operations (CRUD)
    }
}
