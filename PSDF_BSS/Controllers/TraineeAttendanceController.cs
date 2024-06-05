using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;

namespace PSDF_BSSTrainee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraineeAttendanceController : ControllerBase
    {
        private readonly ISRVTraineeAttendance srvTraineeAttendance ;

        public TraineeAttendanceController(ISRVTraineeAttendance srvTraineeAttendance)
        {
            this.srvTraineeAttendance = srvTraineeAttendance;
        }

        // GET: TraineeAttendance
        [HttpGet]
        [Route("GetTraineeAttendance")]
        public IActionResult GetTraineeAttendance()
        {
            try
            {
                return Ok(srvTraineeAttendance.FetchTraineeAttendance());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeAttendance
        [HttpGet]
        [Route("RD_TraineeAttendance")]
        public IActionResult RD_TraineeAttendance()
        {
            try
            {
                return Ok(srvTraineeAttendance.FetchTraineeAttendance(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: TraineeAttendance
        [HttpPost]
        [Route("RD_TraineeAttendanceBy")]
        public IActionResult RD_TraineeAttendanceBy(TraineeAttendanceModel mod)
        {
            try
            {
                return Ok(srvTraineeAttendance.FetchTraineeAttendance(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TraineeAttendance/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TraineeAttendanceModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTraineeAttendance.SaveTraineeAttendance(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TraineeAttendance/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TraineeAttendanceModel d)
        {
            try
            {
               
                srvTraineeAttendance.ActiveInActive(d.TraineeAttendanceID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}