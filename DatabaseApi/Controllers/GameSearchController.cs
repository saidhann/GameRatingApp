using ClassLibrary.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameSearchController : Controller
    {
        private readonly IMySqlService _mySqlService;

        public GameSearchController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpPost]
        public ActionResult<IEnumerable<GameItem>> GameSearch(GameSearchItem search)
        {
                var isValid = _mySqlService.SearchGame(search);
                return Ok(isValid);
            //await _mySqlService.GetEntitiesAsync();
        }
    }
}
