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
    public class TradeDetailMapController : ControllerBase
    {
        private readonly ISRVTradeDetailMap srvTradeDetailMap;
        private readonly ISRVTrade srvSRVTrade ;
        public TradeDetailMapController(ISRVTradeDetailMap srvTradeDetailMap, ISRVTrade srvSRVTrade)
        {
            this.srvTradeDetailMap = srvTradeDetailMap;
            this.srvSRVTrade = srvSRVTrade;
        }
        // GET: TradeDurationMap
        [HttpGet]
        [Route("GetTradeDetailMap")]
        public IActionResult GetTradeDetailMap()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvTradeDetailMap.FetchTradeDetailMap());

                ls.Add(srvSRVTrade.FetchTrade(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TradeDurationMap
        [HttpGet]
        [Route("RD_TradeDetailMap")]
        public IActionResult RD_TradeDetailMap()
        {
            try
            {
                return Ok(srvTradeDetailMap.FetchTradeDetailMap(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: TradeDurationMap
        [HttpPost]
        [Route("RD_TradeDetailMapBy")]
        public IActionResult RD_TradeDurationMapBy(TradeDetailMapModel mod)
        {
            try
            {
                return Ok(srvTradeDetailMap.FetchTradeDetailMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: TradeDurationMap/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(TradeDetailMapModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvTradeDetailMap.SaveTradeDetailMap(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: TradeDurationMap/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(TradeDetailMapModel d)
        {
            try
            {
               
                srvTradeDetailMap.ActiveInActive(d.TradeDetailMapID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

