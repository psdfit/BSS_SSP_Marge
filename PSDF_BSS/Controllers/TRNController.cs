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
    public class TRNController : ControllerBase
    {
        private readonly ISRVTrn srvTRN;

        public TRNController(ISRVTrn srvTRN)
        {
            this.srvTRN = srvTRN;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddTrn()
        {
            try
            {
                await srvTRN.AddTRN();
                return Ok("Working ok");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("GetTRN")]
        public IActionResult GetTRN(QueryFilters mod)
        {
            try
            {
                return Ok(srvTRN.FetchTRN(mod));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetTRNDetails/{trnMasterID}")]
        public IActionResult GetTRNDetails(int trnMasterID)
        {
            try
            {
                List<object> list = new List<object>();
                list.Add(srvTRN.FetchTRNDetails(trnMasterID));
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("GetTRNBulkExcelExportByIDs")]
        public IActionResult GetTRNBulkExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvTRN.GetTRNExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }


        [HttpPost]
        [Route("GetTRNExcelExport")]
        public IActionResult GetTRNExcelExport(TRNMasterModel m)
        {
            try
            {
                return Ok(srvTRN.GetTRNExcelExport(m));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

    }
}
