using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using PSDF_BSS.Logging;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumableMaterialController : ControllerBase
    {
	ISRVConsumableMaterial srv = null;
	 public ConsumableMaterialController(ISRVConsumableMaterial srv)
        {
            this.srv = srv;
        }
        // GET: ConsumableMaterial
		[HttpGet]
        [Route("GetConsumableMaterial")]
        public async Task<IActionResult> GetConsumableMaterial()
        {
           try
            {
	return Ok(srv.FetchConsumableMaterial());
	 }catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }}
// RD: ConsumableMaterial
		[HttpGet]
        [Route("RD_ConsumableMaterial")]
        public async Task<IActionResult> RD_ConsumableMaterial()
        {
           try
            {
			return Ok(srv.FetchConsumableMaterial(false));
	}catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }}// RD: ConsumableMaterial
        [HttpPost]
        [Route("RD_ConsumableMaterialBy")]
        public async Task<IActionResult> RD_ConsumableMaterialBy(ConsumableMaterialModel mod)
        {
            try
            {
                return Ok(srv.FetchConsumableMaterial(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }// POST: ConsumableMaterial/Save
        [HttpPost]
		[Route("Save")]
        public async Task<IActionResult> Save(ConsumableMaterialModel D)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3], D.ConsumableMaterialID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveConsumableMaterial(D));
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
			  // POST: ConsumableMaterial/ActiveInActive
        [HttpPost]
		[Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(ConsumableMaterialModel d)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.ConsumableMaterialID, d.InActive, Convert.ToInt32(User.Identity.Name));
                    return Ok(true);

                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
		}
		}
		
