using CrystalDecisions.CrystalReports.Engine;
using PSDF_BSS.Reports.Models;
using System.Runtime.Caching;
using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace PSDF_BSS.Reports
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string key = ClientQueryString.ToString();
                var cache = MemoryCache.Default;
                var rptReportName = cache.Get(key + "_rptName");
                var rptSource = cache.Get(key + "_rptSource");
                var rptParamters = cache.Get(key + "_rptParam");
                ParameterModel pm = (ParameterModel)rptParamters;
                if (!string.IsNullOrEmpty((string)rptReportName) && rptSource != null) // Checking is Report name and Data source provided or not
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), rptReportName.ToString()));
                    rd.SetDataSource(rptSource);

                    if(key == "NewTradesTSPIndustryQuarterly")
                    {
                        var rptSource1 = cache.Get(key + "_rptSource1"); ;
                        var rptSource2 = cache.Get(key + "_rptSource2"); ;
                        rd.Subreports[0].SetDataSource(rptSource1);
                        rd.Subreports[1].SetDataSource(rptSource2);
                    }

                    if (pm != null)
                    {
                        foreach (var obj in pm)
                        {
                            if (obj.Value != null)
                                rd.SetParameterValue(obj.Key, obj.Value);
                        }
                    }

                    CrystalReportViewer1.ReportSource = rd;
                    CrystalReportViewer1.DataBind();
                }
                else
                {
                    Response.Write("<H3>Data Nothing Found;</H3>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
    }
}