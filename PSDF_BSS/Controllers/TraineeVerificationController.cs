using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraineeVerificationController : ControllerBase
    {
        #region Init Services

        private readonly ISRVTraineeProfile srvTraineeProfile;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVClass srvClass;
        private readonly ISRVUsers srvUsers;

        public TraineeVerificationController(ISRVTraineeProfile srvTraineeProfile, ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail, ISRVClass srvClass, ISRVUsers srvUsers)
        {
            this.srvTraineeProfile = srvTraineeProfile;
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
            this.srvClass = srvClass;
            this.srvUsers = srvUsers;
        }

        #endregion Init Services

        [HttpGet]
        [Route("GetData")]
        public IActionResult GetData([FromQuery] int OID)
        {
            try
            {
                List<object> list = new List<object>();

                list.Add(srvScheme.FetchScheme(new SchemeModel() { OrganizationID = OID, InActive = false }));
                //list.Add(_serviceTraineeProfile.FetchTSPDetail(false));
                //list.Add(_serviceSRVClass.FetchClass(false));

                list.Add(srvTraineeProfile.FetchTraineeProfileByFilters(new[] { 0, 0, 0, OID }));

                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpGet]
        //[Route("GetTraineeProfile/{filter}")]
        //public IActionResult GetTraineeProfile([FromQuery(Name = "filter")] int[] filter)
        //{
        //    //List<TraineeProfileModel> list = new List<TraineeProfileModel>();
        //    try
        //    {
        //        return Ok(srvTraineeProfile.FetchTraineeProfileByFilters(filter));

        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
        [HttpPost]
        [Route("RD_TraineeProfilePaged")]
        public IActionResult RD_TraineeProfilePaged([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                filterModel.UserID = curUserID;

                int totalCount = 0;
                List<object> ls = new List<object>();

                ls.Add(srvTraineeProfile.FetchTraineeProfileVerificationByFiltersPaged(pagingModel, filterModel, out totalCount)
                    .Select(x => new
                    {
                        x.TraineeID,
                        x.ClassID,
                        x.TraineeCode,
                        x.TraineeCNIC,
                        x.ClassCode,
                        x.TraineeVerified,
                        x.TraineeName,
                        x.DateOfBirth,
                        x.AgeVerified,
                        x.AgeUnVerifiedReason,
                        x.DistrictVerified,
                        x.ResidenceUnVerifiedReason,
                        x.CNICVerified,
                        x.CNICUnVerifiedReason
                    }));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("ManualVerify")]
        public IActionResult ManualVerify(TraineeProfileModel model)
        {
          



            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.TraineeID);
            if (IsAuthrized == true)
            {
                try
                {
                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srvTraineeProfile.TraineeManualVerification(model));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

    }
}