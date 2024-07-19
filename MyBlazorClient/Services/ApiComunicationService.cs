using ClassLibrary.Entities;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Diagnostics;
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
        public async Task<IEnumerable<GameItem>> SearchGame(GameSearchItem gsItem)
        {
            var response = await _http.PostAsJsonAsync<GameSearchItem>("api/GameSearch", gsItem);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<GameItem>>();
            }
            return new List<GameItem>();
        }
        public async Task<IEnumerable<CommentItem>> GetComments(string gameName)
        {
            GameItem temp = new GameItem(gameName,DateTime.Now,"");
            var response = await _http.PostAsJsonAsync<GameItem>("api/Comments", temp);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<CommentItem>>();
            }
            return new List<CommentItem>();
        }
        public async Task<IEnumerable<RatingItem>> GetRatings(string gameName)
        {
            GameItem temp = new GameItem(gameName, DateTime.Now, "");
            var response = await _http.PostAsJsonAsync<GameItem>("api/Ratings", temp);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<RatingItem>>();
            }
            return new List<RatingItem>();
        }
        public async Task<bool> AddComment(AddCommentItem comment)
        {
            var response = await _http.PostAsJsonAsync<AddCommentItem>("api/AddComment", comment);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            return false;
        }
        public async Task<bool> AddRating(AddRatingItem rating)
        {
            var response = await _http.PostAsJsonAsync<AddRatingItem>("api/AddRating", rating);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            return false;
        }

        public async Task<bool> AddGame(AddGameItem game)
        {
            return true;
        }
    }
}
