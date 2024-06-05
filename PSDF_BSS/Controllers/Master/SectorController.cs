using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectorController : ControllerBase
    {
        private ISRVSector srv = null;

        public SectorController(ISRVSector srv)
        {
            this.srv = srv;
        }

        // GET: Sector
        [HttpGet]
        [Route("GetSector")]
        public IActionResult GetSector()
        {
            try
            {
                return Ok(srv.FetchSector());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Sector
        [HttpGet]
        [Route("RD_Sector")]
        public IActionResult RD_Sector()
        {
            try
            {
                return Ok(srv.FetchSector(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Sector
        [HttpPost]
        [Route("RD_SectorBy")]
        public IActionResult RD_SectorBy(SectorModel mod)
        {
            try
            {
                return Ok(srv.FetchSector(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Sector/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(SectorModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.SectorID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveSector(D));
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

        // POST: Sector/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(SectorModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.SectorID, d.InActive, Convert.ToInt32(User.Identity.Name));
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