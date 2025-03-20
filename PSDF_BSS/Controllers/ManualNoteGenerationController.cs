using System;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Interfaces;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManualNoteGenerationController : ControllerBase
    {
        private readonly ISRVManualNoteGeneration _srvManualGen;


        public ManualNoteGenerationController(ISRVManualNoteGeneration srvManualGen)
        {
            _srvManualGen = srvManualGen;
        }

        [HttpPost]
        [Route("GetPVRN")]
        public IActionResult GetPVRNReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(_srvManualGen.FetchEligibleClassDataForPVRN(mod));

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GeneratePVRN")]
        public IActionResult GeneratePVRN([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(_srvManualGen.GeneratePVRN(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }



        [HttpPost]
        [Route("GetMRN")]
        public IActionResult GetMRNReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(_srvManualGen.FetchEligibleClassDataForMRN(mod));

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GenerateMRN")]
        public IActionResult GenerateMRN([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(_srvManualGen.GenerateMRN(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetPCRN")]
        public IActionResult GetPCRNReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(_srvManualGen.FetchEligibleClassDataForPCRN(mod));

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GeneratePCRN")]
        public IActionResult GeneratePCRN([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(_srvManualGen.GeneratePCRN(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetOTRN")]
        public IActionResult GetOTRNReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(_srvManualGen.FetchEligibleClassDataForOTRN(mod));

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GenerateOTRN")]
        public IActionResult GenerateOTRN([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(_srvManualGen.GenerateOTRN(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
    }
}
