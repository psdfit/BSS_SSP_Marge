using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using Newtonsoft.Json.Linq;

namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MasterSheetController : ControllerBase
    {
        private readonly ISRVMasterSheet srvMasterSheet;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTSPDetail;
        private readonly ISRVDashboard srvDashboard;

        public MasterSheetController(ISRVMasterSheet srvMasterSheet, ISRVUsers srvUsers, ISRVScheme srvScheme, ISRVTSPDetail srvTSPDetail, ISRVDashboard srvDashboard)
        {
            this.srvMasterSheet = srvMasterSheet;
            this.srvUsers = srvUsers;
            this.srvScheme = srvScheme;
            this.srvTSPDetail = srvTSPDetail;
            this.srvDashboard = srvDashboard;
        }
        // GET: MasterSheet
        [HttpGet]
        [Route("GetMasterSheet")]
        public IActionResult GetMasterSheet()
        {
            try
            {
                List<object> ls = new List<object>();
                int userLevel;
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);
                userLevel = loginuser.UserLevel;


                //ls.Add(srv.FetchMasterSheet());

                ls.Add(srvScheme.FetchScheme(false));

                ls.Add(srvTSPDetail.FetchTSPDetail(false));
                ls.Add(userLevel);

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetFilteredMasterSheet/{filter}")]
        public IActionResult GetFilteredMasterSheet([FromQuery(Name = "filter")] int[] filter)
        {
            try
            {
                int userID = filter?[3] ?? 0;
                int oID = filter?[4] ?? 0;
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;

                List<object> ls = new List<object>();

                ls.Add(srvMasterSheet.FetchMasterSheetByFilters(filter));
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    ls.Add(srvScheme.FetchSchemesByTSPUser(curUserID));
                }
                else
                {
                    ls.Add(srvScheme.FetchScheme(new SchemeModel() { FinalSubmitted = true, OrganizationID = oID }));
                }
                //return Ok(srv.FetchMasterSheetByFilters(filter));
                //ls.Add(srvScheme.FetchSchemeForFilter(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // RD: MasterSheet
        [HttpGet]
        [Route("RD_MasterSheet")]
        public IActionResult RD_MasterSheet()
        {
            try
            {
                return Ok(srvMasterSheet.FetchMasterSheet(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: MasterSheet
        [HttpPost]
        [Route("RD_MasterSheetBy")]
        public IActionResult RD_MasterSheetBy(MasterSheetModel mod)
        {
            try
            {
                return Ok(srvMasterSheet.FetchMasterSheet(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: MasterSheet/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(MasterSheetModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvMasterSheet.SaveMasterSheet(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }


        [HttpPost]
        [Route("GetFilteredMasterSheetPaged")]
        public IActionResult GetFilteredMasterSheetPaged([FromBody] JObject jObject)
        {
            try
            {
                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                curUserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                PagingModel pagingModel = jObject["pagingModel"].ToObject<PagingModel>();
                SearchFilter filterModel = jObject["filterModel"].ToObject<SearchFilter>();
                List<object> ls = new List<object>();
                ls.Add(srvMasterSheet.FetchMasterSheetByPaged(pagingModel, filterModel, out int totalCount));
                if (loggedInUserLevel == (int)EnumUserLevel.TSP)
                {
                    ls.Add(srvDashboard.FetchSchemesByUsers(curUserID));
                }
                else
                {
                    ls.Add(srvDashboard.FetchSchemes());
                }
                pagingModel.TotalCount = totalCount;
                ls.Add(pagingModel);
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}

