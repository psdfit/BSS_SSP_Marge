using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserEventMapController : ControllerBase
    {
        private readonly ISRVUserEventMap srvUserEventMap;
        private readonly ISRVUsers srvUsers;
        public UserEventMapController(ISRVUserEventMap srvUserEventMap, ISRVUsers srvUsers)
        {
            this.srvUserEventMap = srvUserEventMap;
            this.srvUsers = srvUsers;
        }
        // GET: UserEventMap
        [HttpGet]
        [Route("GetUserEventMap")]
        public async Task<IActionResult> GetUserEventMap()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvUserEventMap.FetchUserEventMap());

                ls.Add(srvUsers.FetchUsers(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: UserEventMap
        [HttpGet]
        [Route("RD_UserEventMap")]
        public async Task<IActionResult> RD_UserEventMap()
        {
            try
            {
                return Ok(srvUserEventMap.FetchUserEventMap(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: UserEventMap
        [HttpPost]
        [Route("RD_UserEventMapBy")]
        public async Task<IActionResult> RD_UserEventMapBy(UserEventMapModel mod)
        {
            try
            {
                return Ok(srvUserEventMap.FetchUserEventMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: UserEventMap/Save
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(UserEventMapModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvUserEventMap.SaveUserEventMap(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: UserEventMap/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(UserEventMapModel d)
        {
            try
            {
                srvUserEventMap.ActiveInActive(d.UserEventMapID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

