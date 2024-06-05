using DataLayer;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PSDF_BSS;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private ISRVNotifications srv = null;
        private readonly IHubContext<NotificationsHub, INotificationsHub> _hubContext;

        public NotificationsController(ISRVNotifications srv, IHubContext<NotificationsHub, INotificationsHub> hubContext)
        {
            this.srv = srv;
            this._hubContext = hubContext;
        }

        // GET: Notifications
        [HttpGet]
        [Route("GetNotifications")]
        public IActionResult GetNotifications()
        {
            try
            {                List<object> ls = new List<object>();
                SRVUserNotificationMap u = new SRVUserNotificationMap();
                ls.Add(srv.FetchNotifications());
               // ls.Add(u.FetchUserNotificationMapAll(0));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // RD: Notifications
        [HttpGet]
        [Route("RD_Notifications")]
        public IActionResult RD_Notifications()
        {
            try
            {
                return Ok(srv.FetchNotifications(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // RD: Notifications
        [HttpPost]
        [Route("RD_NotificationsBy")]
        public IActionResult RD_NotificationsBy(NotificationsModel mod)
        {
            try
            {
                return Ok(srv.FetchNotifications(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpGet]
        [Route("RD_NotificationsByID")]
        public IActionResult RD_NotificationsByID(int NotificationID)
        {
            try
            {
                NotificationsModel model = new NotificationsModel();
                model.NotificationID = NotificationID;
                return Ok(srv.FetchNotifications(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        // POST: Notifications/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(NotificationsModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.NotificationID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveNotifications(D));
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

        [HttpGet]
        [Route("GetUserNotifications/{id}")]
        public ActionResult GetUserNotifications(int id)
        {
            try
            {
                return Ok(new SRVUserNotificationMap().FetchUserNotificationMapAll(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // POST: Notifications/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(NotificationsModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.NotificationID, d.InActive, Convert.ToInt32(User.Identity.Name));
                    return Ok(true);
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
        [HttpGet]
        [Route("GetNotificationsDetail")]
        public IActionResult GetNotificationsDetail(int? PageNo,int? RecordParPage)
         {
            try
            {                
                List<NotificationDetailModel> modal = new List<NotificationDetailModel>();
                NotificationDetailModel modal2 = new NotificationDetailModel();
                modal2.CurUserID = Convert.ToInt32(User.Identity.Name);
                modal2.PageNo = PageNo;
                modal2.RecordParPage = RecordParPage;

                modal = srv.FetchNotificationsDetail(modal2);
                return Ok(modal);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}