using Microsoft.Reporting.WebForms;
using PSDF_BSS.Reports.Models;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using DataLayer;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using System.Data.SqlClient;
using System.Data;
using PSDF_BSS.Reports.DataAccess;

namespace PSDF_BSS.Reports.Controllers
{
     public class TraineeProfileController : Controller
    {
        #region Trainee Profile
        [HttpGet]
        public JsonResult GetRegistrationReport(string id)
        {

            //byte[] bytes = null;
            string strMimeType = "";
            string strEncoding = "";
            string strExtension = "";
            string[] strStreams = null;
            Warning[] warnings = null;
            var data = new DataService().TraineeProfileRegistration(0, 0, 0, id);
            ReportDataSource rptDataSource = new ReportDataSource("TraineeProfileDataset", data);
            ReportViewer rptViewer = new ReportViewer();
            rptViewer.ProcessingMode = ProcessingMode.Local;
            rptViewer.LocalReport.ReportPath = Server.MapPath("/RDLC/Reports/TraineeProfileReport.rdlc");
            rptViewer.LocalReport.DisplayName = "Trainee Profile Registration";
            rptViewer.LocalReport.EnableExternalImages = true;
            rptViewer.LocalReport.DataSources.Add(rptDataSource);
            ReportResponse rptResponse = new ReportResponse();
            rptResponse.Response = Convert.ToBase64String(rptViewer.LocalReport.Render("pdf", null, out strMimeType, out strEncoding, out strExtension, out strStreams, out warnings));
            rptResponse.FileName = "TraineeProfile_Report";
            rptResponse.MimeType = strMimeType;
            var jsonResult = Json(rptResponse, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(rptResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion Trainee Profile
    }
}
