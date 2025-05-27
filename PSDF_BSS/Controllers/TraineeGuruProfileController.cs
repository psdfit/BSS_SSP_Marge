using System;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TraineeGuruProfileController : ControllerBase
    {
        private readonly ISRVTraineeGuruProfile _srvGuru;
        public TraineeGuruProfileController(ISRVTraineeGuruProfile srvGuru)
        {
            _srvGuru = srvGuru;
        }

        [HttpGet]
        [Route("GetTraineeGuru")]
        public IActionResult GetTraineeGuru(int UserID)

        {
            try
            {
                var profiles = _srvGuru.GetByTraineeProfileID(UserID);
                return Ok(profiles);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpGet]
        [Route("GetTraineeGuruDetail")]
        public IActionResult GetTraineeGuruDetail(int traineeID)

        {
            try
            {
                var profiles = _srvGuru.GetTraineeGuruDetail(traineeID);
                return Ok(profiles);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        // POST api/traineeguruprofile
        [HttpPost]
        [Route("SaveTraineeGuruProfile")]
        public IActionResult SaveTraineeGuruProfile([FromBody] TraineeGuruModel model)
        {
            try
            {
                var result = _srvGuru.SaveTraineeGuruProfile(model);
                return Ok(GetTraineeGuru(model.CurUserID));
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        //// PUT api/traineeguruprofile
        //[HttpPut]
        //public async Task<IActionResult> UpdateTraineeGuruProfile([FromBody] TraineeGuruModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _service.UpdateTraineeGuruProfile(model);
        //        if (result)
        //            return Ok(model);
        //        else
        //            return BadRequest("Error updating the profile.");
        //    }
        //    return BadRequest("Invalid data.");
        //}

        //// DELETE api/traineeguruprofile/{id}
        //[HttpDelete("{traineeProfileID}")]
        //public async Task<IActionResult> DeleteByTraineeProfileID(int traineeProfileID)
        //{
        //    var result = await _service.DeleteByTraineeProfileID(traineeProfileID);
        //    if (result)
        //        return NoContent();
        //    else
        //        return NotFound();
        //}

    }
}
