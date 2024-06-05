using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Services;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ISRVInvoiceMaster srvInvoiceMaster;
        private readonly ISRVInvoice srvInvoice;
        public InvoiceController(ISRVInvoiceMaster srvInvoiceMaster, ISRVInvoice srvInvoice)
        {
            this.srvInvoiceMaster = srvInvoiceMaster;
            this.srvInvoice = srvInvoice;
        }

        [HttpPost]
        [Route("GetInvoicesForApproval")]
        public IActionResult GetInvoicesForApproval(InvoiceMasterModel model)
        {
            try
            {
                return Ok(srvInvoiceMaster.GetInvoicesForApproval(model, null));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("GetInvoiceDetails")]
        public IActionResult GetInvoiceDetails(InvoiceMasterModel model)
        {
            try
            {
                return Ok(srvInvoiceMaster.GetInvoiceDetails(model, null));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("GetInvoicesForTSP")]
        public IActionResult GetInvoicesForTSP(InvoiceMasterModel model)
        {
            try
            {
                return Ok(srvInvoiceMaster.GetInvoiceHeaderForTSP(model, null));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        

        [HttpPost]
        [Route("GetInvoicesForInternalUser")]
        public IActionResult GetInvoicesForInternalUser(InvoiceMasterModel model)
        {
            try
            {
                return Ok(srvInvoiceMaster.GetInvoiceHeaderForInternalUser(model, null));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost]
        [Route("GetInvoicesForKAM")]
        public IActionResult GetInvoicesForKAM(InvoiceMasterModel model)
        {
            try
            {
                return Ok(srvInvoiceMaster.GetInvoiceHeaderForKAM(model, null));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpPost]
        //[Route("GetInvoicesForTSP")]
        //public IActionResult GetInvoicesForTSP(InvoiceMasterModel model)
        //{
        //    try
        //    {
        //        return Ok(srvInvoiceMaster.GetInvoiceHeaderForTSP(model, null));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("GetInvoicesForKAM")]
        //public IActionResult GetInvoicesForKAM(InvoiceMasterModel model)
        //{
        //    try
        //    {
        //        return Ok(srvInvoiceMaster.GetInvoiceHeaderForKAM(model, null));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpGet]
        [Route("GetInvoiceLines/{id}")]
        public IActionResult GetInvoiceLines(int id)
        {
            try
            {
                return Ok(srvInvoice.GetInvoicesForApproval(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("GetInvoiceBulkExcelExportByIDs")]
        public IActionResult GetInvoiceBulkExcelExportByIDs([FromBody] string ids)
        {
            try
            {
                return Ok(srvInvoice.GetInvoiceExcelExportByIDs(ids));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

    }
}