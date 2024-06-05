using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
namespace PSDF_BSS.Controllers.Master
{
	[ApiController]
    [Route("api/[controller]")]
    public class TradeConsumableMaterialMapController : ControllerBase
    {
	ISRVTradeConsumableMaterialMap srv = null;
	 public TradeConsumableMaterialMapController(ISRVTradeConsumableMaterialMap srv)
        {
            this.srv = srv;
        }
        // GET: TradeConsumableMaterialMap
		[HttpGet]
        [Route("GetTradeConsumableMaterialMap")]
        public async Task<IActionResult> GetTradeConsumableMaterialMap()
        {
           try
            {
List<object> ls=new List<object>();
	
	ls.Add(srv.FetchTradeConsumableMaterialMap());
 
                    ls.Add(new SRVTrade().FetchTrade(false));
                     
                    ls.Add(new SRVConsumableMaterial().FetchConsumableMaterial(false));
                    
   return Ok(ls);
	}catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
}
// RD: TradeConsumableMaterialMap
		[HttpGet]
        [Route("RD_TradeConsumableMaterialMap")]
        public async Task<IActionResult> RD_TradeConsumableMaterialMap()
        {
           try
            {
			return Ok(srv.FetchTradeConsumableMaterialMap(false));
	}catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }}
// RD: TradeConsumableMaterialMap
        [HttpPost]
        [Route("RD_TradeConsumableMaterialMapBy")]
        public async Task<IActionResult> RD_TradeConsumableMaterialMapBy(TradeConsumableMaterialMapModel mod)
        {
            try
            {
                return Ok(srv.FetchTradeConsumableMaterialMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
// POST: TradeConsumableMaterialMap/Save
        [HttpPost]
		[Route("Save")]
        public async Task<IActionResult> Save(TradeConsumableMaterialMapModel D)
        {
            try
            {
				  D.CurUserID = Convert.ToInt32(User.Identity.Name);
				return Ok(srv.SaveTradeConsumableMaterialMap(D));
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
			 
 // POST: TradeConsumableMaterialMap/ActiveInActive
        [HttpPost]
		[Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(TradeConsumableMaterialMapModel d)
        {
            try
            {		
                    srv.ActiveInActive(d.TradeConsumableMaterialMapID,d.InActive, Convert.ToInt32(User.Identity.Name));
                   return Ok(true);
                
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
		}
		}
		
