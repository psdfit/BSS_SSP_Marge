using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PSDF_AMSReports.Interfaces;
using PSDF_AMSReports.Models;
using Rotativa.AspNetCore;
using DataLayer.Interfaces;

namespace PSDF_AMSReports.Controllers
{
    public class AMSReportsController : Controller
    {
        private readonly IAMSReportService _srvAMSReportService;
        private readonly ISRVRTP _srvRTP;
        public AMSReportsController(IAMSReportService srvAMSReportService, ISRVRTP srvRTP)
        {
            _srvAMSReportService = srvAMSReportService;
            _srvRTP = srvRTP;
        }
        public async Task<IActionResult> Index(int? schemeId, int? tspId, int? classId, string dateTime, string reportName)
        {
            return reportName switch
            {
                "FormIII" => RedirectToAction("FormIII", new { schemeId, tspId, classId, dateTime }),
                "Center Inspection" => RedirectToAction("CenterInspection", "AMSReports", new { classId }),
                "Unverified Trainee" => RedirectToAction("UnverifiedTrainee", "AMSReports", new { schemeId, tspId, classId, dateTime }),
                _ => View("~/Shared/Error"),
            };
        }
        public async Task<IActionResult> FormIII(int? schemeId, int? tspId, int? classId, string dateTime)
        {

            var model = await _srvAMSReportService.GetAMSFormThreeReport(schemeId, tspId, classId, dateTime);
            //return View("FormIII", model);
            return new ViewAsPdf("FormIII", model);
        }
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

            //return View("CenterInspection", model);
            return new ViewAsPdf("CenterInspection", model);
        }

        public async Task<IActionResult> UnverifiedTrainee(int? schemeId, int? tspId, int? classId, string dateTime)
        {
            var model = await _srvAMSReportService.GetUnverifiedTraineeReport(schemeId, tspId, classId, dateTime);
            //return View("UnverifiedTraineeReport");
            //return View("UnverifiedTraineeReport", model);
            return new ViewAsPdf("UnverifiedTrainee", model);
        }
    }
}