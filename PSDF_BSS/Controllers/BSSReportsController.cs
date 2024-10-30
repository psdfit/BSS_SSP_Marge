using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CrystalDecisions.ReportAppServer.DataDefModel;
using System.Xml.Linq;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;

namespace PSDF_BSS.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BSSReportsController : Controller
    {
        private readonly ISRVBSSReports srvBSSReports;
        private readonly ISRVUsers srvUsers;

        public BSSReportsController(ISRVBSSReports srvBSSReports, ISRVUsers srvUsers)
        {
            this.srvBSSReports = srvBSSReports;
            this.srvUsers = srvUsers;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("FetchReportData")]
        public IActionResult FetchReportData(string Param)
        {
            try
            {
                var data = Param.Split("/");
                if (data[0] == "RD_SSPTSPAssociationDetail")
                {


                    DataTable dataTable = this.srvBSSReports.FetchReport(Param);
                    string[] attachments = new string[] { "Evidence" };
                    var dataWithAttachment = srvBSSReports.LoopingData(dataTable, attachments);
                    return Ok(dataWithAttachment);


                }
                else
                {
                    return Ok(this.srvBSSReports.FetchReport(Param));

                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpGet]
        [Route("FetchList")]
        public IActionResult FetchList(string Param)
        {
            try
            {
                return Ok(this.srvBSSReports.FetchList(Param));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("FetchReport")]
        public IActionResult FetchReport([FromBody] string param)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    return Ok(srvBSSReports.FetchReport(param));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
    }
}
