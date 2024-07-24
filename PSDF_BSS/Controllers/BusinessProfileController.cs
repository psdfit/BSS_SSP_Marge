using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models.SSP;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using PSDF_BSS.Logging;
using Newtonsoft;
using System.Text.Json;
using System.Dynamic;

namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessProfileController : ControllerBase
    {
        private readonly ISRVBusinessProfile srvprofile;
        private readonly ISRVProvinces srvProvinces;
        private readonly ISRVCluster srvCluster;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;
        public BusinessProfileController(
            ISRVBusinessProfile srvProfile,
            ISRVProvinces srvProvince,
            ISRVCluster srvCluster,
            ISRVDistrict srvDistrict,
            ISRVTehsil srvTehsil
        )
        {
            this.srvprofile = srvProfile;
            this.srvProvinces = srvProvince;
            this.srvCluster = srvCluster;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
        }
        [HttpPost]
        [Route("GetData")]
        public IActionResult GetData(BusinessProfileModel User)
        {
            try
            {
                object Data = new
                {
                    profile = srvprofile.FetchProfile(User.UserID),
                    masterDetail = srvprofile.FetchMaster(User.UserID),
                    salesTax = srvprofile.FetchDropDownList("RD_SSPSalesTax"),
                    programType = srvprofile.FetchDropDownList("RD_ProgramType"),
                    legalStatus = srvprofile.FetchDropDownList("RD_SSPLegalStatus"),
                    //legalStatus = srvprofile.FetchDropDownList("RD_LegalStatus"),
                    province = srvProvinces.FetchProvince(false),
                    cluster = srvCluster.FetchCluster(false),
                    district = srvDistrict.FetchDistrict(false),
                    tehsil = srvTehsil.FetchTehsil(false),
                };
                return Ok(Data);
                //return Ok(Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString() + " in (Bussiness Profile Controller)");
            }
        }
        //POST: Profile/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(BusinessProfileModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(
                false,
                Convert.ToInt32(User.Identity.Name),
                Split[2],
                Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvprofile.SaveProfile(user));
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
        //POST: POC/Save
        [HttpPost]
        [Route("SavePOC")]
        public IActionResult SavePOC(BusinessProfileModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(
                false,
                Convert.ToInt32(User.Identity.Name),
                Split[2],
                Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvprofile.SaveContactPersonProfile(user));
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
        [Route("FetchTspRegistration")]
        public IActionResult FetchTspRegistration(BusinessProfileModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(
                false,
                Convert.ToInt32(User.Identity.Name),
                Split[2],
                Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvprofile.FetchTspRegistration());
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
        [Route("FetchTspDetail")]
        public IActionResult FetchTspDetail(BusinessProfileModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(
                false,
                Convert.ToInt32(User.Identity.Name),
                Split[2],
                Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvprofile.FetchTspRegistrationDetail(user));
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
        [Route("UpdateTradeStatus")]
        public IActionResult UpdateTradeStatus(TradeMapModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(
                false,
                Convert.ToInt32(User.Identity.Name),
                Split[2],
                Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvprofile.UpdateTspTradeValidationStatus(data));
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
        
        
        [HttpPost("GetDashboardData")]
        public IActionResult GetDashboardData([FromBody] dynamic data)
        {
            var startDate = data.StartDate;

            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(
                false,
                Convert.ToInt32(User.Identity.Name),
                Split[2],
                Split[3]
            );
            if (IsAuthrized == true)
            {
                try
                {
                    var _formWiseTSPCount = srvprofile.FetchData(data, "RD_SSPFormWiseTSPCount");
                    var _programWiseRegisteredTSP = srvprofile.FetchData(data, "RD_SSPProgramWiseRegisteredTSP");
                    var _registeredTSPCount = srvprofile.FetchData(data, "RD_SSPRegisteredTSPCount");
                    var _registrationDetail = srvprofile.FetchDropDownList("RD_SSPTSPRegistrationDetail");
                    return Ok(new 
                    { 
                        formWiseTSPCount = _formWiseTSPCount ,
                        programWiseRegisteredTSP= _programWiseRegisteredTSP,
                        registeredTSPCount= _registeredTSPCount,
                        registrationDetail= _registrationDetail
                    });
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
        [Route("GetStatus")]
        public IActionResult GetStatus()
        {
            try
            {
                return Ok(srvprofile.FetchDropDownList("RD_SSPApprovalStatus"));
                //return Ok(Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("GetDocument")]
        public IActionResult GetDocument(BusinessProfileModel user)
        {
            try
            {
                var filePath = user.FilePath;
                if (System.IO.File.Exists(filePath))
                {
                    var contentTypeProvider = new FileExtensionContentTypeProvider();
                    if (!contentTypeProvider.TryGetContentType(filePath, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }
                    return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message?.ToString());
            }
        }
    }
}
