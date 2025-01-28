using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PSDF_BSS.API.Controllers
{
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ISRVInstructor _srvInstructor;
        private readonly ISRVInstructorMaster _srvInstructorMaster;
        private readonly ISRVClass _srvClass;
        public InstructorController(ISRVInstructor srvInstructor, ISRVInstructorMaster srvInstructorMaster, ISRVClass srvClass)
        {
            _srvInstructor = srvInstructor;
            _srvInstructorMaster = srvInstructorMaster;
            _srvClass = srvClass;
        }
        [HttpGet("~/api/Instructor/getallbyclass/{id}")]
        public IActionResult GetInstructorsByClass(int id)
        {
            int userID = Convert.ToInt32(User.Identity.Name);
            if (!_srvClass.FetchClassesByUser_DVV(new QueryFilters() { UserID = userID }).Any(x => x.ClassID == id))
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. You are requested to an invalid class."
                });
            }
            var instuctors = _srvInstructor.FetchInstructor_DVV(id)
                .Select(x => new
                {
                    InstructorID = x.InstrID,
                    Name = x.InstructorName,
                    CNIC = x.CNICofInstructor,
                    x.QualificationHighest,
                    x.GenderID,
                    x.TotalExperience,
                    x.Latitude,
                    x.Longitude,
                    x.BiometricData1,
                    x.BiometricData2,
                    x.BiometricData3,
                    x.BiometricData4,
                    x.TimeStampOfVerification,
                    x.IsVerifiedByDVV,
                    x.LocationAddress
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = instuctors
            });
        }
        [HttpPost("~/api/Instructor/Registration")]
        public IActionResult InstructorRegistration(InstructorDVV model)
        {
            string errMsg = string.Empty;
            if (model == null)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
            if (model.InstructorID == 0)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Invalid InstructorID."
                });
            }
            if (string.IsNullOrEmpty(model.Name)
                //|| string.IsNullOrEmpty(model.CNIC)
                || model.Latitude == 0
                || model.Longitude == 0
                || model.ClassID == 0
                || model.GenderID == 0
                || model.TimeStampOfVerification == null
                || string.IsNullOrEmpty(model.BiometricData1)
                || string.IsNullOrEmpty(model.BiometricData2)
                || string.IsNullOrEmpty(model.BiometricData3)
                || string.IsNullOrEmpty(model.BiometricData4)
                || string.IsNullOrEmpty(model.LocationAddress)

                )
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
            //if (!IsValidCNICFormat(model.CNIC, out errMsg))
            //{
            //    return BadRequest(new ApiResponse()
            //    {
            //        StatusCode = (int)HttpStatusCode.BadRequest,
            //        Message = "Bad request," + errMsg
            //    });
            //}


            model.CurUserID = Convert.ToInt32(User.Identity.Name);
            model.IsVerifiedByDVV = true;
            bool result = _srvInstructor.SaveInstructorDVV(model, out errMsg);
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = result ? "Success" : errMsg,
                Data = new { isSaved = result }
            });
        }
        [HttpPost("~/api/Instructor/Attendance")]
        public IActionResult InstructorAttendance(InstructorAttendanceDVV model)
        {
            if (model == null)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
            if (model.Latitude == 0
                || model.Longitude == 0
                || model.TimeStamp == null
                || model.InstructorID == 0
                || string.IsNullOrEmpty(model.BiometricData1)
                )
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
            if ((model.CheckIn && model.CheckOut) || (!model.CheckIn && !model.CheckOut))
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Accept only single request of CheckIn / CheckOut ."
                });
            }
            string errMsg = string.Empty;
            model.CurUserID = Convert.ToInt32(User.Identity.Name);
            var result = _srvInstructor.SaveInstructorAttendance(model, out errMsg);

            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = result ? "Success" : errMsg,
                Data = new { isSaved = result }
            });
        }
        private bool IsValidCNICFormat(string cnic, out string errMsg)
        {
            Regex regex = new Regex(@"^[0-9]{5}-[0-9]{7}-[0-9]{1}$");
            bool isValid = regex.IsMatch(cnic);
            errMsg = isValid ? string.Empty : $"Invalid CNIC Format, it should be xxxxx-xxxxxxx-x";
            return isValid;

        }

        [HttpGet("~/api/instructor/attendance")]
        public IActionResult GetTrainee()
        {


            int userID = Convert.ToInt32(User.Identity.Name);

            var _instructorAttendance = _srvInstructor.FetchReport(userID, "RD_DVVInstructorAttendance");


            var Data = new
            {
                instructorAttendanceList = _instructorAttendance,
            };

            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = Data
            });
        }

        [HttpPost("~/api/Instructor/biomatricRegistration")]
        public IActionResult biomatricRegistration(InstructorDVV model)
        {
            string errMsg = string.Empty;
            if (model == null)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
            if (model.InstructorID == 0)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Invalid InstructorID."
                });
            }
            if (model.InstructorID == 0
                || string.IsNullOrEmpty(model.BiometricData1)
                || string.IsNullOrEmpty(model.BiometricData2)
                || string.IsNullOrEmpty(model.BiometricData3)
                || string.IsNullOrEmpty(model.BiometricData4)
                || string.IsNullOrEmpty(model.LocationAddress)

                )
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }

            model.CurUserID = Convert.ToInt32(User.Identity.Name);
            bool result = _srvInstructor.SavebiomatricInstructorDVV(model, out errMsg);
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = result ? "Success" : errMsg,
                Data = new { isSaved = result }
            });
        }

    }
}
