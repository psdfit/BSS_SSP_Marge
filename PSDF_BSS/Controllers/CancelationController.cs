/* **** Aamer Rehman Malik *****/

using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace PSDF_BSSd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancelationController : ControllerBase
    {
        private readonly ISRVClassInvoiceExtMap srv = null;
        private readonly ISRVScheme _serviceScheme;
        private readonly ISRVTSPDetail _serviceTSP;
        private readonly ISRVClass _serviceClass;
        private readonly ISRVUsers srvUsers;
        private readonly ISRVDashboard _srvDashboard;

        public CancelationController(ISRVClassInvoiceExtMap srv, ISRVScheme _serviceScheme, ISRVTSPDetail _serviceTSP, ISRVClass _serviceClass, ISRVUsers srvUsers, ISRVDashboard _srvDashboard)
        {
            this.srv = srv;
            this._serviceScheme = _serviceScheme;
            this._serviceTSP = _serviceTSP;
            this._serviceClass = _serviceClass;
            this.srvUsers = srvUsers;
            this._srvDashboard = _srvDashboard;
        }

        // GET: MPR
        [HttpGet]
        [Route("GetData")]
        public IActionResult GetData()
        {
            try
            {
                Dictionary<string, object> ls = new Dictionary<string, object>
                {
                    { "Schemes", _srvDashboard.FetchSchemes() },
                    { "TSP", _serviceTSP.FetchTSPDetail() },
                    { "Classes", _serviceClass.FetchClass() }
                };

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        // GET: MPR
        [HttpPost]
        [Route("GetRelevantUserData")]
        public IActionResult GetRelevantUserData(QueryFilters filters)
        {
            try
            {

                int curUserID = Convert.ToInt32(User.Identity.Name);
                int loggedInUserLevel = srvUsers.GetByUserID(curUserID).UserLevel;
                filters.UserID = loggedInUserLevel == (int)EnumUserLevel.TSP ? curUserID : 0;
                Dictionary<string, object> ls = new Dictionary<string, object>
                {
                    { "Schemes", _serviceScheme.FetchSchemeByUser(filters) },
                    { "TSP", _serviceTSP.FetchTSPByUser(filters) },
                    { "Classes", _serviceClass.FetchClassesByUser(filters) }
                };

                return Ok(ls);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("getCancelationData/{id}")]
        public IActionResult getCancelationData(int id)
        {
            try
            {
                return Ok(srv.GetMPRPRNDetails(id, true));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        
        [HttpGet]
        [Route("getTSPInvoiceStatusData/{id}")]
        public IActionResult getTSPInvoiceStatusData(int id)
        {
            try
            {
                return Ok(srv.GetTSPInvoiceStatusDetails(id, true));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("getTSPInvoiceStatusDataMonthWise")]
        public IActionResult getTSPInvoiceStatusDataMonthWise(TSPInvoiceStatusModel mod)
        {
            try
            {
                var ClassID = mod.ClassID;
                var Month = mod.Month;
                var CurUserID = Convert.ToInt32(User.Identity.Name);
                var InvoiceHeaderID = mod.InvoiceHeaderID;

                return Ok(srv.GetTSPInvoiceStatusDetailsMonthWise(Month, CurUserID, InvoiceHeaderID, ClassID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        
        [HttpPost]
        [Route("getTSPInvoiceStatusDataMonthWiseByInternalUser")]
        public IActionResult getTSPInvoiceStatusDataMonthWiseByInternalUser(TSPInvoiceStatusModel mod)
        {
            try
            {
                var ClassID = mod.ClassID;
                var Month = mod.Month;
                var CurUserID = Convert.ToInt32(User.Identity.Name);
                var InvoiceHeaderID = mod.InvoiceHeaderID;

                return Ok(srv.GetTSPInvoiceStatusDetailsMonthWiseByInternalUser(Month, CurUserID, InvoiceHeaderID, ClassID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpPost]
        [Route("getTSPInvoiceStatusDataMonthWiseForKAM")]
        public IActionResult getTSPInvoiceStatusDataMonthWiseForKAM(TSPInvoiceStatusModel mod)
        {
            try
            {
                var ClassID = mod.ClassID;
                var Month = mod.Month;
                var CurUserID = Convert.ToInt32(User.Identity.Name);
                var InvoiceHeaderID = mod.InvoiceHeaderID;

                //return Ok(srv.getTSPInvoiceStatusDataMonthWiseForKAM(Month, CurUserID, ClassID));
                return Ok(srv.getTSPInvoiceStatusDetailMonthWiseForKAM(Month, CurUserID, InvoiceHeaderID, ClassID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }

        [HttpGet]
        [Route("getInvoice")]
        public IActionResult getInvoice()
        {
            try
            {
                return Ok(srv.GetForApproval());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("getMPRByID/{id}")]
        public IActionResult getMPR(int id)
        {
            try
            {
                return Ok(srv.GetMPR(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
        }
        [HttpGet]
        [Route("getSRN/{id}")]
        public IActionResult getSRN(int id)
        {
            try
            {
                return Ok(srv.GetSRN(id));
            }
            catch (Exception e)  {  return BadRequest(e.InnerException.ToString());  }
        }
        [HttpGet]
        [Route("getPRN/{id}")]
        public IActionResult getPRN(int id)
        {
            try
            {
                return Ok(srv.GetPRNMaster(id));
            }
            catch (Exception e) { return BadRequest(e.InnerException.ToString()); }
        }
        [HttpGet]
        [Route("getPO/{id}")]
        public IActionResult getPO(int id)
        {
            try
            {
                return Ok(srv.GetPOHeader(id));
            }
            catch (Exception e) { return BadRequest(e.InnerException.ToString()); }
        }

        //[HttpGet]
        //[Route("getTSPInvoiceStatusData/{id}")]
        //public IActionResult getTSPInvoiceStatusData(int id)
        //{
        //    try
        //    {
        //        return Ok(srv.GetTSPInvoiceStatusDetails(id, true));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}

        //[HttpPost]
        //[Route("getTSPInvoiceStatusDataMonthWise")]
        //public IActionResult getTSPInvoiceStatusDataMonthWise(TSPInvoiceStatusModel mod)
        //{
        //    try
        //    {
        //        var ClassID = mod.ClassID;
        //        var Month = mod.Month;
        //        var CurUserID = Convert.ToInt32(User.Identity.Name);
        //        var InvoiceHeaderID = mod.InvoiceHeaderID;

        //        return Ok(srv.GetTSPInvoiceStatusDetailsMonthWise(Month, CurUserID, InvoiceHeaderID, ClassID));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}
        //[HttpPost]
        //[Route("getTSPInvoiceStatusDataMonthWiseForKAM")]
        //public IActionResult getTSPInvoiceStatusDataMonthWiseForKAM(TSPInvoiceStatusModel mod)
        //{
        //    try
        //    {
        //        var ClassID = mod.ClassID;
        //        var Month = mod.Month;
        //        var CurUserID = Convert.ToInt32(User.Identity.Name);
        //        var InvoiceHeaderID = mod.InvoiceHeaderID;

        //        return Ok(srv.getTSPInvoiceStatusDataMonthWiseForKAM(Month, CurUserID, InvoiceHeaderID, ClassID));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}


        [HttpGet]
        [Route("getInv/{id}")]
        public IActionResult getInv(int id)
        {
            try
            {
                return Ok(srv.GetInvHeader(id));
            }
            catch (Exception e) { return BadRequest(e.InnerException.ToString()); }
        }
        // POST: MPR/Save
        [HttpGet]
        [Route("Cancelation")]
        public IActionResult Cancelation(int FormID, string Type, int ClassID)
        {
            try
            {
                srv.Cancellation(FormID, Type, ClassID);
                return Ok(srv.GetMPRPRNDetails(ClassID, true));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpPost]
        //[Route("Cancelation")]
        //public IActionResult Cancelation_Old(ClassInvoiceMapExtModel D)
        //{
        //    try
        //    {
        //        srv.Cancellation(D.ClassID, D.Month, D.InvoiceType, D.InvoiceID);
        //        return Ok(srv.GetMPRPRNDetails(D.ClassID, true));
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.InnerException.ToString());
        //    }
        //}

        [HttpPost]
        [Route("Regenerate")]
        public IActionResult Regenerate(ClassInvoiceMapExtModel D)
        {
            try
            {
                srv.Regenerate(D.ClassID, D.Month, D.Type);
                return Ok(srv.GetMPRPRNDetails(D.ClassID, true));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}