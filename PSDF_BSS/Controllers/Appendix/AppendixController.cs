using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PSDF_BSS.Controllers.Appendix
{
  

    [ApiController]
    [Route("api/[controller]")]
    public class AppendixController : ControllerBase
    {
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVClass srvClass;
        private readonly ISRVInstructor srvInstructor;

        public AppendixController(ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail, ISRVClass srvClass, ISRVInstructor srvInstructor)
        {
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
            this.srvClass = srvClass;
            this.srvInstructor = srvInstructor;
        }

        [HttpPost]
        [Route("GetAppendix")]
        public IActionResult GetAppendix([FromBody] int SchemeID)
        {
            List<object> ls = new List<object>();

            try
            {
                ls.Add(srvScheme.GetBySchemeID(SchemeID));
                ls.Add(srvTSPDetail.GetByScheme(SchemeID));
                ls.Add(srvClass.GetBySchemeID(SchemeID));
                ls.Add(srvInstructor.GetBySchemeID(SchemeID));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: Scheme
        [HttpGet]
        [Route("RemoveFromAppendix")]
        public IActionResult RemoveFromAppendix([FromQuery] int formID, [FromQuery] string form)
        {
            try
            {
                srvScheme.RemoveFromAppendix(formID, form);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}