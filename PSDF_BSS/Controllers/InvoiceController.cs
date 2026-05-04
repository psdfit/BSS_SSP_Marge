using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSDF_BSS.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ISRVInvoiceMaster srvInvoiceMaster;
        private readonly ISRVInvoice srvInvoice;
        private readonly ISRVBaseData srvBaseData;
        public InvoiceController(ISRVInvoiceMaster srvInvoiceMaster, ISRVInvoice srvInvoice, ISRVBaseData srvBaseData)
        {
            this.srvInvoiceMaster = srvInvoiceMaster;
            this.srvInvoice = srvInvoice;
            this.srvBaseData = srvBaseData;
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
        [Route("SaveInvoiceLetterhead")]
        public IActionResult SaveInvoiceLetterhead(InvoiceLetterheadAttachmentModel model)
        {
            try
            {

                string[] Split = HttpContext.Request.Path.Value.Split("/");
                bool IsAuthrized = Authorize.CheckAuthorize(
                    false,
                    Convert.ToInt32(User.Identity.Name),
                    Split[2],
                    Split[3]
                );
                model.UserID = Convert.ToInt32(User.Identity.Name);

                if (model == null)
                    return BadRequest("Invalid request");

                var result = srvInvoiceMaster.SaveInvoiceLetterhead(model, null);

                return Ok(new
                {
                    success = true,
                    message = "Invoice letterhead saved successfully",
                    data = result
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    success = false,
                    message = e.Message
                });
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

        [HttpGet]
        [Route("GetInvoiceBuyerSupplierInfo/{id}")]
        public IActionResult GetInvoiceBuyerSupplierInfo(int id)
        {
            try
            {
                // Fetching data in parallel
                var tspDetail = srvBaseData.GetInvoiceBuyerSupplierInfo(id);
                return Ok(tspDetail);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpGet]
        [Route("GetInvoiceLetterheadInfo/{id}")]
        public IActionResult GetInvoiceLetterheadInfo(int id)
        {
            try
            {
                // Fetching data in parallel
                var InvoiceBuyerSupplierInfo = srvInvoiceMaster.GetInvoiceLetterheadInfo(id);
                return Ok(InvoiceBuyerSupplierInfo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        [HttpGet]
        [Route("GetTSPMasterID/{id}")]
        public IActionResult GetTSPMasterID(int id)
        {
            try
            {
                // Fetching data in parallel
                var TSPMasterID = srvInvoiceMaster.GetTSPMasterID(id);
                return Ok(TSPMasterID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
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