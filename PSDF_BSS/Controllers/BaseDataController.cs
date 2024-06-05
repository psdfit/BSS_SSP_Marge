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
namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseDataController : ControllerBase
    {
        private readonly ISRVBaseData srvBaseData;
        private readonly ISRVProvinces srvProvinces;
        private readonly ISRVCluster srvCluster;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;
        public BaseDataController(
            ISRVBaseData srvBaseData,
            ISRVProvinces srvProvince,
            ISRVCluster srvCluster,
            ISRVDistrict srvDistrict,
            ISRVTehsil srvTehsil
           )
        {
            this.srvBaseData = srvBaseData;
            this.srvProvinces = srvProvince;
            this.srvCluster = srvCluster;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
        }
        [HttpPost]
        [Route("GetData")]
        public async Task<IActionResult> GetData(BusinessProfileModel User)
        {
            try
            {
                // Fetching data in parallel
                var bankDetailTask = srvBaseData.FetchBankDetailList(User.UserID);
                var trainingLocationTask = srvBaseData.FetchTrainingLocationList(User.UserID);
                var certificationTask = srvBaseData.FetchCertificationList(User.UserID);
                var tradeMappingTask = srvBaseData.FetchTradeMapList(User.UserID);
                var trainerProfileTask = srvBaseData.FetchTrainerProfileList(User.UserID);
                var trainerDetailTask = srvBaseData.FetchTrainerDetail(User.UserID);
                var bankTask = srvBaseData.FetchDropDownList("RD_SSPBank");
                var registrationAuthorityTask = srvBaseData.FetchDropDownList("RD_SSPRegistrationAuthority");
                var registrationStatusTask = srvBaseData.FetchDropDownList("RD_SSPRegistrationStatus");
                var tradeTask = srvBaseData.FetchDropDownList("RD_Trade");
                var educationTypesTask = srvBaseData.FetchDropDownList("RD_EducationTypes");
                var genderTask = srvBaseData.FetchDropDownList("RD_Gender");
                var provinceTask = srvProvinces.FetchProvince(false);
                var clusterTask = srvCluster.FetchCluster(false);
                var districtTask = srvDistrict.FetchDistrict(false);
                var tehsilTask = srvTehsil.FetchTehsil(false);
                // Construct the object with the fetched data
                var data = new
                {
                    BankDetail = bankDetailTask,
                    TrainingLocation = trainingLocationTask,
                    Certification = certificationTask,
                    TradeMapping = tradeMappingTask,
                    TrainerProfile = trainerProfileTask,
                    TrainerDetail = trainerDetailTask,
                    Bank = bankTask,
                    RegistrationAuthority = registrationAuthorityTask,
                    RegistrationStatus = registrationStatusTask,
                    Trade = tradeTask,
                    EducationTypes = educationTypesTask,
                    Gender = genderTask,
                    province = provinceTask,
                    cluster = clusterTask,
                    district = districtTask,
                    tehsil = tehsilTask
                };
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        //POST: Profile/Save
        [HttpPost]
        [Route("SaveBank")]
        public IActionResult SaveBank(BankModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    if (user.UserID == 0 || user.UserID == null)
                    {
                        user.UserID = user.CurUserID;
                    }
                    if (user.BankDetailID == null)
                    {
                        user.BankDetailID = 0;
                    }
                    return Ok(srvBaseData.SaveBankDetail(user));
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
        [Route("SaveTrainingLocation")]
        public IActionResult SaveTrainingLocation(TrainingLocationModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    if (user.UserID == 0 || user.UserID == null)
                    {
                        user.UserID = user.CurUserID;
                    }
                    if (user.TrainingLocationID == null)
                    {
                        user.TrainingLocationID = 0;
                    }
                    return Ok(srvBaseData.SaveTrainingLocation(user));
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
        [Route("SaveCertification")]
        public IActionResult SaveCertification(CertificateModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    if (user.UserID == 0 || user.UserID == null)
                    {
                        user.UserID = user.CurUserID;
                    }
                    if (user.TrainingCertificationID == null)
                    {
                        user.TrainingCertificationID = 0;
                    }
                    return Ok(srvBaseData.SaveCertification(user));
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
        [Route("SaveTradeMapping")]
        public IActionResult SaveTradeMapping(TradeMapModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    if (user.UserID == 0 || user.UserID == null)
                    {
                        user.UserID = user.CurUserID;
                    }
                    if (user.TradeManageID == null)
                    {
                        user.TradeManageID = 0;
                    }
                    return Ok(srvBaseData.SaveTradeMapping(user));
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
        [Route("SaveTrainerProfile")]
        public IActionResult SaveTrainerProfile(TrainerProfileModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    if (user.UserID == 0 || user.UserID == null)
                    {
                        user.UserID = user.CurUserID;
                    }
                    if (user.TrainerID == null)
                    {
                        user.TrainerID = 0;
                    }
                    return Ok(srvBaseData.SaveTrainerProfile(user));
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
        [Route("GetTrainerDetail")]
        public IActionResult GetTrainerDetail(TrainerProfileModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    if (user.UserID == 0 || user.UserID == null)
                    {
                        user.UserID = user.CurUserID;
                    }
                    if (user.TrainerID == null)
                    {
                        user.TrainerID = 0;
                    }
                    return Ok(srvBaseData.FetchTrainerDetail(user.TrainerID));
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
        [Route("DeleteTrainerDetail")]
        public IActionResult DeleteTrainerDetail(TrainerProfileDetailModel user)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    user.CurUserID = Convert.ToInt32(User.Identity.Name);
                    if (user.UserID == 0 || user.UserID == null)
                    {
                        user.UserID = user.CurUserID;
                    }
                    if (user.TrainerDetailID == null)
                    {
                        user.TrainerDetailID = 0;
                    }
                    return Ok(srvBaseData.DeleteTrainerDetail(user.TrainerDetailID));
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
                return StatusCode(500, e.Message?.ToString());
            }
        }
    }
}