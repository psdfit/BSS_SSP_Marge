using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationMapController : ControllerBase
    {
        private ISRVNotificationMap srv = null;
        private ISRVApprovalProcess _srvApprovalProcess = null;

        public NotificationMapController(ISRVNotificationMap srv, ISRVApprovalProcess srvApprovalProcess)
        {
            this.srv = srv;
            this._srvApprovalProcess = srvApprovalProcess;
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(NotificationsMapModel modal)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], modal.NotificationMapID);
            if (IsAuthrized == true)
            {
                try
                {
                    modal.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveNotificationMap(modal));
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
        [HttpGet]
        [Route("GetNotificationMap")]
        public IActionResult GetNotificationMap()
        {
            try
            {
                List<NotificationsMapModel> ls = new List<NotificationsMapModel>();
                ls = srv.FetchNotificationMap();

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        //[HttpGet]
        //[Route("FetchApprovalStatus")]
        //public IActionResult FetchApprovalStatus()
        //{
        //    try
        //    {
        //        List<ApprovalStatusModel> ls = new List<ApprovalStatusModel>();
        //        ls = _srvApprovalProcess.FetchApprovalStatus();
               
        //        return Ok(ls);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //} 
        [HttpGet]
        [Route("RD_GetProcessInfoByProcessKey")]
        public IActionResult RD_GetProcessInfoByProcessKey(string ProcessKey)
        {
            try
            {
                List<NotificationsMapModel> ls = new List<NotificationsMapModel>();
                ls = srv.GetProcessInfoByProcessKey(ProcessKey);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        //[HttpGet]
        //[Route("RD_GetProcessInfoByProcessKey")]
        //public IActionResult RD_GetProcessInfoByProcessKey(string ProcessKey, int? ApprovalStatusID)
        //{
        //    try
        //    {
        //        List<NotificationsMapModel> ls = new List<NotificationsMapModel>();
        //        ls = srv.GetProcessInfoByProcessKey(ProcessKey, ApprovalStatusID);
        //        return Ok(ls);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}
        //  [HttpGet]


        [HttpPost]
        [Route("ReadNotification")]
        public IActionResult ReadNotification(NotificationsMapModel d)
        {
            try
            {
                bool ls = new bool();
                
                ls = srv.ReadNotification(d.NotificationDetailID);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("MarkAllAsRead")]
        public IActionResult MarkAllAsRead([FromBody] int userId)
        {
            try
            {
                bool success = srv.MarkAllNotificationsAsRead(userId);
                return Ok(success);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException?.ToString());
            }
        }
        [HttpGet]
        [Route("GetNotificationDetasilByID")]
        public IActionResult GetNotificationDetasilByID(int NotificationDetailID)
        {
            try
            {
                List<NotificationDetailModel> NotificationDetail = new List<NotificationDetailModel>();
                NotificationDetail = srv.GetNotificationDetasilByID(NotificationDetailID);
                return Ok(NotificationDetail);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}
