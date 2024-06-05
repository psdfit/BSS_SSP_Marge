using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AMSReportsController : ControllerBase
    {
        private readonly ISRVConfirmedMarginal srvCM;
        private readonly ISRVViolationSummary srvVS;
        private readonly ISRVDeletedDropout srvDD;
        private readonly ISRVAttendancePerception srvAP;
        private readonly ISRVReportExecutiveSummary srvPES;
        private readonly ISRVAdditionalTrainees srvAT;
        private readonly ISRVFakeGhostTrainee srvFGT;
        private readonly ISRVCovidMaskViolation srvCMV;
        private readonly ISRVEmploymentVerificationReport srvEVR;
        public AMSReportsController(ISRVConfirmedMarginal srvCM, ISRVViolationSummary srvVS, ISRVDeletedDropout srvDD, 
            ISRVAttendancePerception srvAP, ISRVReportExecutiveSummary srvPES, ISRVAdditionalTrainees srvAT,
            ISRVFakeGhostTrainee srvFGT, ISRVCovidMaskViolation srvCMV, ISRVEmploymentVerificationReport srvEVR)
        {
            this.srvCM = srvCM;
            this.srvVS = srvVS;
            this.srvDD = srvDD;
            this.srvAP = srvAP;
            this.srvPES = srvPES;
            this.srvAT = srvAT;
            this.srvFGT = srvFGT;
            this.srvCMV = srvCMV;
            this.srvEVR = srvEVR;
        }

        [HttpPost]
        [Route("GetCMReport")]
        public IActionResult GetCMReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvCM.GetConfirmedMarginalList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetVSReport")]
        public IActionResult GetVSReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvVS.GetViolationSummaryList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetDDReport")]
        public IActionResult GetDDReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvDD.GetDeletedDropoutList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetAPReport")]
        public IActionResult GetAPReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvAP.GetAttendancePerceptionList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetPESReport")]
        public IActionResult GetPESReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvPES.GetReportExecutiveSummaryList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetATReport")]
        public IActionResult GetATReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvAT.GetAdditionalTraineesList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetFGTReport")]
        public IActionResult GetFGTReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvFGT.GetFakeGhostTraineeList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetCMVReport")]
        public IActionResult GetCMVReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvCMV.GetCovidMaskViolationList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpPost]
        [Route("GetEVRReport")]
        public IActionResult GetEVRReport([FromBody] AMSReportsParamModel m)
        {
            try
            {
                return Ok(srvEVR.GetEmploymentVerificationReportList(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
    }
}
