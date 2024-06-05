using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DataLayer.Interfaces;
using DataLayer.Services;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataLayer.Classes;
using System.Dynamic;
using PSDF_BSS.Logging;
namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseraReportController : ControllerBase
    {

        private readonly ISRVTSRLiveData srvTSRLiveData;
        private readonly ISRVTraineeProfile srvTraineeProfile;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVDashboard srvDashboard;

        public CourseraReportController(ISRVTSRLiveData srvTSRLiveData, ISRVTraineeProfile srvTraineeProfile, ISRVUsers srvUsers, ISRVScheme srvScheme, ISRVDashboard srvDashboard)
        {
            this.srvTSRLiveData = srvTSRLiveData;
            this.srvTraineeProfile = srvTraineeProfile;
            this.srvScheme = srvScheme;
            this.srvUsers = srvUsers;
            this.srvDashboard = srvDashboard;
        }

        // GET: TSRLiveDataSet
        [HttpGet]
        [Route("GetTSRLiveData")]
        public IActionResult GetTSRLiveData([FromQuery] int OID)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    list.Add("Schemes", srvDashboard.FetchSchemesByUsers(curUserID));
                }
                else
                {
                    list.Add("Schemes", srvDashboard.FetchSchemes());
                }
                list.Add("TSRLiveData", srvTSRLiveData.FetchTSRLiveDataByFilters(new int[] { 0, 0, 0, 0, OID, curUserID }));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("RD_CourseraReportPaged")]
        public IActionResult RD_CourseraReportPaged([FromBody] JObject jObject)
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

                ls.Add(srvTSRLiveData.FetchCourseraPaged(pagingModel, filterModel, out totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: CourseraReport/Save
        [HttpPost]
        [Route("SaveExpell")]
        public IActionResult SaveExpell(TSRLiveDataModel model)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.TraineeID);
            if (IsAuthrized == true)
            {
                //string errMsg = string.Empty;
                try
                {
                    model.CurUserID = Convert.ToInt32(User.Identity.Name);
                    var list = srvTSRLiveData.UpdateTraineeStatusExpell(model);

                    return Ok(list);
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
        //// POST: CourseraReport/Save
        //[HttpPost]
        //[Route("SaveDropout")]
        //public IActionResult SaveDropout(TSRLiveDataModel model)
        //{
        //    string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
        //    bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], model.TraineeID);
        //    if (IsAuthrized == true)
        //    {
        //        //string errMsg = string.Empty;
        //        try
        //        {
        //            model.CurUserID = Convert.ToInt32(User.Identity.Name);
        //            var list = srvTSRLiveData.UpdateTraineeStatusDropout(model);

        //            return Ok(list);
        //        }
        //        catch (Exception e)
        //        {
        //            return BadRequest(e.Message);
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("Access Denied. you are not authorized for this activity");
        //    }
        //}

    }
}
