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
    //[Route("api/[controller]")]
    [ApiController]
    public class TraineeController : ControllerBase
    {
        private readonly ISRVTraineeProfile _srvTraineeProfile;
        private readonly ISRVClass _srvClass;
        public TraineeController(ISRVTraineeProfile srvTraineeProfile, ISRVClass srvClass)
        {
            _srvTraineeProfile = srvTraineeProfile;
            _srvClass = srvClass;
        }
        [HttpGet("~/api/Trainee/getallbyclass/{id}")]
        public IActionResult GetTraineesByClass(int id)
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
            var trainees = _srvTraineeProfile.FetchTraineeProfileByClass_DVV(id)
                .Select(x => new
                {
                    x.TraineeID,
                    x.TraineeName,
                    x.FatherName,
                    x.TraineeCNIC,
                    DateOfBirth = x.DateOfBirth.ToString("dd/MM/yyyy"),
                    x.TraineeCode,
                    //x.DistrictID,
                    TraineeStatus = x.StatusName,
                    x.GenderID,
                    x.EducationID,
                    x.ReligionID,
                    x.Latitude,
                    x.Longitude,
                    x.BiometricData1,
                    x.BiometricData2,
                    x.BiometricData3,
                    x.BiometricData4,
                    x.TimeStampOfVerification,
                    x.PermanentDistrict,
                    x.PermanentTehsil,
                    x.PermanentResidence,
                    x.TemporaryDistrict,
                    x.TemporaryTehsil,
                    x.TemporaryResidence

                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = trainees
            });
        }

        [HttpPost("~/api/Trainee/Eligibility")]
        public IActionResult CheckTraineeEligibility([FromBody] TraineeEligibilityDVV model)
        {

            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.CNIC) && model.ClassID > 0)
                {
                    string errMsg = string.Empty;
                    if (!_srvTraineeProfile.IsValidCNICFormat(model.CNIC, out errMsg))
                    {
                        return BadRequest(new ApiResponse()
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Message = "Bad request , " + errMsg
                        }); ;
                    }
                    //
                    bool result = _srvTraineeProfile.isEligibleTrainee(new TraineeProfileModel()
                    {
                        TraineeID = 0,
                        TraineeCNIC = model.CNIC,
                        ClassID = model.ClassID
                    }, out errMsg);

                    return Ok(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = result ? "Success" : errMsg,
                        Data = new { isEligible = result }
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse()
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = "Bad request. Did you pass valid body?"
                    });
                }
            }
            else
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
        }
        [HttpPost("~/api/Trainee/Registration")]
        public IActionResult TraineeRegistration(TraineeProfileDVV model)
        {
            string errMsg = string.Empty;
            if (model == null)
            {
                _srvTraineeProfile.SaveTraineeProfileDVVResponse(model);
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });


            }
            if (string.IsNullOrEmpty(model.TraineeName)
                || string.IsNullOrEmpty(model.FatherName)
                || string.IsNullOrEmpty(model.TraineeCNIC)
                || model.DateOfBirth == null
                || model.CNICIssueDate == null
                || model.Latitude == 0
                || model.Longitude == 0
                //|| model.DistrictID == 0
                || model.ClassID == 0
                || model.GenderID == 0
                || model.EducationID == null
                || model.ReligionID == null
                || model.TimeStampOfVerification == null
                || string.IsNullOrEmpty(model.BiometricData1)
                || string.IsNullOrEmpty(model.BiometricData2)
                || string.IsNullOrEmpty(model.BiometricData3)
                || string.IsNullOrEmpty(model.BiometricData4)
                || string.IsNullOrEmpty(model.PermanentResidence)
                || model.PermanentDistrict == 0
                || model.PermanentTehsil == 0
                || string.IsNullOrEmpty(model.TemporaryResidence)
                || model.TemporaryDistrict == 0
                || model.TemporaryTehsil == 0
                )
            {
                _srvTraineeProfile.SaveTraineeProfileDVVResponse(model);
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

            //bool isEligible = _srvTraineeProfile.isEligibleTrainee(new TraineeProfileModel()
            //{
            //    TraineeID = 0,
            //    TraineeCNIC = model.CNIC,
            //    ClassID = model.ClassID
            //}, out errMsg);
            //if (!isEligible)
            //{
            //    return BadRequest(new ApiResponse()
            //    {
            //        StatusCode = (int)HttpStatusCode.BadRequest,
            //        Message = "Bad request," + errMsg
            //    });
            //}
            model.CurUserID = Convert.ToInt32(User.Identity.Name);
            int result = _srvTraineeProfile.SaveTraineeProfileDVV(model, out errMsg);
            if (result == 0)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = string.IsNullOrEmpty(errMsg) ? "Error occurred while saving the profile." : errMsg,
                    Data = new { isSaved = false }
                });
            }
            else
            {
                return Ok(new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Success",
                    Data = new { TraineeID = result }
                });
            }

        }
        [HttpPost("~/api/Trainee/Attendance")]
        public IActionResult TraineeAttendance(TraineeAttendanceDVV model)
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
                || model.TraineeID == 0
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
            var result = _srvTraineeProfile.SaveTraineeAttendance(model, out errMsg);

            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = result ? "Success" : errMsg,
                Data = new { isSaved = result }
            });
        }
        [HttpPost("~/api/Trainee/Acknowledge")]
        public IActionResult TraineeAcknowledgement(TraineeAcknowledgementDVV model)
        {
            if (model == null)
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
            if (model.TimeStamp == null
                || (model.AknowledgementTypeID < 1 || model.AknowledgementTypeID > 2)
                //|| (model.AknowledgementTypeID == (int)EnumTraineeAcknowledgementType.StipendReceiving && model.AknowledgementTypeID != (int)EnumTraineeAcknowledgementType.UniformBagReceiving)
                //|| (model.AknowledgementTypeID != (int)EnumTraineeAcknowledgementType.StipendReceiving && model.AknowledgementTypeID == (int)EnumTraineeAcknowledgementType.UniformBagReceiving)
                || model.TraineeID == 0
                || model.Latitude == 0
                || model.Longitude == 0
                )
            {
                return BadRequest(new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Bad request. Did you pass valid body?"
                });
            }
            //StipendReceiving=1
            //UniformBagReceiving=2
            string errMsg = string.Empty;
            model.CurUserID = Convert.ToInt32(User.Identity.Name);
            var result = _srvTraineeProfile.SaveTraineeAcknowledgement(model, out errMsg);

            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = result ? "Success" : errMsg,
                Data = new { isSaved = result }
            });
        }

        [HttpGet("~/api/trainee/attendance")]
        public IActionResult GetTrainee()
        {


            int userID = Convert.ToInt32(User.Identity.Name);

            var _traineeAttendance = _srvTraineeProfile.FetchReport(userID, "RD_DVVTraineeAttendance");


            var Data = new
            {
                traineeAttendanceList = _traineeAttendance,
            };

            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = Data
            });
        }
        [HttpGet("~/api/trainee/deleteattendance/{cnic}")]
        public IActionResult DeleteTraineeAndAttandance(string cnic)
        {

            _srvTraineeProfile.DeleteTraineeandAttandance(cnic);

            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success"
            });
        }
    }




}
