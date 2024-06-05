using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfrastructureController : ControllerBase
    {
        private readonly ISRVInfrastructure srvInfrastructure;
        public InfrastructureController(ISRVInfrastructure srvInfrastructure)
        {
            this.srvInfrastructure = srvInfrastructure;
        }
        // POST: Scheme/Save
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(InfrastructureSaveModel model)
        {
            try
            {
                return Ok(srvInfrastructure.SaveInfrastructure(model));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("GetInfrastructures")]
        public IActionResult GetInfrastructures()
        {
            //if (srv.CheckUserInFormApproval(Convert.ToInt32(User.Identity.Name)) == false)
            //    return Ok(false);

            try
            {
                List<InfrastructureModel> infrastructures = new List<InfrastructureModel>();
                infrastructures = srvInfrastructure.GetInfrastructures();

                if (infrastructures.Count > 0)
                    return Ok(infrastructures);
                else
                    return Ok(null);
            }
            catch (Exception e)
            { throw new Exception(e.ToString()); }
        }
    }
}