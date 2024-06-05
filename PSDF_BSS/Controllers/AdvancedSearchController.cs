using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvancedSearchController : Controller
    {
        private readonly ISRVAdvanceSearch srvAdvanceSearch;

        public AdvancedSearchController(ISRVAdvanceSearch srvAdvanceSearch)
        {
            this.srvAdvanceSearch = srvAdvanceSearch;
        }

        [HttpPost]
        [Route("Search")]
        public IActionResult AdvanceSearch([FromBody]AdvancedSearchModel model)
        {
            try
            {
                var result = srvAdvanceSearch.AdvanceSearch(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetTraineeProfile")]
        public IActionResult GetTraineeProfile(int TraineeID)
        {
            try
            {
                var result = srvAdvanceSearch.GetTraineeProfile(TraineeID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetInstructorProfile")]
        public IActionResult GetInstructorProfile(int InstructorID)
        {
            try
            {
                var result = srvAdvanceSearch.GetInstructorProfile(InstructorID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetClassDetail")]
        public IActionResult GetClassDetail(int ClassID)
        {
            try
            {
                var result = srvAdvanceSearch.GetClassDetail(ClassID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetTSPDetail")]
        public IActionResult GetTSPDetail(int TSPMasterID)
        {
            try
            {
                var result = srvAdvanceSearch.GetTSPDetail(TSPMasterID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetSchemeDetail")]
        public IActionResult GetSchemeDetail(int SchemeID)
        {
            try
            {
                var result = srvAdvanceSearch.GetSchemeDetail(SchemeID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetMPRDetail")]
        public IActionResult GetMPRDetail(int MPRID)
        {
            try
            {
                var result = srvAdvanceSearch.GetMPRDetail(MPRID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetPRNDetail")]
        public IActionResult GetPRNDetail(int PRNID)
        {
            try
            {
                var result = srvAdvanceSearch.GetPRNDetail(PRNID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("GetSRNDetail")]
        public IActionResult GetSRNDetail(int SRNID)
        {
            try
            {
                var result = srvAdvanceSearch.GetSRNDetail(SRNID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("GetInvoiceDetail")]
        public IActionResult GetInvoiceDetail(int InvoiceID)
        {
            try
            {
                var result = srvAdvanceSearch.GetInvoiceDetail(InvoiceID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}