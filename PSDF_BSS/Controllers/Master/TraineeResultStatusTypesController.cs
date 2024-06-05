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
    public class TraineeResultStatusTypesController : ControllerBase
    {
        private ISRVTraineeResultStatusTypes srv = null;

        public TraineeResultStatusTypesController(ISRVTraineeResultStatusTypes srv)
        {
            this.srv = srv;
        }

        // GET: TraineeResultStatusTypes
        [HttpGet]
        [Route("GetTraineeResultStatusTypes")]
        public IActionResult GetTraineeResultStatusTypes()
        {
            try
            {
                return Ok(srv.FetchTraineeResultStatusTypes());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeResultStatusTypes
        [HttpGet]
        [Route("RD_TraineeResultStatusTypes")]
        public IActionResult RD_TraineeResultStatusTypes()
        {
            try
            {
                return Ok(srv.FetchTraineeResultStatusTypes(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeResultStatusTypes
        [HttpPost]
        [Route("RD_TraineeResultStatusTypesBy")]
        public IActionResult RD_TraineeResultStatusTypesBy(TraineeResultStatusTypesModel mod)
        {
            try
            {
                return Ok(srv.FetchTraineeResultStatusTypes(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TraineeResultStatusTypes/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TraineeResultStatusTypesModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveTraineeResultStatusTypes(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TraineeResultStatusTypes/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TraineeResultStatusTypesModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.ResultStatusID, d.InActive, Convert.ToInt32(User.Identity.Name));
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