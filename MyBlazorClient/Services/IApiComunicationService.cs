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
        public Task<IEnumerable<GameItem>> SearchGame(GameSearchItem gsItem);
        public Task<IEnumerable<CommentItem>> GetComments(string gameName);
        public Task<IEnumerable<RatingItem>> GetRatings(string gameName);
        public Task<bool> AddComment(AddCommentItem comment);
        public Task<bool> AddRating(AddRatingItem rating);
        public Task<bool> AddGame(AddGameItem game);
    }
}
