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
    public class SrnDisbursementStatusController : ControllerBase
    {
        private readonly ISRVSrnDisbursementStatus srvStipendDisbursement;
        private readonly ISRVOrgConfig srvOrgConfig;
        private readonly ISRVTraineeStatus srvTraineeStatus;
        private readonly ISRVClass srvClass;

        public SrnDisbursementStatusController(ISRVSrnDisbursementStatus srvStipendDisbursement
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
        [Route("GetTraineeForSrnDisbursement")]
        public IActionResult GetTraineeForSrnDisbursement()
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvStipendDisbursement.FetchSrnDisbursementStatus());

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("GetTraineeForSrnDisbursementByFilters")]
        public IActionResult GetTraineeForSrnDisbursementByFilters([FromBody] SrnDisbursementFiltersModel m)
        {
            try
            {
                List<object> ls = new List<object>();

                ls.Add(srvStipendDisbursement.FetchSrnDisbursementStatusByFilters(m));

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateSrnDisbursementTrainees")]
        public IActionResult UpdateSrnDisbursementTrainees(List<SrnDisbursementModel> mod)
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