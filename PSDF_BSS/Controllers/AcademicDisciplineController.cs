using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademicDisciplineController : ControllerBase
    {
        private readonly ISRVAcademicDiscipline srvAcademicDiscipline = null;
        public AcademicDisciplineController(ISRVAcademicDiscipline srvAcademicDiscipline)
        {
            this.srvAcademicDiscipline = srvAcademicDiscipline;
        }
        // GET: AcademicDiscipline
        [HttpGet]
        [Route("GetAcademicDiscipline")]
        public IActionResult GetAcademicDiscipline()
        {
            try
            {

                return Ok(srvAcademicDiscipline.FetchAcademicDiscipline());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: AcademicDiscipline
        [HttpGet]
        [Route("RD_AcademicDiscipline")]
        public IActionResult RD_AcademicDiscipline()
        {
            try
            {
                return Ok(srvAcademicDiscipline.FetchAcademicDiscipline(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: AcademicDiscipline
        [HttpPost]
        [Route("RD_AcademicDisciplineBy")]
        public IActionResult RD_AcademicDisciplineBy(AcademicDisciplineModel mod)
        {
            try
            {
                return Ok(srvAcademicDiscipline.FetchAcademicDiscipline(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: AcademicDiscipline/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(AcademicDisciplineModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.AcademicDisciplineID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvAcademicDiscipline.SaveAcademicDiscipline(D));
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

        // POST: AcademicDiscipline/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(AcademicDisciplineModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    //{		UsersModel u = Common.GetUserFromRequest(Request);
                    srvAcademicDiscipline.ActiveInActive(d.AcademicDisciplineID, d.InActive, Convert.ToInt32(User.Identity.Name));
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

