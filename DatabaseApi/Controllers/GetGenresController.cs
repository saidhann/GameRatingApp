using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetGenresController : ControllerBase
    {
        private readonly IMySqlService _mySqlService;

        public GetGenresController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllGenres()
        {
            try
            {
                var tables = _mySqlService.GetAllGenres();
                return Ok(tables);
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
