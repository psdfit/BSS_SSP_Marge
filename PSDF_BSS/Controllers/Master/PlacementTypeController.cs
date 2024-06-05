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
    public class PlacementTypeController : ControllerBase
    {
	ISRVPlacementType srv = null;
	 public PlacementTypeController(ISRVPlacementType srv)
        {
            this.srv = srv;
        }
        // GET: PlacementType
		[HttpGet]
        [Route("GetPlacementType")]
        public IActionResult GetPlacementType()
        {
           try
            {

	return Ok(srv.FetchPlacementType());
	 }catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
}
// RD: PlacementType
		[HttpGet]
        [Route("RD_PlacementType")]
        public IActionResult RD_PlacementType()
        {
           try
            {
			return Ok(srv.FetchPlacementType(false));
	}catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }}
// RD: PlacementType
        [HttpPost]
        [Route("RD_PlacementTypeBy")]
        public IActionResult RD_PlacementTypeBy(PlacementTypeModel mod)
        {
            try
            {
                return Ok(srv.FetchPlacementType(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
// POST: PlacementType/Save
        [HttpPost]
		[Route("Save")]
        public IActionResult Save(PlacementTypeModel D)
        {
            try
            {
				  D.CurUserID = Convert.ToInt32(User.Identity.Name);
				return Ok(srv.SavePlacementType(D));
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
			 
 // POST: PlacementType/ActiveInActive
        [HttpPost]
		[Route("ActiveInActive")]
        public IActionResult ActiveInActive(PlacementTypeModel d)
        {
            try
            {		
                    srv.ActiveInActive(d.PlacementTypeID,d.InActive, Convert.ToInt32(User.Identity.Name));
                   return Ok(true);
                
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
		}
		}
		
