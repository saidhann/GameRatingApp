using ClassLibrary.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddGameController : Controller
    {
        private readonly IMySqlService _mySqlService;

        public AddGameController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpPost]
        public ActionResult<bool> AddGameItem(AddGameItem addGameItem)
        {
            var isValid = _mySqlService.AddGame(addGameItem);
            return Ok(isValid);
            //await _mySqlService.GetEntitiesAsync();
        }
    }
}
