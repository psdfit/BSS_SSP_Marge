using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvincesController : ControllerBase
    {
        private ISRVProvinces srv = null;

        public ProvincesController(ISRVProvinces srv)
        {
            this.srv = srv;
        }

        // GET: Provinces
        [HttpGet]
        [Route("GetProvinces")]
        public IActionResult GetProvinces()
        {
            try
            {
                return Ok(srv.FetchProvinces());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Provinces
        [HttpGet]
        [Route("RD_Provinces")]
        public IActionResult RD_Provinces()
        {
            try
            {
                return Ok(srv.FetchProvinces(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // RD: Provinces
        [HttpPost]
        [Route("RD_ProvincesBy")]
        public IActionResult RD_ProvincesBy(ProvincesModel mod)
        {
            try
            {
                return Ok(srv.FetchProvinces(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Provinces/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ProvincesModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srv.SaveProvinces(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: Provinces/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ProvincesModel d)
        {
            try
            {
               
                srv.ActiveInActive(d.Id, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}