using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSSMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeSourceOfCurriculumMapController : ControllerBase
    {
        private readonly ISRVTradeSourceOfCurriculumMap srvTradeSourceOfCurriculumMap;
        private readonly ISRVTrade srvSRVTrade;
        private readonly ISRVSourceOfCurriculum srvSRVSourceOfCurriculum;
        public TradeSourceOfCurriculumMapController(ISRVTradeSourceOfCurriculumMap srvTradeSourceOfCurriculumMap, ISRVTrade srvSRVTrade, ISRVSourceOfCurriculum srvSRVSourceOfCurriculum)
        {
            this.srvTradeSourceOfCurriculumMap = srvTradeSourceOfCurriculumMap;
            this.srvSRVTrade = srvSRVTrade;
            this.srvSRVSourceOfCurriculum = srvSRVSourceOfCurriculum;
        }
        // GET: TradeSourceOfCurriculumMap
        [HttpGet]
        [Route("GetTradeSourceOfCurriculumMap")]
        public async Task<IActionResult> GetTradeSourceOfCurriculumMap()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTradeSourceOfCurriculumMap.FetchTradeSourceOfCurriculumMap());

                ls.Add(srvSRVTrade.FetchTrade(false));

                ls.Add(srvSRVSourceOfCurriculum.FetchSourceOfCurriculum(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TradeSourceOfCurriculumMap
        [HttpGet]
        [Route("RD_TradeSourceOfCurriculumMap")]
        public async Task<IActionResult> RD_TradeSourceOfCurriculumMap()
        {
            try
            {
                return Ok(srvTradeSourceOfCurriculumMap.FetchTradeSourceOfCurriculumMap(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TradeSourceOfCurriculumMap
        [HttpPost]
        [Route("RD_TradeSourceOfCurriculumMapBy")]
        public async Task<IActionResult> RD_TradeSourceOfCurriculumMapBy(TradeSourceOfCurriculumMapModel mod)
        {
            try
            {
                return Ok(srvTradeSourceOfCurriculumMap.FetchTradeSourceOfCurriculumMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: TradeSourceOfCurriculumMap/Save
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(TradeSourceOfCurriculumMapModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTradeSourceOfCurriculumMap.SaveTradeSourceOfCurriculumMap(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TradeSourceOfCurriculumMap/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(TradeSourceOfCurriculumMapModel d)
        {
            try
            {
                srvTradeSourceOfCurriculumMap.ActiveInActive(d.TradeSourceOfCurriculumMapID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

