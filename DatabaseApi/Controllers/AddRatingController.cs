using ClassLibrary.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddRatingController : Controller
    {
        private readonly IMySqlService _mySqlService;

        public AddRatingController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpPost]
        public ActionResult<bool> AddRating(AddRatingItem addRatingItem)
        {
            var isValid = _mySqlService.AddRating(addRatingItem);
            return Ok(isValid);
            //await _mySqlService.GetEntitiesAsync();
        }
    }
}
