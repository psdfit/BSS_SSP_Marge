using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubSectorController : ControllerBase
    {
        private ISRVSubSector srv = null;

        public SubSectorController(ISRVSubSector srv)
        {
            this.srv = srv;
        }

        // GET: SubSector
        [HttpGet]
        [Route("GetSubSector")]
        public IActionResult GetSubSector()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srv.FetchSubSector());

                ls.Add(new SRVSector().FetchSector(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: SubSector
        [HttpGet]
        [Route("RD_SubSector")]
        public IActionResult RD_SubSector()
        {
            try
            {
                return Ok(srv.FetchSubSector(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: SubSector
        [HttpPost]
        [Route("RD_SubSectorBy")]
        public IActionResult RD_SubSectorBy(SubSectorModel mod)
        {
            try
            {
                return Ok(srv.FetchSubSector(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: SubSector/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(SubSectorModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.SubSectorID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveSubSector(D));
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

        // POST: SubSector/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(SubSectorModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.SubSectorID, d.InActive, Convert.ToInt32(User.Identity.Name));
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