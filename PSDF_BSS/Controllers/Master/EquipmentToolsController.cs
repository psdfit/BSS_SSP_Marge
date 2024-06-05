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
    public class EquipmentToolsController : ControllerBase
    {
        ISRVEquipmentTools srv = null;
        public EquipmentToolsController(ISRVEquipmentTools srv)
        {
            this.srv = srv;
        }
        // GET: EquipmentTools
        [HttpGet]
        [Route("GetEquipmentTools")]
        public async Task<IActionResult> GetEquipmentTools()
        {
            try
            {

                List<object> ls = new List<object>();

                ls.Add(srv.FetchEquipmentTools());

                ls.Add(new SRVCertificationAuthority().FetchCertificationAuthority(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: EquipmentTools
        [HttpGet]
        [Route("RD_EquipmentTools")]
        public async Task<IActionResult> RD_EquipmentTools()
        {
            try
            {
                return Ok(srv.FetchEquipmentTools(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: EquipmentTools
        [HttpPost]
        [Route("RD_EquipmentToolsBy")]
        public async Task<IActionResult> RD_EquipmentToolsBy(EquipmentToolsModel mod)
        {
            try
            {
                return Ok(srv.FetchEquipmentTools(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: EquipmentTools/Save
        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(EquipmentToolsModel D)
        {
            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3], D.EquipmentToolID);
            if (IsAuthrized == true)
            {
                try
                {
                    D.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveEquipmentTools(D));
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

        // POST: EquipmentTools/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public async Task<IActionResult> ActiveInActive(EquipmentToolsModel d)
        {
            

            string[] SplitPath = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(true, Convert.ToInt32(User.Identity.Name), SplitPath[2], SplitPath[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    srv.ActiveInActive(d.EquipmentToolID, d.InActive, Convert.ToInt32(User.Identity.Name));
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

