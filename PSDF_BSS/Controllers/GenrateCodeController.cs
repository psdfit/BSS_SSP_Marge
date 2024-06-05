using DataLayer.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenrateCodeController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("CreateClasses/{data}")]
        public IActionResult CreateClasses([FromQuery(Name = "data")]string[] data)
        {
            if (data[0].Length > 0)
            {
                GenrateCode.GenrateClasses(data[0], data[1]);
                return Ok();
            }
            else
                return BadRequest();
        }

        public void aa()
        {
        }
    }
}