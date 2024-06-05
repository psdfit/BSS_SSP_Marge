using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Models;
using DataLayer.Interfaces;
using DataLayer.Services;
using DataLayer.Classes;

namespace PSDF_BSS.Controllers.PurchaseOrder
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ISRVPurchaseOrder srvPurchaseOrder;
        private readonly ISRVPOHeader srvPOHeader;
        private readonly ISRVPOLines srvPOLines;
        private readonly ISRVPOSummary srvPOSummary;
        private readonly ISRVScheme srvScheme;
        private readonly ISRVTSPDetail srvTspDetail;
        private readonly ISRVDashboard srvDashboard;

        public PurchaseOrderController(ISRVPurchaseOrder srvPurchaseOrder, ISRVPOHeader srvPOHeader, ISRVPOLines srvPOLines, ISRVPOSummary srvPOSummary
            ,ISRVScheme srvScheme, ISRVTSPDetail srvTspDetail, ISRVDashboard srvDashboard)
        {
            this.srvPurchaseOrder = srvPurchaseOrder;
            this.srvPOHeader = srvPOHeader;
            this.srvPOLines = srvPOLines;
            this.srvPOLines = srvPOLines;
            this.srvPOSummary = srvPOSummary;
            this.srvScheme = srvScheme;
            this.srvTspDetail = srvTspDetail;
            this.srvDashboard = srvDashboard;
        }


        [Route("GetPOHeaderSchemes")]
        public IActionResult GetPOHeaderSchemes(POHeaderModel model)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                list.Add("scheme", srvDashboard.FetchSchemes());
                return Ok(list);
            }
            catch (Exception e)
            { return BadRequest(e.Message); }
        }

        [HttpPost]
        [Route("GetPOHeader")]
        public IActionResult GetPOHeader(POHeaderModel model)
        {
            try
            {
                Dictionary<string, object> list = new Dictionary<string, object>();
                list.Add("poheader", srvPOHeader.FetchPOHeader(model));
                list.Add("scheme", srvDashboard.FetchSchemes());
                return Ok(list);
            }
            catch (Exception e)
            { return BadRequest(e.Message); }
        }

        [HttpPost]
        [Route("GetPOHeaderByFilters")]
        public IActionResult GetPOHeaderByFilters([FromBody]QueryFilters filters)
        {
            try
            {
                DateTime? Month = null;
                if (filters.Month.HasValue) { Month = filters.Month.Value; }
                //if (!DateTime.TryParse(filters.Month.Value.ToString(), out Month)) { Month = null; }
                Dictionary<string, object> list = new Dictionary<string, object>();
                list.Add("poheader", srvPOHeader.FetchPOHeaderByFilters(Month,filters.SchemeID,filters.TSPID,filters.ProcessKey));
                list.Add("tspdetail", srvDashboard.FetchTSPsByScheme(filters.SchemeID));
                return Ok(list);
            }
            catch (Exception e)
            { return BadRequest(e.Message); }
        }


        [HttpGet]
        [Route("GetPOForApproval")]
        public IActionResult GetPOForApproval()
        
        {
            try
            {
                List<SubmitedPOsModel> po = new List<SubmitedPOsModel>();

                po = srvPOHeader.GetPOForApproval(Convert.ToInt32(User.Identity.Name));

                return Ok(po);
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpGet]
        [Route("GetPOLinesByPOHeaderID/{id}")]
        public IActionResult GetPOLinesByPOHeaderID(int id)
        {
            try
            {
                List<POLinesModel> po = new List<POLinesModel>();
                po = srvPOLines.GetPOLinesByPOHeaderID(id);

                return Ok(po);
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }

        [HttpPost]
        [Route("GetPOSummary")]
        public IActionResult GetPOSummary(POSummaryModel m)
        {
            try
            {
                return Ok(srvPOSummary.GetPOSummary(m.Month,m.SchemeID,m.TSPID,m.ProcessKey));
            }
            catch (Exception e)
            { return BadRequest(e.InnerException.ToString()); }
        }
    }
}