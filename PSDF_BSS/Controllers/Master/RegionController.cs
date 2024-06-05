/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : ControllerBase
    {
        private ISRVRegion srv = null;

        public RegionController(ISRVRegion srv)
        {
            this.srv = srv;
        }

        // GET: Region
        [HttpGet]
        [Route("GetRegion")]
        public IActionResult GetRegion()
        {
            try
            {
                return Ok(srv.FetchRegion());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Region
        [HttpGet]
        [Route("RD_Region")]
        public IActionResult RD_Region()
        {
            try
            {
                return Ok(srv.FetchRegion(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Region
        [HttpPost]
        [Route("RD_RegionBy")]
        public IActionResult RD_RegionBy(RegionModel mod)
        {
            try
            {
                return Ok(srv.FetchRegion(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // CheckName
        [HttpPost]
        [Route("CheckName")]
        public IActionResult CheckName(RegionModel mod)
        {
            if (!String.IsNullOrEmpty(mod.RegionName))
            {
                int ID = mod.RegionID;
                mod.RegionID = 0;
                List<RegionModel> u = srv.FetchRegion(mod);
                if (u == null || u.Count==0)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].RegionID == ID)
                        {
                            return Ok(true);
                        }
                        else
                            return Ok(false);
                    }
                }
            }
            else
                return BadRequest("Bad Request");
        }

        // POST: Region/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(RegionModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.RegionID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveRegion(D));
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

        // POST: Region/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(RegionModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.RegionID, d.InActive, Convert.ToInt32(User.Identity.Name));
                    return Ok(true);
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
    }
}