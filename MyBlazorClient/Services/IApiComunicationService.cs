using ClassLibrary.Entities;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MyBlazorClient.Services
{
    public interface IApiComunicationService
    {
        public Task<IEnumerable<TableItem>> GiveTables();

        public Task<IEnumerable<string>> GetAllGenres();
        public Task<bool> Register(LoginPasswordItem logpas);
        public Task<bool> Login(LoginPasswordItem logpas);
        public Task<IEnumerable<GameItem>> searchGame(GameSearchItem gsItem);
    }
}
