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

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassStatusUpdateController : ControllerBase
    {
        private readonly ISRVTSRLiveData srvTSRLiveData;

        private readonly ISRVClass srvClassData;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVDashboard srvDashboard;


        public ClassStatusUpdateController(ISRVTSRLiveData srvTSRLiveData, ISRVClass srvClassData, ISRVUsers srvUsers, ISRVScheme srvScheme, ISRVDashboard srvDashboard)
        {
            this.srvTSRLiveData = srvTSRLiveData;
            this.srvClassData = srvClassData;
            this.srvScheme = srvScheme;
            this.srvUsers = srvUsers;
            this.srvDashboard = srvDashboard;
        }

        [HttpGet]
        [Route("GetFilteredClassData/{filters}")]
        public IActionResult GetFilteredTSRLiveData([FromQuery(Name = "filters")] int[] filters)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                Array.Resize(ref filters, filters.Length + 1);
                filters[filters.GetUpperBound(0)] = curUserID;

                //return Ok(srvClassData.FetchClassByFiltersForStatusUpdate(filters));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("RD_GetFilteredClassData")]
        public IActionResult RD_GetFilteredClassData([FromBody] JObject jObject)
        {
            try
            {
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;


                if (curUserID == 396 || curUserID == 62 || curUserID == 1568)
                { }
                else {
                    curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                }              

                filterModel.UserID = curUserID;

                List<object> ls = new List<object>();

                ls.Add(srvTSRLiveData.FetchClassLiveData(pagingModel, filterModel,loggedInUserLevel, out int totalCount));
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("CSUpdate")] //Class status update
        public IActionResult CSUpdate([FromQuery] int ClassID, [FromQuery] int ClassStatusID, [FromQuery] string ClassReason)
        {
            try
            {
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTSRLiveData.UpdateClassStatus(ClassID, ClassStatusID, CurUserID, ClassReason));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

    }
}
