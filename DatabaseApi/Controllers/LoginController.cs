using Microsoft.AspNetCore.Mvc;
using DatabaseApi.Services;
using ClassLibrary.Entities;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMySqlService _mySqlService;

        public LoginController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpPost]
        public ActionResult<bool> Login(LoginPasswordItem logpas)
        {
            try
            {
                var isValid = _mySqlService.Login(logpas);
                return Ok(isValid);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }
            //await _mySqlService.GetEntitiesAsync();
        }

        // Other endpoints (CRUD)
    }
}
