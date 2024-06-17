using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace DatabaseApi.Services
{
    public interface IMySqlService
    {

        public string GetConnectionString();

        public IEnumerable<TableItem> ShowTables();

        public IEnumerable<string> GetAllGenres();

        public bool Login(LoginPasswordItem logpas);

        public bool Register(LoginPasswordItem logpas);

        public void AddUser(LoginPasswordItem logpas);

        public IEnumerable<GameItem> SearchGame(GameSearchItem gameSearchItem);

        // Other database operations (CRUD)
    }
}
