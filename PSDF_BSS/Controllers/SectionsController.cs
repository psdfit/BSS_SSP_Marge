using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;

namespace PSDF_BSSRegistration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly ISRVSections srvSections;

        public SectionsController(ISRVSections srvSections)
        {
            this.srvSections = srvSections;
        }

        // GET: Sections
        [HttpGet]
        [Route("GetSections")]
        public IActionResult GetSections()
        {
            try
            {
                return Ok(srvSections.FetchSections());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Sections
        [HttpGet]
        [Route("RD_Sections")]
        public IActionResult RD_Sections()
        {
            try
            {
                return Ok(srvSections.FetchSections(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Sections
        [HttpPost]
        [Route("RD_SectionsBy")]
        public IActionResult RD_SectionsBy(SectionsModel mod)
        {
            try
            {
                return Ok(srvSections.FetchSections(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Sections/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(SectionsModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.SectionID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvSections.SaveSections(D));
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

        // POST: Sections/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(SectionsModel d)
        {
            
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    //
                    srvSections.ActiveInActive(d.SectionID, d.InActive, Convert.ToInt32(User.Identity.Name));
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