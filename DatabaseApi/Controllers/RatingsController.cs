using ClassLibrary.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : Controller
    {
        private readonly IMySqlService _mySqlService;

        public RatingsController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpPost]
        public ActionResult<IEnumerable<RatingItem>> GetRatings(GameItem gameItem)
        {
            var isValid = _mySqlService.GetRatings(gameItem.Title);
            return Ok(isValid);
            //await _mySqlService.GetEntitiesAsync();
        }
    }
}
