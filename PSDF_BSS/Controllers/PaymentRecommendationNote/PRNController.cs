using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Models;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers.PaymentRecommendationNote
{
    [Route("api/[controller]")]
    [ApiController]
    public class PRNController : ControllerBase
    {
        private readonly ISRVPRN srvPRN;

        public PRNController(ISRVPRN srvPRN)
        {
            this.srvPRN = srvPRN;
        }

        [HttpGet]
        [Route("GetPRNForApproval/{PRNMasterID}")]
        public IActionResult GetPRNForApproval(int PRNMasterID)
        {
            try
            {
                return Ok(srvPRN.GetPRNForApproval(PRNMasterID));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpGet]
        [Route("GetPRNExcelExportByIDs/{ids}")]
        public IActionResult GetPRNExcelExport(string ids)
        {
            try
            {
                return Ok(srvPRN.GetPRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetPRNBulkExcelExportByIDs")]
        public IActionResult GetPRNBulkExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvPRN.GetPRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetPRNExcelExport")]
        public IActionResult GetPRNExcelExport(PRNMasterModel m)
        {
            try
            {
                return Ok(srvPRN.GetPRNExcelExport(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetPTBRTrainees")]
        public IActionResult GetPTBRTrainees([FromBody] PRNModel model)
        {
            try
            {
                return Ok(srvPRN.GetPTBRTrainees(model.ClassCode,model.Month.Value));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("PenaltyImposedByMEAndDeductionUniformBag")]
        public IActionResult PenaltyImposedByMEAndDeductionUniformBag([FromBody] PRNModel model)
        {
            try
            {
                model.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvPRN.PenaltyImposedByME_DeductionUniformBag(model));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        } 
        
       
    }
}