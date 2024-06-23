using ClassLibrary.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCommentController : Controller
    {
        private readonly IMySqlService _mySqlService;

        public AddCommentController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpPost]
        public ActionResult<bool> AddComment(AddCommentItem addCommentItem)
        {
            var isValid = _mySqlService.AddComment(addCommentItem);
            return Ok(isValid);
            //await _mySqlService.GetEntitiesAsync();
        }
    }
}
