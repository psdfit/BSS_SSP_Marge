using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PSDF_BSS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PSDF_BSS.API.Controllers
{
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ISRVClass _srvClass;
        public ClassController(ISRVClass srvClass)
        {
            _srvClass = srvClass;
        }
        [HttpGet("~/api/class/getall")]
        public IActionResult GetClasses()
        {
            int userID = Convert.ToInt32(User.Identity.Name);
            var classes = _srvClass.FetchClassesByUser_DVV(new QueryFilters() { UserID = userID })
                .Where(x => x.ClassStatusID == (int)EnumClassStatus.Active)
                .Select(x => new
                {
                    x.ClassID,
                    x.ClassCode,
                    x.Duration,
                    StartDate = x.StartDate.Value.ToString("MM/dd/yyyy"),
                    EndDate = x.EndDate.Value.ToString("MM/dd/yyyy"),
                    ContractualTrainees = x.TraineesPerClass,
                    x.EnrolledTrainees,
                    CenterName = x.TrainingAddressLocation,
                    ClassStatus = x.ClassStatusName,
                    x.DistrictID,
                    x.TehsilID,
                    Instructors = x.Instructors.Select(y => new
                    {
                        InstructorID = y.InstrID,
                        Name = y.InstructorName,
                        CNIC = y.CNICofInstructor
                    })
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = classes.Count() > 0 ? "Success" : "Not Found any active class.",
                Data = classes
            });
        }
        [HttpGet("~/api/class/getallbyscheme/{id}")]
        public IActionResult GetClassesByScheme(int id)
        {
            int userID = Convert.ToInt32(User.Identity.Name);
            var classes = _srvClass.FetchClassesByUser_DVV(new QueryFilters() { SchemeID = id, UserID = userID })
                .Where(x => x.ClassStatusID == (int)EnumClassStatus.Active)
                .Select(x => new
                {
                    x.ClassID,
                    x.ClassCode,
                    x.Duration,
                    StartDate = x.StartDate.Value.ToString("MM/dd/yyyy"),
                    EndDate = x.EndDate.Value.ToString("MM/dd/yyyy"),
                    ContractualTrainees = x.TraineesPerClass,
                    x.EnrolledTrainees,
                    CenterName = x.TrainingAddressLocation,
                    ClassStatus = x.ClassStatusName,
                    x.DistrictID,
                    x.TehsilID,
                    Instructors = x.Instructors.Select(y => new
                    {
                        InstructorID = y.InstrID,
                        Name = y.InstructorName,
                        CNIC = y.CNICofInstructor
                    })
                });
            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = classes
            });
        }

        [HttpGet("~/api/class/getalldevices")]
        public IActionResult GetDeveicesStatus()
        {
            int userID = Convert.ToInt32(User.Identity.Name);

            var _DeveiceStatus = _srvClass.FetchDeveiceStatus(userID);


            var Data = new
            {
                devicestatuscheck = _DeveiceStatus,
            };

            return Ok(new ApiResponse()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Message = "Success",
                Data = Data
            });
        }
    }
}
