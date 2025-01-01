using System;
using DataLayer.Interfaces;
using DataLayer.Models.DVV;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceManagementController : ControllerBase
    {
        private readonly ISRVDeviceManagement srvDeviceManagement;

        public DeviceManagementController(ISRVDeviceManagement srvDeviceManagement)
        {
            this.srvDeviceManagement = srvDeviceManagement;
        }

        [HttpPost]
        [Route("Save")]
        public IActionResult Save(DeviceRegistrationModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvDeviceManagement.Save(data));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpPost]
        [Route("UpdateDeviceStatus")]
        public IActionResult UpdateDeviceStatus(DeviceRegistrationModel param)
        {
            
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    return Ok(srvDeviceManagement.UpdateDeviceStatus(param));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpGet]
        [Route("GetDeviceRegistration")]
        public IActionResult GetDeviceRegistration([FromQuery] int? registrationID = null, [FromQuery] int? userID = null)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized)
            {
                try
                {
                    var result = srvDeviceManagement.GetDeviceRegistration(registrationID, userID);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. You are not authorized for this activity.");
            }
        }


        [HttpPost]
        [Route("GetBiometricAttendanceTrainees")]
        public IActionResult GetBiometricAttendanceTrainees([FromBody] BiometricAttendanceTraineeModel model)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);

            if (IsAuthrized)
            {
                try
                {
                    var result = srvDeviceManagement.GetBiometricAttendanceTrainees(model);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. You are not authorized for this activity.");
            }
        }

        [HttpGet]
        [Route("GetTSPDetailsByClassID")]
        public IActionResult GetTSPDetailsByClassID(int classID)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);

            if (IsAuthrized)
            {
                try
                {
                    var result = srvDeviceManagement.GetTSPDetailsByClassID(classID);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. You are not authorized for this activity.");
            }
        }
    }
}
