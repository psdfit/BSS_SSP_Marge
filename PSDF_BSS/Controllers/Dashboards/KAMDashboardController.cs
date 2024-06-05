using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KAMDashboardController : ControllerBase
    {
        private readonly ISRVTSPDetail srvTspDetail;
        private readonly ISRVClass srvClass;
        private readonly ISRVKAMDashboard srvKamDashboard;
        public KAMDashboardController( ISRVTSPDetail srvTspDetail, ISRVClass srvClass, ISRVKAMDashboard srvKamDashboard)
        {
            this.srvTspDetail = srvTspDetail;
            this.srvClass = srvClass;
            this.srvKamDashboard = srvKamDashboard;
        }


        // GET: KAMDashboards
        [HttpPost]
        [Route("GetKAMDashboardTSPs")]
        public IActionResult GetKAMDashboardTSPs(KAMDashboardModel obj)
        {
            try
            {
                var userid = obj.UserID;
                var tspid = obj.TSPID;
                var month = obj.Month;

                List<object> ls = new List<object>();

                ls.Add(srvKamDashboard.FetchKAMRelevantTSPs(userid));
                ls.Add(srvKamDashboard.FetchKAMDashboardStats(userid, tspid, month));
                //return Ok(srvKamDashboard.FetchKAMRelevantTSPs(userid));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpGet]
        [Route("GetPendingEmploymentsClassesByKAM")]
        public IActionResult GetPendingEmploymentsClassesByKAM(int KAMUserID)
        {
            try
            {
                List<object> ls = new List<object>();//ClassModel
                ls.Add(srvKamDashboard.GetPendingEmploymentsClassesByKAM(KAMUserID));
                
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("GetDeadlinesForKAM")]
        public IActionResult GetDeadlinesForKAM(KAMDashboardModel obj)
        {
            try
            {
                List<object> ls = new List<object>();//ClassModel
                ls.Add(srvKamDashboard.FetchDeadlinesByKAM(obj));
                
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("SendEmailToTSPs")]
        public IActionResult SendEmailToTSPs(KAMDashboardModel obj)
        {
            try
            {
                var UserIDs = obj.UserIDs;
                var subject = obj.Subject;
                var EmailAttachmentFile = obj.EmailAttachmentFile;
                var email = obj.EmailToSent;

                if (EmailAttachmentFile != null&& EmailAttachmentFile!="")
                {
                    int index = obj.EmailAttachmentFile.IndexOf(",");
                    if (index > 0)
                        EmailAttachmentFile = obj.EmailAttachmentFile.Substring(index + 1);
                }

                List<object> ls = new List<object>();

                //ls.Add(srvKamDashboard.SendEmailToTspUsers(UserIDs, email);
                //return Ok(srvKamDashboard.FetchKAMRelevantTSPs(userid));
                srvKamDashboard.SendEmailToTspUsers(obj.UserIDs, subject, EmailAttachmentFile, email);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }



    }
}

