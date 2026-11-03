using System;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Interfaces;
using System.Data.SqlClient;

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
        [HttpPost]
        [Route("GetTakamolRecommnedationNote")]
        public IActionResult GetTakamolRecommnedationNoteReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(_srvManualGen.FetchEligibleClassDataForTakamolRecommnedationNote(mod));

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GenerateTakamolRecommnedationNote")]
        public IActionResult GenerateTakamolRecommnedationNote([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(_srvManualGen.GenerateTakamolRecommnedationNote(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GetInternetCostReport")]
        public IActionResult GetInternetCostReport([FromBody] QueryFilters mod)
        {
            try
            {
                return Ok(_srvManualGen.FetchEligibleClassDataForInterNet(mod));

            }
            catch (SqlException e)
            {
                // RAISERROR text from the procedure, e.g. "IRN for August 2026 has not been generated yet..."
                return BadRequest(new { Message = e.Message });
            }
                catch (Exception e)
            {
                return BadRequest(new { Message = GetDbMessage(e) });
            }
        }

            // Finds the message raised by the database, whatever exception type or wrapper carries it
            private static string GetDbMessage(Exception e)
            {
                for (var ex = e; ex != null; ex = ex.InnerException)
                {
                    // matches both System.Data.SqlClient and Microsoft.Data.SqlClient
                    if (ex.GetType().Name == "SqlException")
                        return ex.Message;
                }
                return e.GetBaseException().Message;
            }

        [HttpPost]
        [Route("GenerateInternetNote")]
        public IActionResult GenerateInternetNote([FromBody] QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(_srvManualGen.GenerateInternetNote(mod, out string IsGenerated));
                ls.Add(Convert.ToBoolean(IsGenerated));
                return Ok(ls);

            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
    }
}
