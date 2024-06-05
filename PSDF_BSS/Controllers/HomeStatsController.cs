using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
namespace MasterDataModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeStatsController : ControllerBase
    {
        private readonly ISRVHomeStats srvHomeStats;
        private readonly ISRVUsers srvUsers;
        public HomeStatsController(ISRVHomeStats srvHomeStats, ISRVUsers srvUsers)
        {
            this.srvHomeStats = srvHomeStats;
            this.srvUsers = srvUsers;
        }
        // GET: HomeStats
        //[HttpGet]
        //[Route("GetHomeStats")]
        //public IActionResult GetHomeStats()
        //{
        //    try
        //    {
        //        List<object> ls = new List<object>();
        //        //int userLevel;
        //        //int CurUserID = Convert.ToInt32(User.Identity.Name);
        //        //UsersModel loginuser = new UsersModel();
        //        //loginuser = srvUser.GetByUserID(CurUserID);
        //        //userLevel = loginuser.UserLevel;


        //        ls.Add(srv.FetchHomeStats());

        //        //    ls.Add(new SRVScheme().FetchScheme(false));

        //        //    ls.Add(new SRVTSPDetail().FetchTSPDetail(false));
        //        //ls.Add(userLevel);

        //        return Ok(ls);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}

        [HttpGet]
        [Route("GetFilteredHomeStats/{filter}")]
        public IActionResult GetFilteredHomeStats([FromQuery(Name = "filter")] int[] filter)
        {
            try
            {
                return Ok(srvHomeStats.FetchHomeStatsByFilters(filter));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetHomeStatsByClassID/{id}")]
        public ActionResult GetHomeStatsByClassID(int id)
        {

            try
            {
                List<object> ls = new List<object>();
                ls.Add(srvHomeStats.FetchHomeStatsByClass(id));
                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: HomeStats
        [HttpGet]
        [Route("RD_HomeStats")]
        public IActionResult RD_HomeStats()
        {
            try
            {
                return Ok(srvHomeStats.FetchHomeStats(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: HomeStats
        [HttpPost]
        [Route("RD_HomeStatsBy")]
        public IActionResult RD_HomeStatsBy(HomeStatsModel mod)
        {
            try
            {
                return Ok(srvHomeStats.FetchHomeStats(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        //// POST: HomeStats/Save
        //        [HttpPost]
        //		[Route("Save")]
        //        public IActionResult Save(HomeStatsModel D)
        //        {
        //            try
        //            {
        //				  D.CurUserID = Convert.ToInt32(User.Identity.Name);
        //				return Ok(srv.SaveHomeStats(D));
        //            }
        //            catch(Exception e)
        //            {
        //                return BadRequest(e.InnerException.ToString());
        //            }
        //        }

        [HttpPost]
        [Route("FetchHomeStatsByUser")]
        public IActionResult FetchHomeStatsByUser(QueryFilters filters)
        {
            try
            {
                List<HomeStatsModel> list = srvHomeStats.FetchHomeStatsByUser(filters);
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("FetchPendingHomeStatsByUser")]
        public IActionResult FetchPendingHomeStatsByUser(QueryFilters filters)
        {
            try
            {
                List<HomeStatsModel> list = srvHomeStats.FetchPendingHomeStatsByUser(filters);
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

