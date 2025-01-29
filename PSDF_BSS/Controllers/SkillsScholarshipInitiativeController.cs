using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using Newtonsoft.Json.Linq;

namespace PSDF_BSS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsScholarshipInitiativeController : Controller
    {
        private readonly ISRVSkillsScholarshipInitiative srvskillscholarship;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail sRVTSPDetail;
        public SkillsScholarshipInitiativeController(ISRVSkillsScholarshipInitiative srvskillscholarship, ISRVUsers srvUsers, ISRVScheme srvScheme, ISRVTSPDetail sRVTSPDetail)
        {
            this.srvskillscholarship = srvskillscholarship;
            this.srvUsers = srvUsers;
            this.srvScheme = srvScheme;
            this.sRVTSPDetail = sRVTSPDetail;
        }

        // GET: MasterSheet
        [HttpGet]
        [Route("GetSkillsScholarshipInitiative")]
        public IActionResult GetSkillsScholarshipInitiative()
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

                ls.Add(srvScheme.FetchSchemesByTSPUser(CurUserID));

                ls.Add(userLevel);

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("GetFilteredSkillsScholarshipInitiative/{id}/{tspid}/{Locality}/{Cluster}/{District}")]
        public IActionResult GetFilteredSkillsScholarshipInitiative(int id, int tspid, int Locality, int Cluster, int District)
        {
            try
            {
                List<object> ls = new List<object>();
                int userLevel;
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);
                userLevel = loginuser.UserLevel;

                if (userLevel != 4) //Not TSP
                {
                    CurUserID = 0;
                    if (id == 0)
                    { id = -1; }
                    ls.Add(sRVTSPDetail.FetchTSPDetailByScheme(id));
                }
                if (userLevel == 4)
                {

                    QueryFilters queryFilters = new QueryFilters();
                    queryFilters.UserID = loginuser.UserID;
                    queryFilters.SchemeID = id;
                    queryFilters.Locality = Locality;

                    TSPDetailModel tSPDetailModel = new TSPDetailModel();
                    tSPDetailModel = sRVTSPDetail.FetchTSPByUserID(queryFilters);

                    tspid = tSPDetailModel.TSPID;

                }

                ls.Add(srvScheme.FetchSchemesBySkillScholarshipType(CurUserID));

                ls.Add(srvskillscholarship.GetSkillsScholarshipBySchemeID(id, tspid, Locality, Cluster, District));
                

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("GetFilteredSkillsScholarshipInitiativeReport/{id}/{tspid}/{Locality}/{Cluster}")]
        public IActionResult GetFilteredSkillsScholarshipInitiativeReport(int id, int tspid, int Locality, int Cluster)
        {
            try
            {
                List<object> ls = new List<object>();
                int userLevel;
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);
                userLevel = loginuser.UserLevel;

                if (userLevel != 4) //Not TSP
                {
                    CurUserID = 0;
                    if (id == 0)
                    { id = -1; }
                    ls.Add(sRVTSPDetail.FetchTSPDetailByScheme(id));
                }
                if (userLevel == 4)
                {

                    QueryFilters queryFilters = new QueryFilters();
                    queryFilters.UserID = loginuser.UserID;
                    queryFilters.SchemeID = id;
                    queryFilters.Locality = Locality;

                    TSPDetailModel tSPDetailModel = new TSPDetailModel();
                    tSPDetailModel = sRVTSPDetail.FetchTSPByUserID(queryFilters);

                    tspid = tSPDetailModel.TSPID;

                }

                ls.Add(srvScheme.FetchSchemesBySkillScholarshipType(CurUserID));

                ls.Add(srvskillscholarship.GetSkillsScholarshipBySchemeIDReport(id, tspid, Locality, Cluster));


                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetFilteredUserbyID/{id}")]
        public IActionResult GetFilteredUserbyID(int id)
        {
            try
            {
                List<object> ls = new List<object>();

                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);

                    QueryFilters queryFilters = new QueryFilters();
                    queryFilters.UserID = loginuser.UserID;
                    queryFilters.SchemeID = id;
                    ls.Add(sRVTSPDetail.FetchTSPByUser(queryFilters));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        
        [HttpGet]
        [Route("GetFilteredClusters/{id}")]
        public IActionResult GetFilteredClusters(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvskillscholarship.FetchClustersByLocality(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetFilteredDistricts/{id}")]
        public IActionResult GetFilteredDistricts(int id)
        {
            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvskillscholarship.FetchDistrictsByCluster(id));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("StartRace/{SchemeID}/{ClusterID}/{DistrictID}/{TradeID}")]
        public IActionResult GetStartRace(int SchemeID, int ClusterID, int DistrictID, int TradeID)
        {
            try
            {
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);


                srvskillscholarship.GetStartRace(SchemeID, ClusterID, DistrictID, TradeID, loginuser.UserID);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("StopRace/{SchemeID}/{ClusterID}/{DistrictID}/{TradeID}")]
        public IActionResult GetStopRace(int SchemeID, int ClusterID, int DistrictID, int TradeID)
        {
            try
            {
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);
                srvskillscholarship.GetStopRace(SchemeID, ClusterID, DistrictID, TradeID, loginuser.UserID);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetFilteredSessionCount/{id}/{tspid}")]
        public IActionResult GetFilteredSessionCount(int id, int tspid)
        {
            try
            {
                List<object> ls = new List<object>();
                int userLevel;
                int CurUserID = Convert.ToInt32(User.Identity.Name);
                UsersModel loginuser = new UsersModel();
                loginuser = srvUsers.GetByUserID(CurUserID);
                userLevel = loginuser.UserLevel;

                if (userLevel != 4) //Not TSP
                {
                    CurUserID = 0;
                    if (id == 0)
                    { id = -1; }
                    ls.Add(sRVTSPDetail.FetchTSPDetailByScheme(id));
                }
                if (userLevel == 4)
                {

                    QueryFilters queryFilters = new QueryFilters();
                    queryFilters.UserID = loginuser.UserID;
                    queryFilters.SchemeID = id;

                    TSPDetailModel tSPDetailModel = new TSPDetailModel();
                    tSPDetailModel = sRVTSPDetail.FetchTSPByUserID(queryFilters);

                    tspid = tSPDetailModel.TSPID;

                }

                ls.Add(srvScheme.FetchSchemesBySkillScholarshipType(CurUserID));
                ls.Add(srvskillscholarship.GetFilteredSessionCount(id, tspid));


                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("DeleteSession/{SchemeID}/{TSPID}/{SessionID}")]
        public IActionResult DeleteSession(int SchemeID, int TSPID, int SessionID)
        {
            try
            {
                srvskillscholarship.GetDeleteSession(SchemeID, TSPID, SessionID);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
