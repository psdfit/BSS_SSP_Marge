using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RTPController : ControllerBase
    {
        private readonly ISRVRTP srvRTP;
        private readonly ISRVClass srvClass;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVNotificationMap _srvNotifications;

        public RTPController(ISRVRTP srvRTP, ISRVClass srvClass, ISRVOrgConfig srvOrgConfig, ISRVScheme srvScheme, ISRVNotificationMap sRVNotifications)
        {
            this._srvNotifications = sRVNotifications;
            this.srvRTP = srvRTP;
            this.srvClass = srvClass;
            this.srvOrgConfig = srvOrgConfig;
            this.srvScheme = srvScheme;
        }
        // GET: RTP
        [HttpGet]
        [Route("GetRTP")]
        public IActionResult GetRTP()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvRTP.FetchRTP());

                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpGet]
        [Route("GetRTPByID/{rtpid}")]
        public IActionResult GetRTPByID(int rtpid)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvRTP.GetByRTPID(rtpid));

                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // GET: RTP
        [HttpGet]
        [Route("GetRTPByKAM")]
        public IActionResult GetRTPByKAM([FromQuery]int userid, [FromQuery] int OID)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvRTP.FetchRTPByKAM(userid, OID));

                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: RTP
        [HttpPost]
        [Route("GetRTPByKAMUser")]
        public IActionResult GetRTPByKAMUser(RTPByKAMModel rtp)
        {
            try
            {
                List<object> ls = new List<object>();
                long userid = Convert.ToInt32(User.Identity.Name);
                ls.Add(srvRTP.FetchRTPByKAMUser(rtp));
                ls.Add(srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true }));
                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));
                List<RTPModel> FetchRTPByKAMUser = srvRTP.FetchRTPByKAMUser(rtp);
                if (FetchRTPByKAMUser.Count>0)
                {
                    List<RTPModel> CenterInspectionValueList = FetchRTPByKAMUser.Where(c => c.CenterInspectionValue != null && c.CenterInspectionValue!="").ToList();
                    if(CenterInspectionValueList.Count>0 && (userid == 67 || userid == 66 || userid == 65))
                        srvRTP.saveCentreMonitoringClassRecordNotification(CenterInspectionValueList,Convert.ToInt32(User.Identity.Name));
                }

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: RTP
        [HttpGet]
        [Route("GetCenterInspectionReport/{classid}")]
        public IActionResult GetCenterInspectionReport(int classid)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvRTP.GetCenterInspection(classid));
                ls.Add(srvRTP.GetCenterInspectionData(classid));
                ls.Add(srvRTP.GetCenterInspectionDataSecurity(classid));
                ls.Add(srvRTP.GetCenterInspectionDataIntegrity(classid));
                ls.Add(srvRTP.GetCenterInspectionDataIncharge(classid));
                ls.Add(srvRTP.GetCenterInspectionAdditionalCompliance(classid));
                ls.Add(srvRTP.GetCenterInspectionTradeDetail(classid));
                ls.Add(srvRTP.GetCenterInspectionClassDetail(classid));
                ls.Add(srvRTP.GetCenterInspectionNecessaryFacilities(classid));
                ls.Add(srvRTP.GetCenterInspectionTradeTools(classid));

                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: NTP
        [HttpPost]
        [Route("GenerateNTP")]
        public IActionResult GenerateNTP(RTPModel rtp)
        {
            try
            {
                srvRTP.UpdateNTP(rtp);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: CenterInspection
        [HttpGet]
        [Route("GetCenterInspectionByClass/{classid}")]
        public IActionResult GetCenterInspectionByClass(int classid)
        {
            try
            {
                //srv.GetCenterInspection(classid);
                return Ok(srvRTP.GetCenterInspection(classid));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: CenterInspection
        [HttpGet]
        [Route("UpdateCenterInspection/{classid}")]
        public IActionResult UpdateCenterInspection(int classid)
        {
            try
            {
                srvRTP.UpdateCenterInspection(classid);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        } 
        [HttpGet]
        [Route("GetRTPByTPM")]
        public IActionResult GetRTPByTPM()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvRTP.FetchRTPByTPM());

                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("GetRTPByTSP")]
        public IActionResult GetRTPByTSP(NTPByUserModel ntp)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvRTP.FetchRTPByTSP(ntp));

                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        // RD: RTP
        [HttpGet]
        [Route("RD_RTP")]
        public IActionResult RD_RTP()
        {
            try
            {
                return Ok(srvRTP.FetchRTP(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: RTP
        [HttpPost]
        [Route("RD_RTPBy")]
        public IActionResult RD_RTPBy(RTPModel mod)
        {
            try
            {
                return Ok(srvRTP.FetchRTP(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: RTP/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(RTPModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                srvRTP.SaveRTP(D);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: InceptionReport
        [HttpPost]
        [Route("RD_RTPClassDataBy")]
        public IActionResult RD_RTPClassDataBy(RTPModel model)
        {
            try
            {
                List<object> list = new List<object>();
                list.Add(srvRTP.GetByClassID(model.ClassID));
                list.Add(srvOrgConfig.FetchOrgConfig(new OrgConfigModel() { ClassID = model.ClassID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        [HttpPost]
        [Route("ApproveRTP")]
        public IActionResult ApproveRTP(RTPModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                srvRTP.ApproveRTPRequest(D);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: RTP/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(RTPModel d)
        {
            try
            {
               
                srvRTP.ActiveInActive(d.RTPID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

