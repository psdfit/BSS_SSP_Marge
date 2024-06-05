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
    public class TraineeDisabilityController : ControllerBase
    {
        private ISRVTraineeDisability srv = null;

        public TraineeDisabilityController(ISRVTraineeDisability srv)
        {
            this.srv = srv;
        }

        // GET: TraineeDisability
        [HttpGet]
        [Route("GetTraineeDisability")]
        public IActionResult GetTraineeDisability()
        {
            try
            {
                return Ok(srv.FetchTraineeDisability());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeDisability
        [HttpGet]
        [Route("RD_TraineeDisability")]
        public IActionResult RD_TraineeDisability()
        {
            try
            {
                return Ok(srv.FetchTraineeDisability(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeDisability
        [HttpPost]
        [Route("RD_TraineeDisabilityBy")]
        public IActionResult RD_TraineeDisabilityBy(TraineeDisabilityModel mod)
        {
            try
            {
                return Ok(srv.FetchTraineeDisability(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TraineeDisability/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TraineeDisabilityModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.Id);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTraineeDisability(D));
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

        // POST: TraineeDisability/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TraineeDisabilityModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.Id, d.InActive, Convert.ToInt32(User.Identity.Name));
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