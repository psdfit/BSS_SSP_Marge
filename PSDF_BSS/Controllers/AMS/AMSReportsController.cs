using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;

namespace PSDF_BSS.AMS.Controllers
{
    
    [ Controller]
    [Route("ams/[controller]")]
    [AllowAnonymous]
    public class AMSReportsController : Controller
    {
        private readonly ISRVAMSReportService _srvAMSReportService;

        private readonly ISRVRTP _srvRTP;
        public AMSReportsController(ISRVAMSReportService srvAMSReportService, ISRVRTP srvRTP)
        {
            _srvAMSReportService = srvAMSReportService;
            _srvRTP = srvRTP;
        }
        [Route("FormIII")]
        public async Task<IActionResult> FormIII(int? schemeId, int? tspId, int? classId, string date)
        {

            var model = await _srvAMSReportService.GetAMSFormThreeReport(schemeId, tspId, classId, date);
            if(model!=null)
                return new ViewAsPdf("FormIII", model);
            else
                return NotFound();
        }
        [Route("CenterInspection")]
        public async Task<IActionResult> CenterInspection(int? classId)
        {
            CenterInspectionReportModel model = new CenterInspectionReportModel();
            model.CenterInspectionModelList = _srvRTP.GetCenterInspection((int)classId);
            model.CenterInspectionDataModelList = _srvRTP.GetCenterInspectionData((int)classId);
            model.CenterInspectionDataModelList1 = _srvRTP.GetCenterInspectionDataSecurity((int)classId);
            model.CenterInspectionDataModelList2 = _srvRTP.GetCenterInspectionDataIntegrity((int)classId);
            model.CenterInspectionDataModelList3 = _srvRTP.GetCenterInspectionDataIncharge((int)classId);
            model.CenterInspectionAdditionalComplianceaModelList = _srvRTP.GetCenterInspectionAdditionalCompliance((int)classId);
            model.CenterInspectionTradeDetailModelList = _srvRTP.GetCenterInspectionTradeDetail((int)classId);
            model.CenterInspectionClassDetailModelList = _srvRTP.GetCenterInspectionClassDetail((int)classId);
            model.CenterInspectionNecessaryFacilitiesModelList = _srvRTP.GetCenterInspectionNecessaryFacilities((int)classId);
            model.CenterInspectionTradeToolsModelList = _srvRTP.GetCenterInspectionTradeTools((int)classId);
            if (model != null)
                return new ViewAsPdf("CenterInspection", model);
            else
                return NotFound();
        }
        [Route("UnverifiedTrainee")]
        public async Task<IActionResult> UnverifiedTrainee(int? schemeId, int? tspId, int? classId, string date)
        {
            var model = await _srvAMSReportService.GetUnverifiedTraineeReport(schemeId, tspId, classId, date);
            if (model != null)
                return new ViewAsPdf("UnverifiedTrainee", model);
            else
                return NotFound();
        }
        [Route("FormIV")]
        public async Task<IActionResult> FormIV(int? schemeId, int? tspId, int? classId, string date)
        {

            var model = await _srvAMSReportService.GetFormFourReport(schemeId, tspId, classId, date);
            if (model != null)
                return new ViewAsPdf("FormIV", model);
            else
                return NotFound();
        }
        [Route("ProfileVerification")]
        public async Task<IActionResult> GetProfileVerificationReport(int? schemeId, int? tspId, int? classId, string date)
        {

            var model = await _srvAMSReportService.GetProfileVerificationReport(schemeId, tspId, classId, date);
            if (model != null)
                return new ViewAsPdf("ProfileVerification", model);
            else
                return NotFound();
        }
    }
}