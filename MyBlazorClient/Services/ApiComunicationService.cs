using ClassLibrary.Entities;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Net.Http.Json;


namespace MyBlazorClient.Services
{
    public class ApiComunicationService : IApiComunicationService
    {

        private readonly HttpClient _http;
        public ApiComunicationService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<TableItem>> GiveTables()
        {
            try
            {
                var tables = await _http.GetFromJsonAsync<IEnumerable<TableItem>>("api/Database");
                return tables;
            }
            catch (Exception) {
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetAllGenres()
        {
            try
            {
                var genres = await _http.GetFromJsonAsync<IEnumerable<string>>("api/GetGenres");
                return genres;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Login(LoginPasswordItem logpas)
        {
            var response = await _http.PostAsJsonAsync<LoginPasswordItem>("api/Login", logpas);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Register(LoginPasswordItem logpas)
        {
            var response = await _http.PostAsJsonAsync<LoginPasswordItem>("api/Register", logpas);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<GameItem>> searchGame(GameSearchItem gsItem)
        {
            var response = await _http.PostAsJsonAsync<GameSearchItem>("api/GameSearch", gsItem);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<GameItem>>();
            }
            return new List<GameItem>();
        }
    }
}
