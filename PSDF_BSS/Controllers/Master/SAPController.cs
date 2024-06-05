using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace PSDF_BSS.Controllers.Master
{
    [ApiController]
    [Route("api/[controller]")]
    public class SAPController : ControllerBase
    {
        private readonly ISRVSAPApi srvSAPApi;
        public SAPController(ISRVSAPApi srvSAPApi)
        {
            this.srvSAPApi = srvSAPApi;
        }
        [HttpGet]
        [Route("GetSAPBraches")]
        public async Task<IActionResult> GetSAPBraches()
        {
            try
            {
                return Ok(await srvSAPApi.FetchSAPBranches());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpPost]
        [Route("SynceBranches")]
        public async Task<IActionResult> SynceBranches()
        {
            try
            {
                return Ok(await srvSAPApi.SynceBranches());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
    }
}