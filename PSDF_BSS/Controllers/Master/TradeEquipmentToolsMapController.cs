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
    public class TradeEquipmentToolsMapController : ControllerBase
    {
	ISRVTradeEquipmentToolsMap srv = null;
	 public TradeEquipmentToolsMapController(ISRVTradeEquipmentToolsMap srv)
        {
            this.srv = srv;
        }
        // GET: TradeEquipmentToolsMap
		[HttpGet]
        [Route("GetTradeEquipmentToolsMap")]
        public async Task<IActionResult> GetTradeEquipmentToolsMap()
        {
           try
            {List<object> ls=new List<object>();
	
	ls.Add(srv.FetchTradeEquipmentToolsMap()); 
                    ls.Add(new SRVTrade().FetchTrade(false));
                     
                    ls.Add(new SRVEquipmentTools().FetchEquipmentTools(false));
                       return Ok(ls);
	}catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }}
// RD: TradeEquipmentToolsMap
		[HttpGet]
        [Route("RD_TradeEquipmentToolsMap")]
        public async Task<IActionResult> RD_TradeEquipmentToolsMap()
        {
           try
            {
			return Ok(srv.FetchTradeEquipmentToolsMap(false));
	}catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }}// RD: TradeEquipmentToolsMap
        [HttpPost]
        [Route("RD_TradeEquipmentToolsMapBy")]
        public async Task<IActionResult> RD_TradeEquipmentToolsMapBy(TradeEquipmentToolsMapModel mod)
        {
            try
            {
                return Ok(srv.FetchTradeEquipmentToolsMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }// POST: TradeEquipmentToolsMap/Save
        [HttpPost]
		[Route("Save")]
        public async Task<IActionResult> Save(TradeEquipmentToolsMapModel D)
        {
            try
            {
				  D.CurUserID = Convert.ToInt32(User.Identity.Name);
				return Ok(srv.SaveTradeEquipmentToolsMap(D));
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
			  // POST: TradeEquipmentToolsMap/ActiveInActive
        [HttpPost]
		[Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(TradeEquipmentToolsMapModel d)
        {
            try
            {		
                    srv.ActiveInActive(d.TradeEquipmentToolsMapID,d.InActive, Convert.ToInt32(User.Identity.Name));
                   return Ok(true);
                
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
		}
		}
		
