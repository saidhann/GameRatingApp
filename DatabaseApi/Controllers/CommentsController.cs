using ClassLibrary.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {

        private readonly IMySqlService _mySqlService;

        public CommentsController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }

        [HttpPost]
        public ActionResult<IEnumerable<CommentItem>> GetComents(GameItem gameItem)
        {
            var isValid = _mySqlService.GetComments(gameItem.Title);
            return Ok(isValid);
            //await _mySqlService.GetEntitiesAsync();
        }
    }
}
