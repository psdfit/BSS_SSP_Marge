using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GISController : ControllerBase
    {
        private readonly ISRVGIS srvGIS;


        public GISController(ISRVGIS srvGIS)
        {
            this.srvGIS = srvGIS;

        }

        // GET: Class
        [HttpPost]
        [Route("GetClassWithTraineeCount")]
        public IActionResult GetClassWithTraineeCount(GISModel model)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvGIS.GetClassWithTraineeCount(model));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
    }
}
