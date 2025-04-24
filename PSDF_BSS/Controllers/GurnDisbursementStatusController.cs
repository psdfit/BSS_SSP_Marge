using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSSDisbursement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GurnDisbursementStatusController : ControllerBase
    {
        private readonly ISRVGurnDisbursementStatus srvStipendDisbursement;
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVTraineeStatus srvTraineeStatus;
        private readonly ISRVClass srvClass;

        public GurnDisbursementStatusController(ISRVGurnDisbursementStatus srvStipendDisbursement
            , ISRVOrgConfig srvOrgConfig
            , ISRVTraineeStatus srvTraineeStatus
            , ISRVClass srvClass

            )
        {
            this.srvStipendDisbursement = srvStipendDisbursement;
            this.srvOrgConfig = srvOrgConfig;
            this.srvTraineeStatus = srvTraineeStatus;
            this.srvClass = srvClass;
        }

        [HttpGet]
        [Route("GetTraineeForGurnDisbursement")]
        public IActionResult GetTraineeForGurnDisbursement()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvStipendDisbursement.FetchGurnDisbursementStatus());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("GetTraineeForGurnDisbursementByFilters")]
        public IActionResult GetTraineeForGurnDisbursementByFilters([FromBody] GurnDisbursementFiltersModel m)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvStipendDisbursement.FetchGurnDisbursementStatusByFilters(m));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateGurnDisbursementTrainees")]
        public IActionResult UpdateGurnDisbursementTrainees(List<GurnDisbursementModel> mod)
        {
            try
            {
                srvStipendDisbursement.UpdateTraineesDisbursement(mod);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

    }
}