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
    public class ClassEventMapController : ControllerBase
    {
        private readonly ISRVClassEventMap srvClassEventMap = null;
        public ClassEventMapController(ISRVClassEventMap srvClassEventMap)
        {
            this.srvClassEventMap = srvClassEventMap;
        }
        // GET: ClassEventMap
        [HttpGet]
        [Route("GetClassEventMap")]
        public IActionResult GetClassEventMap()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvClassEventMap.FetchClassEventMap());

                //ls.Add(new SRVPaymentRecommendationNoteClassData().FetchPaymentRecommendationNoteClassData(false));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: ClassEventMap
        [HttpGet]
        [Route("RD_ClassEventMap")]
        public IActionResult RD_ClassEventMap()
        {
            try
            {
                return Ok(srvClassEventMap.FetchClassEventMap(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // RD: ClassEventMap
        [HttpPost]
        [Route("RD_ClassEventMapBy")]
        public IActionResult RD_ClassEventMapBy(ClassEventMapModel mod)
        {
            try
            {
                return Ok(srvClassEventMap.FetchClassEventMap(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // POST: ClassEventMap/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(ClassEventMapModel D)
        {
            try
            {
                D.CurUserID = Convert.ToInt32(User.Identity.Name);
                return Ok(srvClassEventMap.SaveClassEventMap(D));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        // POST: ClassEventMap/ActiveInActive
        [HttpPost]
        [Route("ActiveInActive")]
        public IActionResult ActiveInActive(ClassEventMapModel d)
        {
            try
            {
                srvClassEventMap.ActiveInActive(d.ClassEventMapID, d.InActive, Convert.ToInt32(User.Identity.Name));
                return Ok(true);

            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}

