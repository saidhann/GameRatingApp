using ClassLibrary.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IMySqlService _mySqlService;

        public RegisterController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpPost]
        public ActionResult<bool> Login(LoginPasswordItem logpas)
        {
            //try
            //{
                var isValid = _mySqlService.Register(logpas);
                return Ok(isValid);
            //}
            //catch (Exception)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError,
             //                   "Error retrieving data from the database");
            //}
            //await _mySqlService.GetEntitiesAsync();
        }

        // Other endpoints (CRUD)
    }
}
