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
    public class TraineeStatusTypesController : ControllerBase
    {
        private ISRVTraineeStatusTypes srv = null;

        public TraineeStatusTypesController(ISRVTraineeStatusTypes srv)
        {
            this.srv = srv;
        }

        // GET: TraineeStatusTypes
        [HttpGet]
        [Route("GetTraineeStatusTypes")]
        public IActionResult GetTraineeStatusTypes()
        {
            try
            {
                return Ok(srv.FetchTraineeStatusTypes());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeStatusTypes
        [HttpGet]
        [Route("RD_TraineeStatusTypes")]
        public IActionResult RD_TraineeStatusTypes()
        {
            try
            {
                return Ok(srv.FetchTraineeStatusTypes(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeStatusReason
        [HttpGet]
        [Route("RD_TraineeStatusReason")]
        public IActionResult RD_TraineeStatusReason()
        {
            try
            {
                return Ok(srv.FetchTraineeStatusReason(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeStatusTypes
        [HttpPost]
        [Route("RD_TraineeStatusTypesBy")]
        public IActionResult RD_TraineeStatusTypesBy(TraineeStatusTypesModel mod)
        {
            try
            {
                return Ok(srv.FetchTraineeStatusTypes(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TraineeStatusTypes/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TraineeStatusTypesModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveTraineeStatusTypes(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // CheckName
        [HttpPost]
        [Route("CheckName")]
        public IActionResult CheckName(TraineeStatusTypesModel mod)
        {
            if (!String.IsNullOrEmpty(mod.StatusName))
            {
                int ID = mod.TraineeStatusTypeID;
                mod.TraineeStatusTypeID = 0;
                List<TraineeStatusTypesModel> u = srv.FetchTraineeStatusTypes(mod);
                if (u == null || u.Count == 0)
                {
                    return Ok(true);
                }
                else
                {
                    if (u.Count > 1)
                        return Ok(false);
                    else
                    {
                        if (u[0].TraineeStatusTypeID == ID)
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
        // POST: TraineeStatusTypes/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TraineeStatusTypesModel d)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.TraineeStatusTypeID, d.InActive, Convert.ToInt32(User.Identity.Name));
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