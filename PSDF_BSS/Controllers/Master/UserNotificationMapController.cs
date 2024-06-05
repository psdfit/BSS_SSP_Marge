using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserNotificationMapController : ControllerBase
    {
        private ISRVUserNotificationMap srv = null;

        public UserNotificationMapController(ISRVUserNotificationMap srv)
        {
            this.srv = srv;
        }

        // GET: UserNotificationMap
        [HttpGet]
        [Route("GetUserNotificationMap")]
        public IActionResult GetUserNotificationMap()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchUserNotificationMap());

                ls.Add(new SRVUsers().FetchUsers(false));

                ls.Add(new SRVNotifications().FetchNotifications(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: UserNotificationMap
        [HttpGet]
        [Route("RD_UserNotificationMap")]
        public IActionResult RD_UserNotificationMap()
        {
            try
            {
                return Ok(srv.FetchUserNotificationMap(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: UserNotificationMap
        [HttpPost]
        [Route("RD_UserNotificationMapBy")]
        public IActionResult RD_UserNotificationMapBy(UserNotificationMapModel mod)
        {
            try
            {
                return Ok(srv.FetchUserNotificationMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: UserNotificationMap/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(UserNotificationMapModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveUserNotificationMap(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: UserNotificationMap/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(UserNotificationMapModel d)
        {
            try
            {
                srv.ActiveInActive(d.UserNotificationMapID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}