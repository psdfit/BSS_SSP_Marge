using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SRNCourseraController : ControllerBase
    {
        private readonly ISRVSRNCoursera srvsrnCoursera;
       
       
        public SRNCourseraController(ISRVSRNCoursera srvsrnCoursera)
        {
            this.srvsrnCoursera = srvsrnCoursera;
        }

        [HttpPost]
        [Route("GetSRNCourseraReport")]
        public IActionResult GetSRNCourseraReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(srvsrnCoursera.FetchSRNCourseraTrainees(mod));

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GenerateSRNCoursera")]
        public IActionResult GenerateSRNCoursera([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvsrnCoursera.GenerateSRNCoursera(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        //// Develop by Rao Ali Haider 24-july-2024
        /// Fetch the VRN Classes 
        [HttpPost]
        [Route("GetVRNReport")]
        public IActionResult GetVRNReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(srvsrnCoursera.FetchVRNClasses(mod));

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        //// Develop by Rao Ali Haider 24-july-2024
        /// Fetch the VRN Classes 
        [HttpPost]
        [Route("GenerateVRN")]
        public IActionResult GenerateVRN([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvsrnCoursera.GenerateVRN(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
    }
}
