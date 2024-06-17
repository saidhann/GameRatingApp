using MyBlazorClient.Services;
using Microsoft.AspNetCore.Components;
using ClassLibrary.Entities;

namespace MyBlazorClient.Pages
{
    public class DatabaseBase : ComponentBase
    {
        [Inject]
        public IApiComunicationService ApiComunicationService { get; set; }

        public IEnumerable<TableItem> Tables { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Tables = await ApiComunicationService.GiveTables();
        }
        /*
        protected override async void OnInitialized()
        {
            strings = await ApiComunicationService.GiveTables();
        }
        */
    }
}
