using Microsoft.AspNetCore.Mvc;
using DatabaseApi.Services;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IMySqlService _mySqlService;
        
        public DatabaseController(IMySqlService mySqlService)
        {
            _mySqlService = mySqlService;
        }
        

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetSampleEntity()
        {
            try
            {
                var tables = _mySqlService.ShowTables();
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


