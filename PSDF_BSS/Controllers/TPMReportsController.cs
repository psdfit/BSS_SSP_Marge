using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Services;
using DataLayer.Models;
using DataLayer.Interfaces;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TPMReportsController : ControllerBase
    {
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTSPDetail;

        public TPMReportsController(ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail)
        {
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
        }
        [HttpGet]
        public IActionResult GetData()
        {
            List<object> ls = new List<object>();
            try
            {
                ls.Add(srvScheme.FetchScheme(false));
                ls.Add(srvTSPDetail.FetchTSPDetail());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        public IActionResult GetReport(TPMReportsModel mod)
        {
            if (mod.ReportType == TPMReportConstants.CenterInspection)
            {
                // call SRVTPMReport with relative function here
            }
            else if (mod.ReportType == TPMReportConstants.ClassFormIII)
            {

            }
            else if (mod.ReportType == TPMReportConstants.TSPSummaryReportFormIV)
            {

            }
            else if (mod.ReportType == TPMReportConstants.SchemeViolationReport)
            {

            }
            else if (mod.ReportType == TPMReportConstants.ConfirmedMarginal)
            {

            }
            else if (mod.ReportType == TPMReportConstants.AdditionalTrainees)
            {

            }
            else if (mod.ReportType == TPMReportConstants.DeletedOrDropoutTrainees)
            {

            }
            else if (mod.ReportType == TPMReportConstants.AttendanceAndPerception)
            {

            }
            else if (mod.ReportType == TPMReportConstants.InstructorDetails)
            {

            }
            else if (mod.ReportType == TPMReportConstants.ProfileVerificationPV)
            {

            }
            else if (mod.ReportType == TPMReportConstants.ProfileVerificationSummary)
            {

            }
            else if (mod.ReportType == TPMReportConstants.OnJobTraining)
            {

            }
            else if (mod.ReportType == TPMReportConstants.EmploymentVerification)
            {

            }

            return Ok();
        }
    }
}