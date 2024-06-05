using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class WebsiteController : Controller
    {
        private readonly ISRVWebsite _srvWebsite;

        public WebsiteController(ISRVWebsite srvWebsite)
        {
            _srvWebsite = srvWebsite;
        }

        [HttpGet]
        [Route("GetDistricts")]
        public IActionResult GetDistricts(string ModifiedDate)
        {
            try
            {
                return Ok(this._srvWebsite.GetDistricts(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpGet]
        [Route("GetTehsils")]
        public IActionResult GetTehsils(string ModifiedDate)
        {
            try
            {
                return Ok(this._srvWebsite.GetTehsils(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpGet]
        [Route("GetSectors")]
        public IActionResult GetSectors(string ModifiedDate)
        {
            try
            {
                return Ok(this._srvWebsite.GetSectors(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpGet]
        [Route("GetSubSectors")]
        public IActionResult GetSubSectors(string ModifiedDate)
        {
            try
            {
                return Ok(this._srvWebsite.GetSubSectors(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpGet]
        [Route("GetTrades")]
        public IActionResult GetTrades(string ModifiedDate)
        {
            try
            {
                //return Ok(this._srvWebsite.GetTrades(ModifiedDate));
                return Ok(this._srvWebsite.GetBSSTrades(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpGet]
        [Route("GetClasses")]
        public IActionResult GetClasses(string ModifiedDate)
        {
            try
            {
                return Ok(this._srvWebsite.GetClasses(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpGet]
        [Route("GetGenders")]
        public IActionResult GetGenders(string ModifiedDate)
        {
            try
            {
                return Ok(this._srvWebsite.GetGenders(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpGet]
        [Route("GetEducationTypes")]
        public IActionResult GetEducationTypes(string ModifiedDate)
        {
            try
            {
                return Ok(this._srvWebsite.GetEducationTypes(ModifiedDate));
            }
            catch (Exception e)
            {
                return BadRequest("System is unble to process your request");
            }
        }

        [HttpPost]
        [Route("SavePotentialTrainee")]
        public IActionResult SavePotentialTrainee(PotentialTraineesModel Model)
        {
            try
            {
                return Ok(this._srvWebsite.SavePotentialTrainee(Model));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
