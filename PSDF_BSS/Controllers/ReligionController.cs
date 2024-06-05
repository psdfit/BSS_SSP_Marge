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
    public class ReligionController : ControllerBase
    {
        private readonly ISRVReligion srvReligion;

        public ReligionController(ISRVReligion srvReligion)
        {
            this.srvReligion = srvReligion;
        }

        // GET: Religion
        [HttpGet]
        [Route("GetReligion")]
        public IActionResult GetReligion()
        {
            try
            {
                return Ok(srvReligion.FetchReligion());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Religion
        [HttpGet]
        [Route("RD_Religion")]
        public IActionResult RD_Religion()
        {
            try
            {
                return Ok(srvReligion.FetchReligion(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Religion
        [HttpPost]
        [Route("RD_ReligionBy")]
        public IActionResult RD_ReligionBy(ReligionModel mod)
        {
            try
            {
                return Ok(srvReligion.FetchReligion(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Religion/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ReligionModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.ReligionID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvReligion.SaveReligion(D));
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

        // POST: Religion/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ReligionModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srvReligion.ActiveInActive(d.ReligionID, d.InActive, Convert.ToInt32(User.Identity.Name));
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