using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DataLayer.Interfaces;
using DataLayer.Models;
using DataLayer.Models.SSP;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Build.Tasks;
using Newtonsoft.Json;
using PSDF_BSS.Logging;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramDesignController : ControllerBase
    {
        private ISRVProgramDesign srv = null;
        private readonly ISRVProvinces srvProvinces;
        private readonly ISRVCluster srvCluster;
        private readonly ISRVDistrict srvDistrict;
        private readonly ISRVTehsil srvTehsil;



        public ProgramDesignController(
            ISRVProvinces srvProvince,
            ISRVCluster srvCluster,
            ISRVDistrict srvDistrict,
            ISRVTehsil srvTehsil,
            ISRVProgramDesign srv
           )
        {
            this.srvProvinces = srvProvince;
            this.srvCluster = srvCluster;
            this.srvDistrict = srvDistrict;
            this.srvTehsil = srvTehsil;
            this.srv = srv;

        }


       


        //Program Design 
        [HttpGet]
        [Route("GetProgramDesign")]
        public IActionResult GetProgramDesign()
        {
            try
            {
                var Province = srv.FetchDropDownList("RD_Province");
                var Cluster = srv.FetchDropDownList("RD_Cluster");
                var District = srv.FetchDropDownList("RD_District");
                var GetFinancialYear = srv.FetchDropDownList("RD_CustomFinancialYears");
                var GetProgramType = srv.FetchDropDownList("RD_ProgramType");
                var FundingSource = srv.FetchDropDownList("RD_FundingSource");
                var Gender = srv.FetchDropDownList("RD_Gender");
                var EducationTypes = srv.FetchDropDownList("RD_EducationTypes");
                var PaymentStructure = srv.FetchDropDownList("RD_SAP_Scheme");
                var TraineeSupportItems = srv.FetchDropDownList("RD_SSPTraineeSupportItems");
                var PlaningType = srv.FetchDropDownList("RD_SSPPlaningType");
                var SelectionMethods = srv.FetchDropDownList("RD_SSPSelectionMethods");

                var ProgramCategory = srv.FetchDropDownList("RD_ProgramCategory");
                var FundingCategory = srv.FetchDropDownList("RD_FundingCategory");
                var BusinessRuleType = srv.FetchDropDownList("RD_BusinessRuleType");

                var programData = srv.FetchProgramDesign(false);
                string[] Attachments = { "ApprovalAttachment", "AttachmentCriteria", "AttachmentTORs" };
                var ProgramDesign = srv.LoopinData(programData, Attachments);

                var Object = new {
                    Province= Province,
                    Cluster=Cluster,
                    District=District,
                    ProgramDesign= ProgramDesign,
                    GetFinancialYear= GetFinancialYear,
                    GetProgramType= GetProgramType,
                    FundingSource=FundingSource,
                    Gender=Gender,
                    EducationTypes= EducationTypes,
                    PaymentStructure= PaymentStructure,
                    TraineeSupportItems= TraineeSupportItems,
                    PlaningType=PlaningType,
                    SelectionMethods=SelectionMethods,
                    ProgramCategory=ProgramCategory,
                    FundingCategory=FundingCategory,
                    BusinessRuleType=BusinessRuleType,


                };
                return Ok(Object);  
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        // POST: ProgramDesign/Save
        [HttpPost]
        [Route("SaveProgramDesign")]
        public IActionResult SaveProgramDesign(ProgramDesignModel model)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    model.UserID = Convert.ToInt32(User.Identity.Name);
                    srv.SaveProgramDesign(model);
                    var programData = srv.FetchProgramDesign(false);
                    string[] Attachments = { "ApprovalAttachment", "AttachmentCriteria", "AttachmentTORs" };
                    var ProgramDesign = srv.LoopinData(programData, Attachments);
                    return Ok(ProgramDesign);

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }


        [HttpPost]
        [Route("LoadData")]
        public IActionResult LoadData(ModelBase User)
        {
            try
            {


                var programDesign = srv.FetchDropDownList("RD_SSPProgramDesignForTradeLot");
                var trade = srv.FetchDropDownList("RD_SSPTradeForTradeLot");
                var tradeLayer = srv.FetchDropDownList("RD_SSPTradeLayerForTradeLot");
                var programFocus = srv.FetchDropDownList("RD_ProgramFocus");
                var tradeWiseTarget = srv.FetchDropDownList("RD_SSPTradeWiseTarget");
                var lotWiseTarget = srv.FetchDropDownList("RD_SSPLotWiseTarget");
                var programWiseBudget = srv.FetchDropDownList("RD_SSPProgramDesignSummary");
                var programBudget = srv.FetchDropDownList("RD_SSPProgramBudget");
                var gender = srv.FetchDropDownList("RD_Gender");

                var province = srvProvinces.FetchProvince(false);
                var cluster = srvCluster.FetchCluster(false);
                var district = srvDistrict.FetchDistrict(false);
                var tehsil = srvTehsil.FetchTehsil(false);


                // Construct the object with the fetched data
                var data = new
                {

                    programDesign = programDesign,
                    trade = trade,
                    tradeLayer = tradeLayer,
                    province = province,
                    cluster = cluster,
                    district = district,
                    tehsil = tehsil,
                    programFocus = programFocus,
                    programWiseBudget = programWiseBudget,
                    tradeWiseTarget= tradeWiseTarget,
                    lotWiseTarget = lotWiseTarget,
                    programBudget = programBudget,
                    gender= gender
                };

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
         [HttpPost]
        [Route("LoadSchemeData")]
        public IActionResult LoadSchemeData(ModelBase User)
        {
            try
            {


                var programDesignSummary = srv.FetchDropDownList("RD_SSPProgramDesignSummary");
                var tradeWiseTarget = srv.FetchDropDownList("RD_SSPTradeWiseTarget");
                var lotWiseTarget = srv.FetchDropDownList("RD_SSPLotWiseTarget");
                var data = new
                {
                    programDesignSummary = programDesignSummary,
                    tradeWiseTarget = tradeWiseTarget,
                    lotWiseTarget = lotWiseTarget,
                };

                return Ok(data);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }  
        
      

        [HttpGet]
        [Route("LoadAnalysisReport/{filter}")]
        public IActionResult LoadAnalysisReport([FromQuery(Name = "filter")] int[] filter)
        {
            try
            {
                return Ok(srv.FetchAnalysisReportFilters(filter));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("LoadAnalysisReport")]
        public IActionResult LoadAnalysisReport()
        {
            try
            {
                return Ok(srv.FetchAnalysisReport());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("SaveTradeDesign")]
        public IActionResult SaveTradeDesign(TradeLotDesignModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");

            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveTradeDesign(data));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        } 
        
        [HttpPost]
        [Route("UpdateSchemeInitialization")]
        public IActionResult UpdateSchemeInitialization(ProgramDesignModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.UpdateSchemeInitialization(data));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpPost]
        [Route("FetchCTMTradeWise")]
        public IActionResult FetchCTMTradeWise(CTMCalculationModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {

                    
                    return Ok(srv.FetchCTMTradeWise(data));


                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
        [HttpPost]
        [Route("FetchCTMBulkReport")]
        public IActionResult FetchCTMBulkReport(CTMCalculationModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {

                    
                    return Ok(srv.FetchCTMBulkReport(data));


                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }

        [HttpPost]
        [Route("LoadCTMReport")]
        public  IActionResult LoadCTMReport(ModelBase User)
        {
            try
            {


                //var CTMCalculationReport = srv.FetchDropDownList("CTMCalculationReport");
                var FundingSource = srv.FetchDropDownList("RD_FundingSource");
                var FundingCategory = srv.FetchDropDownList("RD_FundingCategory");
                var ProgramType = srv.FetchDropDownList("RD_ProgramType");
                var Sector = srv.FetchDropDownList("RD_Sector");
                var Trade = srv.FetchDropDownList("RD_Trade");
                var Duration = srv.FetchDropDownList("RD_Duration");
                var Province = srv.FetchDropDownList("RD_Province");
                var Cluster = srv.FetchDropDownList("RD_Cluster");
                var District = srv.FetchDropDownList("RD_District");
               
                var data = new
                {
                    //CTMCalculationReport=CTMCalculationReport,
                    FundingSource=FundingSource,
                    FundingCategory=FundingCategory,
                    ProgramType=ProgramType,
                    Sector=Sector,
                    Trade=Trade,
                    Duration=Duration,
                    Province=Province,
                    Cluster=Cluster,
                    District=District,
                };

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("LoadHistoricalReport")]
        public  IActionResult LoadHistoricalReport(ModelBase User)
        {
            try
            {


                //var HistoricalReport = srv.FetchDropDownList("HistoricalReport");
                var FundingSource = srv.FetchDropDownList("RD_FundingSource");
                var ProgramFocus = srv.FetchDropDownList("RD_ProgramFocus");
                var Sector = srv.FetchDropDownList("RD_Sector");
                var SubSector = srv.FetchDropDownList("RD_SubSector");
                var Trade = srv.FetchDropDownList("RD_Trade");
                var TSPMaster = srv.FetchDropDownList("RD_TSPMaster");
                var Province = srv.FetchDropDownList("RD_Province");
                var Cluster = srv.FetchDropDownList("RD_Cluster");
                var District = srv.FetchDropDownList("RD_District");
                var ProgramType = srv.FetchDropDownList("RD_ProgramType");

        
                var data = new
                {
                    //HistoricalReport = HistoricalReport,
                    ProgramType = ProgramType,
                    FundingSource=FundingSource,
                    ProgramFocus = ProgramFocus,
                    Sector=Sector,
                    SubSector = SubSector,
                    Trade=Trade,
                    TSPMaster = TSPMaster,
                    Province=Province,
                    Cluster=Cluster,
                    District=District,
                };

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
        [HttpPost]
        [Route("LoadHistoricalReportByFilter")]
        public IActionResult LoadHistoricalReportByFilter(HistoricalReportModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    //return Ok(data);

                    return Ok(srv.FetchHistoryReport(data));


                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }


        [HttpPost]
        [Route("SaveProgramWorkflowHistory")]
        public IActionResult SaveProgramWorkflowHistory(ProgramWorkflowHistoryModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveProgramWorkflowHistory(data));

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
        [HttpPost]
        [Route("SaveProgramStatusHistory")]
        public IActionResult SaveProgramStatusHistory(ProgramStatusHistoryModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveProgramStatusHistory(data));

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }
        [HttpPost]
        [Route("SaveProgramCriteriaHistory")]
        public IActionResult SaveProgramCriteriaHistory(ProgramCriteriaHistoryModel data)
        {
            string[] Split = HttpContext.Request.Path.Value.Split("/");
            bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
            if (IsAuthrized == true)
            {
                try
                {
                    data.CurUserID = Convert.ToInt32(User.Identity.Name);
                    return Ok(srv.SaveProgramCriteriaHistory(data));

                }
                catch (Exception e)
                {
                    return BadRequest(e.Message.ToString());
                }
            }
            else
            {
                return BadRequest("Access Denied. you are not authorized for this activity");
            }
        }


        //  [HttpPost]
        //[Route("GetCriteriaSubCategory")]
        //public IActionResult GetCriteriaSubCategory(ModelBase data)
        //{
        //    string[] Split = HttpContext.Request.Path.Value.Split("/");
        //    bool IsAuthrized = Authorize.CheckAuthorize(false, Convert.ToInt32(User.Identity.Name), Split[2], Split[3]);
        //    if (IsAuthrized == true)
        //    {
        //        try
        //        {
        //            data.CurUserID = Convert.ToInt32(User.Identity.Name);


        //            var CriteriaSubCategory = srv.FetchDropDownList("RD_CriteriaSubCategory");

        //            string[] Attachments = { "Attachment" };
        //            var CriteriaSubCategoryWithAttachment = srv.LoopinData(CriteriaSubCategory, Attachments);

        //            return Ok(CriteriaSubCategoryWithAttachment);

        //        }
        //        catch (Exception e)
        //        {
        //            return BadRequest(e.Message.ToString());
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("Access Denied. you are not authorized for this activity");
        //    }
        //}

    }
}
