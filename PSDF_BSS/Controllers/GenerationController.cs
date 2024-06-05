using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerationController : Controller
    {
        private readonly ISRVClass srvClass;
        private readonly ISRVPRN srvPRN;
        private readonly ISRVTrn srvTRN;
        public GenerationController(ISRVClass srvClass, ISRVPRN srvPRN, ISRVTrn srvTRN)
        {
            this.srvClass = srvClass;
            this.srvPRN = srvPRN;
            this.srvTRN = srvTRN;
        }

        [HttpPost]
        [Route("RD_ClassesForPRNCompletion")]
        public IActionResult RD_ClassesForPRNCompletion(QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvClass.FetchClassesForPRNCompletion(mod, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated));
                ls.Add(Convert.ToInt32(TotalCompletedClasses));
                ls.Add(Convert.ToInt32(CompletedClassesWithResult));
                ls.Add(Convert.ToInt32(IsGenerated));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("GeneratePRNCompletion")]
        public IActionResult GeneratePRNCompletion(QueryFilters mod)
        {
            try
            {
                return Ok(srvPRN.GeneratePRNCompletion(mod));
            }
            catch (Exception e)
            { 
                return BadRequest(e.Message); 
            }
        }
        [HttpPost]
        [Route("RD_ClassesForPRNFinal")]
        public IActionResult RD_ClassesForPRNFinal(QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvClass.FetchClassesForPRNFinal(mod, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated));
                ls.Add(Convert.ToInt32(TotalCompletedClasses));
                ls.Add(Convert.ToInt32(CompletedClassesWithResult));
                ls.Add(Convert.ToInt32(IsGenerated));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("GeneratePRNFinal")]
        public IActionResult GeneratePRNFinal(QueryFilters mod)
        {
            try
            {
                return Ok(srvPRN.GeneratePRNFinal(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("RD_ClassesForTRN")]
        public IActionResult RD_ClassesForTRN(QueryFilters mod)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvClass.FetchClassesForTRN(mod, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated));
                ls.Add(Convert.ToInt32(TotalCompletedClasses));
                ls.Add(Convert.ToInt32(CompletedClassesWithResult));
                ls.Add(Convert.ToInt32(IsGenerated));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("GenerateTRN")]
        public IActionResult GenerateTRN(QueryFilters mod)
        {
            try
            {
                return Ok(srvTRN.GenerateTRN(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
