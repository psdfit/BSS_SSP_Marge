//using System;
//using System.Collections.Generic;
//using System.Linq;
//using DataLayer.Models;
//using System.Text;
//using System.Data.SqlClient;
//using System.Data;
//using System.Data.Common;
////using Genie.PSDF.DTOs;
////using Genie.PSDF.Common;
////using Genie.PSDF.DTOs.Commons;
////using Genie.PSDF.DTOs.Security;
//using System.Diagnostics;
//using System.Globalization;
//using System.IO;

//namespace DataLayer.Services
//{
//    public partial class SRVTPMReports
//    {
//        public string ROOT_PATH = AppDomain.CurrentDomain.BaseDirectory;
//        //public const string IMAGES_PATH = @"G:\mis\AMSDataService\Uploads\";  // for localhost
//        //public string IMAGES_PATH = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"..\..\AMSDataService\Uploads\");  // for localhost
//        //public string IMAGES_PATH = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"..\misapi_test\Uploads\"); // for mis/test/
//        public string IMAGES_PATH = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"misapi\Uploads\"); // for mis/

//        public const string NA = "N/A";
//        public const string CNST_COST_SHARING_SCHEME = @"8a078782-4936-4a93-8cbf-d7a07a2300e6";
//        public Dictionary<string, string> yesNoValues = new Dictionary<string, string>()
//            {
//                {"y", "Yes"},
//                {"n", "No"},
//                {"na", "N/A"},
//                {"", "N/A"}
//            };

//        public string schNA = "na";
//        public string schFormal = "formal";
//        public string schCommunity = "community";
//        public string schIndustrial = "industrial";

//        /************************Develop by Ali Haider ********************************************************/
//        /************************ TSP Wise Scheme Class Violation Summary Report ******************************/

//        public DataTable LoadTSPWiseSchemeViolationSummaryReport(string SchemeID, string month, string Tsp)
//        {
//            DbCommand objDbCommand = null;
//            DbCommand objDbSubCommand = null;
//            IDataReader objDataReader = null;
//            IDataReader objDataSubReader = null;

//            //string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");
//            string StartDate = Convert.ToDateTime(month).ToString("yyyy/MM/dd");

//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", StartDate);

//            try
//            {

//                // Fetch scheme type of selected scheme
//                string vioTypeCol = string.Empty;
//                string schemeType = string.Empty;
//                Dictionary<string, string> AMSSchemeType = new Dictionary<string, string>()
//                {
//                    {PSDFConstants.CNST_SchemeType_NA, "na"},
//                    {PSDFConstants.CNST_SchemeType_Formal, "formal"},
//                    {PSDFConstants.CNST_SchemeType_Community, "community"},
//                    {PSDFConstants.CNST_SchemeType_Industrial, "industrial"}
//                };

//                string strSQL = string.Format(@"SELECT SchemeType FROM Scheme WHERE SchemeID = '{0}'", SchemeID);
//                objDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objDataReader = TMData.ExecuteReader(objDbCommand);
//                while (objDataReader.Read())
//                {
//                    schemeType = objDataReader["SchemeType"].ToString();
//                }
//                vioTypeCol = getVioTypeColumnNameBySchemeType(AMSSchemeType[schemeType]);

//                strSQL = string.Format(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS 'Sr.No', userinfo.DisplayName as 'TSP',
//                                    scheme.Code + '-' + tsp.Code + '-' + ClassCode as 'ClassCode', 
//                                    sum(case when (vio.{0} = 'minor' and IsViolation = 'y') then 1 else 0 end) Minor,
//                                    sum(case when (vio.{0} = 'major' and IsViolation = 'y') then 1 else 0 end) Major,
//	                                sum(case when (vio.{0} = 'serious' and IsViolation = 'y') then 1 else 0 end) Serious,
//	                                count(ClassID) Total, 
//	                                sum(case when (vio.{0} = 'observation' and IsViolation = 'y') then 1 else 0 end) Observation,
//                                    ClassID,
//                                    tsp.code as TspCode
//                                  FROM dbo.AMSClassViolations cl_vio
//                                  inner join dbo.AMSViolations vio on vio.ID = cl_vio.ViolationID
//                                  inner join AMSClassMonitoring as cl_mr on cl_mr.ID = cl_vio.ClassMonitoringID
//                                  inner join dbo.class class on class.ID = cl_vio.ClassID
//                                  inner join dbo.UserInfo userinfo on userinfo.ID = class.SpID
//                                  inner join dbo.ServiceProvider tsp on tsp.ID = class.SpID
//                                  inner join dbo.Scheme scheme on scheme.SchemeID = class.SchemeID
//                                  where ClassID in (select ID from class where SchemeID = '{1}')
//                                   And userinfo.ID = '{3}'
//                                  and vio.{0} in ('major', 'minor', 'serious', 'observation') 
//                                  {2} group by class.ClassCode, DisplayName, scheme.Code, tsp.Code, ClassID order by TspCode", vioTypeCol, SchemeID, DateClause, Tsp);

//                strSQL = strSQL.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSQL);

//                objDataReader = TMData.ExecuteReader(objDbCommand);
//                int index = 0;

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Minor Violations");
//                dt.Columns.Add("Major Violations");
//                dt.Columns.Add("Serious Violations");
//                dt.Columns.Add("Total Violations");
//                dt.Columns.Add("Observations Total");
//                dt.Columns.Add("Remarks");

//                //Dictionary<string, int> tspMajorVio = new Dictionary<string, int>();

//                int tspMajorVio = 0;
//                int tspMinorVio = 0;
//                int tspSeriousVio = 0;
//                int tspTotalVio = 0;
//                int tspObservVio = 0;

//                string tspName = string.Empty;

//                DataTable dtTemp = new DataTable();
//                dtTemp.Load(TMData.ExecuteReader(objDbCommand));
//                int rowsCount = dtTemp.Rows.Count;


//                Dictionary<string, string> ViolationsDescription = getVioDescription();
//                int GrandMajorCount = 0;
//                int GrandMinorCount = 0;
//                int GrandSeriousCount = 0;
//                int GrandTotalCount = 0;
//                int GrandObservCount = 0;
//                DateClause = string.Format(" AND month(VisitDateTime) = month('{0}') AND year(VisitDateTime ) = year('{0}')", StartDate);


//                while (objDataReader.Read())
//                {

//                    int majorCount = (int)objDataReader["Major"];
//                    int minorCount = (int)objDataReader["Minor"];
//                    int seriousCount = (int)objDataReader["Serious"];
//                    int totalViolations = majorCount + minorCount + seriousCount;
//                    int observationCount = (int)objDataReader["Observation"];

//                    GrandMajorCount += majorCount;
//                    GrandMinorCount += minorCount;
//                    GrandSeriousCount += seriousCount;
//                    GrandTotalCount += totalViolations;
//                    GrandObservCount += observationCount;


//                    string classID = objDataReader["ClassID"].ToString();
//                    string subStrSQL = string.Format(@"SELECT  
//                            ViolationID, vio.{0} as Type, vio.Description
//                            FROM dbo.AMSClassViolations cl_vio
//                            inner join dbo.AMSViolations vio
//                            on vio.ID = cl_vio.ViolationID
//                            where ClassID = '{1}'
//                            and IsViolation = 'y' and vio.{0} in ('major', 'minor', 'serious', 'observation')
//                            {2} order by ClassMonitoringID, ViolationID, ClassID", vioTypeCol, classID, DateClause);

//                    objDbSubCommand = TMData.GetSqlStringCommand(subStrSQL);
//                    objDataSubReader = TMData.ExecuteReader(objDbSubCommand);

//                    string majorRemarks = string.Empty;
//                    string minorRemarks = string.Empty;
//                    string seriousRemarks = string.Empty;
//                    string obsevRemarks = string.Empty;


//                    List<string> marjorRemarks_ = new List<string>();
//                    List<string> minorRemarks_ = new List<string>();
//                    List<string> seriousRemarks_ = new List<string>();
//                    List<string> obsevRemarks_ = new List<string>();


//                    while (objDataSubReader.Read())
//                    {
//                        string violationID = objDataSubReader["ViolationID"].ToString();
//                        string violationType = objDataSubReader["Type"].ToString();
//                        string vioDesp = ViolationsDescription.ContainsKey(violationID) ? ViolationsDescription[violationID] : "";

//                        /*Violations will be shown as Observations if Scheme is Cost Sharing 2016*/
//                        if (SchemeID == CNST_COST_SHARING_SCHEME)
//                        {

//                            if (obsevRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                            {
//                                obsevRemarks_.Add(vioDesp);
//                            }
//                            observationCount += totalViolations;
//                            majorCount = 0;
//                            minorCount = 0;
//                            seriousCount = 0;
//                            totalViolations = 0;
//                        }
//                        else
//                        {

//                            if (violationType == "major" && majorCount > 0)
//                            {
//                                if (marjorRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    marjorRemarks_.Add(vioDesp);
//                                }
//                            }
//                            else if (violationType == "minor" && minorCount > 0)
//                            {
//                                if (minorRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    minorRemarks_.Add(vioDesp);
//                                }
//                            }
//                            else if (violationType == "serious" && seriousCount > 0)
//                            {
//                                if (seriousRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    seriousRemarks_.Add(vioDesp);
//                                }
//                            }
//                            else if (violationType == "observation" && observationCount > 0)
//                            {
//                                if (obsevRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    obsevRemarks_.Add(vioDesp);
//                                }
//                            }
//                        }
//                    }


//                    if (objDataSubReader != null)
//                    {
//                        if (!objDataSubReader.IsClosed)
//                            objDataSubReader.Close();

//                        objDataSubReader.Dispose();
//                        objDataSubReader = null;
//                    }

//                    if (objDbSubCommand != null)
//                    {
//                        objDbSubCommand.Dispose();
//                        objDbSubCommand = null;
//                    }

//                    string remarks = string.Empty;
//                    if (majorCount > 0)
//                    {
//                        majorRemarks = "Major: " + string.Join(", ", marjorRemarks_.ToArray());
//                        remarks += majorRemarks + "\n";
//                    }
//                    if (minorCount > 0)
//                    {
//                        minorRemarks = "Minor: " + string.Join(", ", minorRemarks_.ToArray());
//                        remarks += minorRemarks + "\n";
//                    }
//                    if (seriousCount > 0)
//                    {
//                        seriousRemarks = "Serious: " + string.Join(", ", seriousRemarks_.ToArray());
//                        remarks += seriousRemarks + "\n";
//                    }
//                    if (observationCount > 0)
//                    {
//                        obsevRemarks = "Observation: " + string.Join(", ", obsevRemarks_.ToArray());
//                        remarks += obsevRemarks;
//                    }

//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = (++index).ToString();
//                    _row["Service Provider Name"] = objDataReader["TSP"].ToString();
//                    _row["Class Code"] = objDataReader["ClassCode"].ToString();
//                    _row["Major Violations"] = majorCount;
//                    _row["Minor Violations"] = minorCount;
//                    _row["Serious Violations"] = seriousCount;
//                    _row["Total Violations"] = totalViolations;
//                    _row["Observations Total"] = observationCount;
//                    _row["Remarks"] = remarks;

//                    if (String.IsNullOrEmpty(tspName))
//                    {
//                        tspName = objDataReader["TSP"].ToString();
//                    }

//                    if (objDataReader["TSP"].ToString() == tspName)
//                    {
//                        tspMajorVio += majorCount;
//                        tspMinorVio += minorCount;
//                        tspSeriousVio += seriousCount;
//                        tspTotalVio += majorCount + minorCount + seriousCount;
//                        tspObservVio += observationCount;

//                    }
//                    else
//                    {
//                        /*Violations will be shown as Observations if Scheme is Cost Sharing 2016*/
//                        if (SchemeID == CNST_COST_SHARING_SCHEME)
//                        {
//                            tspObservVio = tspObservVio + tspTotalVio;
//                            tspTotalVio = 0;
//                            tspMajorVio = 0;
//                            tspMinorVio = 0;
//                            tspSeriousVio = 0;
//                        }

//                        DataRow _rowTotal = dt.NewRow();
//                        _rowTotal["Sr No"] = string.Empty;
//                        _rowTotal["Service Provider Name"] = tspName + " Total";
//                        _rowTotal["Class Code"] = string.Empty;
//                        _rowTotal["Major Violations"] = tspMajorVio;
//                        _rowTotal["Minor Violations"] = tspMinorVio;
//                        _rowTotal["Serious Violations"] = tspSeriousVio;
//                        _rowTotal["Total Violations"] = tspTotalVio;
//                        _rowTotal["Observations Total"] = tspObservVio;

//                        tspMajorVio = majorCount;
//                        tspMinorVio = minorCount;
//                        tspSeriousVio = seriousCount;
//                        tspTotalVio = totalViolations;
//                        tspObservVio = observationCount;

//                        tspName = objDataReader["TSP"].ToString();
//                        dt.Rows.Add(_rowTotal);
//                    }


//                    dt.Rows.Add(_row);

//                    if (index == rowsCount)
//                    {
//                        /*Violations will be shown as Observations if Scheme is Cost Sharing 2016*/
//                        if (SchemeID == CNST_COST_SHARING_SCHEME)
//                        {
//                            tspObservVio = tspObservVio + tspTotalVio;
//                            tspTotalVio = 0;
//                            tspMajorVio = 0;
//                            tspMinorVio = 0;
//                            tspSeriousVio = 0;


//                            GrandObservCount += GrandTotalCount;
//                            GrandMajorCount = 0;
//                            GrandMinorCount = 0;
//                            GrandTotalCount = 0;
//                            GrandSeriousCount = 0;
//                        }

//                        DataRow _rowTotal = dt.NewRow();
//                        _rowTotal["Sr No"] = string.Empty;
//                        _rowTotal["Service Provider Name"] = tspName + " Total";
//                        _rowTotal["Class Code"] = string.Empty;
//                        _rowTotal["Major Violations"] = tspMajorVio;
//                        _rowTotal["Minor Violations"] = tspMinorVio;
//                        _rowTotal["Serious Violations"] = tspSeriousVio;
//                        _rowTotal["Total Violations"] = tspTotalVio;
//                        _rowTotal["Observations Total"] = tspObservVio;

//                        tspMajorVio = majorCount;
//                        tspMinorVio = minorCount;
//                        tspSeriousVio = seriousCount;
//                        tspTotalVio = totalViolations;
//                        tspObservVio = observationCount;

//                        tspName = objDataReader["TSP"].ToString();
//                        dt.Rows.Add(_rowTotal);

//                        DataRow _rowGrandTotal = dt.NewRow();
//                        _rowGrandTotal["Sr No"] = string.Empty;
//                        _rowGrandTotal["Service Provider Name"] = "Grand Total";
//                        _rowGrandTotal["Class Code"] = string.Empty;
//                        _rowGrandTotal["Major Violations"] = GrandMajorCount;
//                        _rowGrandTotal["Minor Violations"] = GrandMinorCount;
//                        _rowGrandTotal["Serious Violations"] = GrandSeriousCount;
//                        _rowGrandTotal["Total Violations"] = GrandTotalCount;
//                        _rowGrandTotal["Observations Total"] = GrandObservCount;

//                        dt.Rows.Add(_rowGrandTotal);
//                    }
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objDataReader != null)
//                {
//                    if (!objDataReader.IsClosed)
//                        objDataReader.Close();

//                    objDataReader.Dispose();
//                    objDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;

//                }

//            }
//        }
//        /**************************TSP Wise Confirmed Marginal Trainee Report *************************************/
//        public DataTable LoadTSPWiseCMTraineeReport(string SchemeID, string month, string Tsp)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            DbCommand objMonitoringDbCommand = null;
//            IDataReader objMonitoringDataReader = null;

//            //DbCommand objTraineeDbCommand = null;
//            // IDataReader objTraineeDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string SelectedMonth = month.ToDateTime().ToString("MM");
//            string SelectedYear = month.ToDateTime().ToString("yyyy");

//            int SelectMonthInt = Convert.ToInt16(SelectedMonth);
//            int SelectedYearInt = Convert.ToInt16(SelectedYear);

//            string PrevMonth = (SelectMonthInt == 1) ? "12" : (SelectMonthInt - 1).ToString();
//            SelectedMonth = Convert.ToInt32(SelectedMonth).ToString();

//            int PrevMonthYear = (SelectedMonth == "1") ? (SelectedYearInt - 1) : SelectedYearInt;


//            string PrevMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(PrevMonth)) + " " + PrevMonthYear;
//            string SelectedMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(SelectedMonth)) + " " + SelectedYear;

//            string currentMonthStartDate = SelectedYear + "-" + SelectedMonth + "-01";
//            string previousMonthStartDate = PrevMonthYear + "-" + PrevMonth + "-01";

//            string lastDay = (DateTime.DaysInMonth(SelectedYearInt, SelectMonthInt)).ToString();
//            string PrevMonthStartDate = PrevMonthYear + "/" + PrevMonth + "/1";
//            string currentMonthEndDate = SelectedYear + "/" + SelectedMonth + "/" + lastDay;
//            string SchemeType = string.Empty;

//            string DateClause = string.Empty;
//            string strSQL = string.Empty;

//            strSQL = string.Format(@"SELECT SchemeType FROM Scheme WHERE SchemeID = '{0}'", SchemeID);
//            objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//            objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);

//            if (objRequestDataReader.Read())
//            {
//                SchemeType = objRequestDataReader["SchemeType"].ToString();
//            }

//            // DateClause = string.Format(" AND year(VisitDateTime ) = year('{0}')", StartDate);
//            DateClause = " AND VisitDateTime >= '" + PrevMonthStartDate + "' AND VisitDateTime <= '" + currentMonthEndDate + "'";

//            try
//            {
//                strSQL = @"select TSPName, SchemeName, Cir.ClassCode,Class.Contract_TrainingDuration ,ClassInspectionRequestID as CirId, ClMr.ID as ClMrId, TraineesImported as TraineesImported, month(VisitDateTime) as Month " +
//                                "from AMSClassMonitoring  ClMr " +

//                                "inner join AMSClassInspectionRequest Cir " +
//                                "on Cir.ID = ClMr.ClassInspectionRequestID " +

//                                "inner join Class " +
//                                "on Class.ID = Cir.ClassID " +
//                                "where ClassInspectionRequestID in " +
//                                "(select ID from AMSClassInspectionRequest where ClassID in " +
//                                    "(select ID from Class where SchemeID = '" + SchemeID + "'  And SpID = '" + Tsp + "') " +
//                                ")" + DateClause + " order by ClassInspectionRequestID";


//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);


//                Dictionary<string, Dictionary<string, List<string>>> Cir = new Dictionary<string, Dictionary<string, List<string>>>();



//                while (objRequestDataReader.Read())
//                {


//                    List<string> SchemeName = objRequestDataReader["SchemeName"].ToString().Split(',').ToList();
//                    List<string> TSPName = objRequestDataReader["TSPName"].ToString().Split(',').ToList();
//                    List<string> ClassCode = objRequestDataReader["ClassCode"].ToString().Split(',').ToList();
//                    List<string> Duration = objRequestDataReader["Contract_TrainingDuration"].ToString().Split(',').ToList();

//                    string RequestID = objRequestDataReader["CirId"].ToString();
//                    string ClMrId = objRequestDataReader["ClMrId"].ToString();
//                    string TraineesImported = objRequestDataReader["TraineesImported"].ToString();
//                    string Month = objRequestDataReader["Month"].ToString();

//                    if (Cir.ContainsKey(RequestID))
//                    {
//                        if (Month == SelectedMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                Cir[RequestID]["SelectedMonthTsr"].Add(ClMrId);
//                            }
//                            else
//                            {
//                                Cir[RequestID]["SelectedMonthNoTsr"].Add(ClMrId);
//                            }
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                Cir[RequestID]["PrevMonthTsr"].Add(ClMrId);
//                            }
//                            else
//                            {
//                                Cir[RequestID]["PrevMonthNoTsr"].Add(ClMrId);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        List<string> SelectedMonthTsr = new List<string>();
//                        List<string> SelectedMonthNoTsr = new List<string>();
//                        List<string> PrevMonthTsr = new List<string>();
//                        List<string> PrevMonthNoTsr = new List<string>();

//                        if (Month == SelectedMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                SelectedMonthTsr.Add(ClMrId);
//                            }
//                            else
//                            {
//                                SelectedMonthNoTsr.Add(ClMrId);
//                            }
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                PrevMonthTsr.Add(ClMrId);
//                            }
//                            else
//                            {
//                                PrevMonthNoTsr.Add(ClMrId);
//                            }
//                        }
//                        Dictionary<string, List<string>> CirIds = new Dictionary<string, List<string>>();

//                        CirIds.Add("SelectedMonthTsr", SelectedMonthTsr);
//                        CirIds.Add("SelectedMonthNoTsr", SelectedMonthNoTsr);
//                        CirIds.Add("PrevMonthTsr", PrevMonthTsr);
//                        CirIds.Add("PrevMonthNoTsr", PrevMonthNoTsr);

//                        CirIds.Add("SchemeName", SchemeName);
//                        CirIds.Add("TspName", TSPName);
//                        CirIds.Add("ClassCode", ClassCode);
//                        CirIds.Add("Duration", Duration);

//                        Cir.Add(RequestID, CirIds);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;
//                }

//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> TraineesList = getCMTraineeFromDb(SchemeType, Cir, SelectedMonthString, PrevMonthString, currentMonthStartDate, previousMonthStartDate);

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Course Duration (Months)");
//                //dt.Columns.Add("Trainee ID");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Father/Husband Name");
//                dt.Columns.Add("CNIC");
//                // dt.Columns.Add(SelectedMonthString + " - Visit2");
//                dt.Columns.Add("Confirmed Marginal " + SelectedMonthString);
//                //dt.Columns.Add(PrevMonthString + " - Visit1");
//                dt.Columns.Add("Confirmed Marginal " + PrevMonthString);
//                dt.Columns.Add("Remarks");

//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in TraineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Service Provider Name"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];
//                    _row["Course Duration (Months)"] = entry.Value["Duration"];
//                    //_row["Trainee ID"] = entry.Value["ID"];
//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["Father/Husband Name"] = entry.Value["FName"];
//                    _row["CNIC"] = entry.Value["CNIC"];

//                    //_row[SelectedMonthString + " - Visit2"] = entry.Value.ContainsKey(SelectedMonthString + " - Visit2") ? entry.Value[SelectedMonthString + " - Visit2"] : "";

//                    _row["Confirmed Marginal " + SelectedMonthString] = entry.Value.ContainsKey(SelectedMonthString) ? entry.Value[SelectedMonthString] : "";

//                    //_row[PrevMonthString + " - Visit1"] = entry.Value.ContainsKey(PrevMonthString + " - Visit1") ? entry.Value[PrevMonthString + " - Visit1"] : "";

//                    _row["Confirmed Marginal " + PrevMonthString] = entry.Value.ContainsKey(PrevMonthString) ? entry.Value[PrevMonthString] : "";
//                    if (SchemeType == PSDFConstants.CNST_SchemeType_NA)
//                    {
//                        _row["Remarks"] = "Marked as " + entry.Value["Remarks"].Replace("_", " ").Replace("marked", "");
//                    }
//                    else
//                    {
//                        _row["Remarks"] = entry.Value["Remarks"];
//                    }


//                    dt.Rows.Add(_row);
//                    index++;
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objMonitoringDataReader != null)
//                {
//                    if (!objMonitoringDataReader.IsClosed)
//                        objMonitoringDataReader.Close();

//                    objMonitoringDataReader.Dispose();
//                    objMonitoringDataReader = null;

//                }

//                if (objMonitoringDbCommand != null)
//                {
//                    objMonitoringDbCommand.Dispose();
//                    objMonitoringDbCommand = null;
//                }
//            }

//        }
//        /************************** Additional Trainee Report *************************************/
//        public DataTable LoadTSPWiseAdditionalTraineeReport(string SchemeID, string month, string Tsp)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string SelectedMonth = month.ToDateTime().ToString("MM");
//            string SelectedYear = month.ToDateTime().ToString("yyyy");

//            int SelectedMonthInt = Convert.ToInt32(SelectedMonth);
//            string PrevMonth = (SelectedMonthInt == 1) ? "12" : (SelectedMonthInt - 1).ToString();

//            int prevMonthYearInt = Convert.ToInt32(SelectedYear);
//            int PrevMonthYear = (SelectedMonthInt == 1) ? prevMonthYearInt - 1 : prevMonthYearInt;
//            SelectedMonth = Convert.ToInt32(SelectedMonth).ToString();

//            string PrevMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(PrevMonth)) + " " + PrevMonthYear;
//            string SelectedMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(SelectedMonth)) + " " + SelectedYear;

//            string lastDay = (DateTime.DaysInMonth(Convert.ToInt16(SelectedYear), SelectedMonthInt)).ToString();
//            string PrevMonthStartDate = PrevMonthYear + "/" + PrevMonth + "/1";
//            string currentMonthEndDate = SelectedYear + "/" + SelectedMonth + "/" + lastDay;

//            string DateClause = string.Empty;
//            //DateClause = string.Format(" AND year(VisitDateTime ) = year('{0}')", StartDate);
//            DateClause = " AND VisitDateTime >= '" + PrevMonthStartDate + "' AND VisitDateTime <= '" + currentMonthEndDate + "'";

//            try
//            {
//                string strSQL = @"select TSPName, SchemeName, Cir.ClassCode,Class.Contract_TrainingDuration ,ClassInspectionRequestID as CirId, ClMr.ID as ClMrId, TraineesImported as TraineesImported, month(VisitDateTime) as Month " +
//                                 "from AMSClassMonitoring  ClMr " +

//                                 "inner join AMSClassInspectionRequest Cir " +
//                                 "on Cir.ID = ClMr.ClassInspectionRequestID " +

//                                 "inner join Class " +
//                                 "on Class.ID = Cir.ClassID " +
//                                 "where ClassInspectionRequestID in " +
//                                 "(select ID from AMSClassInspectionRequest where ClassID in " +
//                                     "(select ID from Class where SchemeID = '" + SchemeID + "' And SpID = '" + Tsp + "') " +
//                                 ")" + DateClause + " order by ClassInspectionRequestID";


//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);


//                Dictionary<string, Dictionary<string, List<string>>> Cir = new Dictionary<string, Dictionary<string, List<string>>>();

//                while (objRequestDataReader.Read())
//                {

//                    List<string> SchemeName = objRequestDataReader["SchemeName"].ToString().Split(',').ToList();
//                    List<string> TSPName = objRequestDataReader["TSPName"].ToString().Split(',').ToList();
//                    List<string> ClassCode = objRequestDataReader["ClassCode"].ToString().Split(',').ToList();
//                    List<string> Duration = objRequestDataReader["Contract_TrainingDuration"].ToString().Split(',').ToList();

//                    string RequestID = objRequestDataReader["CirId"].ToString();
//                    string ClMrId = objRequestDataReader["ClMrId"].ToString();
//                    string Month = objRequestDataReader["Month"].ToString();

//                    if (Cir.ContainsKey(RequestID))
//                    {
//                        if (Month == SelectedMonth)
//                        {
//                            Cir[RequestID]["SelectedMonthVisitIDs"].Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            Cir[RequestID]["PrevMonthVisitIDs"].Add(ClMrId);
//                        }
//                    }
//                    else
//                    {
//                        List<string> SelectedMonthVisitIDs = new List<string>();
//                        List<string> PrevMonthVisitIDs = new List<string>();

//                        if (Month == SelectedMonth)
//                        {
//                            SelectedMonthVisitIDs.Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            PrevMonthVisitIDs.Add(ClMrId);
//                        }
//                        Dictionary<string, List<string>> CirIds = new Dictionary<string, List<string>>();

//                        CirIds.Add("SelectedMonthVisitIDs", SelectedMonthVisitIDs);
//                        CirIds.Add("PrevMonthVisitIDs", PrevMonthVisitIDs);

//                        CirIds.Add("SchemeName", SchemeName);
//                        CirIds.Add("TspName", TSPName);
//                        CirIds.Add("ClassCode", ClassCode);
//                        CirIds.Add("Duration", Duration);

//                        Cir.Add(RequestID, CirIds);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;
//                }

//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> TraineesList = getAdditionalTraineeFromDb(Cir, SelectedMonthString, PrevMonthString);


//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                // dt.Columns.Add("Course Duration (Months)");
//                //dt.Columns.Add("Trainee ID");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Father/Husband Name");
//                dt.Columns.Add("CNIC");

//                // dt.Columns.Add(SelectedMonthString + " - Visit2");

//                dt.Columns.Add(SelectedMonthString);

//                //dt.Columns.Add(PrevMonthString + " - Visit1");

//                dt.Columns.Add(PrevMonthString);
//                dt.Columns.Add("Remarks");



//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in TraineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Service Provider Name"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];
//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["Father/Husband Name"] = string.IsNullOrEmpty(entry.Value["FName"]) ? "Not Provided" : entry.Value["FName"];
//                    _row["CNIC"] = string.IsNullOrEmpty(entry.Value["CNIC"]) ? "Not Provided" : entry.Value["CNIC"];
//                    _row[SelectedMonthString] = entry.Value.ContainsKey(SelectedMonthString) ? entry.Value[SelectedMonthString] : "";
//                    _row[PrevMonthString] = entry.Value.ContainsKey(PrevMonthString) ? entry.Value[PrevMonthString] : "";
//                    _row["Remarks"] = "Additional";

//                    dt.Rows.Add(_row);
//                    index++;
//                }
//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {

//            }

//        }
//        /************************** Deleted Dropout Trainee Report *************************************/
//        public DataTable LoadTSPWiseDelDOTraineeReport(string SchemeID, string month, string Tsp)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string SelectedMonth = month.ToDateTime().ToString("MM");
//            string SelectedYear = month.ToDateTime().ToString("yyyy");

//            int SelectedMonthInt = Convert.ToInt32(SelectedMonth);
//            string PrevMonth = (SelectedMonthInt == 1) ? "12" : (SelectedMonthInt - 1).ToString();
//            SelectedMonth = Convert.ToInt32(SelectedMonth).ToString();

//            int SelectedYearInt = Convert.ToInt32(SelectedYear);
//            int PrevMonthYear = (SelectedMonthInt == 1) ? SelectedYearInt - 1 : SelectedYearInt;

//            string PrevMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(PrevMonth)) + " " + PrevMonthYear;
//            string SelectedMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(SelectedMonth)) + " " + SelectedYear;

//            string lastDay = (DateTime.DaysInMonth(SelectedYearInt, SelectedMonthInt)).ToString();
//            string PrevMonthStartDate = PrevMonthYear + "/" + PrevMonth + "/1";
//            string currentMonthEndDate = SelectedYear + "/" + SelectedMonth + "/" + lastDay;

//            string DateClause = string.Empty;
//            // DateClause = string.Format(" AND year(VisitDateTime ) = year('{0}')", StartDate);
//            DateClause = " AND VisitDateTime >= '" + PrevMonthStartDate + "' AND VisitDateTime <= '" + currentMonthEndDate + "'";


//            try
//            {

//                string strSQL = @"select TSPName, SchemeName, Cir.ClassCode,Class.Contract_TrainingDuration ,ClassInspectionRequestID as CirId, ClMr.ID as ClMrId, TraineesImported as TraineesImported, month(VisitDateTime) as Month " +
//                                 "from AMSClassMonitoring  ClMr " +

//                                 "inner join AMSClassInspectionRequest Cir " +
//                                 "on Cir.ID = ClMr.ClassInspectionRequestID " +

//                                 "inner join Class " +
//                                 "on Class.ID = Cir.ClassID " +
//                                 "where ClassInspectionRequestID in " +
//                                 "(select ID from AMSClassInspectionRequest where ClassID in " +
//                                     "(select ID from Class where SchemeID = '" + SchemeID + "' And SpID = '" + Tsp + "') " +
//                                 ")" + DateClause + " order by ClassInspectionRequestID, VisitNo DESC";


//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);

//                Dictionary<string, Dictionary<string, List<string>>> Cir = new Dictionary<string, Dictionary<string, List<string>>>();

//                while (objRequestDataReader.Read())
//                {

//                    List<string> SchemeName = objRequestDataReader["SchemeName"].ToString().Split(',').ToList();
//                    List<string> TSPName = objRequestDataReader["TSPName"].ToString().Split(',').ToList();
//                    List<string> ClassCode = objRequestDataReader["ClassCode"].ToString().Split(',').ToList();

//                    string RequestID = objRequestDataReader["CirId"].ToString();
//                    string ClMrId = objRequestDataReader["ClMrId"].ToString();
//                    string Month = objRequestDataReader["Month"].ToString();

//                    if (Cir.ContainsKey(RequestID))
//                    {
//                        if (Month == SelectedMonth)
//                        {
//                            Cir[RequestID]["SelectedMonthVisitIDs"].Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            Cir[RequestID]["PrevMonthVisitIDs"].Add(ClMrId);
//                        }
//                    }
//                    else
//                    {
//                        List<string> SelectedMonthVisitIDs = new List<string>();
//                        List<string> PrevMonthVisitIDs = new List<string>();

//                        if (Month == SelectedMonth)
//                        {
//                            SelectedMonthVisitIDs.Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            PrevMonthVisitIDs.Add(ClMrId);
//                        }
//                        Dictionary<string, List<string>> CirIds = new Dictionary<string, List<string>>();

//                        CirIds.Add("SelectedMonthVisitIDs", SelectedMonthVisitIDs);
//                        CirIds.Add("PrevMonthVisitIDs", PrevMonthVisitIDs);

//                        CirIds.Add("SchemeName", SchemeName);
//                        CirIds.Add("TspName", TSPName);
//                        CirIds.Add("ClassCode", ClassCode);

//                        Cir.Add(RequestID, CirIds);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;

//                }

//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> TraineesList = getDelDOTraineeFromDb(Cir, SelectedMonthString, PrevMonthString);


//                DataTable dt = new DataTable();

//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                // dt.Columns.Add("Course Duration (Months)");
//                //dt.Columns.Add("Trainee ID");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Father/Husband Name");
//                dt.Columns.Add("CNIC");

//                // dt.Columns.Add(SelectedMonthString + " - Visit2");

//                dt.Columns.Add(SelectedMonthString);

//                //dt.Columns.Add(PrevMonthString + " - Visit1");

//                dt.Columns.Add(PrevMonthString);
//                dt.Columns.Add("Remarks");

//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in TraineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Service Provider Name"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];

//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["Father/Husband Name"] = entry.Value["FName"];
//                    _row["CNIC"] = entry.Value["CNIC"];

//                    //_row[SelectedMonthString + " - Visit2"] = entry.Value.ContainsKey(SelectedMonthString + " - Visit2") ? entry.Value[SelectedMonthString + " - Visit2"] : "";

//                    _row[SelectedMonthString] = entry.Value.ContainsKey(SelectedMonthString) ? entry.Value[SelectedMonthString] : "";

//                    //_row[PrevMonthString + " - Visit1"] = entry.Value.ContainsKey(PrevMonthString + " - Visit1") ? entry.Value[PrevMonthString + " - Visit1"] : "";

//                    _row[PrevMonthString] = entry.Value.ContainsKey(PrevMonthString) ? entry.Value[PrevMonthString] : "";
//                    _row["Remarks"] = entry.Value["Remarks"];

//                    dt.Rows.Add(_row);
//                    index++;
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }

//        }
//        /************************** Unverified Trainee Report *************************************/
//        public DataTable LoadTSPWiseUnverifiedTraineeReport(string SchemeID, string month, string Tsp)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string SelectedMonth = month.ToDateTime().ToString("MM");
//            string SelectedYear = month.ToDateTime().ToString("yyyy");

//            int SelectedMonthInt = Convert.ToInt32(SelectedMonth);
//            string PrevMonth = (SelectedMonthInt == 1) ? "12" : (SelectedMonthInt - 1).ToString();
//            SelectedMonth = Convert.ToInt32(SelectedMonth).ToString();

//            int SelectedYearInt = Convert.ToInt32(SelectedYear);
//            int PrevMonthYear = (SelectedMonthInt == 1) ? SelectedYearInt - 1 : SelectedYearInt;

//            string PrevMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(PrevMonth)) + " " + PrevMonthYear;
//            string SelectedMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(SelectedMonth)) + " " + SelectedYear;

//            string lastDay = (DateTime.DaysInMonth(SelectedYearInt, SelectedMonthInt)).ToString();
//            string PrevMonthStartDate = PrevMonthYear + "/" + PrevMonth + "/1";
//            string currentMonthEndDate = SelectedYear + "/" + SelectedMonth + "/" + lastDay;

//            string DateClause = string.Empty;
//            // DateClause = string.Format(" AND year(VisitDateTime ) = year('{0}')", StartDate);
//            DateClause = " AND VisitDateTime >= '" + PrevMonthStartDate + "' AND VisitDateTime <= '" + currentMonthEndDate + "'";


//            try
//            {

//                string strSQL = @"SELECT Scheme.Name AS Scheme
//                                            , UserInfo_SP.DisplayName AS [Training Service Provider]
//                                            , TradeGroup.TradeGroupName AS [Trade Group]
//                                            , Trade.TradeName as [Trade]
//                                            , Scheme.Code + '-' + ServiceProvider.Code + '-' + Class.ClassCode As [Class Code] 
//                                            , CS.ClassStatus AS [Class Status] 
//                                            , TraineeProfile.TraineeID [Trainee ID]
//                                            , UserInfo.DisplayName AS [Trainee Name]
//                                            , UserInfo.FatherName [Father's Name]
//                                            , UserInfo.Cnic AS [CNIC]
//                                            , Convert(varchar,UserInfo.DateOfCNICIssuance, 103) [CNIC Issuance Date]
//                                            , Convert(varchar,UserInfo.Dob, 103) [Date Of Birth]
//                                            , TraineeProfile.RollNo AS [Roll #]
//                                            , Class.BatchID AS Batch
//                                            , Section.Name AS Section
//                                            , Shift.ShiftName AS Shift
//                                            , UserInfo.House + ' ' + UserInfo.Street + ' ' + UserInfo.Moza as [Trainee Address]
//                                            , Tehsilname.Name as [Residence Tehsil]
//                                            , TraineeDist.Name [District of Residence]
//                                            , CASE WHEN UserInfo.Gender = 1 THEN 'Male' ELSE 'Female' END AS Gender
//                                            , Education_Levels.DegreeName as Education
//                                            , UserInfo.Mobile AS [Contact Number]
//                                            , convert(varchar,class.Contract_stratingDate,103) as [Class Start Date]
//                                            , convert(varchar,class.Contract_EndDate,103) as [Class End Date]
//                                            , Center.Name + ' ' + Center.HouseNo + ' ' + ISNULL( Center.Street, '') + ' ' + ISNULL(Center.Town, '') + PSDFTehsils.Name AS [Training Location]
//                                            , PSDFDistricts.Name as [District of Training Location]
//                                            , Case((Case IsResidenceVerified when 1 then 1 else 0 end) + (Case IsAgeVerified when 1 then 1 else 0 end) + (Case CNICVerified when 1 then 1 else 0 end)) when 3 then 'Yes' else 'No' END AS [CNIC Verified]
//                                            , TraineesStatus.Name AS [Trainee Status]
//                                            , CASE WHEN TraineeProfile.IsDual=0 THEN '' Else 'Dual' END [Is Dual]
//                                            , Convert(varchar,TraineeProfile.StatusChangeDate, 103) [Trainee Status Update Date]
//                                            , TraineeAssessment.Assessment as [Examination Assesment]
//                                            , CASE WHEN TraineeProfile.VoucherHolder=1 THEN 'Yes' Else 'No' END [Voucher Holder]
//                                            , Isnull(TraineeProfile.StatusReason,'') +' ' + Isnull(TraineeProfile.CNICVerificationReason,'') + ' ' + Isnull(TraineeProfile.ResidenceVerifiedReason,'') + ' ' + Isnull(TraineeProfile.AgeVerifiedReason,'') as [Reason]
//                                            FROM TraineeProfile 
//                                            INNER JOIN Class ON TraineeProfile.ClassID = Class.ID 
//                                            INNER JOIN EMPLOYMENT_STATUS AS ES ON TraineeProfile.EmploymentStatus = ES.STATUSID
//                                            INNER JOIN ClassStatus AS CS ON Class.ClassStatus = CS.ID
//                                            INNER JOIN TraineeAssessment ON TraineeProfile.Assessment = TraineeAssessment.ID 
//                                            INNER JOIN Trade ON Class.TradeID = Trade.ID 
//                                            Left Outer JOIN TestingAgency ON Trade.TestingAgencyID = TestingAgency.TestingAgencyID 
//                                            INNER JOIN TradeGroup ON Trade.TradeGroupID = TradeGroup.ID 
//                                            INNER JOIN Scheme ON Class.SchemeID = Scheme.SchemeID 
//                                            INNER JOIN ServiceProvider ON Class.SpID = ServiceProvider.ID
//                                            INNER JOIN Section ON Class.ClassSection = Section.SectionID 
//                                            INNER JOIN Shift ON Class.Shift = Shift.ID 
//                                            INNER JOIN Center ON Class.CentreID = Center.ID 
//                                            INNER JOIN UserInfo ON TraineeProfile.ID = UserInfo.ID 
//                                            INNER JOIN UserInfo AS UserInfo_SP ON ServiceProvider.ID = UserInfo_SP.ID 
//                                            INNER JOIN PSDFDistricts ON Center.DistrictID = PSDFDistricts.ID 
//                                            INNER JOIN PSDFDistricts As TraineeDist ON UserInfo.District = TraineeDist.ID 
//                                            INNER JOIN TraineesStatus ON TraineeProfile.TraineeStatusID = TraineesStatus.ID 
//                                            INNER JOIN Education_Levels ON TraineeProfile.HighestEducation=Education_Levels.ID 
//                                            INNER JOIN PSDFTehsils as Tehsilname On UserInfo.Tehsil=Tehsilname.ID
//                                            inner join PSDFTehsils on Center.Tehsil=PSDFTehsils.ID
//                                            WHERE UserInfo.IsDeleted = 0 
//                                           and( TraineeProfile.IsResidenceVerified in (0,2) or (TraineeProfile.IsAgeVerified in (0,2) and TraineeProfile.CNICVerified in (0,2)))
//                                           and UserInfo.IsDeleted=0 and (Class.ClassStatus=3 or Class.ClassStatus=4 )
//                                           and Class.Contract_EndDate > DATEADD(m, -6, current_timestamp) and class.Contract_stratingDate < DateAdd(DAY,-45,GetDate()) 
//                                           and ServiceProvider.ID = '" + Tsp + "' and scheme.SchemeID  = '" + SchemeID + "' Order by Scheme.Code, ServiceProvider.Code, Class.ClassCode";
//                //and Class.SpID = '" + Tsp + "' and Class.SchemeID = '" + SchemeID + "' Order by Scheme.Code, ServiceProvider.Code, Class.ClassCode"

//                //                                            WHERE UserInfo.IsDeleted = 0 
//                //                                            and TraineeProfile.IsAgeVerified=0 and TraineeProfile.IsResidenceVerified=0 and TraineeProfile.CNICVerified=0 
//                //                                            and UserInfo.IsDeleted=0 and (Class.ClassStatus=3 or Class.ClassStatus=4 )
//                //                                            and ((Class.Contract_stratingDate <= GETDATE() -183) or (class.Contract_EndDate>=GETDATE() -183))



//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);

//                Dictionary<string, Dictionary<string, List<string>>> Cir = new Dictionary<string, Dictionary<string, List<string>>>();

//                while (objRequestDataReader.Read())
//                {

//                    List<string> SchemeName = objRequestDataReader["Scheme"].ToString().Split(',').ToList();
//                    List<string> TSPName = objRequestDataReader["Training Service Provider"].ToString().Split(',').ToList();
//                    List<string> ClassCode = objRequestDataReader["Class Code"].ToString().Split(',').ToList();

//                    string RequestID = "";// objRequestDataReader["CirId"].ToString();
//                    string ClMrId = "";//objRequestDataReader["ClMrId"].ToString();
//                    string Month = "";// objRequestDataReader["Month"].ToString();

//                    if (Cir.ContainsKey(RequestID))
//                    {
//                        if (Month == SelectedMonth)
//                        {
//                            Cir[RequestID]["SelectedMonthVisitIDs"].Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            Cir[RequestID]["PrevMonthVisitIDs"].Add(ClMrId);
//                        }
//                    }
//                    else
//                    {
//                        List<string> SelectedMonthVisitIDs = new List<string>();
//                        List<string> PrevMonthVisitIDs = new List<string>();

//                        if (Month == SelectedMonth)
//                        {
//                            SelectedMonthVisitIDs.Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            PrevMonthVisitIDs.Add(ClMrId);
//                        }
//                        Dictionary<string, List<string>> CirIds = new Dictionary<string, List<string>>();

//                        CirIds.Add("SelectedMonthVisitIDs", SelectedMonthVisitIDs);
//                        CirIds.Add("PrevMonthVisitIDs", PrevMonthVisitIDs);

//                        CirIds.Add("SchemeName", SchemeName);
//                        CirIds.Add("TspName", TSPName);
//                        CirIds.Add("ClassCode", ClassCode);

//                        Cir.Add(RequestID, CirIds);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;

//                }

//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> TraineesList = getUnverifiedTraineeFromDb(Cir, SelectedMonthString, PrevMonthString, Tsp, SchemeID);


//                DataTable dt = new DataTable();

//                dt.Columns.Add("Sr.#");
//                //dt.Columns.Add("Service Provider Name");
//                //dt.Columns.Add("Trade Group");
//                //dt.Columns.Add("Trade");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Trainee ID");
//                // dt.Columns.Add("Course Duration (Months)");
//                //dt.Columns.Add("Trainee ID");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Father/Husband Name");
//                dt.Columns.Add("CNIC Issuance Date");
//                dt.Columns.Add("CNIC");
//                dt.Columns.Add("Date Of Birth");
//                //dt.Columns.Add("Roll #");
//                //dt.Columns.Add("Batch");
//                //dt.Columns.Add("Section");
//                //dt.Columns.Add("Shift");
//                //dt.Columns.Add("Trainee Address");
//                //dt.Columns.Add("Residence Tehsil");
//                dt.Columns.Add("District of Residence");
//                dt.Columns.Add("Gender");
//                //dt.Columns.Add("Education");
//                dt.Columns.Add("Contact Number");

//                //dt.Columns.Add("Training Location");
//                //dt.Columns.Add("District of Training Location");
//                dt.Columns.Add("CNIC Verified");
//                dt.Columns.Add("Trainee Status");
//                //dt.Columns.Add("Is Dual");
//                //dt.Columns.Add("Trainee Status Update Date");
//                //dt.Columns.Add("Examination Assesment");
//                //dt.Columns.Add("Voucher Holder");
//                dt.Columns.Add("Reason");
//                dt.Columns.Add("Correct CNIC Issue Date");
//                dt.Columns.Add("Correct CNIC number");

//                //dt.Columns.Add("Class Status");
//                //dt.Columns.Add("Class Start Date");
//                //dt.Columns.Add("Class End Date");



//                //dt.Columns.Add(SelectedMonthString);
//                //dt.Columns.Add(PrevMonthString);
//                //dt.Columns.Add("Remarks");

//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in TraineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr.#"] = index; //
//                    _row["Class Code"] = entry.Value["ClassCode"]; //
//                    _row["Trainee Name"] = entry.Value["Name"]; //
//                    _row["Father/Husband Name"] = entry.Value["FName"]; //
//                    _row["CNIC"] = entry.Value["CNIC"];//
//                    _row["CNIC Issuance Date"] = entry.Value["CNIC Issuance Date"]; //
//                    _row["Date Of Birth"] = entry.Value["Date Of Birth"];//
//                    _row["District of Residence"] = entry.Value["District of Residence"];//
//                    _row["Gender"] = entry.Value["Gender"];//
//                    _row["Contact Number"] = entry.Value["Contact Number"];//
//                    _row["CNIC Verified"] = entry.Value["CNIC Verified"];//
//                    _row["Trainee Status"] = entry.Value["Trainee Status"];//
//                    _row["Reason"] = entry.Value["Reason"];//
//                    _row["Trainee ID"] = entry.Value["ID"];//
//                    //_row[SelectedMonthString] = entry.Value.ContainsKey(SelectedMonthString) ? entry.Value[SelectedMonthString] : "";
//                    //_row[PrevMonthString] = entry.Value.ContainsKey(PrevMonthString) ? entry.Value[PrevMonthString] : "";
//                    //_row["Remarks"] = entry.Value["Remarks"];

//                    dt.Rows.Add(_row);
//                    index++;
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }

//        }

//        /************************** Unverified Trainee Report *************************************/
//        private Dictionary<string, Dictionary<string, string>> getUnverifiedTraineeFromDb(Dictionary<string, Dictionary<string, List<string>>> CirList, string SelectedMonth, string PrevMonth, string Tsp, string SchemeID)
//        {

//            try
//            {
//                Dictionary<string, Dictionary<string, string>> TraineesList = new Dictionary<string, Dictionary<string, string>>();

//                foreach (KeyValuePair<string, Dictionary<string, List<string>>> entry in CirList)
//                {
//                    string joinString = ",";

//                    string SchemeName = string.Join("", entry.Value["SchemeName"].ToArray());
//                    string TspName = string.Join("", entry.Value["TspName"].ToArray());
//                    string ClassCode = string.Join("", entry.Value["ClassCode"].ToArray());

//                    string SelectedMonthString = string.Join(joinString, entry.Value["SelectedMonthVisitIDs"].ToArray());
//                    string PrevMonthString = string.Join(joinString, entry.Value["PrevMonthVisitIDs"].ToArray());

//                    string strSqlSelectedMonthNoTsr = @"SELECT Scheme.Name AS Scheme
//                                            , UserInfo_SP.DisplayName AS [Training Service Provider]
//                                            , TradeGroup.TradeGroupName AS [Trade Group]
//                                            , Trade.TradeName as [Trade]
//                                            , Scheme.Code + '-' + ServiceProvider.Code + '-' + Class.ClassCode As [Class Code] 
//                                            , CS.ClassStatus AS [Class Status] 
//                                            , TraineeProfile.TraineeID [Trainee ID]
//                                            , UserInfo.DisplayName AS [Trainee Name]
//                                            , UserInfo.FatherName [Father's Name]
//                                            , UserInfo.Cnic AS [CNIC]
//                                            , Convert(varchar,UserInfo.DateOfCNICIssuance, 103) [CNIC Issuance Date]
//                                            , Convert(varchar,UserInfo.Dob, 103) [Date Of Birth]
//                                            , TraineeProfile.RollNo AS [Roll #]
//                                            , Class.BatchID AS Batch
//                                            , Section.Name AS Section
//                                            , Shift.ShiftName AS Shift
//                                            , UserInfo.House + ' ' + UserInfo.Street + ' ' + UserInfo.Moza as [Trainee Address]
//                                            , Tehsilname.Name as [Residence Tehsil]
//                                            , TraineeDist.Name [District of Residence]
//                                            , CASE WHEN UserInfo.Gender = 1 THEN 'Male' ELSE 'Female' END AS Gender
//                                            , Education_Levels.DegreeName as Education
//                                            , UserInfo.Mobile AS [Contact Number]
//                                            , convert(varchar,class.Contract_stratingDate,103) as [Class Start Date]
//                                            , convert(varchar,class.Contract_EndDate,103) as [Class End Date]
//                                            , Center.Name + ' ' + Center.HouseNo + ' ' + ISNULL( Center.Street, '') + ' ' + ISNULL(Center.Town, '') + PSDFTehsils.Name AS [Training Location]
//                                            , PSDFDistricts.Name as [District of Training Location]
//                                            , Case((Case IsResidenceVerified when 1 then 1 else 0 end) + (Case IsAgeVerified when 1 then 1 else 0 end) + (Case CNICVerified when 1 then 1 else 0 end)) when 3 then 'Yes' else 'No' END AS [CNIC Verified]
//                                            , TraineesStatus.Name AS [Trainee Status]
//                                            , CASE WHEN TraineeProfile.IsDual=0 THEN '' Else 'Dual' END [Is Dual]
//                                            , Convert(varchar,TraineeProfile.StatusChangeDate, 103) [Trainee Status Update Date]
//                                            , TraineeAssessment.Assessment as [Examination Assesment]
//                                            , CASE WHEN TraineeProfile.VoucherHolder=1 THEN 'Yes' Else 'No' END [Voucher Holder]
//                                            , Isnull(TraineeProfile.StatusReason,'') +' ' + Isnull(TraineeProfile.CNICVerificationReason,'') + ' ' + Isnull(TraineeProfile.ResidenceVerifiedReason,'') + ' ' + Isnull(TraineeProfile.AgeVerifiedReason,'') as [Reason]
//                                            FROM TraineeProfile 
//                                            INNER JOIN Class ON TraineeProfile.ClassID = Class.ID 
//                                            INNER JOIN EMPLOYMENT_STATUS AS ES ON TraineeProfile.EmploymentStatus = ES.STATUSID
//                                            INNER JOIN ClassStatus AS CS ON Class.ClassStatus = CS.ID
//                                            INNER JOIN TraineeAssessment ON TraineeProfile.Assessment = TraineeAssessment.ID 
//                                            INNER JOIN Trade ON Class.TradeID = Trade.ID 
//                                            Left Outer JOIN TestingAgency ON Trade.TestingAgencyID = TestingAgency.TestingAgencyID 
//                                            INNER JOIN TradeGroup ON Trade.TradeGroupID = TradeGroup.ID 
//                                            INNER JOIN Scheme ON Class.SchemeID = Scheme.SchemeID 
//                                            INNER JOIN ServiceProvider ON Class.SpID = ServiceProvider.ID
//                                            INNER JOIN Section ON Class.ClassSection = Section.SectionID 
//                                            INNER JOIN Shift ON Class.Shift = Shift.ID 
//                                            INNER JOIN Center ON Class.CentreID = Center.ID 
//                                            INNER JOIN UserInfo ON TraineeProfile.ID = UserInfo.ID 
//                                            INNER JOIN UserInfo AS UserInfo_SP ON ServiceProvider.ID = UserInfo_SP.ID 
//                                            INNER JOIN PSDFDistricts ON Center.DistrictID = PSDFDistricts.ID 
//                                            INNER JOIN PSDFDistricts As TraineeDist ON UserInfo.District = TraineeDist.ID 
//                                            INNER JOIN TraineesStatus ON TraineeProfile.TraineeStatusID = TraineesStatus.ID 
//                                            INNER JOIN Education_Levels ON TraineeProfile.HighestEducation=Education_Levels.ID 
//                                            INNER JOIN PSDFTehsils as Tehsilname On UserInfo.Tehsil=Tehsilname.ID
//                                            inner join PSDFTehsils on Center.Tehsil=PSDFTehsils.ID
//                                            WHERE UserInfo.IsDeleted = 0 
//                                           and( TraineeProfile.IsResidenceVerified in (0,2) or (TraineeProfile.IsAgeVerified in (0,2) and TraineeProfile.CNICVerified in (0,2)))
//                                           and UserInfo.IsDeleted=0 and (Class.ClassStatus=3 or Class.ClassStatus=4 )
//                                           and Class.Contract_EndDate > DATEADD(m, -6, current_timestamp) and class.Contract_stratingDate < DateAdd(DAY,-45,GetDate()) 
//                                           and ServiceProvider.ID = '" + Tsp + "' and scheme.SchemeID  = '" + SchemeID + "' Order by Scheme.Code, ServiceProvider.Code, Class.ClassCode";


//                    // WHERE UserInfo.IsDeleted = 0 
//                    // and TraineeProfile.IsAgeVerified in (0,2) and TraineeProfile.IsResidenceVerified in (0,2)
//                    // and TraineeProfile.CNICVerified in (0,2) 
//                    // and UserInfo.IsDeleted=0 and (Class.ClassStatus=3 or Class.ClassStatus=4 )
//                    // and Class.Contract_EndDate > DATEADD(m, -6, current_timestamp) and class.Contract_stratingDate < DateAdd(DAY,-45,GetDate())
//                    //and ServiceProvider.ID = '" + Tsp + "' and scheme.SchemeID  = '" + SchemeID + "' Order by Scheme.Code, ServiceProvider.Code, Class.ClassCode";


//                    //@"SELECT Scheme.Name AS Scheme
//                    //                                            , UserInfo_SP.DisplayName AS [Training Service Provider]
//                    //                                            , TradeGroup.TradeGroupName AS [Trade Group]
//                    //                                            , Trade.TradeName as [Trade]
//                    //                                            , Scheme.Code + '-' + ServiceProvider.Code + '-' + Class.ClassCode As [Class Code] 
//                    //                                            , CS.ClassStatus AS [Class Status] 
//                    //                                            , TraineeProfile.TraineeID [Trainee ID]
//                    //                                            , UserInfo.DisplayName AS [Trainee Name]
//                    //                                            , UserInfo.FatherName [Father's Name]
//                    //                                            , UserInfo.Cnic AS [CNIC]
//                    //                                            , Convert(varchar,UserInfo.DateOfCNICIssuance, 103) [CNIC Issuance Date]
//                    //                                            , Convert(varchar,UserInfo.Dob, 103) [Date Of Birth]
//                    //                                            , TraineeProfile.RollNo AS [Roll #]
//                    //                                            , Class.BatchID AS Batch
//                    //                                            , Section.Name AS Section
//                    //                                            , Shift.ShiftName AS Shift
//                    //                                            , UserInfo.House + ' ' + UserInfo.Street + ' ' + UserInfo.Moza as [Trainee Address]
//                    //                                            , Tehsilname.Name as [Residence Tehsil]
//                    //                                            , TraineeDist.Name [District of Residence]
//                    //                                            , CASE WHEN UserInfo.Gender = 1 THEN 'Male' ELSE 'Female' END AS Gender
//                    //                                            , Education_Levels.DegreeName as Education
//                    //                                            , UserInfo.Mobile AS [Contact Number]
//                    //                                            , convert(varchar,class.Contract_stratingDate,103) as [Class Start Date]
//                    //                                            , convert(varchar,class.Contract_EndDate,103) as [Class End Date]
//                    //                                            , Center.Name + ' ' + Center.HouseNo + ' ' + ISNULL( Center.Street, '') + ' ' + ISNULL(Center.Town, '') + PSDFTehsils.Name AS [Training Location]
//                    //                                            , PSDFDistricts.Name as [District of Training Location]
//                    //                                            , Case((Case IsResidenceVerified when 1 then 1 else 0 end) + (Case IsAgeVerified when 1 then 1 else 0 end) + (Case CNICVerified when 1 then 1 else 0 end)) when 3 then 'Yes' else 'No' END AS [CNIC Verified]
//                    //                                            , TraineesStatus.Name AS [Trainee Status]
//                    //                                            , CASE WHEN TraineeProfile.IsDual=0 THEN '' Else 'Dual' END [Is Dual]
//                    //                                            , Convert(varchar,TraineeProfile.StatusChangeDate, 103) [Trainee Status Update Date]
//                    //                                            , TraineeAssessment.Assessment as [Examination Assesment]
//                    //                                            , CASE WHEN TraineeProfile.VoucherHolder=1 THEN 'Yes' Else 'No' END [Voucher Holder]
//                    //                                            , Isnull(TraineeProfile.StatusReason,'') +' ' + Isnull(TraineeProfile.CNICVerificationReason,'') + ' ' + Isnull(TraineeProfile.ResidenceVerifiedReason,'') + ' ' + Isnull(TraineeProfile.AgeVerifiedReason,'') as [Reason]
//                    //                                            FROM TraineeProfile 
//                    //                                            INNER JOIN Class ON TraineeProfile.ClassID = Class.ID 
//                    //                                            INNER JOIN EMPLOYMENT_STATUS AS ES ON TraineeProfile.EmploymentStatus = ES.STATUSID
//                    //                                            INNER JOIN ClassStatus AS CS ON Class.ClassStatus = CS.ID
//                    //                                            INNER JOIN TraineeAssessment ON TraineeProfile.Assessment = TraineeAssessment.ID 
//                    //                                            INNER JOIN Trade ON Class.TradeID = Trade.ID 
//                    //                                            Left Outer JOIN TestingAgency ON Trade.TestingAgencyID = TestingAgency.TestingAgencyID 
//                    //                                            INNER JOIN TradeGroup ON Trade.TradeGroupID = TradeGroup.ID 
//                    //                                            INNER JOIN Scheme ON Class.SchemeID = Scheme.SchemeID 
//                    //                                            INNER JOIN ServiceProvider ON Class.SpID = ServiceProvider.ID
//                    //                                            INNER JOIN Section ON Class.ClassSection = Section.SectionID 
//                    //                                            INNER JOIN Shift ON Class.Shift = Shift.ID 
//                    //                                            INNER JOIN Center ON Class.CentreID = Center.ID 
//                    //                                            INNER JOIN UserInfo ON TraineeProfile.ID = UserInfo.ID 
//                    //                                            INNER JOIN UserInfo AS UserInfo_SP ON ServiceProvider.ID = UserInfo_SP.ID 
//                    //                                            INNER JOIN PSDFDistricts ON Center.DistrictID = PSDFDistricts.ID 
//                    //                                            INNER JOIN PSDFDistricts As TraineeDist ON UserInfo.District = TraineeDist.ID 
//                    //                                            INNER JOIN TraineesStatus ON TraineeProfile.TraineeStatusID = TraineesStatus.ID 
//                    //                                            INNER JOIN Education_Levels ON TraineeProfile.HighestEducation=Education_Levels.ID 
//                    //                                            INNER JOIN PSDFTehsils as Tehsilname On UserInfo.Tehsil=Tehsilname.ID
//                    //                                            inner join PSDFTehsils on Center.Tehsil=PSDFTehsils.ID
//                    //                                            WHERE UserInfo.IsDeleted = 0 
//                    //                                            and TraineeProfile.IsAgeVerified=0 and TraineeProfile.IsResidenceVerified=0 and TraineeProfile.CNICVerified=0 
//                    //                                            and UserInfo.IsDeleted=0 and (Class.ClassStatus=3 or Class.ClassStatus=4 )
//                    //                                            and ((Class.Contract_stratingDate <= GETDATE() -183) or (class.Contract_EndDate>=GETDATE() -183))
//                    //                                            and ServiceProvider.ID = '" + Tsp + "' and scheme.SchemeID = '" + SchemeID + "' Order by Scheme.Code, ServiceProvider.Code, Class.ClassCode";

//                    DbCommand objTraineeDbCommand = null;
//                    IDataReader objTraineeDataReader = null;

//                    objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlSelectedMonthNoTsr);
//                    objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                    while (objTraineeDataReader.Read())
//                    {
//                        string TraineeID = objTraineeDataReader["Trainee ID"].ToString();
//                        //string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                        if (!TraineesList.ContainsKey(TraineeID))
//                        {
//                            Dictionary<string, string> trainee = new Dictionary<string, string>();
//                            trainee.Add("ID", TraineeID);
//                            trainee.Add("Tsp", TspName);
//                            trainee.Add("ClassCode", objTraineeDataReader["Class Code"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Class Code"]);
//                            trainee.Add("Name", objTraineeDataReader["Trainee Name"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Trainee Name"]);
//                            trainee.Add("FName", objTraineeDataReader["Father's Name"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Father's Name"]);
//                            trainee.Add("Trade", objTraineeDataReader["Trade"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Trade"]);
//                            trainee.Add("Trade Group", objTraineeDataReader["Trade Group"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Trade Group"]);
//                            trainee.Add("Class Status", objTraineeDataReader["Class Status"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Class Status"]);
//                            trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);
//                            trainee.Add("CNIC Issuance Date", objTraineeDataReader["CNIC Issuance Date"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["CNIC Issuance Date"]);
//                            trainee.Add("Date Of Birth", objTraineeDataReader["Date Of Birth"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Date Of Birth"]);
//                            trainee.Add("Roll #", objTraineeDataReader["Roll #"].ToString());
//                            trainee.Add("Batch", objTraineeDataReader["Batch"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Batch"]);
//                            trainee.Add("Section", objTraineeDataReader["Section"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Section"]);
//                            trainee.Add("Shift", objTraineeDataReader["Shift"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Shift"]);
//                            trainee.Add("Trainee Address", objTraineeDataReader["Trainee Address"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Trainee Address"]);
//                            trainee.Add("Residence Tehsil", objTraineeDataReader["Residence Tehsil"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Residence Tehsil"]);
//                            trainee.Add("District of Residence", objTraineeDataReader["District of Residence"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["District of Residence"]);
//                            trainee.Add("Gender", objTraineeDataReader["Gender"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Gender"]);
//                            trainee.Add("Education", objTraineeDataReader["Education"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Education"]);
//                            trainee.Add("Contact Number", objTraineeDataReader["Contact Number"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Contact Number"]);
//                            trainee.Add("Class Start Date", objTraineeDataReader["Class Start Date"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Class Start Date"]);
//                            trainee.Add("Class End Date", objTraineeDataReader["Class End Date"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Class End Date"]);
//                            trainee.Add("Training Location", objTraineeDataReader["Training Location"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Training Location"]);
//                            trainee.Add("District of Training Location", objTraineeDataReader["District of Training Location"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["District of Training Location"]);
//                            trainee.Add("CNIC Verified", objTraineeDataReader["CNIC Verified"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["CNIC Verified"]);
//                            trainee.Add("Trainee Status", objTraineeDataReader["Trainee Status"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Trainee Status"]);
//                            trainee.Add("Is Dual", objTraineeDataReader["Is Dual"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Is Dual"]);
//                            trainee.Add("Trainee Status Update Date", objTraineeDataReader["Trainee Status Update Date"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Trainee Status Update Date"]);
//                            trainee.Add("Examination Assesment", objTraineeDataReader["Examination Assesment"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Examination Assesment"]);
//                            trainee.Add("Voucher Holder", objTraineeDataReader["Voucher Holder"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Voucher Holder"]);
//                            trainee.Add("Reason", objTraineeDataReader["Reason"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Reason"]);
//                            trainee.Add(SelectedMonth, objTraineeDataReader["Trainee Status"].ToString());
//                            //trainee.Add("Remarks", objTraineeDataReader["Remarks"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Remarks"]);

//                            TraineesList.Add(TraineeID, trainee);
//                        }
//                    }

//                    if (objTraineeDataReader != null)
//                    {
//                        if (!objTraineeDataReader.IsClosed)
//                            objTraineeDataReader.Close();

//                        objTraineeDataReader.Dispose();
//                        objTraineeDataReader = null;
//                    }

//                    if (objTraineeDbCommand != null)
//                    {
//                        objTraineeDbCommand.Dispose();
//                        objTraineeDbCommand = null;
//                    }
//                }

//                return TraineesList;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        /* Centre Monitoring Report */
//        public CentreReport LoadCentreReport(string requestID)
//        {

//            IDataReader objIDataReader = null;
//            DbCommand objDbCommand = null;
//            CentreReport objCentreReport = null;

//            try
//            {
//                string strSql = @"SELECT request.ID,cn_mr.ID as CentreMrID,request.TrainingLocAddress,request.TrainingCentreName,request.SchemeName, request.TSPName,request.CentreInchargeName,
//                                request.CentreInchargeMob, request.ProjectCoordinatorName,request.DistrictInchargeName,cn_mr.FMName,request.PCAssignDateTime,request.SUAssignDateTime,request.DIAssignDateTime,
//                                cn_mr.VisitDateTime, cn_mr.ClassesInspectedCount, cn_mr.TrainingCentreName, cn_mr.LocAccessSuitabilityValue,
//                                cn_mr.LocAccessSuitabilityObservRemarks, cn_mr.LocAccessSuitabilityRecomRemarks, cn_mr.SecurityPremValue,
//                                cn_mr.SecurityPremObservRemarks, cn_mr.SecurityPremRecomRemarks, cn_mr.StructIntegValue, cn_mr.StructIntegObservRemarks,
//                                cn_mr.StructIntegRecomRemarks, cn_mr.CentreInchrgRoomValue, cn_mr.CentreInchrgRoomObservRemarks, cn_mr.CentreInchrgRoomRecomRemarks,
//                                cn_mr.ElecAvalValue, cn_mr.ElecAvalObservRemarks, cn_mr.ElecAvalRecomRemarks, cn_mr.ToiletAvalValue, cn_mr.ToiletAvalObservRemarks,
//                                cn_mr.ToiletAvalRecomRemarks, cn_mr.ToiletCount, cn_mr.WaterAvalValue, cn_mr.WaterAvalObservRemarks, cn_mr.WaterAvalRecomRemarks,
//                                cn_mr.FrstAidAvalValue, cn_mr.FrstAidAvalObservRemarks, cn_mr.FrstAidAvalRecomRemarks, 
//								cn_mr.KeyFacMissing, cn_mr.KeyFacBuildingStructInteg, cn_mr.KeyFacFurnitureAval, cn_mr.KeyFacElecBackup,
//								cn_mr.KeyFacEquipAval, cn_mr.SignOffTspName,cn_mr.SignOffTspRemarks, cn_mr.SignOffFmRemarks,
//                                cn_mr.TspSignatureImgPath, cn_mr.FMSignatureImgPath, cn_mr.FMSubmissionDateTime, cn_mr.DIAssignDateTime, cn_mr.TspSignatureImgPath
//                                FROM AMSCentreRequest as request INNER JOIN
//                                AMSCentreMonitoring as cn_mr ON request.ID = cn_mr.CentreRequestID
//                                WHERE request.ID=" + requestID;

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);
//                int moinitoringID = 0;
//                string imageUrl = string.Empty;

//                if (objIDataReader.Read())
//                {
//                    moinitoringID = Convert.ToInt16(objIDataReader["CentreMrID"]);
//                    objCentreReport = new CentreReport();
//                    objCentreReport.ClassList = new List<CentreClass>();

//                    objCentreReport.SchemeName = objIDataReader["SchemeName"].ToString();
//                    objCentreReport.TSPName = objIDataReader["TSPName"].ToString();
//                    objCentreReport.VisitDateTime = DateTime.Parse(objIDataReader["VisitDateTime"].ToString()).ToString("dd-MMM-yy h:mm:tt");
//                    objCentreReport.CentreAddress = objIDataReader["TrainingLocAddress"].ToString();
//                    objCentreReport.TrainingCentreName = objIDataReader["TrainingCentreName"].ToString();
//                    objCentreReport.ClassesInspectedCount = Convert.ToInt16(objIDataReader["ClassesInspectedCount"]);
//                    objCentreReport.InchargeName = (objIDataReader["CentreInchargeName"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["CentreInchargeName"].ToString();
//                    objCentreReport.InchargeMob = (objIDataReader["CentreInchargeMob"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["CentreInchargeMob"].ToString();

//                    objCentreReport.LocSuitable = (objIDataReader["LocAccessSuitabilityValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["LocAccessSuitabilityValue"].ToString()];
//                    objCentreReport.LocSuitableRemarks = (objIDataReader["LocAccessSuitabilityObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["LocAccessSuitabilityObservRemarks"].ToString();
//                    objCentreReport.LocSuitableRecom = (objIDataReader["LocAccessSuitabilityRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["LocAccessSuitabilityRecomRemarks"].ToString();

//                    objCentreReport.PremisesSecurity = (objIDataReader["SecurityPremValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["SecurityPremValue"].ToString()];
//                    objCentreReport.PremisesSecurityRemarks = (objIDataReader["SecurityPremObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["SecurityPremObservRemarks"].ToString();
//                    objCentreReport.PremisesSecurityRecom = (objIDataReader["SecurityPremRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["SecurityPremRecomRemarks"].ToString();

//                    objCentreReport.StructIntegrity = (objIDataReader["StructIntegValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["StructIntegValue"].ToString()];
//                    objCentreReport.StructIntegrityRemarks = (objIDataReader["StructIntegObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["StructIntegObservRemarks"].ToString();
//                    objCentreReport.StructIntegrityRecom = (objIDataReader["StructIntegRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["StructIntegRecomRemarks"].ToString();

//                    objCentreReport.InchargeRoom = (objIDataReader["CentreInchrgRoomValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["CentreInchrgRoomValue"].ToString()];
//                    objCentreReport.InchargeRoomRemarks = (objIDataReader["CentreInchrgRoomObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["CentreInchrgRoomObservRemarks"].ToString();
//                    objCentreReport.InchargeRoomRecom = (objIDataReader["CentreInchrgRoomRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["CentreInchrgRoomRecomRemarks"].ToString();

//                    objCentreReport.ElecticSupply = (objIDataReader["ElecAvalValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["ElecAvalValue"].ToString()];
//                    objCentreReport.ElecticSupplyRemarks = (objIDataReader["ElecAvalObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["ElecAvalObservRemarks"].ToString();
//                    objCentreReport.ElecticSupplyRecom = (objIDataReader["ElecAvalRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["ElecAvalRecomRemarks"].ToString();

//                    objCentreReport.ToiletAvail = (objIDataReader["ToiletAvalValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["ToiletAvalValue"].ToString()];
//                    objCentreReport.ToiletAvailRemarks = (objIDataReader["ToiletAvalObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["ToiletAvalObservRemarks"].ToString();
//                    objCentreReport.ToiletAvailRecom = (objIDataReader["ToiletAvalRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["ToiletAvalRecomRemarks"].ToString();
//                    objCentreReport.ToiletCount = (objIDataReader["ToiletCount"].Equals(DBNull.Value)) ? 0 : Convert.ToInt16(objIDataReader["ToiletCount"]);

//                    objCentreReport.WaterAvail = (objIDataReader["WaterAvalValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["WaterAvalValue"].ToString()];
//                    objCentreReport.WaterAvailRemarks = (objIDataReader["WaterAvalObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["WaterAvalObservRemarks"].ToString();
//                    objCentreReport.WaterAvailRecom = (objIDataReader["WaterAvalRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["WaterAvalRecomRemarks"].ToString();

//                    objCentreReport.FirstAidAvail = (objIDataReader["FrstAidAvalValue"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["FrstAidAvalValue"].ToString()];
//                    objCentreReport.FirstAidAvailRemarks = (objIDataReader["FrstAidAvalObservRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["FrstAidAvalObservRemarks"].ToString();
//                    objCentreReport.FirstAidAvailRecom = (objIDataReader["FrstAidAvalRecomRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["FrstAidAvalRecomRemarks"].ToString();

//                    objCentreReport.FieldMonitorName = (objIDataReader["FMName"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["FMName"].ToString();
//                    objCentreReport.TSPRepName = (objIDataReader["SignOffTspName"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["SignOffTspName"].ToString();
//                    objCentreReport.DistrictInchargeName = (objIDataReader["DistrictInchargeName"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["DistrictInchargeName"].ToString();
//                    objCentreReport.FieldMonitorRemarks = (objIDataReader["SignOffFmRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["SignOffFmRemarks"].ToString();
//                    objCentreReport.InchargeRemarks = (objIDataReader["SignOffTspRemarks"].Equals(DBNull.Value)) ? String.Empty : objIDataReader["SignOffTspRemarks"].ToString();

//                    objCentreReport.keyFacMissing = (objIDataReader["KeyFacMissing"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["KeyFacMissing"].ToString()];
//                    objCentreReport.keyFacStructure = (objIDataReader["KeyFacBuildingStructInteg"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["KeyFacBuildingStructInteg"].ToString()];
//                    objCentreReport.keyFacFurnitureAvail = (objIDataReader["KeyFacFurnitureAval"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["KeyFacFurnitureAval"].ToString()];
//                    objCentreReport.keyFacElecAvail = (objIDataReader["KeyFacElecBackup"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["KeyFacElecBackup"].ToString()];
//                    objCentreReport.keyFacEquipAvail = (objIDataReader["KeyFacEquipAval"].Equals(DBNull.Value)) ? String.Empty : yesNoValues[objIDataReader["KeyFacEquipAval"].ToString()];

//                    objCentreReport.DIAssignDateTime = DateTime.Parse(objIDataReader["VisitDateTime"].ToString()).ToString("dd-MMM-yy");
//                    objCentreReport.FMSubmissionDateTime = DateTime.Parse(objIDataReader["VisitDateTime"].ToString()).ToString("dd-MMM-yy");

//                    objCentreReport.TspSignatureImg = string.Empty;
//                    objCentreReport.FmSignatureImg = string.Empty;

//                    imageUrl = objIDataReader["TspSignatureImgPath"].ToString();
//                    if (imageUrl != null && imageUrl != string.Empty)
//                    {
//                        objCentreReport.TspSignatureImg = IMAGES_PATH + "/centre/" + objIDataReader["ID"].ToString() + "/" + imageUrl;
//                    }

//                    imageUrl = objIDataReader["FMSignatureImgPath"].ToString();
//                    if (imageUrl != null && imageUrl != string.Empty)
//                    {
//                        objCentreReport.FmSignatureImg = IMAGES_PATH + "/centre/" + objIDataReader["ID"].ToString() + "/" + imageUrl;
//                    }

//                }

//                //Debug.WriteLine(objCentreReport);
//                //Trace.WriteLine("tracing output");
//                //Debug.WriteLine("debugging output");

//                // Get Centre class monitoring Data
//                strSql = @"SELECT * FROM AMSCentreMonitoringClass WHERE CentreMonitoringID = " + moinitoringID;

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                while (objIDataReader.Read())
//                {

//                    CentreClass objCentreClass = new CentreClass();
//                    objCentreClass.ClassCode = objIDataReader["ClassCode"].ToString();
//                    objCentreClass.TradeName = objIDataReader["TradeName"].ToString();
//                    objCentreClass.ClassStartDate = DateTime.Parse(objIDataReader["ExpectedStartDate"].ToString()).ToString("dd-MMM-yy");
//                    objCentreClass.BoardAvail = yesNoValues[objIDataReader["BoardAval"].ToString()];
//                    objCentreClass.ChairsAvail = yesNoValues[objIDataReader["SufficientFurniture"].ToString()];
//                    objCentreClass.BulbsAvail = yesNoValues[objIDataReader["LightAval"].ToString()];
//                    objCentreClass.VentAvail = yesNoValues[objIDataReader["VentFanAval"].ToString()];
//                    objCentreClass.SpaceSufficient = yesNoValues[objIDataReader["ClassSpaceSufficient"].ToString()];

//                    objCentreReport.ClassList.Add(objCentreClass);
//                }

//                // Get Centre class monitoring Data
//                strSql = @"SELECT * FROM AMSCentreMonitoringTrade WHERE CentreMonitoringID = " + moinitoringID;

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                while (objIDataReader.Read())
//                {
//                    CentreTrade objCentreTrade = new CentreTrade();
//                    objCentreTrade.TradeName = objIDataReader["TradeName"].ToString();
//                    objCentreTrade.ClassesPerBatch = Convert.ToInt32(objIDataReader["ClassesPrBatch"]);
//                    objCentreTrade.QuantitySufficient = yesNoValues[objIDataReader["SufficientQty"].ToString()];
//                    objCentreTrade.TraineesPerClass = Convert.ToInt32(objIDataReader["ContrTraineePrClas"]);
//                    objCentreTrade.TraineesPerTrade = Convert.ToInt32(objIDataReader["ContrTraineePrTrade"]);
//                    objCentreTrade.LabRooms = objIDataReader["LabRoomCount"].ToString();
//                    objCentreTrade.SpaceSufficient = yesNoValues[objIDataReader["SufficientSpace"].ToString()];
//                    objCentreTrade.PowerBackup = yesNoValues[objIDataReader["PowerBackupAval"].ToString()];

//                    objCentreReport.TradeList.Add(objCentreTrade);
//                }

//                // Get Trade Tools list

//                strSql = @"SELECT cn_mr_trade.ID AS TradeID, cn_mr_trade.CentreMonitoringID, cn_mr_trade.TradeName, cn_mr_trade.TradeDuration, cn_mr_trade.ContrTraineePrTrade AS headCount
//                        , cn_mr_tool.ToolName, cn_mr_tool.QuantityTotal, cn_mr_tool.QuantityFound
//                        FROM AMSCentreMonitoringTrade AS cn_mr_trade INNER JOIN AMSCentreMonitoringTradeTool cn_mr_tool
//                        ON cn_mr_trade.ID = cn_mr_tool.CentreMonitoringTradeID 
//                        WHERE cn_mr_trade.CentreMonitoringID = " + moinitoringID;

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                Dictionary<int, int> tradeIDsHash = new Dictionary<int, int>();
//                string tradeName, tradeDuration, headCount, toolName, toolQuantityTotal, toolQuantityFound;
//                tradeName = tradeDuration = headCount = toolName = toolQuantityTotal = toolQuantityFound = string.Empty;
//                int index, tradeID, tradeListIndex;
//                index = tradeID = tradeListIndex = 0;
//                Dictionary<string, string> objDict = null;
//                List<Dictionary<string, string>> objList = null;

//                while (objIDataReader.Read())
//                {
//                    tradeID = Convert.ToInt32(objIDataReader["TradeID"]);
//                    tradeName = objIDataReader["TradeName"].ToString();
//                    tradeDuration = objIDataReader["TradeDuration"].ToString();
//                    headCount = objIDataReader["headCount"].ToString();
//                    toolName = objIDataReader["ToolName"].ToString();
//                    toolQuantityTotal = objIDataReader["QuantityTotal"].ToString();
//                    toolQuantityFound = objIDataReader["QuantityFound"].ToString();

//                    objDict = objCentreReport.AddTradeTool(tradeName, tradeDuration, headCount, toolName, toolQuantityTotal, toolQuantityFound);

//                    if (tradeIDsHash.ContainsKey(tradeID))
//                    {
//                        tradeListIndex = tradeIDsHash[tradeID];
//                        objCentreReport.TradeToolsList[tradeListIndex].Add(objDict);
//                    }
//                    else
//                    {
//                        objList = new List<Dictionary<string, string>>();
//                        objList.Add(objDict);
//                        objCentreReport.TradeToolsList.Add(objList);

//                        tradeIDsHash.Add(tradeID, index);
//                        index++;
//                    }

//                }

//                return objCentreReport;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objIDataReader != null && !objIDataReader.IsClosed)
//                {
//                    objIDataReader.Close();
//                    objIDataReader.Dispose();
//                    objIDataReader = null;
//                }
//                if (objDbCommand != null)
//                {
//                    if (objDbCommand.Connection != null)
//                    {
//                        if (objDbCommand.Connection.State != ConnectionState.Closed)
//                            objDbCommand.Connection.Close();
//                        objDbCommand.Connection.Dispose();
//                        objDbCommand.Connection = null;
//                    }
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }
//            }
//        }

//        /******************************************* Profile Verification Report *****************************************/
//        public ProfileVerificationReport LoadPVReport(string requestID, string dateStr)
//        {

//            IDataReader objIDataReader = null;
//            DbCommand objDbCommand = null;

//            ProfileVerificationReport objPV = new ProfileVerificationReport();
//            Dictionary<string, string> statusValues = new Dictionary<string, string>()
//            {
//                {"deleted", "This trainee was deleted from attendance register"},
//                {"dropout", "This trianee was mentioned as dropout"},
//                {"blank_space", "The trainee was absent and there was blank space against this trainee"},
//                {"short_leave", "This trainee was on short leave on the day of visit"},
//                {"absent", "This trainee was absent on the day of visit"},
//                {"marked_present_found_absent", "This trainee was marked present in attendance register but was not present on the day of visit"},
//                {"dropout_but_present", "This trainee was dropout but marked present in attendance register"}
//            };

//            int index = 0;
//            string mrID = @"";
//            string classID = @"";

//            string name, fatherName, cnic, statusText, remarks, attStatus, veriStatus, isPresent;
//            name = fatherName = cnic = statusText = remarks = attStatus = veriStatus = isPresent = string.Empty;

//            try
//            {

//                string dateClause = string.Format(" AND month(AMSClassMonitoring.VisitDateTime) = month('{0}') AND year(AMSClassMonitoring.VisitDateTime ) = year('{0}')", dateStr);

//                string strSql = @"SELECT cir.ID, cir.ClassID, cir.ClassCode, cir.TradeName, cir.SchemeName, cir.TSPName, cir.TrainingCentreAddress, cl.ClassSize FROM 
//                                    AMSClassInspectionRequest AS cir INNER JOIN Class AS cl ON cir.ClassID = cl.ID 
//                                    WHERE cir.ID = " + requestID;

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                while (objIDataReader.Read())
//                {
//                    classID = objIDataReader["ClassID"].ToString();
//                    objPV.SchemeName = objIDataReader["SchemeName"].ToString();
//                    objPV.TSPName = objIDataReader["TSPName"].ToString();
//                    objPV.CentreAddress = objIDataReader["TrainingCentreAddress"].ToString();
//                    objPV.ClassCode = objIDataReader["ClassCode"].ToString();
//                    objPV.TradeName = objIDataReader["TradeName"].ToString();

//                    objPV.registeredTraineesCount = Convert.ToInt16(objIDataReader["ClassSize"].ToString());
//                }

//                // Get Latest class monitoring detail
//                strSql = @"SELECT TOP 1 ID, VisitDateTime, VisitNo, TraineeCountRemarks, IsLock FROM AMSClassMonitoring WHERE ClassInspectionRequestID = " + requestID + dateClause + " ORDER BY VisitNo DESC";
//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                while (objIDataReader.Read())
//                {
//                    mrID = objIDataReader["ID"].ToString();
//                    objPV.VisitDate = DateTime.Parse(objIDataReader["VisitDateTime"].ToString()).ToString("dd-MMM-yy");
//                    objPV.TraineeCountRemarks = objIDataReader["TraineeCountRemarks"].ToString();
//                    objPV.IsLock = objIDataReader["IsLock"].ToString();
//                }

//                if (objPV.IsLock == "y")
//                {
//                    objPV.presentTraineesCount = 0;

//                    // Get Trainees list
//                    strSql = @"SELECT * from AMSClassTrainee WHERE ClassID = '" + classID + "'";

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objIDataReader = TMData.ExecuteReader(objDbCommand);

//                    index = 1;

//                    while (objIDataReader.Read())
//                    {

//                        name = objIDataReader["Name"].ToString();
//                        fatherName = objIDataReader["FatherName"].ToString();
//                        cnic = objIDataReader["Cnic"].ToString();
//                        statusText = "Not Verfied";
//                        remarks = @"Class was found locked/non-functional";

//                        objPV.AddTrainee(index.ToString(), name, fatherName, cnic, statusText, remarks);
//                        index++;
//                    }
//                }
//                else
//                {
//                    // Get present trainees count against latest monitoring id

//                    strSql = @"Select ClassMonitoringID,
//                        count(CASE WHEN ((AttendanceStatus='present' AND VerificationStatus != 'fake') OR (AttendanceStatus='blank_space' AND IsPresent='y' AND VerificationStatus != 'fake')) THEN 1 ELSE null END) as trainee_present_count
//                        FROM AMSClassMonitoringTrainee 
//                        WHERE ClassMonitoringID = " + mrID + " Group By ClassMonitoringID";

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objIDataReader = TMData.ExecuteReader(objDbCommand);

//                    while (objIDataReader.Read())
//                    {
//                        objPV.presentTraineesCount = Convert.ToInt16(objIDataReader["trainee_present_count"].ToString());
//                    }

//                    // Get Trainees list
//                    strSql = @"SELECT usr.DisplayName, usr.FatherName, usr.Cnic, cl_mr_tr.AttendanceStatus, cl_mr_tr.IsPresent, cl_mr_tr.VerificationStatus FROM 
//                            AMSClassMonitoringTrainee AS cl_mr_tr INNER JOIN UserInfo AS usr ON cl_mr_tr.TraineeID = usr.ID
//                            WHERE cl_mr_tr.ClassMonitoringID = " + mrID;

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objIDataReader = TMData.ExecuteReader(objDbCommand);

//                    index = 1;

//                    while (objIDataReader.Read())
//                    {

//                        name = objIDataReader["DisplayName"].ToString();
//                        fatherName = objIDataReader["FatherName"].ToString();
//                        cnic = objIDataReader["Cnic"].ToString();
//                        attStatus = objIDataReader["AttendanceStatus"].ToString();
//                        veriStatus = objIDataReader["VerificationStatus"].ToString();
//                        isPresent = objIDataReader["IsPresent"].ToString();

//                        if (attStatus == "present")
//                        {
//                            if (veriStatus != "fake")
//                            {
//                                statusText = "Verified";

//                                if (veriStatus == "not_verified")
//                                {
//                                    remarks = "The trainee was present but reported as Not Verified by field monitor";
//                                }
//                                else
//                                {
//                                    remarks = "The trainee was present on the date of visit";
//                                }
//                            }
//                            else
//                            {
//                                statusText = "Not Verified";
//                                if (veriStatus == "fake")
//                                {
//                                    remarks = "The trainee was present but was reported Fake by field monitor";
//                                }
//                            }
//                        }
//                        else
//                        {

//                            if (attStatus == "blank_space" && isPresent == "y")
//                            {

//                                if (veriStatus == "fake")
//                                {
//                                    statusText = "Not Verified";
//                                    remarks = "The trainee was present but fake and there was blank space against this trainee";
//                                }
//                                else
//                                {
//                                    statusText = "Verified";
//                                    remarks = "The trainee was present but there was blank space against this trainee";
//                                }

//                            }
//                            else
//                            {
//                                statusText = "Not Verified";

//                                if (veriStatus == "fake")
//                                {
//                                    remarks = "The trainee was absent, fake and there was blank space against this trainee";
//                                }
//                                else
//                                {
//                                    remarks = statusValues[attStatus];
//                                }

//                            }
//                        }

//                        objPV.AddTrainee(index.ToString(), name, fatherName, cnic, statusText, remarks);
//                        index++;
//                    }
//                }



//                return objPV;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objIDataReader != null && !objIDataReader.IsClosed)
//                {
//                    objIDataReader.Close();
//                    objIDataReader.Dispose();
//                    objIDataReader = null;
//                }
//                if (objDbCommand != null)
//                {
//                    if (objDbCommand.Connection != null)
//                    {
//                        if (objDbCommand.Connection.State != ConnectionState.Closed)
//                            objDbCommand.Connection.Close();
//                        objDbCommand.Connection.Dispose();
//                        objDbCommand.Connection = null;
//                    }
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }
//            }

//        }
//        /* Get violation column name based on scheme type */
//        private string getVioTypeColumnNameBySchemeType(string schemeType)
//        {
//            string vioTypeCol = @"";
//            if (schemeType == schNA)
//            {
//                vioTypeCol = "Type";
//            }
//            else if (schemeType == schFormal)
//            {
//                vioTypeCol = "TypeFti";
//            }
//            else if (schemeType == schCommunity)
//            {
//                vioTypeCol = "TypeCommunity";
//            }
//            else if (schemeType == schIndustrial)
//            {
//                vioTypeCol = "TypeIndustrial";
//            }
//            return vioTypeCol;
//        }
//        /******************************************* Class Monitoring Report *********************************************/

//        public ClassMonitoringReport LoadClassMrReport(string requestID, string dateStr)
//        {

//            IDataReader objIDataReader = null;
//            DbCommand objDbCommand = null;
//            ClassMonitoringReport objClassReport = null;
//            Dictionary<string, int> mrIDHash = new Dictionary<string, int>();
//            int index = 0;
//            Dictionary<string, string> yesNoValues = new Dictionary<string, string>()
//            {
//                {"y", "Yes"},
//                {"n", "No"},
//                {"na", "N/A"}
//            };

//            try
//            {
//                string dateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", dateStr);

//                string strSql = @"SELECT Class.SchemeID, req.ClassID, req.ClassCode, req.TradeName, req.SchemeName, req.SchemeType, req.TSPName, req.InstructorsInfo, Center.Name as CentreName,
//                                cl_mr.ID as ClMrID, cl_mr.TraineesImported, cl_mr.VisitDateTime, cl_mr.VisitNo, cl_mr.IsLock, cl_mr.IsLockRemarks, cl_mr.IsRelocated, cl_mr.IsRelocatedRemarks,
//                                cl_mr.IsEquipmentAvailable, cl_mr.IsEquipmentAvailableRemarks, cl_mr.InstructorChanged, cl_mr.IsAllocatedTrainerRemarks,
//                                cl_mr.AttendRegisterAval, cl_mr.TraineeCount, cl_mr.TraineeAttendCountOne, cl_mr.TraineeAttendCountTwo, cl_mr.TraineeAttendCountThree,
//                                cl_mr.SignOffTspRemarks, cl_mr.SignOffFmRemarks, cl_mr_fb.TraineeProb1, cl_mr_fb.TraineeProb2, cl_mr_fb.FMRemarks,
//                                cl_mr.FMSignatureImgPath, cl_mr.TspSignatureImgPath
//                                FROM 
//                                AMSClassInspectionRequest as req 
//                                INNER JOIN AMSClassMonitoring as cl_mr ON req.ID = cl_mr.ClassInspectionRequestID
//                                LEFT JOIN AMSClassMonitoringTraineeFeedback as cl_mr_fb ON cl_mr.ID = cl_mr_fb.ClassMonitoringID
//								INNER JOIN Class on req.ClassID = Class.ID
//								INNER JOIN Center on Class.CentreID = Center.ID
//                                WHERE cl_mr.ClassInspectionRequestID = " + requestID + dateClause +
//                                " ORDER BY cl_mr.VisitNo ASC";

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                objClassReport = new ClassMonitoringReport();
//                Dictionary<string, string> objParameter = null;

//                string mrIDsJoined = @"";
//                string visitNoStr = @"";
//                int visitNo = 1;

//                string lockRemarks, equipRemarks, relocatedRemarks, instructorRemarks, traineeCount1, traineeCount2, traineeCount3;
//                lockRemarks = equipRemarks = relocatedRemarks = instructorRemarks = traineeCount1 = traineeCount2 = traineeCount3 = string.Empty;
//                string schemeType = string.Empty;

//                while (objIDataReader.Read())
//                {
//                    objParameter = new Dictionary<string, string>();
//                    mrIDsJoined += objIDataReader["ClMrID"] + ",";

//                    if (index == 0)
//                    {
//                        objClassReport.SchemeID = objIDataReader["SchemeID"].ToString();
//                        objClassReport.SchemeName = objIDataReader["SchemeName"].ToString();
//                        objClassReport.TSPName = objIDataReader["TSPName"].ToString();
//                        objClassReport.CentreName = objIDataReader["CentreName"].ToString();
//                        objClassReport.ClassCode = objIDataReader["ClassCode"].ToString();
//                        objClassReport.TradeName = objIDataReader["TradeName"].ToString();
//                        objClassReport.ReportingMonth = DateTime.Parse(objIDataReader["VisitDateTime"].ToString()).ToString("MMM-yy");
//                        objClassReport.TrainerName = objIDataReader["InstructorsInfo"].ToString();
//                        objClassReport.FMRemarks = objIDataReader["SignOffFmRemarks"].ToString();
//                        objClassReport.TSPRemarks = objIDataReader["SignOffTspRemarks"].ToString();
//                        schemeType = objIDataReader["SchemeType"].ToString();
//                    }


//                    if (Convert.ToInt16(objIDataReader["TraineesImported"]) == 0)
//                    {
//                        objParameter.Add("trainee_present_count", objIDataReader["TraineeCount"].ToString());
//                    }
//                    else
//                    {
//                        objParameter.Add("trainee_present_count", "0");
//                    }

//                    objParameter.Add("trainees_imported", objIDataReader["TraineesImported"].ToString());
//                    objParameter.Add("is_lock", yesNoValues[objIDataReader["IsLock"].ToString()]);

//                    // set TSP Signature image
//                    string imageUrl = objIDataReader["TspSignatureImgPath"].ToString();
//                    if (imageUrl != null && imageUrl != string.Empty)
//                    {
//                        imageUrl = IMAGES_PATH + "/class/" + objIDataReader["ClassCode"].ToString() + "/" + imageUrl;
//                    }

//                    objParameter.Add("signature_tsp_rep", imageUrl);

//                    // set FM Signature image
//                    imageUrl = objIDataReader["FMSignatureImgPath"].ToString();
//                    if (imageUrl != null && imageUrl != string.Empty)
//                    {
//                        imageUrl = IMAGES_PATH + "/class/" + objIDataReader["ClassCode"].ToString() + "/" + imageUrl;
//                    }

//                    objParameter.Add("signature_fm", imageUrl);

//                    if (objIDataReader["IsLock"].ToString() == "y")
//                    {
//                        objParameter.Add("is_relocated", yesNoValues["na"]);
//                        objParameter.Add("is_equip_avail", yesNoValues["na"]);
//                        objParameter.Add("instructor_changed", yesNoValues["na"]);
//                        objParameter.Add("tranee_attend_one", yesNoValues["na"]);
//                        objParameter.Add("tranee_attend_two", yesNoValues["na"]);
//                        objParameter.Add("tranee_attend_three", yesNoValues["na"]);
//                        objParameter["trainee_present_count"] = yesNoValues["na"];
//                    }
//                    else
//                    {
//                        objParameter.Add("is_relocated", yesNoValues[objIDataReader["IsRelocated"].ToString()]);
//                        objParameter.Add("is_equip_avail", yesNoValues[objIDataReader["IsEquipmentAvailable"].ToString()]);
//                        objParameter.Add("instructor_changed", yesNoValues[objIDataReader["InstructorChanged"].ToString()]);

//                        if (objIDataReader["AttendRegisterAval"].ToString() == "n")
//                        {
//                            objParameter.Add("tranee_attend_one", yesNoValues["na"]);
//                            objParameter.Add("tranee_attend_two", yesNoValues["na"]);
//                            objParameter.Add("tranee_attend_three", yesNoValues["na"]);
//                        }
//                        else
//                        {
//                            traineeCount1 = objIDataReader["TraineeAttendCountOne"].ToString();
//                            traineeCount2 = objIDataReader["TraineeAttendCountTwo"].ToString();
//                            traineeCount3 = objIDataReader["TraineeAttendCountThree"].ToString();

//                            if (traineeCount1 == "-1")
//                            {
//                                traineeCount1 = yesNoValues["na"];
//                            }
//                            if (traineeCount2 == "-1")
//                            {
//                                traineeCount2 = yesNoValues["na"];
//                            }
//                            if (traineeCount3 == "-1")
//                            {
//                                traineeCount3 = yesNoValues["na"];
//                            }
//                            objParameter.Add("tranee_attend_one", traineeCount1);
//                            objParameter.Add("tranee_attend_two", traineeCount2);
//                            objParameter.Add("tranee_attend_three", traineeCount3);
//                        }

//                    }

//                    objParameter.Add("fm_trainee_remarks", "");
//                    if (objIDataReader["SignOffFmRemarks"].ToString() != string.Empty && objIDataReader["SignOffFmRemarks"].ToString() != null)
//                    {
//                        objParameter["fm_trainee_remarks"] += "FM Remarks: " + objIDataReader["SignOffFmRemarks"].ToString() + "\n";
//                    }

//                    if (objIDataReader["TraineeProb1"].ToString() != string.Empty && objIDataReader["TraineeProb1"].ToString() != null)
//                    {
//                        objParameter["fm_trainee_remarks"] += "Trainee Problem 1: " + objIDataReader["TraineeProb1"].ToString() + "\n";
//                    }

//                    if (objIDataReader["TraineeProb2"].ToString() != string.Empty && objIDataReader["TraineeProb2"].ToString() != null)
//                    {
//                        objParameter["fm_trainee_remarks"] += "Trainee Problem 2: " + objIDataReader["TraineeProb2"].ToString() + "\n";
//                    }

//                    if (objIDataReader["FMRemarks"].ToString() != string.Empty && objIDataReader["FMRemarks"].ToString() != null)
//                    {
//                        objParameter["fm_trainee_remarks"] += "FM Remarks on Trainee Feedback: " + objIDataReader["FMRemarks"].ToString() + "\n";
//                    }

//                    if (visitNo == 1)
//                    {
//                        visitNoStr += "I,";

//                    }
//                    else if (visitNo == 2)
//                    {
//                        visitNoStr += "II,";
//                    }
//                    else if (visitNo == 3)
//                    {
//                        visitNoStr += "III,";
//                    }
//                    else if (visitNo == 4)
//                    {
//                        visitNoStr += "IV";
//                    }

//                    if (objIDataReader["IsLockRemarks"].ToString() != string.Empty && objIDataReader["IsLockRemarks"].ToString() != null)
//                    {
//                        lockRemarks += "Visit " + visitNo + ": " + objIDataReader["IsLockRemarks"].ToString() + "\n";
//                    }

//                    if (objIDataReader["IsRelocatedRemarks"].ToString() != string.Empty && objIDataReader["IsRelocatedRemarks"].ToString() != null)
//                    {
//                        relocatedRemarks += "Visit " + visitNo + ": " + objIDataReader["IsRelocatedRemarks"].ToString() + "\n";
//                    }

//                    if (objIDataReader["IsEquipmentAvailableRemarks"].ToString() != string.Empty && objIDataReader["IsEquipmentAvailableRemarks"].ToString() != null)
//                    {
//                        equipRemarks += "Visit " + visitNo + ": " + objIDataReader["IsEquipmentAvailableRemarks"].ToString() + "\n";
//                    }

//                    if (objIDataReader["IsAllocatedTrainerRemarks"].ToString() != string.Empty && objIDataReader["IsAllocatedTrainerRemarks"].ToString() != null)
//                    {
//                        instructorRemarks += "Visit " + visitNo + ": " + objIDataReader["IsAllocatedTrainerRemarks"].ToString() + "\n";
//                    }

//                    //objClassReport.VisitNo += objIDataReader["VisitNo"].ToString() + ",";

//                    objParameter.Add("visit_date", DateTime.Parse(objIDataReader["VisitDateTime"].ToString()).ToString("dd MMMM yyyy"));
//                    objParameter.Add("visit_time", DateTime.Parse(objIDataReader["VisitDateTime"].ToString()).ToString("h:mm tt"));
//                    objParameter.Add("visit_no", objIDataReader["VisitNo"].ToString());


//                    objClassReport.MrInfo.Add(index, objParameter);

//                    //  Set trianee feedback data for each monitoring
//                    objClassReport.TraineeFeedback.Add(new Dictionary<string, string>());

//                    // Set Violations data for each monitoring
//                    objClassReport.ViolationsList.Add(new Dictionary<string, string>());

//                    mrIDHash[objIDataReader["ClMrID"].ToString()] = index;

//                    visitNo++;
//                    index++;
//                }

//                objClassReport.IsLockRemarks = lockRemarks;
//                objClassReport.IsRelocatedRemarks = relocatedRemarks;
//                objClassReport.EqipmentAvailRemarks = equipRemarks;
//                objClassReport.InstructorRemarks = instructorRemarks;

//                visitNoStr = visitNoStr.TrimEnd(',');
//                objClassReport.VisitNo = visitNoStr;

//                mrIDsJoined = mrIDsJoined.TrimEnd(',');

//                // Get present trainees count

//                strSql = @"SELECT ClassMonitoringID, COUNT(CASE WHEN ((AttendanceStatus='present' AND VerificationStatus != 'fake') OR (AttendanceStatus='blank_space' AND IsPresent='y' AND VerificationStatus != 'fake')) THEN 1 ELSE null END) as trainee_present_count
//                        FROM AMSClassMonitoringTrainee
//                        WHERE ClassMonitoringID in (" + mrIDsJoined + ") GROUP BY ClassMonitoringID; ";

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                while (objIDataReader.Read())
//                {
//                    index = mrIDHash[objIDataReader["ClassMonitoringID"].ToString()];
//                    if (objClassReport.MrInfo[index]["trainees_imported"] == "1")
//                    {
//                        objClassReport.MrInfo[index]["trainee_present_count"] = objIDataReader["trainee_present_count"].ToString();
//                    }

//                }

//                string vioTypeCol = getVioTypeColumnNameBySchemeType(schemeType);

//                // Get Violations Count
//                strSql = string.Format(@"SELECT 
//                    count(CASE WHEN vio.{0}='major' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) AS major_count,
//                    count(CASE WHEN vio.{0}='minor' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) as minor_count,
//                    count(CASE WHEN vio.{0}='serious' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) as serious_count,
//                    count(CASE WHEN vio.{0}='observation' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) as observation_count
//                    FROM AMSClassViolations as cl_vio 
//                    INNER JOIN AMSViolations as vio ON cl_vio.ViolationID = vio.ID 
//                    WHERE cl_vio.ClassMonitoringID in ({1})", vioTypeCol, mrIDsJoined);

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                while (objIDataReader.Read())
//                {
//                    objClassReport.MajorCount = Convert.ToInt16(objIDataReader["major_count"]);
//                    objClassReport.MinorCount = Convert.ToInt16(objIDataReader["minor_count"]);
//                    objClassReport.SeriousCount = Convert.ToInt16(objIDataReader["serious_count"]);
//                    objClassReport.ObservationCount = Convert.ToInt16(objIDataReader["observation_count"]);
//                }

//                if (objClassReport.SchemeID == "8a078782-4936-4a93-8cbf-d7a07a2300e6")
//                {
//                    objClassReport.ObservationCount = objClassReport.MajorCount + objClassReport.MinorCount + objClassReport.SeriousCount + objClassReport.ObservationCount;
//                    objClassReport.MajorCount = 0;
//                    objClassReport.MinorCount = 0;
//                    objClassReport.SeriousCount = 0;
//                }

//                // Get static violations to set violation type based on scheme
//                strSql = @"SELECT * from AMSViolations";
//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);
//                while (objIDataReader.Read())
//                {
//                    objClassReport.ViolationType.Add(objIDataReader["ID"].ToString(), objIDataReader[vioTypeCol].ToString());
//                }


//                // Trainee Feedback and violations
//                strSql = @"SELECT cl_vio.ClassMonitoringID, cl_mr.VisitNo, cl_vio.ViolationID, cl_vio.IsViolation, cl_vio.PercentageSatisfied, 
//                        vio.Name, vio.Type , vio.TypeFti, vio.TypeCommunity, vio.TypeIndustrial
//                        FROM AMSClassViolations as cl_vio 
//                        INNER JOIN AMSViolations as vio ON cl_vio.ViolationID = vio.ID 
//                        INNER JOIN AMSClassMonitoring as cl_mr on cl_mr.ID = cl_vio.ClassMonitoringID
//                        WHERE cl_vio.ClassMonitoringID in (" + mrIDsJoined + ") ORDER BY cl_mr.VisitNo ASC";

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                Dictionary<string, string> objDict = null;
//                string violationIDStr = @"";
//                int violationIDInt = 0;
//                string percentSatis = @"";
//                string isViolation = @"";
//                string violationType = @"";

//                Dictionary<string, Dictionary<string, string>> visitWiseData = new Dictionary<string, Dictionary<string, string>>();

//                while (objIDataReader.Read())
//                {

//                    index = mrIDHash[objIDataReader["ClassMonitoringID"].ToString()];
//                    violationIDStr = objIDataReader["ViolationID"].ToString();
//                    violationIDInt = Convert.ToInt16(objIDataReader["ViolationID"]);
//                    violationType = objIDataReader[vioTypeCol].ToString();

//                    if (violationIDInt >= 21 && violationIDInt <= 27)
//                    {
//                        if (objIDataReader["IsViolation"].ToString() == "na")
//                        {
//                            percentSatis = yesNoValues["na"];
//                        }
//                        else
//                        {
//                            percentSatis = objIDataReader["PercentageSatisfied"].ToString() + "%";
//                        }

//                        objClassReport.TraineeFeedback[index].Add(violationIDStr, percentSatis);
//                    }
//                    //Change by Rao Ali Haider  
//                    //06-Jan-2020
//                    else if ((Convert.ToInt16(objIDataReader["ViolationID"]) >= 1 && Convert.ToInt16(objIDataReader["ViolationID"]) <= 20) ||
//                            ((Convert.ToInt16(objIDataReader["ViolationID"]) >= 28 && Convert.ToInt16(objIDataReader["ViolationID"]) <= 31)))
//                    {

//                        isViolation = yesNoValues[objIDataReader["IsViolation"].ToString()];

//                        // if class is not locked, Fake/Ghost trainee violation can never be N/A
//                        if (violationIDInt == 7 && objIDataReader["IsViolation"].ToString() == "na")
//                        {
//                            isViolation = yesNoValues["n"];
//                        }

//                        objClassReport.ViolationsList[index].Add(violationIDStr, isViolation);
//                    }

//                }


//                // If class is locked, set traineefeedback and violations answers to N/A.
//                for (index = 0; index < objClassReport.MrInfo.Count; index++)
//                {
//                    // Violation # 28, 29 and 30 does not exist for scheme type "na"
//                    if (!objClassReport.ViolationsList[index].ContainsKey("28"))
//                    {
//                        objClassReport.ViolationsList[index].Add("28", yesNoValues["na"]);
//                    }
//                    if (!objClassReport.ViolationsList[index].ContainsKey("29"))
//                    {
//                        objClassReport.ViolationsList[index].Add("29", yesNoValues["na"]);
//                    }
//                    if (!objClassReport.ViolationsList[index].ContainsKey("30"))
//                    {
//                        objClassReport.ViolationsList[index].Add("30", yesNoValues["na"]);
//                    }

//                    if (objClassReport.MrInfo[index]["is_lock"] == yesNoValues["y"])
//                    {
//                        objClassReport.TraineeFeedback[index].Add("21", yesNoValues["na"]);
//                        objClassReport.TraineeFeedback[index].Add("22", yesNoValues["na"]);
//                        objClassReport.TraineeFeedback[index].Add("23", yesNoValues["na"]);
//                        objClassReport.TraineeFeedback[index].Add("24", yesNoValues["na"]);
//                        objClassReport.TraineeFeedback[index].Add("25", yesNoValues["na"]);
//                        objClassReport.TraineeFeedback[index].Add("26", yesNoValues["na"]);
//                        objClassReport.TraineeFeedback[index].Add("27", yesNoValues["na"]);

//                        objClassReport.ViolationsList[index].Add("4", yesNoValues["na"]);
//                        objClassReport.ViolationsList[index].Add("8", yesNoValues["na"]);
//                        objClassReport.ViolationsList[index].Add("9", yesNoValues["na"]);
//                        objClassReport.ViolationsList[index].Add("10", yesNoValues["na"]);
//                        objClassReport.ViolationsList[index].Add("11", yesNoValues["na"]);
//                        objClassReport.ViolationsList[index].Add("12", yesNoValues["na"]);
//                        objClassReport.ViolationsList[index].Add("13", yesNoValues["na"]);

//                        objClassReport.ViolationsList[index]["7"] = yesNoValues["na"];
//                        objClassReport.ViolationsList[index]["1"] = yesNoValues["na"];
//                        objClassReport.ViolationsList[index]["3"] = yesNoValues["na"];

//                        objClassReport.ViolationsList[index]["28"] = yesNoValues["na"];
//                        objClassReport.ViolationsList[index]["29"] = yesNoValues["na"];
//                        objClassReport.ViolationsList[index]["30"] = yesNoValues["na"];
//                    }
//                }

//                // Get Fake, Ghost and Marginal, deleted, dropuout trainees list
//                string marginalStatus = string.Empty;
//                if (schemeType == "na")
//                {
//                    marginalStatus = "OR IsMarginal='y'";
//                }

//                strSql = @"SELECT cl_mr_tr.ID as ClMrTraineeID, tr.ID as UserID, cl_mr_tr.IsPresent, cl_mr_tr.AttendanceStatus, cl_mr_tr.VerificationStatus, cl_mr_tr.IsMarginal, cl_mr_tr.IsGhost, 
//                    tr.DisplayName, tr.FatherName, tr.Cnic
//                    FROM AMSClassMonitoringTrainee AS cl_mr_tr INNER JOIN UserInfo as tr ON cl_mr_tr.TraineeID=tr.ID
//                    WHERE cl_mr_tr.ClassMonitoringID in (" + mrIDsJoined + ") AND (VerificationStatus='fake' OR IsGhost='y' " + marginalStatus + " OR AttendanceStatus = 'deleted' OR AttendanceStatus = 'dropout' OR AttendanceStatus='dropout_but_present')";

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                Dictionary<string, string> trainee = null;
//                bool traineeReported = false;
//                string remarks = string.Empty;

//                while (objIDataReader.Read())
//                {
//                    trainee = new Dictionary<string, string>();

//                    if (objIDataReader["IsGhost"].ToString() == "y")
//                    {
//                        traineeReported = true;
//                        objClassReport.GhostTrainees.Add(trainee);
//                        remarks = "Ghost";
//                    }

//                    if (objIDataReader["IsMarginal"].ToString() == "y")
//                    {
//                        traineeReported = true;
//                        objClassReport.MarginalTrainees.Add(trainee);
//                        remarks = "Marginal";
//                    }

//                    if (objIDataReader["VerificationStatus"].ToString() == "fake")
//                    {
//                        traineeReported = true;
//                        objClassReport.FakeTrainees.Add(trainee);
//                        remarks = "Fake";
//                    }

//                    if (objIDataReader["AttendanceStatus"].ToString() == "deleted")
//                    {
//                        traineeReported = true;
//                        objClassReport.DeletedTrainees.Add(trainee);
//                        remarks = "Deleted";
//                    }
//                    else if (objIDataReader["AttendanceStatus"].ToString() == "dropout" || objIDataReader["AttendanceStatus"].ToString() == "dropout_but_present")
//                    {
//                        traineeReported = true;
//                        objClassReport.DropoutTrainees.Add(trainee);
//                        if (objIDataReader["AttendanceStatus"].ToString() == "dropout")
//                        {
//                            remarks = "Dropout";
//                        }
//                        else
//                        {
//                            remarks = "Dropout but Marked Present";
//                        }
//                    }

//                    if (traineeReported == true)
//                    {
//                        trainee.Add("name", objIDataReader["DisplayName"].ToString());
//                        trainee.Add("father_name", objIDataReader["FatherName"].ToString());
//                        trainee.Add("cnic", objIDataReader["Cnic"].ToString());
//                        trainee.Add("remarks", remarks);
//                        traineeReported = false;
//                    }
//                }

//                /* Get additional, ghost and marginal trainees */

//                strSql = @"SELECT * FROM AMSClassMonitoringReportedTrainee WHERE ClassMonitoringID in (" + mrIDsJoined + ")";
//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                remarks = string.Empty;

//                while (objIDataReader.Read())
//                {
//                    trainee = new Dictionary<string, string>();
//                    trainee.Add("name", objIDataReader["Name"].ToString());
//                    trainee.Add("father_name", objIDataReader["FatherName"].ToString());
//                    trainee.Add("cnic", objIDataReader["Cnic"].ToString());

//                    if (objIDataReader["Type"].ToString() == "additional")
//                    {
//                        objClassReport.AdditionalTrainees.Add(trainee);
//                        remarks = "Additional";
//                    }
//                    else if (objIDataReader["Type"].ToString() == "marginal")
//                    {
//                        objClassReport.MarginalTrainees.Add(trainee);
//                        remarks = "Marginal";
//                    }
//                    else if (objIDataReader["Type"].ToString() == "ghost")
//                    {
//                        objClassReport.GhostTrainees.Add(trainee);
//                        remarks = "Ghost";
//                    }
//                    trainee.Add("remarks", remarks);
//                }

//                // Marginal Trainees of scheme type != na
//                if (schemeType != "na")
//                {
//                    string monthString = dateStr.ToDateTime().ToString("yyyy-MM-01");
//                    strSql = @"SELECT cl_mr_tr_marginal.ID as ClMrTraineeID, 
//                                cl_mr_tr_marginal.ClassInspectionRequestID, 
//                                cl_mr_tr_marginal.VisitMonth,
//                                tr.DisplayName AS Name,
//                                tr.FatherName,
//                                tr.Cnic
//                                FROM AMSClassMonitoringTraineeMarginal AS cl_mr_tr_marginal 
//                                INNER JOIN UserInfo as tr ON cl_mr_tr_marginal.TraineeID=tr.ID
//                                WHERE ClassInspectionRequestID = " + requestID + " AND VisitMonth = '" + monthString + "'";

//                    strSql = strSql.Replace("\r\n", string.Empty);

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objIDataReader = TMData.ExecuteReader(objDbCommand);

//                    while (objIDataReader.Read())
//                    {
//                        trainee = new Dictionary<string, string>();
//                        trainee.Add("name", objIDataReader["Name"].ToString());
//                        trainee.Add("father_name", objIDataReader["FatherName"].ToString());
//                        trainee.Add("cnic", objIDataReader["Cnic"].ToString());
//                        trainee.Add("remarks", remarks);

//                        objClassReport.MarginalTrainees.Add(trainee);
//                        remarks = "Marginal";
//                    }
//                }

//                // remove duplicate fake trainees
//                removeDuplicateTrainees(objClassReport);

//                return objClassReport;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objIDataReader != null && !objIDataReader.IsClosed)
//                {
//                    objIDataReader.Close();
//                    objIDataReader.Dispose();
//                    objIDataReader = null;
//                }
//                if (objDbCommand != null)
//                {
//                    if (objDbCommand.Connection != null)
//                    {
//                        if (objDbCommand.Connection.State != ConnectionState.Closed)
//                            objDbCommand.Connection.Close();
//                        objDbCommand.Connection.Dispose();
//                        objDbCommand.Connection = null;
//                    }
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }
//            }

//        }

//        /************** Remove duplicate trainees *********************/
//        private static void removeDuplicateTrainees(ClassMonitoringReport objClassReport)
//        {
//            List<string> objList = new List<string>();
//            int totalItems = objClassReport.FakeTrainees.Count;
//            int index = 0;
//            string cnic = string.Empty;

//            // Remove Duplicate Fake Trainees
//            for (index = 0; index < totalItems; index++)
//            {
//                if (!objList.Contains(objClassReport.FakeTrainees[index]["cnic"]))
//                {
//                    objList.Add(objClassReport.FakeTrainees[index]["cnic"]);
//                }
//                else
//                {
//                    objClassReport.FakeTrainees.RemoveAt(index);
//                    index--;
//                    totalItems = totalItems - 1;
//                }
//            }


//            // remove duplicate Ghost trainees
//            totalItems = objClassReport.GhostTrainees.Count;
//            objList.Clear();

//            for (index = 0; index < totalItems; index++)
//            {
//                cnic = objClassReport.GhostTrainees[index]["cnic"];

//                if (cnic != null && cnic != string.Empty)
//                {
//                    if (!objList.Contains(objClassReport.GhostTrainees[index]["cnic"]))
//                    {
//                        objList.Add(objClassReport.GhostTrainees[index]["cnic"]);
//                    }
//                    else
//                    {
//                        objClassReport.GhostTrainees.RemoveAt(index);
//                        index--;
//                        totalItems = totalItems - 1;
//                    }
//                }
//            }

//            // remove duplicate marginal trainees
//            totalItems = objClassReport.MarginalTrainees.Count;
//            objList.Clear();

//            for (index = 0; index < totalItems; index++)
//            {
//                cnic = objClassReport.MarginalTrainees[index]["cnic"];

//                if (cnic != null && cnic != string.Empty)
//                {
//                    if (!objList.Contains(objClassReport.MarginalTrainees[index]["cnic"]))
//                    {
//                        objList.Add(objClassReport.MarginalTrainees[index]["cnic"]);
//                    }
//                    else
//                    {
//                        objClassReport.MarginalTrainees.RemoveAt(index);
//                        index--;
//                        totalItems = totalItems - 1;
//                    }
//                }

//            }


//            // remove duplicate deleted trainees
//            totalItems = objClassReport.DeletedTrainees.Count;
//            objList.Clear();

//            for (index = 0; index < totalItems; index++)
//            {
//                if (!objList.Contains(objClassReport.DeletedTrainees[index]["cnic"]))
//                {
//                    objList.Add(objClassReport.DeletedTrainees[index]["cnic"]);
//                }
//                else
//                {
//                    objClassReport.DeletedTrainees.RemoveAt(index);
//                    index--;
//                    totalItems = totalItems - 1;
//                }
//            }

//            // remove duplicate dropout trainees

//            totalItems = objClassReport.DropoutTrainees.Count;
//            objList.Clear();

//            for (index = 0; index < totalItems; index++)
//            {
//                if (!objList.Contains(objClassReport.DropoutTrainees[index]["cnic"]))
//                {
//                    objList.Add(objClassReport.DropoutTrainees[index]["cnic"]);
//                }
//                else
//                {
//                    objClassReport.DropoutTrainees.RemoveAt(index);
//                    index--;
//                    totalItems = totalItems - 1;
//                }
//            }

//            // remove duplicate additional trainees

//            totalItems = objClassReport.AdditionalTrainees.Count;
//            objList.Clear();


//            for (index = 0; index < totalItems; index++)
//            {
//                cnic = objClassReport.AdditionalTrainees[index]["cnic"];

//                if (cnic != null && cnic != string.Empty)
//                {
//                    if (!objList.Contains(objClassReport.AdditionalTrainees[index]["cnic"]))
//                    {
//                        objList.Add(objClassReport.AdditionalTrainees[index]["cnic"]);
//                    }
//                    else
//                    {
//                        objClassReport.AdditionalTrainees.RemoveAt(index);
//                        index--;
//                        totalItems = totalItems - 1;
//                    }
//                }
//            }

//            objList.Clear();
//        }


//        /************************** TSP Class Violation Summary Report (Form IV) *************************************/
//        public ViolationSummary LoadTSPViolationSummaryReport(string schemeID, string TSPID, string dateStr)
//        {
//            IDataReader objIDataReader = null;
//            DbCommand objDbCommand = null;
//            ViolationSummary objViolationSummary = null;

//            string SelectedDate = dateStr.ToDateTime().ToString("yyyy/MM/dd");
//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", SelectedDate);


//            Dictionary<string, string> classIDHash = new Dictionary<string, string>();
//            Dictionary<string, Dictionary<string, string>> classIDRemarks = new Dictionary<string, Dictionary<string, string>>();

//            Dictionary<int, int> visitsCountInfo = new Dictionary<int, int>();
//            Dictionary<string, string> objDict = new Dictionary<string, string>();

//            int index, key, value;
//            index = key = value = 0;
//            int totalClassViolations, majorCountInt, minorCountInt, seriousCountInt, observationCountInt, allMajorCount, allMinorCount, allSeriousCount, allVioCount, allObservCount;
//            totalClassViolations = majorCountInt = minorCountInt = seriousCountInt = observationCountInt = allMajorCount = allMinorCount = allSeriousCount = allVioCount = allObservCount = 0;

//            string classIDsJoined, mrIDsJoined, classID, classCode, tspName, majorCount, minorCount, seriousCount, totalVioStr, observCount, remarks;
//            classIDsJoined = mrIDsJoined = classID = classCode = tspName = majorCount = minorCount = seriousCount = totalVioStr = observCount = remarks = string.Empty;

//            try
//            {
//                string strSql = @"SELECT Class.SchemeID, cir.SchemeName, cir.SchemeType, cir.TSPName, cir.ClassID, cir.ClassCode, cl_mr.ID AS ClMrID, cl_mr.VisitDateTime, cl_mr.VisitNo
//                                    FROM AMSClassInspectionRequest as cir 
//                                    INNER JOIN AMSClassMonitoring as cl_mr ON cir.ID = cl_mr.ClassInspectionRequestID
//                                    INNER JOIN Class on cir.ClassID = Class.ID 
//                                    INNER JOIN UserInfo on Class.SpID = UserInfo.ID
//                                    WHERE Class.SchemeID = '" + schemeID + "' AND Class.SpID = '" + TSPID + "'" + DateClause;

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                objViolationSummary = new ViolationSummary();

//                while (objIDataReader.Read())
//                {
//                    mrIDsJoined += objIDataReader["ClMrID"].ToString() + ",";

//                    classID = objIDataReader["ClassID"].ToString();
//                    if (classIDHash.ContainsKey(classID) == false)
//                    {
//                        classIDHash.Add(classID, objIDataReader["ClassCode"].ToString());
//                        classIDsJoined += "'" + objIDataReader["ClassID"] + "',";
//                    }

//                    if (classIDRemarks.ContainsKey(classID) == false)
//                    {
//                        objDict = new Dictionary<string, string>() {
//                                {"major" , string.Empty },
//                                {"minor" , string.Empty },
//                                {"serious" , string.Empty },
//                                {"observation" , string.Empty }
//                            };

//                        classIDRemarks.Add(classID, objDict);
//                    }

//                    if (index == 0)
//                    {
//                        objViolationSummary.SchemeID = objIDataReader["SchemeID"].ToString();
//                        objViolationSummary.SchemeName = objIDataReader["SchemeName"].ToString();
//                        objViolationSummary.SchemeType = objIDataReader["SchemeType"].ToString();
//                        objViolationSummary.TSPName = objIDataReader["TSPName"].ToString();
//                        objViolationSummary.ReportingMonth = dateStr.ToDateTime().ToString("MMM-yy");

//                    }
//                    index++;
//                }

//                if (index > 0)
//                {
//                    classIDsJoined = classIDsJoined.TrimEnd(',');
//                    mrIDsJoined = mrIDsJoined.TrimEnd(',');

//                    /********************** Get Visits Count info for each class ************************/
//                    strSql = @"SELECT cir.ClassID, count(cir.ClassID) as VisitsCount
//                                FROM AMSClassInspectionRequest as cir 
//                                INNER JOIN AMSClassMonitoring as cl_mr ON cir.ID = cl_mr.ClassInspectionRequestID
//                                WHERE cl_mr.ID IN (" + mrIDsJoined + ") GROUP BY cir.ClassID";

//                    strSql = strSql.Replace("\r\n", string.Empty);

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objIDataReader = TMData.ExecuteReader(objDbCommand);

//                    while (objIDataReader.Read())
//                    {
//                        key = Convert.ToInt16(objIDataReader["VisitsCount"]);

//                        if (visitsCountInfo.ContainsKey(key))
//                        {
//                            value = visitsCountInfo[key] + 1;
//                            visitsCountInfo[key] = value;
//                        }
//                        else
//                        {
//                            visitsCountInfo.Add(key, 1);
//                        }
//                    }

//                    if (visitsCountInfo.Count == 1)
//                    {
//                        List<int> keysList = new List<int>(visitsCountInfo.Keys);
//                        if (Convert.ToInt16(keysList[0]) == 1)
//                        {
//                            objViolationSummary.MonthlyVisitsInfo = keysList[0].ToString() + " visit for each class";
//                        }
//                        else
//                        {
//                            objViolationSummary.MonthlyVisitsInfo = keysList[0].ToString() + " visits for each class";
//                        }
//                    }
//                    else
//                    {
//                        string visitsText = string.Empty;
//                        string classesText = string.Empty;

//                        foreach (int dictKey in visitsCountInfo.Keys)
//                        {
//                            if (Convert.ToInt16(visitsCountInfo[dictKey]) == 1)
//                            {
//                                classesText = " class &";
//                            }
//                            else
//                            {
//                                classesText = " classes &";
//                            }

//                            if (Convert.ToInt16(dictKey) == 1)
//                            {
//                                visitsText = " visit for ";
//                            }
//                            else
//                            {
//                                visitsText = " visits for ";
//                            }


//                            objViolationSummary.MonthlyVisitsInfo += dictKey.ToString() + visitsText + visitsCountInfo[dictKey].ToString() + classesText;
//                        }
//                    }
//                    objViolationSummary.MonthlyVisitsInfo = objViolationSummary.MonthlyVisitsInfo.TrimEnd(' ');
//                    objViolationSummary.MonthlyVisitsInfo = objViolationSummary.MonthlyVisitsInfo.TrimEnd('&');

//                    // Get Remarks (Violations info) of each each

//                    Dictionary<string, string> objDictRemarks = new Dictionary<string, string>();
//                    Dictionary<string, string> violationNames = new Dictionary<string, string>()
//                    {
//                        {"attend_reg_aval" , "Trainee Attendance Register"},
//                        {"tr_count" , "Attendance Accuracy"},
//                        {"instructor_attend" , "Trainer Attendance Register"},
//                        {"stipend_recv" , "Payment of Stipend"},
//                        {"stipend_payment" , "Stipend Register Maintenance"},
//                        {"tr_id_card" , "Trainee ID Card"},
//                        {"fake_ghost_trainee" , "Fake/Ghost Trainee"},
//                        {"syllabus_provided" , "Syllabus"},
//                        {"free_training" , "Free Training"},
//                        {"books_recv" , "Books"},
//                        {"uniform_recv" , "Uniform"},
//                        {"stationary_recv" , "Stationary"},
//                        {"bags_recv" , "Bags"},
//                        {"is_relocated" , "Class Relocate"},
//                        {"is_merged" , "Class Merge"},
//                        {"is_split" , "Unannounced Shift"},
//                        {"is_shift_changed" , "Shift Change"},
//                        {"is_locked" , "Non-functional"},
//                        {"reg_format" , "Attendance Register Format"},
//                        {"instructor_changed_with_approval" , "Trainer Changed"},
//                        {"teacher_trainee_ratio" , "Teacher Trainee Ratio"},
//                        {"short_leave" , "Short Leave"},
//                        {"contractual_hours" , "Contractual Hours Followed"},
//                    };

//                    string vioTypeCol = getVioTypeColumnNameBySchemeType(objViolationSummary.SchemeType);
//                    string violationName, observationStr = string.Empty;
//                    string vioTypeValue = string.Empty;

//                    strSql = string.Format(@"SELECT cl_vio.ClassID, vio.Name, vio.{0}, cl_vio.IsViolation
//                            FROM AMSClassViolations as cl_vio 
//                            INNER JOIN AMSViolations as vio ON cl_vio.ViolationID = vio.ID 
//                            WHERE cl_vio.ClassMonitoringID IN ({1}) AND cl_vio.IsViolation='y' AND vio.{0} IN ('major','minor','serious','observation')", vioTypeCol, mrIDsJoined);

//                    strSql = strSql.Replace("\r\n", string.Empty);

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objIDataReader = TMData.ExecuteReader(objDbCommand);

//                    while (objIDataReader.Read())
//                    {

//                        classID = objIDataReader["ClassID"].ToString();
//                        violationName = objIDataReader["Name"].ToString();
//                        vioTypeValue = objIDataReader[vioTypeCol].ToString();

//                        /*
//                        if(classIDRemarks.ContainsKey(classID) == false)
//                        {
//                            objDictRemarks = new Dictionary<string, string>() {
//                                {"major" , string.Empty },
//                                {"minor" , string.Empty },
//                                {"serious" , string.Empty },
//                                {"observation" , string.Empty }
//                            };

//                            classIDRemarks.Add(classID, objDictRemarks);
//                        }
//                        */
//                        if (vioTypeValue == "major" &&
//                            classIDRemarks[classID]["major"].Contains(violationNames[violationName]) == false)
//                        {
//                            classIDRemarks[classID]["major"] += violationNames[violationName] + ", ";
//                        }
//                        else if (vioTypeValue == "minor" &&
//                            classIDRemarks[classID]["minor"].Contains(violationNames[violationName]) == false)
//                        {
//                            classIDRemarks[classID]["minor"] += violationNames[violationName] + ", ";
//                        }
//                        else if (vioTypeValue == "serious" &&
//                            classIDRemarks[classID]["serious"].Contains(violationNames[violationName]) == false)
//                        {
//                            classIDRemarks[classID]["serious"] += violationNames[violationName] + ", ";
//                        }
//                        else if (vioTypeValue == "observation" &&
//                            classIDRemarks[classID]["observation"].Contains(violationNames[violationName]) == false)
//                        {
//                            classIDRemarks[classID]["observation"] += violationNames[violationName] + ", ";
//                        }

//                    }

//                    // Get Violations count information
//                    index = 1;
//                    strSql = string.Format(@"SELECT cl_vio.ClassID,
//                            count(CASE WHEN vio.{0}='major' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) AS major_count,
//                            count(CASE WHEN vio.{0}='minor' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) as minor_count,
//                            count(CASE WHEN vio.{0}='serious' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) as serious_count,
//                            count(CASE WHEN vio.{0}='observation' AND cl_vio.IsViolation='y' THEN 1 ELSE null END) as observation_count
//                            FROM AMSClassViolations as cl_vio 
//                            INNER JOIN AMSViolations as vio ON cl_vio.ViolationID = vio.ID 
//                            WHERE cl_vio.ClassMonitoringID IN ({1}) GROUP BY cl_vio.ClassID", vioTypeCol, mrIDsJoined);

//                    strSql = strSql.Replace("\r\n", string.Empty);

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objIDataReader = TMData.ExecuteReader(objDbCommand);

//                    while (objIDataReader.Read())
//                    {
//                        classID = objIDataReader["ClassID"].ToString();
//                        classCode = classIDHash[classID];

//                        majorCountInt = Convert.ToInt16(objIDataReader["major_count"]);
//                        minorCountInt = Convert.ToInt16(objIDataReader["minor_count"]);
//                        seriousCountInt = Convert.ToInt16(objIDataReader["serious_count"]);
//                        observationCountInt = Convert.ToInt16(objIDataReader["observation_count"]);



//                        if (objViolationSummary.SchemeID == CNST_COST_SHARING_SCHEME)
//                        {
//                            totalClassViolations = 0;
//                            majorCount = minorCount = seriousCount = totalVioStr = "-";
//                            observCount = (majorCountInt + minorCountInt + seriousCountInt + observationCountInt).ToString();

//                            allMajorCount = allMinorCount = allSeriousCount = allVioCount = 0;
//                            allObservCount += Convert.ToInt16(observCount);
//                        }
//                        else
//                        {
//                            majorCount = (majorCountInt == 0) ? "-" : objIDataReader["major_count"].ToString();
//                            minorCount = (minorCountInt == 0) ? "-" : objIDataReader["minor_count"].ToString();
//                            seriousCount = (seriousCountInt == 0) ? "-" : objIDataReader["serious_count"].ToString();

//                            totalClassViolations = majorCountInt + minorCountInt + seriousCountInt;
//                            totalVioStr = (totalClassViolations == 0) ? "-" : totalClassViolations.ToString();

//                            observCount = (observationCountInt == 0) ? "-" : objIDataReader["observation_count"].ToString();

//                            allMajorCount += majorCountInt;
//                            allMinorCount += minorCountInt;
//                            allSeriousCount += seriousCountInt;
//                            allVioCount += totalClassViolations;
//                            allObservCount += observationCountInt;
//                        }



//                        remarks = string.Empty;

//                        if (classIDRemarks[classID]["major"] != string.Empty)
//                        {
//                            classIDRemarks[classID]["major"] = classIDRemarks[classID]["major"].TrimEnd(' ');
//                            classIDRemarks[classID]["major"] = classIDRemarks[classID]["major"].TrimEnd(',');

//                            if (objViolationSummary.SchemeID == CNST_COST_SHARING_SCHEME)
//                            {
//                                remarks += classIDRemarks[classID]["major"] + ",";
//                            }
//                            else
//                            {
//                                remarks += "Major: " + classIDRemarks[classID]["major"] + "\n";
//                            }
//                        }

//                        if (classIDRemarks[classID]["minor"] != string.Empty)
//                        {
//                            classIDRemarks[classID]["minor"] = classIDRemarks[classID]["minor"].TrimEnd(' ');
//                            classIDRemarks[classID]["minor"] = classIDRemarks[classID]["minor"].TrimEnd(',');

//                            if (objViolationSummary.SchemeID == CNST_COST_SHARING_SCHEME)
//                            {
//                                remarks += classIDRemarks[classID]["minor"] + ",";
//                            }
//                            else
//                            {
//                                remarks += "Minor: " + classIDRemarks[classID]["minor"] + "\n";
//                            }
//                        }

//                        if (classIDRemarks[classID]["serious"] != string.Empty)
//                        {
//                            classIDRemarks[classID]["serious"] = classIDRemarks[classID]["serious"].TrimEnd(' ');
//                            classIDRemarks[classID]["serious"] = classIDRemarks[classID]["serious"].TrimEnd(',');

//                            if (objViolationSummary.SchemeID == CNST_COST_SHARING_SCHEME)
//                            {
//                                remarks += classIDRemarks[classID]["serious"] + ",";
//                            }
//                            else
//                            {
//                                remarks += "Serious: " + classIDRemarks[classID]["serious"] + "\n";
//                            }
//                        }

//                        if (classIDRemarks[classID]["observation"] != string.Empty)
//                        {
//                            classIDRemarks[classID]["observation"] = classIDRemarks[classID]["observation"].TrimEnd(' ');
//                            classIDRemarks[classID]["observation"] = classIDRemarks[classID]["observation"].TrimEnd(',');

//                            if (objViolationSummary.SchemeID == CNST_COST_SHARING_SCHEME)
//                            {
//                                remarks += classIDRemarks[classID]["observation"] + ",";
//                            }
//                            else
//                            {
//                                remarks += "Observation: " + classIDRemarks[classID]["observation"] + "\n";
//                            }
//                        }

//                        if (objViolationSummary.SchemeID == CNST_COST_SHARING_SCHEME && remarks != string.Empty)
//                        {
//                            remarks = "Observation: " + remarks;
//                            remarks = remarks.TrimEnd(',');
//                        }

//                        objViolationSummary.AddClassVioation(index.ToString(), tspName, classCode, majorCount, minorCount, seriousCount, totalVioStr, observCount, remarks);

//                        index++;
//                    }

//                    objViolationSummary.AllMajorCount = (allMajorCount == 0) ? "-" : allMajorCount.ToString();
//                    objViolationSummary.AllMinorCount = (allMinorCount == 0) ? "-" : allMinorCount.ToString();
//                    objViolationSummary.AllSeriousCount = (allSeriousCount == 0) ? "-" : allSeriousCount.ToString();
//                    objViolationSummary.AllObservationsCount = (allObservCount == 0) ? "-" : allObservCount.ToString();
//                    objViolationSummary.AllViolationsCount = (allVioCount == 0) ? "-" : allVioCount.ToString();
//                }

//                return objViolationSummary;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                {
//                    if (objIDataReader != null && !objIDataReader.IsClosed)
//                    {
//                        objIDataReader.Close();
//                        objIDataReader.Dispose();
//                        objIDataReader = null;
//                    }
//                    if (objDbCommand != null)
//                    {
//                        if (objDbCommand.Connection != null)
//                        {
//                            if (objDbCommand.Connection.State != ConnectionState.Closed)
//                                objDbCommand.Connection.Close();
//                            objDbCommand.Connection.Dispose();
//                            objDbCommand.Connection = null;
//                        }
//                        objDbCommand.Dispose();
//                        objDbCommand = null;
//                    }
//                }
//            }

//        }

//        /*********************************** PV Executive Summary Report *********************************************************/

//        public DataTable LoadPVSummaryReport(string dateStr)
//        {
//            DbCommand objDbCommand = null;
//            IDataReader objDataReader = null;
//            DataTable dt = new DataTable();

//            List<Dictionary<string, string>> objList = new List<Dictionary<string, string>>();
//            Dictionary<string, string> objDict = null;
//            Dictionary<string, int> cirIDsHash = new Dictionary<string, int>();
//            Dictionary<string, int> mrIDsInfo = new Dictionary<string, int>();
//            Dictionary<string, string> cirClMrIDsHash = new Dictionary<string, string>(); // For keeping latest visits monitoring ids
//            Dictionary<string, string> lockedMrIDs = new Dictionary<string, string>(); // Monitoring IDs for which class was locked

//            int index = 0;
//            string mrIDsJoined = string.Empty;
//            string StartDate = dateStr.ToDateTime().ToString("yyyy/MM/dd");
//            string DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", StartDate);

//            try
//            {
//                string strSql = @"Select cir.ClassID, cl_mr.ClassInspectionRequestID, cl_mr.ID AS ClMrID, cir.ClassCode, cir.SchemeName, 
//                                    cir.TSPName, cl_mr.IsLock, cl_mr.VisitDateTime, cl_mr.VisitNo, 
//                                    cl_mr.TraineesImported, cl.ClassSize, cl_mr.TraineeCount
//                                    FROM AMSClassInspectionRequest AS cir
//                                    INNER JOIN AMSClassMonitoring AS cl_mr ON cir.ID = cl_mr.ClassInspectionRequestID
//                                    INNER JOIN  Class AS cl ON cir.ClassID = cl.ID 
//                                    WHERE cl_mr.TraineesImported = 1 " + DateClause +
//                                    " ORDER BY cl_mr.ClassInspectionRequestID, cl_mr.VisitNo";

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objDataReader = TMData.ExecuteReader(objDbCommand);

//                index = 0;
//                int listIndex = 0;
//                string cirID, clMrID;
//                cirID = clMrID = string.Empty;

//                while (objDataReader.Read())
//                {

//                    cirID = objDataReader["ClassInspectionRequestID"].ToString();
//                    clMrID = objDataReader["ClMrID"].ToString();

//                    if (cirIDsHash.ContainsKey(cirID))
//                    {
//                        cirClMrIDsHash[cirID] = clMrID;

//                        listIndex = cirIDsHash[cirID];

//                        mrIDsInfo.Add(clMrID, listIndex);
//                    }
//                    else
//                    {

//                        objDict = new Dictionary<string, string>();

//                        objDict.Add("class_id", objDataReader["ClassID"].ToString());
//                        objDict.Add("scheme_name", objDataReader["SchemeName"].ToString());
//                        objDict.Add("tsp_name", objDataReader["TSPName"].ToString());
//                        objDict.Add("class_code", objDataReader["ClassCode"].ToString());

//                        objDict.Add("registered_trainees", objDataReader["ClassSize"].ToString());
//                        objDict.Add("present_trainees", "-");

//                        objList.Add(objDict);

//                        cirIDsHash.Add(cirID, index);
//                        mrIDsInfo.Add(clMrID, index);
//                        cirClMrIDsHash.Add(cirID, clMrID);
//                        index++;

//                    }

//                    if (objDataReader["IsLock"].ToString() == "y")
//                    {
//                        lockedMrIDs.Add(clMrID, "y");
//                    }
//                }
//                if (objDataReader != null)
//                {
//                    if (!objDataReader.IsClosed)
//                        objDataReader.Close();

//                    objDataReader.Dispose();
//                    objDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }

//                foreach (var item in cirClMrIDsHash)
//                {
//                    mrIDsJoined = mrIDsJoined + item.Value.ToString() + ",";
//                }
//                mrIDsJoined = mrIDsJoined.TrimEnd(',');

//                if (cirClMrIDsHash.Count > 0)
//                {
//                    strSql = @"Select ClassMonitoringID,
//                                    count(CASE WHEN ((AttendanceStatus='present' AND VerificationStatus != 'fake') OR (AttendanceStatus='blank_space' AND IsPresent='y' AND VerificationStatus != 'fake')) THEN 1 ELSE null END) as trainee_present_count
//                                    FROM AMSClassMonitoringTrainee 
//                                    WHERE ClassMonitoringID in (" + mrIDsJoined + ") Group By ClassMonitoringID";

//                    strSql = strSql.Replace("\r\n", string.Empty);

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objDataReader = TMData.ExecuteReader(objDbCommand);

//                    while (objDataReader.Read())
//                    {
//                        if (lockedMrIDs.ContainsKey(objDataReader["ClassMonitoringID"].ToString()))
//                        {
//                            objDict["present_trainees"] = @"0";
//                        }
//                        else
//                        {
//                            listIndex = mrIDsInfo[objDataReader["ClassMonitoringID"].ToString()];
//                            objDict = objList[listIndex];

//                            objDict["present_trainees"] = objDataReader["trainee_present_count"].ToString();
//                        }
//                    }
//                }

//                // Add Columns in report
//                dt.Columns.Add("sr_no");
//                dt.Columns.Add("scheme");
//                dt.Columns.Add("tsp_name");
//                dt.Columns.Add("class_code");
//                dt.Columns.Add("total_trainees");
//                dt.Columns.Add("trainees_present");

//                for (index = 0; index < objList.Count; index++)
//                {
//                    objDict = objList[index];
//                    DataRow row = dt.NewRow();

//                    row["sr_no"] = index + 1;
//                    row["scheme"] = objDict["scheme_name"];
//                    row["tsp_name"] = objDict["tsp_name"];
//                    row["class_code"] = objDict["class_code"];

//                    row["total_trainees"] = objDict["registered_trainees"];
//                    row["trainees_present"] = objDict["present_trainees"];

//                    dt.Rows.Add(row);
//                }

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objDataReader != null)
//                {
//                    if (!objDataReader.IsClosed)
//                        objDataReader.Close();

//                    objDataReader.Dispose();
//                    objDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }
//            }

//            return dt;
//        }


//        /*********************************** Employment Verification Report *********************************************************/

//        public DataTable LoadEVReport(string dateStr)
//        {
//            DbCommand objDbCommand = null;
//            IDataReader objDataReader = null;
//            DataTable dt = new DataTable();

//            List<Dictionary<string, string>> objList = new List<Dictionary<string, string>>();
//            Dictionary<string, string> objDict = null;


//            int index = 0;
//            string mrIDsJoined = string.Empty;
//            string StartDate = dateStr.ToDateTime().ToString("yyyy/MM/dd");
//            string DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", StartDate);

//            try
//            {
//                string strSql = @"Select cir.ClassID, cl_mr.ClassInspectionRequestID, cl_mr.ID AS ClMrID, cir.ClassCode, cir.SchemeName, 
//                                    cir.TSPName, cl_mr.IsLock, cl_mr.VisitDateTime, cl_mr.VisitNo, 
//                                    cl_mr.TraineesImported, cl.ClassSize, cl_mr.TraineeCount
//                                    FROM AMSClassInspectionRequest AS cir
//                                    INNER JOIN AMSClassMonitoring AS cl_mr ON cir.ID = cl_mr.ClassInspectionRequestID
//                                    INNER JOIN  Class AS cl ON cir.ClassID = cl.ID 
//                                    WHERE cl_mr.TraineesImported = 1 " + DateClause +
//                                    " ORDER BY cl_mr.ClassInspectionRequestID, cl_mr.VisitNo";

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objDataReader = TMData.ExecuteReader(objDbCommand);


//                // Add Columns in report
//                dt.Columns.Add("sr_no");
//                dt.Columns.Add("tsp_name");
//                dt.Columns.Add("class_code");
//                dt.Columns.Add("name");
//                dt.Columns.Add("cnic");
//                dt.Columns.Add("location");
//                dt.Columns.Add("visit_status");
//                dt.Columns.Add("remarks");

//                for (index = 0; index < objList.Count; index++)
//                {
//                    objDict = objList[index];
//                    DataRow row = dt.NewRow();

//                    row["sr_no"] = index + 1;
//                    row["tsp_name"] = objDict["tsp_name"];
//                    row["class_code"] = objDict["class_code"];

//                    row["total_trainees"] = objDict["registered_trainees"];
//                    row["trainees_present"] = objDict["present_trainees"];

//                    dt.Rows.Add(row);
//                }

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objDataReader != null)
//                {
//                    if (!objDataReader.IsClosed)
//                        objDataReader.Close();

//                    objDataReader.Dispose();
//                    objDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }
//            }

//            return dt;
//        }
//        /*********************************** Instructor Report *********************************************************/

//        public DataTable LoadInstructorReport(string schemeID, string dateStr)
//        {
//            DbCommand objDbCommand = null;
//            IDataReader objDataReader = null;
//            DataTable dt = new DataTable();

//            List<Dictionary<string, string>> objList = new List<Dictionary<string, string>>();
//            Dictionary<string, string> objDict = null;
//            Dictionary<string, int> classIDsHash = new Dictionary<string, int>();
//            Dictionary<string, int> classVisitsInfo = new Dictionary<string, int>();

//            Dictionary<string, string> reportText = new Dictionary<string, string>()
//            {
//                {"y", "Matched"},
//                {"n", "Not Matched"},
//                {"", "-"}
//            };

//            int index, VisitCount;
//            index = VisitCount = 0;

//            string mrIDsJoined, visitNo, visit;
//            mrIDsJoined = visitNo = visit = string.Empty;

//            string StartDate = dateStr.ToDateTime().ToString("yyyy/MM/dd");
//            string DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", StartDate);

//            try
//            {
//                string strSql = @"Select cir.ClassID, cl.SpID, cl_mr.ClassInspectionRequestID, cl_mr.ID AS ClMrID, cir.ClassCode, cir.SchemeName, cir.TSPName, 
//                                    cir.TradeName, cl_mr.IsLock, cl_mr.VisitDateTime, cl_mr.VisitNo, cl_mr.InstructorChanged, cl_mr.InstructorChangedWithApproval,
//                                cl_mr.IsAllocatedTrainerRemarks
//                                FROM AMSClassInspectionRequest as cir
//                                INNER JOIN AMSClassMonitoring AS cl_mr ON cir.ID = cl_mr.ClassInspectionRequestID
//                                INNER JOIN  Class AS cl ON cir.ClassID = cl.ID
//                                WHERE cl.SchemeID = '" + schemeID + "'" + DateClause +
//                                " ORDER BY cl_mr.ClassInspectionRequestID, cl_mr.VisitNo";

//                strSql = strSql.Replace("\r\n", string.Empty);
//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objDataReader = TMData.ExecuteReader(objDbCommand);

//                index = 0;
//                int listIndex = 0;
//                string remarks = string.Empty;

//                while (objDataReader.Read())
//                {

//                    if (classIDsHash.ContainsKey(objDataReader["ClassID"].ToString()))
//                    {
//                        VisitCount = classVisitsInfo[objDataReader["ClassID"].ToString()] + 1;
//                        classVisitsInfo[objDataReader["ClassID"].ToString()] = VisitCount;

//                        listIndex = classIDsHash[objDataReader["ClassID"].ToString()];
//                        objDict = objList[listIndex];
//                    }
//                    else
//                    {
//                        // set visit no to 1 for 1st visit
//                        classVisitsInfo.Add(objDataReader["ClassID"].ToString(), 1);

//                        objDict = new Dictionary<string, string>();

//                        objDict.Add("class_id", objDataReader["ClassID"].ToString());
//                        objDict.Add("tsp_name", objDataReader["TSPName"].ToString());
//                        objDict.Add("class_code", objDataReader["ClassCode"].ToString());
//                        objDict.Add("appendix_with_cir", "N/A");
//                        objDict.Add("appendix_instructor_same_v1", "-");
//                        objDict.Add("appendix_instructor_same_v2", "-");
//                        objDict.Add("appendix_instructor_same_v3", "-");
//                        objDict.Add("appendix_instructor_same_v4", "-");
//                        objDict.Add("cir_instructor_same_v1", "-");
//                        objDict.Add("cir_instructor_same_v2", "-");
//                        objDict.Add("cir_instructor_same_v3", "-");
//                        objDict.Add("cir_instructor_same_v4", "-");
//                        objDict.Add("remarks", "");

//                        objList.Add(objDict);
//                    }


//                    remarks = objDataReader["IsAllocatedTrainerRemarks"].ToString();
//                    visit = classVisitsInfo[objDataReader["ClassID"].ToString()].ToString();
//                    visitNo = "v" + visit;
//                    if (objDataReader["IsLock"].ToString() == "y")
//                    {
//                        objDict["cir_instructor_same_" + visitNo] = "N/A";
//                    }
//                    else
//                    {
//                        objDict["cir_instructor_same_" + visitNo] = reportText[objDataReader["InstructorChanged"].ToString()];
//                    }


//                    if (remarks != string.Empty && remarks != null)
//                    {

//                        objDict["remarks"] += "Visit " + visit + ": " + remarks + "\n";
//                    }

//                    mrIDsJoined = mrIDsJoined + objDataReader["ClMrID"].ToString() + ",";

//                    if (classIDsHash.ContainsKey(objDataReader["ClassID"].ToString()) == false)
//                    {
//                        classIDsHash.Add(objDataReader["ClassID"].ToString(), index);
//                        index++;
//                    }
//                }

//                mrIDsJoined = mrIDsJoined.TrimEnd(',');

//                // Add Columns in report
//                dt.Columns.Add("sr_no");
//                dt.Columns.Add("tsp_name");
//                dt.Columns.Add("class_code");
//                dt.Columns.Add("appendix_with_cir");
//                dt.Columns.Add("appendix_instructor_same_v1");
//                dt.Columns.Add("appendix_instructor_same_v2");
//                dt.Columns.Add("appendix_instructor_same_v3");
//                dt.Columns.Add("appendix_instructor_same_v4");

//                dt.Columns.Add("cir_instructor_same_v1");
//                dt.Columns.Add("cir_instructor_same_v2");
//                dt.Columns.Add("cir_instructor_same_v3");
//                dt.Columns.Add("cir_instructor_same_v4");
//                dt.Columns.Add("remarks");

//                for (index = 0; index < objList.Count; index++)
//                {
//                    objDict = objList[index];
//                    DataRow row = dt.NewRow();

//                    row["sr_no"] = index + 1;
//                    row["tsp_name"] = objDict["tsp_name"];
//                    row["class_code"] = objDict["class_code"];

//                    row["appendix_with_cir"] = objDict["appendix_with_cir"];
//                    row["appendix_instructor_same_v1"] = objDict["appendix_instructor_same_v1"];
//                    row["appendix_instructor_same_v2"] = objDict["appendix_instructor_same_v2"];
//                    row["appendix_instructor_same_v3"] = objDict["appendix_instructor_same_v3"];
//                    row["appendix_instructor_same_v4"] = objDict["appendix_instructor_same_v4"];

//                    row["cir_instructor_same_v1"] = objDict["cir_instructor_same_v1"];
//                    row["cir_instructor_same_v2"] = objDict["cir_instructor_same_v2"];
//                    row["cir_instructor_same_v3"] = objDict["cir_instructor_same_v3"];
//                    row["cir_instructor_same_v4"] = objDict["cir_instructor_same_v4"];

//                    row["remarks"] = (objDict["remarks"] == string.Empty) ? "-" : objDict["remarks"];

//                    dt.Rows.Add(row);
//                }

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objDataReader != null)
//                {
//                    if (!objDataReader.IsClosed)
//                        objDataReader.Close();

//                    objDataReader.Dispose();
//                    objDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }
//            }

//            return dt;
//        }

//        /************************** Attendance & Percepton Report ********************************************/

//        private Dictionary<string, string> getPerceptionReportLayoutData()
//        {
//            Dictionary<string, string> objDict = new Dictionary<string, string>();

//            objDict.Add("tsp_name", "");
//            objDict.Add("trade_name", "");
//            objDict.Add("class_code", "");
//            objDict.Add("class_size", "");

//            objDict.Add("v1_is_lock", "-");
//            objDict.Add("v1_trainees_present", "-");
//            objDict.Add("v1_trainee_count_1", "-");
//            objDict.Add("v1_trainee_count_2", "-");
//            objDict.Add("v1_trainee_count_3", "-");
//            objDict.Add("v1_consumables", "-");
//            objDict.Add("v1_equipment", "-");
//            objDict.Add("v1_training_quality", "-");
//            objDict.Add("v1_meal_quality", "-");
//            objDict.Add("v1_boarding_facility", "-");
//            objDict.Add("v1_training_usefull", "-");
//            objDict.Add("v1_daily_hours", "-");

//            objDict.Add("v2_is_lock", "-");
//            objDict.Add("v2_trainees_present", "-");
//            objDict.Add("v2_trainee_count_1", "-");
//            objDict.Add("v2_trainee_count_2", "-");
//            objDict.Add("v2_trainee_count_3", "-");
//            objDict.Add("v2_consumables", "-");
//            objDict.Add("v2_equipment", "-");
//            objDict.Add("v2_training_quality", "-");
//            objDict.Add("v2_meal_quality", "-");
//            objDict.Add("v2_boarding_facility", "-");
//            objDict.Add("v2_training_usefull", "-");
//            objDict.Add("v2_daily_hours", "-");

//            objDict.Add("v3_is_lock", "-");
//            objDict.Add("v3_trainees_present", "-");
//            objDict.Add("v3_trainee_count_1", "-");
//            objDict.Add("v3_trainee_count_2", "-");
//            objDict.Add("v3_trainee_count_3", "-");
//            objDict.Add("v3_consumables", "-");
//            objDict.Add("v3_equipment", "-");
//            objDict.Add("v3_training_quality", "-");
//            objDict.Add("v3_meal_quality", "-");
//            objDict.Add("v3_boarding_facility", "-");
//            objDict.Add("v3_training_usefull", "-");
//            objDict.Add("v3_daily_hours", "-");

//            objDict.Add("v4_is_lock", "-");
//            objDict.Add("v4_trainees_present", "-");
//            objDict.Add("v4_trainee_count_1", "-");
//            objDict.Add("v4_trainee_count_2", "-");
//            objDict.Add("v4_trainee_count_3", "-");
//            objDict.Add("v4_consumables", "-");
//            objDict.Add("v4_equipment", "-");
//            objDict.Add("v4_training_quality", "-");
//            objDict.Add("v4_meal_quality", "-");
//            objDict.Add("v4_boarding_facility", "-");
//            objDict.Add("v4_training_usefull", "-");
//            objDict.Add("v4_daily_hours", "-");
//            objDict.Add("remarks", "-");

//            return objDict;
//        }

//        /*********************************** Attendance & Perception Report ********************************************/
//        public DataTable LoadAttendanceAndPerceptionReport(string schemeID, string dateStr)
//        {

//            DbCommand objDbCommand = null;
//            IDataReader objDataReader = null;
//            DataTable dt = new DataTable();

//            List<Dictionary<string, string>> objList = new List<Dictionary<string, string>>();
//            Dictionary<string, string> objDict = null;
//            Dictionary<string, int> classIDsHash = new Dictionary<string, int>();
//            Dictionary<string, int> classMrVisitNoHash = new Dictionary<string, int>();
//            Dictionary<string, string> classMrIDsHash = new Dictionary<string, string>();
//            Dictionary<string, int> ClassVisitsInfo = new Dictionary<string, int>();

//            int index, VisitCount;
//            index = VisitCount = 0;

//            string mrIDsJoined, visitNo, value = @"";
//            mrIDsJoined = visitNo = value = string.Empty;

//            string StartDate = dateStr.ToDateTime().ToString("yyyy/MM/dd");
//            string DateClause = string.Format(" AND month(VisitDateTime) = month('{0}') AND year(VisitDateTime ) = year('{0}')", StartDate);

//            try
//            {

//                string strSql = @"Select cir.ClassID, cl.SpID, cl_mr.ClassInspectionRequestID, cl_mr.ID AS ClMrID, cir.ClassCode, cir.SchemeName, cir.TSPName,
//                                cir.TradeName, cl_mr.IsLock, cir.TrainingCentreAddress, cl.ClassSize,
//                                cl_mr.VisitDateTime, cl_mr.VisitNo, cl_mr.TraineesImported, cl_mr.TraineeCount, cl_mr.TraineeAttendCountOne,cl_mr.TraineeAttendCountTwo, cl_mr.TraineeAttendCountThree
//                                FROM AMSClassInspectionRequest as cir
//                                INNER JOIN AMSClassMonitoring AS cl_mr ON cir.ID = cl_mr.ClassInspectionRequestID
//                                INNER JOIN  Class AS cl ON cir.ClassID = cl.ID
//                                WHERE cl.SchemeID = '" + schemeID + "'" + DateClause;

//                strSql = strSql.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSql);
//                objDataReader = TMData.ExecuteReader(objDbCommand);

//                index = 0;
//                int listIndex = 0;


//                while (objDataReader.Read())
//                {

//                    if (classIDsHash.ContainsKey(objDataReader["ClassID"].ToString()))
//                    {

//                        VisitCount = ClassVisitsInfo[objDataReader["ClassID"].ToString()] + 1;
//                        ClassVisitsInfo[objDataReader["ClassID"].ToString()] = VisitCount;

//                        listIndex = Convert.ToInt16(classIDsHash[objDataReader["ClassID"].ToString()]);
//                        objDict = objList[listIndex];
//                        objDict.Add(objDataReader["ClMrID"].ToString(), VisitCount.ToString());
//                        objDict.Add("cl_mr_" + objDataReader["ClMrID"].ToString(), objDataReader["ClassID"].ToString()); // cl_mr_id = class no

//                    }
//                    else
//                    {
//                        // set visit no to 1 for 1st visit
//                        ClassVisitsInfo.Add(objDataReader["ClassID"].ToString(), 1);

//                        // Get perception report layout columns 
//                        objDict = getPerceptionReportLayoutData();

//                        objDict.Add(objDataReader["ClMrID"].ToString(), "1"); // cl_mr_id : visit no
//                        objDict.Add("class_id", objDataReader["ClassID"].ToString());

//                        objDict["tsp_name"] = objDataReader["TSPName"].ToString();
//                        objDict["trade_name"] = objDataReader["TradeName"].ToString();
//                        objDict["class_code"] = objDataReader["ClassCode"].ToString();
//                        objDict["class_size"] = objDataReader["ClassSize"].ToString();

//                        objList.Add(objDict);
//                    }

//                    value = "v" + ClassVisitsInfo[objDataReader["ClassID"].ToString()].ToString() + "_";
//                    objDict[value + "is_lock"] = objDataReader["IsLock"].ToString();

//                    if (objDataReader["TraineesImported"].ToString() == "0")
//                    {
//                        objDict[value + "trainees_present"] = objDataReader["TraineeCount"].ToString();
//                    }

//                    if (objDataReader["IsLock"].ToString() == "y")
//                    {
//                        objDict[value + "trainee_count_1"] = yesNoValues["na"];
//                        objDict[value + "trainee_count_2"] = yesNoValues["na"];
//                        objDict[value + "trainee_count_3"] = yesNoValues["na"];
//                    }
//                    else
//                    {
//                        objDict[value + "trainee_count_1"] = (objDataReader["TraineeAttendCountOne"].ToString() == "-1") ? yesNoValues["na"] : objDataReader["TraineeAttendCountOne"].ToString();
//                        objDict[value + "trainee_count_2"] = (objDataReader["TraineeAttendCountTwo"].ToString() == "-1") ? yesNoValues["na"] : objDataReader["TraineeAttendCountTwo"].ToString();
//                        objDict[value + "trainee_count_3"] = (objDataReader["TraineeAttendCountThree"].ToString() == "-1") ? yesNoValues["na"] : objDataReader["TraineeAttendCountThree"].ToString();
//                    }

//                    mrIDsJoined = mrIDsJoined + objDataReader["ClMrID"].ToString() + ",";

//                    if (classIDsHash.ContainsKey(objDataReader["ClassID"].ToString()) == false)
//                    {
//                        classIDsHash.Add(objDataReader["ClassID"].ToString(), index);
//                        index++;
//                    }
//                    classMrIDsHash.Add(objDataReader["ClMrID"].ToString(), objDataReader["ClassID"].ToString());
//                }

//                if (objDataReader != null)
//                {
//                    if (!objDataReader.IsClosed)
//                        objDataReader.Close();

//                    objDataReader.Dispose();
//                    objDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;
//                }

//                mrIDsJoined = mrIDsJoined.TrimEnd(',');

//                if (index > 0)
//                {

//                    // Get Info of Trainee present on visit data 

//                    strSql = @"Select ClassMonitoringID,
//                            count(CASE WHEN ((AttendanceStatus='present' AND VerificationStatus != 'fake') OR (AttendanceStatus='blank_space' AND IsPresent='y' AND VerificationStatus != 'fake')) THEN 1 ELSE null END) as trainee_present_count
//                            FROM AMSClassMonitoringTrainee
//                            WHERE ClassMonitoringID in (" + mrIDsJoined + ") GROUP BY ClassMonitoringID";

//                    strSql = strSql.Replace("\r\n", string.Empty);

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objDataReader = TMData.ExecuteReader(objDbCommand);

//                    string classID = @"";
//                    string mrVisitNo = @"";
//                    while (objDataReader.Read())
//                    {
//                        classID = classMrIDsHash[objDataReader["ClassMonitoringID"].ToString()];

//                        listIndex = classIDsHash[classID];
//                        mrVisitNo = objList[listIndex][objDataReader["ClassMonitoringID"].ToString()];
//                        value = "v" + mrVisitNo + "_";
//                        if (objList[listIndex][value + "trainees_present"] != null)
//                        {
//                            objList[listIndex][value + "trainees_present"] = objDataReader["trainee_present_count"].ToString();
//                        }

//                    }

//                    if (objDataReader != null)
//                    {
//                        if (!objDataReader.IsClosed)
//                            objDataReader.Close();

//                        objDataReader.Dispose();
//                        objDataReader = null;

//                    }
//                    if (objDbCommand != null)
//                    {
//                        objDbCommand.Dispose();
//                        objDbCommand = null;
//                    }

//                    // Get Trainee Perception Info
//                    strSql = @"Select cl_vio.ClassInspectionRequestID, cl_vio.ClassMonitoringID, cl_vio.ClassID, cl_vio.ViolationID,
//                                cl_vio.IsViolation, cl_vio.PercentageSatisfied
//                                FROM AMSClassViolations AS cl_vio
//                                INNER JOIN AMSViolations as vio ON cl_vio.ViolationID = vio.ID
//                                WHERE cl_vio.ClassMonitoringID IN (" + mrIDsJoined + ") and vio.Type = 'feedback' " +
//                                "ORDER BY cl_vio.ClassMonitoringID, cl_vio.ViolationID";

//                    strSql = strSql.Replace("\r\n", string.Empty);

//                    objDbCommand = TMData.GetSqlStringCommand(strSql);
//                    objDataReader = TMData.ExecuteReader(objDbCommand);

//                    while (objDataReader.Read())
//                    {
//                        index = classIDsHash[objDataReader["ClassID"].ToString()];
//                        objDict = objList[index];

//                        visitNo = objDict[objDataReader["ClassMonitoringID"].ToString()];
//                        value = "v" + visitNo + "_";

//                        if (objDataReader["ViolationID"].ToString() == "21")
//                        {
//                            objDict[value + "consumables"] = (objDataReader["IsViolation"].ToString() == "na") ? "N/A" : objDataReader["PercentageSatisfied"].ToString() + "%";
//                        }
//                        else if (objDataReader["ViolationID"].ToString() == "22")
//                        {
//                            objDict[value + "equipment"] = (objDataReader["IsViolation"].ToString() == "na") ? "N/A" : objDataReader["PercentageSatisfied"].ToString() + "%";
//                        }
//                        else if (objDataReader["ViolationID"].ToString() == "23")
//                        {
//                            objDict[value + "training_quality"] = (objDataReader["IsViolation"].ToString() == "na") ? "N/A" : objDataReader["PercentageSatisfied"].ToString() + "%";
//                        }
//                        else if (objDataReader["ViolationID"].ToString() == "24")
//                        {
//                            objDict[value + "meal_quality"] = (objDataReader["IsViolation"].ToString() == "na") ? "N/A" : objDataReader["PercentageSatisfied"].ToString() + "%";
//                        }
//                        else if (objDataReader["ViolationID"].ToString() == "25")
//                        {
//                            objDict[value + "boarding_facility"] = (objDataReader["IsViolation"].ToString() == "na") ? "N/A" : objDataReader["PercentageSatisfied"].ToString() + "%";
//                        }
//                        else if (objDataReader["ViolationID"].ToString() == "26")
//                        {
//                            objDict[value + "training_usefull"] = (objDataReader["IsViolation"].ToString() == "na") ? "N/A" : objDataReader["PercentageSatisfied"].ToString() + "%";
//                        }
//                        else if (objDataReader["ViolationID"].ToString() == "27")
//                        {
//                            objDict[value + "daily_hours"] = (objDataReader["IsViolation"].ToString() == "na") ? "N/A" : objDataReader["PercentageSatisfied"].ToString() + "%";
//                        }
//                    }

//                    if (objDataReader != null)
//                    {
//                        if (!objDataReader.IsClosed)
//                            objDataReader.Close();

//                        objDataReader.Dispose();
//                        objDataReader = null;

//                    }
//                    if (objDbCommand != null)
//                    {
//                        objDbCommand.Dispose();
//                        objDbCommand = null;
//                    }
//                }


//                // Add Columns in report
//                dt.Columns.Add("Sr. No");
//                dt.Columns.Add("TSP");
//                dt.Columns.Add("Trade");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Contractual Trainees");

//                for (index = 0; index < 4; index++)
//                {
//                    dt.Columns.Add((index + 1) + "-Is Lock");
//                    dt.Columns.Add((index + 1) + "-Visit Date Attendance");
//                    dt.Columns.Add((index + 1) + "-1 day before");
//                    dt.Columns.Add((index + 1) + "-2 days before");
//                    dt.Columns.Add((index + 1) + "-3 days before");
//                    dt.Columns.Add((index + 1) + "-Sufficient Consumables");
//                    dt.Columns.Add((index + 1) + "-Sufficient equipment/tools");
//                    dt.Columns.Add((index + 1) + "-Quality of practical training");
//                    dt.Columns.Add((index + 1) + "-Quality of meals");
//                    dt.Columns.Add((index + 1) + "-Quality of boarding facility");
//                    dt.Columns.Add((index + 1) + "-Training Usefulness");
//                    dt.Columns.Add((index + 1) + "-Average daily hours");
//                }

//                dt.Columns.Add("Remarks");

//                int totalVisits = 0;

//                for (index = 0; index < objList.Count; index++)
//                {
//                    objDict = objList[index];

//                    totalVisits = ClassVisitsInfo[objDict["class_id"]];

//                    DataRow row = dt.NewRow();
//                    row["Sr. No"] = index + 1;
//                    row["TSP"] = objDict["tsp_name"];
//                    row["Trade"] = objDict["trade_name"];
//                    row["Class Code"] = objDict["class_code"];
//                    row["Contractual Trainees"] = objDict["class_size"];

//                    row["1-Is Lock"] = objDict["v1_is_lock"];
//                    row["1-Visit Date Attendance"] = objDict["v1_trainees_present"];
//                    row["1-1 day before"] = objDict["v1_trainee_count_1"];
//                    row["1-2 days before"] = objDict["v1_trainee_count_2"];
//                    row["1-3 days before"] = objDict["v1_trainee_count_3"];
//                    row["1-Sufficient Consumables"] = objDict["v1_consumables"];
//                    row["1-Sufficient equipment/tools"] = objDict["v1_equipment"];
//                    row["1-Quality of practical training"] = objDict["v1_training_quality"];
//                    row["1-Quality of meals"] = objDict["v1_meal_quality"];
//                    row["1-Quality of boarding facility"] = objDict["v1_boarding_facility"];
//                    row["1-Training Usefulness"] = objDict["v1_training_usefull"];
//                    row["1-Average daily hours"] = objDict["v1_daily_hours"];

//                    row["2-Is Lock"] = objDict["v2_is_lock"];
//                    row["2-Visit Date Attendance"] = objDict["v2_trainees_present"];
//                    row["2-1 day before"] = objDict["v2_trainee_count_1"];
//                    row["2-2 days before"] = objDict["v2_trainee_count_2"];
//                    row["2-3 days before"] = objDict["v2_trainee_count_3"];
//                    row["2-Sufficient Consumables"] = objDict["v2_consumables"];
//                    row["2-Sufficient equipment/tools"] = objDict["v2_equipment"];
//                    row["2-Quality of practical training"] = objDict["v2_training_quality"];
//                    row["2-Quality of meals"] = objDict["v2_meal_quality"];
//                    row["2-Quality of boarding facility"] = objDict["v2_boarding_facility"];
//                    row["2-Training Usefulness"] = objDict["v2_training_usefull"];
//                    row["2-Average daily hours"] = objDict["v2_daily_hours"];

//                    row["3-Is Lock"] = objDict["v3_is_lock"];
//                    row["3-Visit Date Attendance"] = objDict["v3_trainees_present"];
//                    row["3-1 day before"] = objDict["v3_trainee_count_1"];
//                    row["3-2 days before"] = objDict["v3_trainee_count_2"];
//                    row["3-3 days before"] = objDict["v3_trainee_count_3"];
//                    row["3-Sufficient Consumables"] = objDict["v3_consumables"];
//                    row["3-Sufficient equipment/tools"] = objDict["v3_equipment"];
//                    row["3-Quality of practical training"] = objDict["v3_training_quality"];
//                    row["3-Quality of meals"] = objDict["v3_meal_quality"];
//                    row["3-Quality of boarding facility"] = objDict["v3_boarding_facility"];
//                    row["3-Training Usefulness"] = objDict["v3_training_usefull"];
//                    row["3-Average daily hours"] = objDict["v3_daily_hours"];

//                    row["4-Is Lock"] = objDict["v4_is_lock"];
//                    row["4-Visit Date Attendance"] = objDict["v4_trainees_present"];
//                    row["4-1 day before"] = objDict["v4_trainee_count_1"];
//                    row["4-2 days before"] = objDict["v4_trainee_count_2"];
//                    row["4-3 days before"] = objDict["v4_trainee_count_3"];
//                    row["4-Sufficient Consumables"] = objDict["v4_consumables"];
//                    row["4-Sufficient equipment/tools"] = objDict["v4_equipment"];
//                    row["4-Quality of practical training"] = objDict["v4_training_quality"];
//                    row["4-Quality of meals"] = objDict["v4_meal_quality"];
//                    row["4-Quality of boarding facility"] = objDict["v4_boarding_facility"];
//                    row["4-Training Usefulness"] = objDict["v4_training_usefull"];
//                    row["4-Average daily hours"] = objDict["v4_daily_hours"];
//                    row["Remarks"] = objDict["remarks"];

//                    dt.Rows.Add(row);
//                }

//            }
//            catch
//            {
//                throw;
//            }

//            return dt;
//        }

//        /************************** Scheme Class Violation Summary Report *************************************/

//        public DataTable LoadSchemeViolationSummaryReport(string SchemeID, string month)
//        {
//            DbCommand objDbCommand = null;
//            DbCommand objDbSubCommand = null;
//            IDataReader objDataReader = null;
//            IDataReader objDataSubReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", StartDate);

//            try
//            {

//                // Fetch scheme type of selected scheme
//                string vioTypeCol = string.Empty;
//                string schemeType = string.Empty;
//                Dictionary<string, string> AMSSchemeType = new Dictionary<string, string>()
//                {
//                    {PSDFConstants.CNST_SchemeType_NA, "na"},
//                    {PSDFConstants.CNST_SchemeType_Formal, "formal"},
//                    {PSDFConstants.CNST_SchemeType_Community, "community"},
//                    {PSDFConstants.CNST_SchemeType_Industrial, "industrial"}
//                };

//                string strSQL = string.Format(@"SELECT SchemeType FROM Scheme WHERE SchemeID = '{0}'", SchemeID);
//                objDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objDataReader = TMData.ExecuteReader(objDbCommand);
//                while (objDataReader.Read())
//                {
//                    schemeType = objDataReader["SchemeType"].ToString();
//                }
//                vioTypeCol = getVioTypeColumnNameBySchemeType(AMSSchemeType[schemeType]);

//                strSQL = string.Format(@"SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS 'Sr.No', userinfo.DisplayName as 'TSP',
//                                    scheme.Code + '-' + tsp.Code + '-' + ClassCode as 'ClassCode', 
//                                    sum(case when (vio.{0} = 'minor' and IsViolation = 'y') then 1 else 0 end) Minor,
//                                    sum(case when (vio.{0} = 'major' and IsViolation = 'y') then 1 else 0 end) Major,
//	                                sum(case when (vio.{0} = 'serious' and IsViolation = 'y') then 1 else 0 end) Serious,
//	                                count(ClassID) Total, 
//	                                sum(case when (vio.{0} = 'observation' and IsViolation = 'y') then 1 else 0 end) Observation,
//                                    ClassID,
//                                    tsp.code as TspCode
//                                  FROM dbo.AMSClassViolations cl_vio
//                                  inner join dbo.AMSViolations vio on vio.ID = cl_vio.ViolationID
//                                  inner join AMSClassMonitoring as cl_mr on cl_mr.ID = cl_vio.ClassMonitoringID
//                                  inner join dbo.class class on class.ID = cl_vio.ClassID
//                                  inner join dbo.UserInfo userinfo on userinfo.ID = class.SpID
//                                  inner join dbo.ServiceProvider tsp on tsp.ID = class.SpID
//                                  inner join dbo.Scheme scheme on scheme.SchemeID = class.SchemeID
//                                  where ClassID in (select ID from class where SchemeID = '{1}')
//                                  and vio.{0} in ('major', 'minor', 'serious', 'observation') 
//                                  {2} group by class.ClassCode, DisplayName, scheme.Code, tsp.Code, ClassID order by TspCode", vioTypeCol, SchemeID, DateClause);

//                strSQL = strSQL.Replace("\r\n", string.Empty);

//                objDbCommand = TMData.GetSqlStringCommand(strSQL);

//                objDataReader = TMData.ExecuteReader(objDbCommand);
//                int index = 0;

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Minor Violations");
//                dt.Columns.Add("Major Violations");
//                dt.Columns.Add("Serious Violations");
//                dt.Columns.Add("Total Violations");
//                dt.Columns.Add("Observations Total");
//                dt.Columns.Add("Remarks");

//                //Dictionary<string, int> tspMajorVio = new Dictionary<string, int>();

//                int tspMajorVio = 0;
//                int tspMinorVio = 0;
//                int tspSeriousVio = 0;
//                int tspTotalVio = 0;
//                int tspObservVio = 0;

//                string tspName = string.Empty;

//                DataTable dtTemp = new DataTable();
//                dtTemp.Load(TMData.ExecuteReader(objDbCommand));
//                int rowsCount = dtTemp.Rows.Count;


//                Dictionary<string, string> ViolationsDescription = getVioDescription();
//                int GrandMajorCount = 0;
//                int GrandMinorCount = 0;
//                int GrandSeriousCount = 0;
//                int GrandTotalCount = 0;
//                int GrandObservCount = 0;
//                DateClause = string.Format(" AND month(VisitDateTime) = month('{0}') AND year(VisitDateTime ) = year('{0}')", StartDate);


//                while (objDataReader.Read())
//                {

//                    int majorCount = (int)objDataReader["Major"];
//                    int minorCount = (int)objDataReader["Minor"];
//                    int seriousCount = (int)objDataReader["Serious"];
//                    int totalViolations = majorCount + minorCount + seriousCount;
//                    int observationCount = (int)objDataReader["Observation"];

//                    GrandMajorCount += majorCount;
//                    GrandMinorCount += minorCount;
//                    GrandSeriousCount += seriousCount;
//                    GrandTotalCount += totalViolations;
//                    GrandObservCount += observationCount;


//                    string classID = objDataReader["ClassID"].ToString();
//                    string subStrSQL = string.Format(@"SELECT  
//                            ViolationID, vio.{0} as Type, vio.Description
//                            FROM dbo.AMSClassViolations cl_vio
//                            inner join dbo.AMSViolations vio
//                            on vio.ID = cl_vio.ViolationID
//                            where ClassID = '{1}'
//                            and IsViolation = 'y' and vio.{0} in ('major', 'minor', 'serious', 'observation')
//                            {2} order by ClassMonitoringID, ViolationID, ClassID", vioTypeCol, classID, DateClause);

//                    objDbSubCommand = TMData.GetSqlStringCommand(subStrSQL);
//                    objDataSubReader = TMData.ExecuteReader(objDbSubCommand);

//                    string majorRemarks = string.Empty;
//                    string minorRemarks = string.Empty;
//                    string seriousRemarks = string.Empty;
//                    string obsevRemarks = string.Empty;


//                    List<string> marjorRemarks_ = new List<string>();
//                    List<string> minorRemarks_ = new List<string>();
//                    List<string> seriousRemarks_ = new List<string>();
//                    List<string> obsevRemarks_ = new List<string>();


//                    while (objDataSubReader.Read())
//                    {
//                        string violationID = objDataSubReader["ViolationID"].ToString();
//                        string violationType = objDataSubReader["Type"].ToString();
//                        string vioDesp = ViolationsDescription.ContainsKey(violationID) ? ViolationsDescription[violationID] : "";

//                        /*Violations will be shown as Observations if Scheme is Cost Sharing 2016*/
//                        if (SchemeID == CNST_COST_SHARING_SCHEME)
//                        {

//                            if (obsevRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                            {
//                                obsevRemarks_.Add(vioDesp);
//                            }
//                            observationCount += totalViolations;
//                            majorCount = 0;
//                            minorCount = 0;
//                            seriousCount = 0;
//                            totalViolations = 0;
//                        }
//                        else
//                        {

//                            if (violationType == "major" && majorCount > 0)
//                            {
//                                if (marjorRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    marjorRemarks_.Add(vioDesp);
//                                }
//                            }
//                            else if (violationType == "minor" && minorCount > 0)
//                            {
//                                if (minorRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    minorRemarks_.Add(vioDesp);
//                                }
//                            }
//                            else if (violationType == "serious" && seriousCount > 0)
//                            {
//                                if (seriousRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    seriousRemarks_.Add(vioDesp);
//                                }
//                            }
//                            else if (violationType == "observation" && observationCount > 0)
//                            {
//                                if (obsevRemarks_.FindIndex(o => string.Equals(vioDesp, o, StringComparison.OrdinalIgnoreCase)) == -1)
//                                {
//                                    obsevRemarks_.Add(vioDesp);
//                                }
//                            }
//                        }
//                    }


//                    if (objDataSubReader != null)
//                    {
//                        if (!objDataSubReader.IsClosed)
//                            objDataSubReader.Close();

//                        objDataSubReader.Dispose();
//                        objDataSubReader = null;
//                    }

//                    if (objDbSubCommand != null)
//                    {
//                        objDbSubCommand.Dispose();
//                        objDbSubCommand = null;
//                    }

//                    string remarks = string.Empty;
//                    if (majorCount > 0)
//                    {
//                        majorRemarks = "Major: " + string.Join(", ", marjorRemarks_.ToArray());
//                        remarks += majorRemarks + "\n";
//                    }
//                    if (minorCount > 0)
//                    {
//                        minorRemarks = "Minor: " + string.Join(", ", minorRemarks_.ToArray());
//                        remarks += minorRemarks + "\n";
//                    }
//                    if (seriousCount > 0)
//                    {
//                        seriousRemarks = "Serious: " + string.Join(", ", seriousRemarks_.ToArray());
//                        remarks += seriousRemarks + "\n";
//                    }
//                    if (observationCount > 0)
//                    {
//                        obsevRemarks = "Observation: " + string.Join(", ", obsevRemarks_.ToArray());
//                        remarks += obsevRemarks;
//                    }

//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = (++index).ToString();
//                    _row["Service Provider Name"] = objDataReader["TSP"].ToString();
//                    _row["Class Code"] = objDataReader["ClassCode"].ToString();
//                    _row["Major Violations"] = majorCount;
//                    _row["Minor Violations"] = minorCount;
//                    _row["Serious Violations"] = seriousCount;
//                    _row["Total Violations"] = totalViolations;
//                    _row["Observations Total"] = observationCount;
//                    _row["Remarks"] = remarks;

//                    if (String.IsNullOrEmpty(tspName))
//                    {
//                        tspName = objDataReader["TSP"].ToString();
//                    }

//                    if (objDataReader["TSP"].ToString() == tspName)
//                    {
//                        tspMajorVio += majorCount;
//                        tspMinorVio += minorCount;
//                        tspSeriousVio += seriousCount;
//                        tspTotalVio += majorCount + minorCount + seriousCount;
//                        tspObservVio += observationCount;

//                    }
//                    else
//                    {
//                        /*Violations will be shown as Observations if Scheme is Cost Sharing 2016*/
//                        if (SchemeID == CNST_COST_SHARING_SCHEME)
//                        {
//                            tspObservVio = tspObservVio + tspTotalVio;
//                            tspTotalVio = 0;
//                            tspMajorVio = 0;
//                            tspMinorVio = 0;
//                            tspSeriousVio = 0;
//                        }

//                        DataRow _rowTotal = dt.NewRow();
//                        _rowTotal["Sr No"] = string.Empty;
//                        _rowTotal["Service Provider Name"] = tspName + " Total";
//                        _rowTotal["Class Code"] = string.Empty;
//                        _rowTotal["Major Violations"] = tspMajorVio;
//                        _rowTotal["Minor Violations"] = tspMinorVio;
//                        _rowTotal["Serious Violations"] = tspSeriousVio;
//                        _rowTotal["Total Violations"] = tspTotalVio;
//                        _rowTotal["Observations Total"] = tspObservVio;

//                        tspMajorVio = majorCount;
//                        tspMinorVio = minorCount;
//                        tspSeriousVio = seriousCount;
//                        tspTotalVio = totalViolations;
//                        tspObservVio = observationCount;

//                        tspName = objDataReader["TSP"].ToString();
//                        dt.Rows.Add(_rowTotal);
//                    }


//                    dt.Rows.Add(_row);

//                    if (index == rowsCount)
//                    {
//                        /*Violations will be shown as Observations if Scheme is Cost Sharing 2016*/
//                        if (SchemeID == CNST_COST_SHARING_SCHEME)
//                        {
//                            tspObservVio = tspObservVio + tspTotalVio;
//                            tspTotalVio = 0;
//                            tspMajorVio = 0;
//                            tspMinorVio = 0;
//                            tspSeriousVio = 0;


//                            GrandObservCount += GrandTotalCount;
//                            GrandMajorCount = 0;
//                            GrandMinorCount = 0;
//                            GrandTotalCount = 0;
//                            GrandSeriousCount = 0;
//                        }

//                        DataRow _rowTotal = dt.NewRow();
//                        _rowTotal["Sr No"] = string.Empty;
//                        _rowTotal["Service Provider Name"] = tspName + " Total";
//                        _rowTotal["Class Code"] = string.Empty;
//                        _rowTotal["Major Violations"] = tspMajorVio;
//                        _rowTotal["Minor Violations"] = tspMinorVio;
//                        _rowTotal["Serious Violations"] = tspSeriousVio;
//                        _rowTotal["Total Violations"] = tspTotalVio;
//                        _rowTotal["Observations Total"] = tspObservVio;

//                        tspMajorVio = majorCount;
//                        tspMinorVio = minorCount;
//                        tspSeriousVio = seriousCount;
//                        tspTotalVio = totalViolations;
//                        tspObservVio = observationCount;

//                        tspName = objDataReader["TSP"].ToString();
//                        dt.Rows.Add(_rowTotal);

//                        DataRow _rowGrandTotal = dt.NewRow();
//                        _rowGrandTotal["Sr No"] = string.Empty;
//                        _rowGrandTotal["Service Provider Name"] = "Grand Total";
//                        _rowGrandTotal["Class Code"] = string.Empty;
//                        _rowGrandTotal["Major Violations"] = GrandMajorCount;
//                        _rowGrandTotal["Minor Violations"] = GrandMinorCount;
//                        _rowGrandTotal["Serious Violations"] = GrandSeriousCount;
//                        _rowGrandTotal["Total Violations"] = GrandTotalCount;
//                        _rowGrandTotal["Observations Total"] = GrandObservCount;

//                        dt.Rows.Add(_rowGrandTotal);
//                    }
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objDataReader != null)
//                {
//                    if (!objDataReader.IsClosed)
//                        objDataReader.Close();

//                    objDataReader.Dispose();
//                    objDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;

//                }

//            }
//        }

//        private Dictionary<string, string> getVioDescription()
//        {
//            Dictionary<string, string> ViolationsDescription = new Dictionary<string, string>();
//            ViolationsDescription.Add("1", "Trainee Attendance Register");
//            ViolationsDescription.Add("2", "Attendance Accuracy");
//            ViolationsDescription.Add("3", "Trainer Attendance register");
//            ViolationsDescription.Add("4", "Payment of Stipend");
//            ViolationsDescription.Add("5", "Stipend register Maintenance");
//            ViolationsDescription.Add("6", "Trainee ID Card");
//            ViolationsDescription.Add("7", "Fake/Ghost Trainee");
//            ViolationsDescription.Add("8", "Syllabus");
//            ViolationsDescription.Add("9", "Free Training");
//            ViolationsDescription.Add("10", "Books");
//            ViolationsDescription.Add("11", "Uniform");
//            ViolationsDescription.Add("12", "Stationary");
//            ViolationsDescription.Add("13", "Bags");
//            ViolationsDescription.Add("14", "Center Relocate");
//            ViolationsDescription.Add("15", "Class Merge");
//            ViolationsDescription.Add("16", "Class split");
//            ViolationsDescription.Add("17", "Shift changed");
//            ViolationsDescription.Add("18", "Non-functional");
//            ViolationsDescription.Add("19", "Attendance Register Format");
//            ViolationsDescription.Add("20", "Trainer changed");
//            ViolationsDescription.Add("21", "Sufficient Consumables");
//            ViolationsDescription.Add("22", "Sufficient equipment/tools for training");
//            ViolationsDescription.Add("23", "Quality of practical training");
//            ViolationsDescription.Add("24", "Quality of meals");
//            ViolationsDescription.Add("25", "Quality of boarding facility");
//            ViolationsDescription.Add("26", "Usefulness of the training");
//            ViolationsDescription.Add("27", "Number of average daily hours");
//            ViolationsDescription.Add("28", "Teacher trainee ratio");
//            ViolationsDescription.Add("29", "Short Leave");
//            ViolationsDescription.Add("30", "Contractual hours followed");
//            ViolationsDescription.Add("31", "Trainer changed");

//            return ViolationsDescription;
//        }
//        /********************************* Classes List to show for Form(III) report ******************************/
//        public DataTable LoadCentreList(Class objClass, string Month)
//        {

//            string SchemeID = objClass.Scheme.ID.ToString();
//            string TspID = objClass.ServiceProvider.ID.ToString();
//            string StartDate = Month.ToDateTime().ToString("yyyy/MM/dd");

//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(VisitDateTime) = month('{0}') AND year(VisitDateTime ) = year('{0}')", StartDate);


//            IDataReader objIDataReader = null;
//            DbCommand objDbCommand = null;

//            try
//            {
//                string strSql = @"select CnMr.CentreRequestID, Rtp.TrainingCentreName, Rtp.TrainingLocAddress, Rtp.TSPName, Rtp.SchemeName from AMSCentreMonitoring CnMr
//                                inner join AMSCentreMonitoringClass CnMrClass
//                                on CnMr.ID = CnMrClass.CentreMonitoringID

//                                inner join Class
//                                on Class.ID = CnMrClass.ClassID

//                                inner join AMSCentreRequest Rtp
//                                on Rtp.ID = CnMr.CentreRequestID

//                                where Class.SchemeID = '" + SchemeID + " ' and Class.SpID = '" + TspID + "'" + DateClause +
//                                " group by CnMr.CentreRequestID, Rtp.TrainingLocAddress, Rtp.TSPName, Rtp.SchemeName, Rtp.TrainingCentreName ";

//                objDbCommand = this.TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Select");
//                dt.Columns.Add("TSP Name");
//                dt.Columns.Add("Centre Name");
//                dt.Columns.Add("Centre Address");
//                dt.Columns.Add("Download");

//                while (objIDataReader.Read())
//                {
//                    string RequestID = objIDataReader["CentreRequestID"].ToString();
//                    DataRow _row = dt.NewRow();

//                    _row["Select"] = "<input id=\"" + RequestID + "\" type=\"checkbox\" class=\"ReportCheckbox\">";
//                    _row["Centre Name"] = objIDataReader["TrainingCentreName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["TrainingCentreName"].ToString();
//                    _row["TSP Name"] = objIDataReader["TSPName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["TSPName"].ToString();
//                    _row["Centre Address"] = objIDataReader["TrainingLocAddress"].Equals(DBNull.Value) ? string.Empty : objIDataReader["TrainingLocAddress"].ToString();
//                    _row["Download"] = "<a href=\"" + "JavaScript:Submit('AmsReports.aspx?requestID=" + RequestID + "&msg=download&reportType=1&date=" + StartDate + "','')" + "\"" + ">Download</a>";

//                    dt.Rows.Add(_row);

//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objIDataReader != null)
//                {
//                    if (!objIDataReader.IsClosed)
//                        objIDataReader.Close();

//                    objIDataReader.Dispose();
//                    objIDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;

//                }

//            }

//        }
//        public DataTable LoadClassList(Class objClass, string Month)
//        {
//            string SchemeID = objClass.Scheme.ID.ToString();
//            string TspId = objClass.ServiceProvider.ID.ToString();
//            string StartDate = Month.ToDateTime().ToString("yyyy/MM/dd");

//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", StartDate);


//            IDataReader objIDataReader = null;
//            DbCommand objDbCommand = null;
//            ResultSet<ClassMonitoringRequest> lstClass = new ResultSet<ClassMonitoringRequest>();

//            try
//            {
//                string strSql = @"select distinct cir.ClassCode, cir.ID, cir.SchemeName, cir.TSPName, trade.TradeName
//                                from dbo.AMSClassInspectionRequest cir 
//                                inner join dbo.AMSClassMonitoring cl_mr
//                                on cir.ID = cl_mr.ClassInspectionRequestID
//                                inner join dbo.Class class
//                                on class.ID = cir.ClassID
//                                inner join dbo.Trade trade
//                                on trade.ID = class.TradeID
//                                where ClassID in (
//	                            select ID from dbo.Class class 
//	                            where class.SchemeID = '" + SchemeID + "' AND class.SpID = '" + TspId + "') " + DateClause;

//                objDbCommand = this.TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Select");
//                dt.Columns.Add("Scheme");
//                dt.Columns.Add("Service Provider");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Trade");
//                dt.Columns.Add("Download");

//                while (objIDataReader.Read())
//                {
//                    string RequestID = objIDataReader["ID"].ToString();
//                    DataRow _row = dt.NewRow();

//                    _row["Select"] = "<input id=\"" + RequestID + "\" type=\"checkbox\" class=\"ReportCheckbox\">";
//                    _row["Class Code"] = objIDataReader["ClassCode"].Equals(DBNull.Value) ? string.Empty : objIDataReader["ClassCode"].ToString();
//                    _row["Scheme"] = objIDataReader["SchemeName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["SchemeName"].ToString();
//                    _row["Service Provider"] = objIDataReader["TSPName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["TSPName"].ToString();
//                    _row["Trade"] = objIDataReader["TradeName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["TradeName"].ToString();
//                    _row["Download"] = "<a href=\"" + "JavaScript:Submit('AmsReports.aspx?requestID=" + RequestID + "&msg=download&reportType=2&date=" + StartDate + "','')" + "\"" + ">Download</a>";

//                    dt.Rows.Add(_row);

//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objIDataReader != null)
//                {
//                    if (!objIDataReader.IsClosed)
//                        objIDataReader.Close();

//                    objIDataReader.Dispose();
//                    objIDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;

//                }

//            }


//        }

//        /************************** Confirmed Marginal Trainee Report *************************************/
//        public DataTable LoadCMTraineeReport(string SchemeID, string month)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            DbCommand objMonitoringDbCommand = null;
//            IDataReader objMonitoringDataReader = null;

//            //DbCommand objTraineeDbCommand = null;
//            // IDataReader objTraineeDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string SelectedMonth = month.ToDateTime().ToString("MM");
//            string SelectedYear = month.ToDateTime().ToString("yyyy");

//            int SelectMonthInt = Convert.ToInt16(SelectedMonth);
//            int SelectedYearInt = Convert.ToInt16(SelectedYear);

//            string PrevMonth = (SelectMonthInt == 1) ? "12" : (SelectMonthInt - 1).ToString();
//            SelectedMonth = Convert.ToInt32(SelectedMonth).ToString();

//            int PrevMonthYear = (SelectedMonth == "1") ? (SelectedYearInt - 1) : SelectedYearInt;


//            string PrevMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(PrevMonth)) + " " + PrevMonthYear;
//            string SelectedMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(SelectedMonth)) + " " + SelectedYear;

//            string currentMonthStartDate = SelectedYear + "-" + SelectedMonth + "-01";
//            string previousMonthStartDate = PrevMonthYear + "-" + PrevMonth + "-01";

//            string lastDay = (DateTime.DaysInMonth(SelectedYearInt, SelectMonthInt)).ToString();
//            string PrevMonthStartDate = PrevMonthYear + "/" + PrevMonth + "/1";
//            string currentMonthEndDate = SelectedYear + "/" + SelectedMonth + "/" + lastDay;
//            string SchemeType = string.Empty;

//            string DateClause = string.Empty;
//            string strSQL = string.Empty;

//            strSQL = string.Format(@"SELECT SchemeType FROM Scheme WHERE SchemeID = '{0}'", SchemeID);
//            objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//            objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);

//            if (objRequestDataReader.Read())
//            {
//                SchemeType = objRequestDataReader["SchemeType"].ToString();
//            }

//            // DateClause = string.Format(" AND year(VisitDateTime ) = year('{0}')", StartDate);
//            DateClause = " AND CONVERT(date, VisitDateTime) >= '" + PrevMonthStartDate + "' AND CONVERT(date, VisitDateTime) <= '" + currentMonthEndDate + "'";

//            try
//            {
//                strSQL = @"select TSPName, SchemeName, Cir.ClassCode,Class.Contract_TrainingDuration ,ClassInspectionRequestID as CirId, ClMr.ID as ClMrId, TraineesImported as TraineesImported, month(VisitDateTime) as Month " +
//                                "from AMSClassMonitoring  ClMr " +

//                                "inner join AMSClassInspectionRequest Cir " +
//                                "on Cir.ID = ClMr.ClassInspectionRequestID " +

//                                "inner join Class " +
//                                "on Class.ID = Cir.ClassID " +
//                                "where ClassInspectionRequestID in " +
//                                "(select ID from AMSClassInspectionRequest where ClassID in " +
//                                    "(select ID from Class where SchemeID = '" + SchemeID + "') " +
//                                ")" + DateClause + " order by ClassInspectionRequestID";


//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);


//                Dictionary<string, Dictionary<string, List<string>>> Cir = new Dictionary<string, Dictionary<string, List<string>>>();



//                while (objRequestDataReader.Read())
//                {


//                    List<string> SchemeName = objRequestDataReader["SchemeName"].ToString().Split(',').ToList();
//                    List<string> TSPName = objRequestDataReader["TSPName"].ToString().Split(',').ToList();
//                    List<string> ClassCode = objRequestDataReader["ClassCode"].ToString().Split(',').ToList();
//                    List<string> Duration = objRequestDataReader["Contract_TrainingDuration"].ToString().Split(',').ToList();

//                    string RequestID = objRequestDataReader["CirId"].ToString();
//                    string ClMrId = objRequestDataReader["ClMrId"].ToString();
//                    string TraineesImported = objRequestDataReader["TraineesImported"].ToString();
//                    string Month = objRequestDataReader["Month"].ToString();

//                    if (Cir.ContainsKey(RequestID))
//                    {
//                        if (Month == SelectedMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                Cir[RequestID]["SelectedMonthTsr"].Add(ClMrId);
//                            }
//                            else
//                            {
//                                Cir[RequestID]["SelectedMonthNoTsr"].Add(ClMrId);
//                            }
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                Cir[RequestID]["PrevMonthTsr"].Add(ClMrId);
//                            }
//                            else
//                            {
//                                Cir[RequestID]["PrevMonthNoTsr"].Add(ClMrId);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        List<string> SelectedMonthTsr = new List<string>();
//                        List<string> SelectedMonthNoTsr = new List<string>();
//                        List<string> PrevMonthTsr = new List<string>();
//                        List<string> PrevMonthNoTsr = new List<string>();

//                        if (Month == SelectedMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                SelectedMonthTsr.Add(ClMrId);
//                            }
//                            else
//                            {
//                                SelectedMonthNoTsr.Add(ClMrId);
//                            }
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            if (TraineesImported == "1")
//                            {
//                                PrevMonthTsr.Add(ClMrId);
//                            }
//                            else
//                            {
//                                PrevMonthNoTsr.Add(ClMrId);
//                            }
//                        }
//                        Dictionary<string, List<string>> CirIds = new Dictionary<string, List<string>>();

//                        CirIds.Add("SelectedMonthTsr", SelectedMonthTsr);
//                        CirIds.Add("SelectedMonthNoTsr", SelectedMonthNoTsr);
//                        CirIds.Add("PrevMonthTsr", PrevMonthTsr);
//                        CirIds.Add("PrevMonthNoTsr", PrevMonthNoTsr);

//                        CirIds.Add("SchemeName", SchemeName);
//                        CirIds.Add("TspName", TSPName);
//                        CirIds.Add("ClassCode", ClassCode);
//                        CirIds.Add("Duration", Duration);

//                        Cir.Add(RequestID, CirIds);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;
//                }

//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> TraineesList = getCMTraineeFromDb(SchemeType, Cir, SelectedMonthString, PrevMonthString, currentMonthStartDate, previousMonthStartDate);

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Course Duration (Months)");
//                //dt.Columns.Add("Trainee ID");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Father/Husband Name");
//                dt.Columns.Add("CNIC");
//                // dt.Columns.Add(SelectedMonthString + " - Visit2");
//                dt.Columns.Add("Confirmed Marginal " + SelectedMonthString);
//                //dt.Columns.Add(PrevMonthString + " - Visit1");
//                dt.Columns.Add("Confirmed Marginal " + PrevMonthString);
//                dt.Columns.Add("Remarks");

//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in TraineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Service Provider Name"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];
//                    _row["Course Duration (Months)"] = entry.Value["Duration"];
//                    //_row["Trainee ID"] = entry.Value["ID"];
//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["Father/Husband Name"] = entry.Value["FName"];
//                    _row["CNIC"] = entry.Value["CNIC"];

//                    //_row[SelectedMonthString + " - Visit2"] = entry.Value.ContainsKey(SelectedMonthString + " - Visit2") ? entry.Value[SelectedMonthString + " - Visit2"] : "";

//                    _row["Confirmed Marginal " + SelectedMonthString] = entry.Value.ContainsKey(SelectedMonthString) ? entry.Value[SelectedMonthString] : "";

//                    //_row[PrevMonthString + " - Visit1"] = entry.Value.ContainsKey(PrevMonthString + " - Visit1") ? entry.Value[PrevMonthString + " - Visit1"] : "";

//                    _row["Confirmed Marginal " + PrevMonthString] = entry.Value.ContainsKey(PrevMonthString) ? entry.Value[PrevMonthString] : "";
//                    if (SchemeType == PSDFConstants.CNST_SchemeType_NA)
//                    {
//                        _row["Remarks"] = "Marked as " + entry.Value["Remarks"].Replace("_", " ").Replace("marked", "");
//                    }
//                    else
//                    {
//                        _row["Remarks"] = entry.Value["Remarks"];
//                    }


//                    dt.Rows.Add(_row);
//                    index++;
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objMonitoringDataReader != null)
//                {
//                    if (!objMonitoringDataReader.IsClosed)
//                        objMonitoringDataReader.Close();

//                    objMonitoringDataReader.Dispose();
//                    objMonitoringDataReader = null;

//                }

//                if (objMonitoringDbCommand != null)
//                {
//                    objMonitoringDbCommand.Dispose();
//                    objMonitoringDbCommand = null;
//                }
//            }

//        }

//        private Dictionary<string, Dictionary<string, string>> getCMTraineeFromDb(string SchemeType, Dictionary<string, Dictionary<string, List<string>>> CirList, string SelectedMonth, string PrevMonth, string SelectedMonthStartDate, string PrevMonthStartDate)
//        {

//            try
//            {
//                Dictionary<string, Dictionary<string, string>> TraineesList = new Dictionary<string, Dictionary<string, string>>();

//                foreach (KeyValuePair<string, Dictionary<string, List<string>>> entry in CirList)
//                {
//                    string joinString = ",";

//                    string RequestID = entry.Key;
//                    string SchemeName = string.Join("", entry.Value["SchemeName"].ToArray());
//                    string TspName = string.Join("", entry.Value["TspName"].ToArray());
//                    string ClassCode = string.Join("", entry.Value["ClassCode"].ToArray());
//                    string Duration = string.Join("", entry.Value["Duration"].ToArray());

//                    string SelectedMonthTsrString = string.Join(joinString, entry.Value["SelectedMonthTsr"].ToArray());
//                    string SelectedMonthNoTsrString = string.Join(joinString, entry.Value["SelectedMonthNoTsr"].ToArray());
//                    string PrevMonthTsrString = string.Join(joinString, entry.Value["PrevMonthTsr"].ToArray());
//                    string PrevMonthNoTsrString = string.Join(joinString, entry.Value["PrevMonthNoTsr"].ToArray());

//                    if (!string.IsNullOrEmpty(SelectedMonthTsrString))
//                    {
//                        if (SchemeType == PSDFConstants.CNST_SchemeType_NA)
//                        {
//                            string strSqlSelectMonthWithTsr = string.Empty;

//                            strSqlSelectMonthWithTsr = @"select ClassMonitoringID, AttendanceStatus, TraineeID, DisplayName, Cnic, FatherName," +
//                                                          "case when IsMarginal = 'y' then '\u0050' " +
//                                                                "end as IsMarginal " +
//                                                          "from AMSClassMonitoringTrainee " +
//                                                                "inner join UserInfo " +
//                                                                "on UserInfo.ID = AMSClassMonitoringTrainee.TraineeID " +
//                                                                "where ClassMonitoringID in (" + SelectedMonthTsrString + ") and IsMarginal = 'y' order by TraineeID";

//                            DbCommand objTraineeDbCommand = null;
//                            IDataReader objTraineeDataReader = null;

//                            objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlSelectMonthWithTsr);
//                            objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);
//                            while (objTraineeDataReader.Read())
//                            {
//                                string TraineeID = objTraineeDataReader["TraineeID"].ToString();
//                                string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                                if (!TraineesList.ContainsKey(TraineeID))
//                                {
//                                    Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                    trainee.Add("ID", TraineeID);
//                                    trainee.Add("Tsp", TspName);
//                                    trainee.Add("ClassCode", ClassCode);
//                                    trainee.Add("Duration", Duration);
//                                    trainee.Add("Name", objTraineeDataReader["DisplayName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["DisplayName"]);
//                                    trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                    trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);

//                                    trainee.Add("Remarks", objTraineeDataReader["AttendanceStatus"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["AttendanceStatus"]);


//                                    trainee.Add(SelectedMonth + " - Visit2", MonitoringID);
//                                    trainee.Add(SelectedMonth, objTraineeDataReader["IsMarginal"].ToString());

//                                    trainee.Add(PrevMonth + " - Visit1", "-");
//                                    trainee.Add(PrevMonth, "");
//                                    TraineesList.Add(TraineeID, trainee);
//                                }
//                            }

//                            if (objTraineeDataReader != null)
//                            {
//                                if (!objTraineeDataReader.IsClosed)
//                                    objTraineeDataReader.Close();

//                                objTraineeDataReader.Dispose();
//                                objTraineeDataReader = null;

//                            }

//                            if (objTraineeDbCommand != null)
//                            {
//                                objTraineeDbCommand.Dispose();
//                                objTraineeDbCommand = null;
//                            }
//                        }
//                        else
//                        {
//                            TraineesList = getNonNASchemeCMTrainees(ref TraineesList, RequestID, TspName, ClassCode, Duration, SelectedMonth, PrevMonth, SelectedMonthStartDate, "current");
//                        }

//                    }

//                    if (!string.IsNullOrEmpty(SelectedMonthNoTsrString))
//                    {
//                        string strSqlSelectedMonthNoTsr = @"select ID, ClassMonitoringID, Name, Cnic, FatherName," +
//                            "case when type = 'marginal' then '\u0050' end as IsMarginal " +
//                            "from AMSClassMonitoringReportedTrainee  " +
//                            "where ClassMonitoringID in (" + SelectedMonthNoTsrString + ") and type = 'marginal' order by ID";

//                        DbCommand objTraineeDbCommand = null;
//                        IDataReader objTraineeDataReader = null;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlSelectedMonthNoTsr);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        while (objTraineeDataReader.Read())
//                        {
//                            string TraineeID = objTraineeDataReader["ID"].ToString() + objTraineeDataReader["Cnic"].ToString();
//                            string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                            if (!TraineesList.ContainsKey(TraineeID))
//                            {
//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                trainee.Add("ID", TraineeID);
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Duration", Duration);
//                                trainee.Add("Name", objTraineeDataReader["Name"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Name"]);
//                                trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);

//                                trainee.Add("Remarks", "Marginal Trainee");

//                                trainee.Add(SelectedMonth + " - Visit2", MonitoringID);
//                                trainee.Add(SelectedMonth, objTraineeDataReader["IsMarginal"].ToString());

//                                trainee.Add(PrevMonth + " - Visit1", "-");
//                                trainee.Add(PrevMonth, "");
//                                TraineesList.Add(TraineeID, trainee);
//                            }
//                        }

//                        if (objTraineeDataReader != null)
//                        {
//                            if (!objTraineeDataReader.IsClosed)
//                                objTraineeDataReader.Close();

//                            objTraineeDataReader.Dispose();
//                            objTraineeDataReader = null;

//                        }

//                        if (objTraineeDbCommand != null)
//                        {
//                            objTraineeDbCommand.Dispose();
//                            objTraineeDbCommand = null;
//                        }
//                    }

//                    if (!string.IsNullOrEmpty(PrevMonthTsrString))
//                    {
//                        if (SchemeType == PSDFConstants.CNST_SchemeType_NA)
//                        {
//                            string strSqlPrevMonthTsrString = @"select ClassMonitoringID, AttendanceStatus, TraineeID, DisplayName, Cnic, FatherName," +
//                                                      "case when IsMarginal = 'y' then '\u0050' " +
//                                                            "end as IsMarginal " +
//                                                      "from AMSClassMonitoringTrainee " +
//                                                            "inner join UserInfo " +
//                                                            "on UserInfo.ID = AMSClassMonitoringTrainee.TraineeID " +
//                                                            "where ClassMonitoringID in (" + PrevMonthTsrString + ") and IsMarginal = 'y' order by TraineeID";

//                            DbCommand objTraineeDbCommand = null;
//                            IDataReader objTraineeDataReader = null;

//                            objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlPrevMonthTsrString);
//                            objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);
//                            while (objTraineeDataReader.Read())
//                            {
//                                string TraineeID = objTraineeDataReader["TraineeID"].ToString();
//                                string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                                if (!TraineesList.ContainsKey(TraineeID))
//                                {
//                                    Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                    trainee.Add("ID", TraineeID);
//                                    trainee.Add("Tsp", TspName);
//                                    trainee.Add("ClassCode", ClassCode);
//                                    trainee.Add("Duration", Duration);
//                                    trainee.Add("Name", objTraineeDataReader["DisplayName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["DisplayName"]);
//                                    trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                    trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);

//                                    trainee.Add("Remarks", objTraineeDataReader["AttendanceStatus"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["AttendanceStatus"]);

//                                    trainee.Add(PrevMonth + " - Visit1", MonitoringID);
//                                    trainee.Add(PrevMonth, objTraineeDataReader["IsMarginal"].ToString());

//                                    trainee.Add(SelectedMonth + " - Visit2", "-");
//                                    trainee.Add(SelectedMonth, "");
//                                    TraineesList.Add(TraineeID, trainee);
//                                }
//                                else
//                                {
//                                    TraineesList[TraineeID][PrevMonth + " - Visit1"] += ", " + MonitoringID;
//                                    TraineesList[TraineeID][PrevMonth] = objTraineeDataReader["IsMarginal"].ToString();
//                                }
//                            }

//                            if (objTraineeDataReader != null)
//                            {
//                                if (!objTraineeDataReader.IsClosed)
//                                    objTraineeDataReader.Close();

//                                objTraineeDataReader.Dispose();
//                                objTraineeDataReader = null;

//                            }

//                            if (objTraineeDbCommand != null)
//                            {
//                                objTraineeDbCommand.Dispose();
//                                objTraineeDbCommand = null;
//                            }
//                        }
//                        else
//                        {
//                            TraineesList = getNonNASchemeCMTrainees(ref TraineesList, RequestID, TspName, ClassCode, Duration, SelectedMonth, PrevMonth, PrevMonthStartDate, "previous");
//                        }



//                    }

//                    if (!string.IsNullOrEmpty(PrevMonthNoTsrString))
//                    {
//                        string strSqlPrevMonthNoTsr = @"select ID, ClassMonitoringID, Name, Cnic, FatherName," +
//                            "case when type = 'marginal' then '\u0050' end as IsMarginal " +
//                            "from AMSClassMonitoringReportedTrainee  " +
//                            "where ClassMonitoringID in (" + PrevMonthNoTsrString + ") and type = 'marginal' order by ID";

//                        DbCommand objTraineeDbCommand = null;
//                        IDataReader objTraineeDataReader = null;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlPrevMonthNoTsr);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        while (objTraineeDataReader.Read())
//                        {
//                            string TraineeID = objTraineeDataReader["ID"].ToString() + objTraineeDataReader["Cnic"].ToString();
//                            string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                            if (!TraineesList.ContainsKey(TraineeID))
//                            {
//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                trainee.Add("ID", TraineeID);
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Duration", Duration);
//                                trainee.Add("Name", objTraineeDataReader["Name"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Name"]);
//                                trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);

//                                trainee.Add("Remarks", "Marginal Trainee");

//                                trainee.Add(PrevMonth + " - Visit1", MonitoringID);
//                                trainee.Add(PrevMonth, objTraineeDataReader["IsMarginal"].ToString());

//                                trainee.Add(SelectedMonth + " - Visit2", "-");
//                                trainee.Add(SelectedMonth, "");
//                                TraineesList.Add(TraineeID, trainee);
//                            }
//                            else
//                            {
//                                TraineesList[TraineeID][PrevMonth + " - Visit1"] += ", " + MonitoringID;
//                                TraineesList[TraineeID][PrevMonth] = objTraineeDataReader["IsMarginal"].ToString() + " " + MonitoringID;
//                            }
//                        }

//                        if (objTraineeDataReader != null)
//                        {
//                            if (!objTraineeDataReader.IsClosed)
//                                objTraineeDataReader.Close();

//                            objTraineeDataReader.Dispose();
//                            objTraineeDataReader = null;

//                        }

//                        if (objTraineeDbCommand != null)
//                        {
//                            objTraineeDbCommand.Dispose();
//                            objTraineeDbCommand = null;
//                        }
//                    }

//                }
//                return TraineesList;

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {

//            }
//        }

//        private Dictionary<string, Dictionary<string, string>> getNonNASchemeCMTrainees(ref Dictionary<string, Dictionary<string, string>> TraineesList,
//                                        string RequestID, string TspName, string ClassCode, string Duration, string SelectedMonth, string PrevMonth, string VisitMonthStartDate, string MonthStatus)
//        {

//            try
//            {
//                string strSqlSelectMonthWithTsr = string.Empty;

//                strSqlSelectMonthWithTsr = @"select VisitMonth, TraineeID, DisplayName, Cnic, FatherName, case when IsMarginal='y' then 'P' end as IsMarginal
//                                                    from AMSClassMonitoringTraineeMarginal inner join UserInfo on UserInfo.ID = AMSClassMonitoringTraineeMarginal.TraineeID
//                                                    Where ClassInspectionRequestID = '" + RequestID + "'  and VisitMonth='" + VisitMonthStartDate + "'" +
//                                                    " and IsMarginal='y' order by TraineeID";

//                strSqlSelectMonthWithTsr = strSqlSelectMonthWithTsr.Replace("\r\n", string.Empty);
//                DbCommand objTraineeDbCommand = null;
//                IDataReader objTraineeDataReader = null;

//                objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlSelectMonthWithTsr);
//                objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);
//                while (objTraineeDataReader.Read())
//                {
//                    string TraineeID = objTraineeDataReader["TraineeID"].ToString();

//                    if (!TraineesList.ContainsKey(TraineeID))
//                    {
//                        Dictionary<string, string> trainee = new Dictionary<string, string>();
//                        trainee.Add("ID", TraineeID);
//                        trainee.Add("Tsp", TspName);
//                        trainee.Add("ClassCode", ClassCode);
//                        trainee.Add("Duration", Duration);
//                        trainee.Add("Name", objTraineeDataReader["DisplayName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["DisplayName"]);
//                        trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                        trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);
//                        trainee.Add("Remarks", null);
//                        if (MonthStatus == "current")
//                        {
//                            trainee.Add(SelectedMonth, objTraineeDataReader["IsMarginal"].ToString());
//                            trainee.Add(PrevMonth, "");
//                        }
//                        else
//                        {
//                            trainee.Add(SelectedMonth, "");
//                            trainee.Add(PrevMonth, objTraineeDataReader["IsMarginal"].ToString());
//                        }
//                        TraineesList.Add(TraineeID, trainee);
//                    }
//                    if (MonthStatus == "previous")
//                    {
//                        TraineesList[TraineeID][PrevMonth] = objTraineeDataReader["IsMarginal"].ToString();
//                    }
//                }

//                if (objTraineeDataReader != null)
//                {
//                    if (!objTraineeDataReader.IsClosed)
//                        objTraineeDataReader.Close();

//                    objTraineeDataReader.Dispose();
//                    objTraineeDataReader = null;

//                }

//                if (objTraineeDbCommand != null)
//                {
//                    objTraineeDbCommand.Dispose();
//                    objTraineeDbCommand = null;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {

//            }

//            return TraineesList;
//        }
//        /************************** Additional Trainee Report *************************************/
//        public DataTable LoadAdditionalTraineeReport(string SchemeID, string month)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string SelectedMonth = month.ToDateTime().ToString("MM");
//            string SelectedYear = month.ToDateTime().ToString("yyyy");

//            int SelectedMonthInt = Convert.ToInt32(SelectedMonth);
//            string PrevMonth = (SelectedMonthInt == 1) ? "12" : (SelectedMonthInt - 1).ToString();

//            int prevMonthYearInt = Convert.ToInt32(SelectedYear);
//            int PrevMonthYear = (SelectedMonthInt == 1) ? prevMonthYearInt - 1 : prevMonthYearInt;
//            SelectedMonth = Convert.ToInt32(SelectedMonth).ToString();

//            string PrevMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(PrevMonth)) + " " + PrevMonthYear;
//            string SelectedMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(SelectedMonth)) + " " + SelectedYear;

//            string lastDay = (DateTime.DaysInMonth(Convert.ToInt16(SelectedYear), SelectedMonthInt)).ToString();
//            string PrevMonthStartDate = PrevMonthYear + "/" + PrevMonth + "/1";
//            string currentMonthEndDate = SelectedYear + "/" + SelectedMonth + "/" + lastDay;

//            string DateClause = string.Empty;
//            //DateClause = string.Format(" AND year(VisitDateTime ) = year('{0}')", StartDate);
//            DateClause = " AND CONVERT(date, VisitDateTime) >= '" + PrevMonthStartDate + "' AND CONVERT(date, VisitDateTime) <= '" + currentMonthEndDate + "'";

//            try
//            {
//                string strSQL = @"select TSPName, SchemeName, Cir.ClassCode,Class.Contract_TrainingDuration ,ClassInspectionRequestID as CirId, ClMr.ID as ClMrId, TraineesImported as TraineesImported, month(VisitDateTime) as Month " +
//                                 "from AMSClassMonitoring  ClMr " +

//                                 "inner join AMSClassInspectionRequest Cir " +
//                                 "on Cir.ID = ClMr.ClassInspectionRequestID " +

//                                 "inner join Class " +
//                                 "on Class.ID = Cir.ClassID " +
//                                 "where ClassInspectionRequestID in " +
//                                 "(select ID from AMSClassInspectionRequest where ClassID in " +
//                                     "(select ID from Class where SchemeID = '" + SchemeID + "') " +
//                                 ")" + DateClause + " order by ClassInspectionRequestID";


//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);


//                Dictionary<string, Dictionary<string, List<string>>> Cir = new Dictionary<string, Dictionary<string, List<string>>>();

//                while (objRequestDataReader.Read())
//                {

//                    List<string> SchemeName = objRequestDataReader["SchemeName"].ToString().Split(',').ToList();
//                    List<string> TSPName = objRequestDataReader["TSPName"].ToString().Split(',').ToList();
//                    List<string> ClassCode = objRequestDataReader["ClassCode"].ToString().Split(',').ToList();
//                    List<string> Duration = objRequestDataReader["Contract_TrainingDuration"].ToString().Split(',').ToList();

//                    string RequestID = objRequestDataReader["CirId"].ToString();
//                    string ClMrId = objRequestDataReader["ClMrId"].ToString();
//                    string Month = objRequestDataReader["Month"].ToString();

//                    if (Cir.ContainsKey(RequestID))
//                    {
//                        if (Month == SelectedMonth)
//                        {
//                            Cir[RequestID]["SelectedMonthVisitIDs"].Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            Cir[RequestID]["PrevMonthVisitIDs"].Add(ClMrId);
//                        }
//                    }
//                    else
//                    {
//                        List<string> SelectedMonthVisitIDs = new List<string>();
//                        List<string> PrevMonthVisitIDs = new List<string>();

//                        if (Month == SelectedMonth)
//                        {
//                            SelectedMonthVisitIDs.Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            PrevMonthVisitIDs.Add(ClMrId);
//                        }
//                        Dictionary<string, List<string>> CirIds = new Dictionary<string, List<string>>();

//                        CirIds.Add("SelectedMonthVisitIDs", SelectedMonthVisitIDs);
//                        CirIds.Add("PrevMonthVisitIDs", PrevMonthVisitIDs);

//                        CirIds.Add("SchemeName", SchemeName);
//                        CirIds.Add("TspName", TSPName);
//                        CirIds.Add("ClassCode", ClassCode);
//                        CirIds.Add("Duration", Duration);

//                        Cir.Add(RequestID, CirIds);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;
//                }

//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> TraineesList = getAdditionalTraineeFromDb(Cir, SelectedMonthString, PrevMonthString);


//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                // dt.Columns.Add("Course Duration (Months)");
//                //dt.Columns.Add("Trainee ID");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Father/Husband Name");
//                dt.Columns.Add("CNIC");

//                // dt.Columns.Add(SelectedMonthString + " - Visit2");

//                dt.Columns.Add(SelectedMonthString);

//                //dt.Columns.Add(PrevMonthString + " - Visit1");

//                dt.Columns.Add(PrevMonthString);
//                dt.Columns.Add("Remarks");



//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in TraineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Service Provider Name"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];
//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["Father/Husband Name"] = string.IsNullOrEmpty(entry.Value["FName"]) ? "Not Provided" : entry.Value["FName"];
//                    _row["CNIC"] = string.IsNullOrEmpty(entry.Value["CNIC"]) ? "Not Provided" : entry.Value["CNIC"];
//                    _row[SelectedMonthString] = entry.Value.ContainsKey(SelectedMonthString) ? entry.Value[SelectedMonthString] : "";
//                    _row[PrevMonthString] = entry.Value.ContainsKey(PrevMonthString) ? entry.Value[PrevMonthString] : "";
//                    _row["Remarks"] = "Additional";

//                    dt.Rows.Add(_row);
//                    index++;
//                }
//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {

//            }

//        }

//        private Dictionary<string, Dictionary<string, string>> getAdditionalTraineeFromDb(Dictionary<string, Dictionary<string, List<string>>> CirList, string SelectedMonth, string PrevMonth)
//        {
//            try
//            {
//                Dictionary<string, Dictionary<string, string>> TraineesList = new Dictionary<string, Dictionary<string, string>>();

//                foreach (KeyValuePair<string, Dictionary<string, List<string>>> entry in CirList)
//                {
//                    string joinString = ",";

//                    string SchemeName = string.Join("", entry.Value["SchemeName"].ToArray());
//                    string TspName = string.Join("", entry.Value["TspName"].ToArray());
//                    string ClassCode = string.Join("", entry.Value["ClassCode"].ToArray());
//                    string Duration = string.Join("", entry.Value["Duration"].ToArray());

//                    string SelectedMonthString = string.Join(joinString, entry.Value["SelectedMonthVisitIDs"].ToArray());
//                    string PrevMonthString = string.Join(joinString, entry.Value["PrevMonthVisitIDs"].ToArray());


//                    if (!string.IsNullOrEmpty(SelectedMonthString))
//                    {
//                        string strSqlSelectedMonthNoTsr = @"select ID, ClassMonitoringID, Name, Cnic, FatherName," +
//                            "case when type = 'additional' then '\u0050' end as IsAdditional " +
//                            "from AMSClassMonitoringReportedTrainee  " +
//                            "where ClassMonitoringID in (" + SelectedMonthString + ") and type = 'additional' order by ID";

//                        DbCommand objTraineeDbCommand = null;
//                        IDataReader objTraineeDataReader = null;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlSelectedMonthNoTsr);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        while (objTraineeDataReader.Read())
//                        {
//                            string TraineeID = objTraineeDataReader["ID"].ToString() + objTraineeDataReader["Cnic"].ToString();
//                            string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                            if (!TraineesList.ContainsKey(TraineeID))
//                            {
//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                trainee.Add("ID", TraineeID);
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Duration", Duration);
//                                trainee.Add("Name", objTraineeDataReader["Name"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Name"]);
//                                trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);

//                                trainee.Add(SelectedMonth + " - Visit2", MonitoringID);
//                                trainee.Add(SelectedMonth, objTraineeDataReader["IsAdditional"].ToString());

//                                trainee.Add(PrevMonth + " - Visit1", "-");
//                                trainee.Add(PrevMonth, "");
//                                TraineesList.Add(TraineeID, trainee);
//                            }
//                        }

//                        if (objTraineeDataReader != null)
//                        {
//                            if (!objTraineeDataReader.IsClosed)
//                                objTraineeDataReader.Close();

//                            objTraineeDataReader.Dispose();
//                            objTraineeDataReader = null;
//                        }

//                        if (objTraineeDbCommand != null)
//                        {
//                            objTraineeDbCommand.Dispose();
//                            objTraineeDbCommand = null;
//                        }
//                    }

//                    if (!string.IsNullOrEmpty(PrevMonthString))
//                    {
//                        string strSqlPrevMonthNoTsr = @"select ID, ClassMonitoringID, Name, Cnic, FatherName," +
//                            "case when type = 'additional' then '\u0050' end as IsAdditional " +
//                            "from AMSClassMonitoringReportedTrainee  " +
//                            "where ClassMonitoringID in (" + PrevMonthString + ") and type = 'additional' order by ID";

//                        DbCommand objTraineeDbCommand = null;
//                        IDataReader objTraineeDataReader = null;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlPrevMonthNoTsr);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        while (objTraineeDataReader.Read())
//                        {
//                            string TraineeID = objTraineeDataReader["ID"].ToString() + objTraineeDataReader["Cnic"].ToString();
//                            string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                            if (!TraineesList.ContainsKey(TraineeID))
//                            {
//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                trainee.Add("ID", TraineeID);
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Duration", Duration);
//                                trainee.Add("Name", objTraineeDataReader["Name"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Name"]);
//                                trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);


//                                trainee.Add(PrevMonth + " - Visit1", MonitoringID);
//                                trainee.Add(PrevMonth, objTraineeDataReader["IsAdditional"].ToString());

//                                trainee.Add(SelectedMonth + " - Visit2", "-");
//                                trainee.Add(SelectedMonth, "");
//                                TraineesList.Add(TraineeID, trainee);
//                            }
//                            else
//                            {
//                                TraineesList[TraineeID][PrevMonth + " - Visit1"] += ", " + MonitoringID;
//                                TraineesList[TraineeID][PrevMonth] = objTraineeDataReader["IsAdditional"].ToString() + " " + MonitoringID;
//                            }
//                        }
//                        if (objTraineeDataReader != null)
//                        {
//                            if (!objTraineeDataReader.IsClosed)
//                                objTraineeDataReader.Close();

//                            objTraineeDataReader.Dispose();
//                            objTraineeDataReader = null;
//                        }

//                        if (objTraineeDbCommand != null)
//                        {
//                            objTraineeDbCommand.Dispose();
//                            objTraineeDbCommand = null;
//                        }
//                    }

//                }

//                return TraineesList;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        /************************** Deleted Dropout Trainee Report *************************************/
//        private Dictionary<string, Dictionary<string, string>> getDelDOTraineeFromDb(Dictionary<string, Dictionary<string, List<string>>> CirList, string SelectedMonth, string PrevMonth)
//        {

//            try
//            {
//                Dictionary<string, Dictionary<string, string>> TraineesList = new Dictionary<string, Dictionary<string, string>>();

//                foreach (KeyValuePair<string, Dictionary<string, List<string>>> entry in CirList)
//                {
//                    string joinString = ",";

//                    string SchemeName = string.Join("", entry.Value["SchemeName"].ToArray());
//                    string TspName = string.Join("", entry.Value["TspName"].ToArray());
//                    string ClassCode = string.Join("", entry.Value["ClassCode"].ToArray());

//                    string SelectedMonthString = string.Join(joinString, entry.Value["SelectedMonthVisitIDs"].ToArray());
//                    string PrevMonthString = string.Join(joinString, entry.Value["PrevMonthVisitIDs"].ToArray());

//                    if (!string.IsNullOrEmpty(SelectedMonthString))
//                    {
//                        string strSqlSelectedMonthNoTsr = @"select ClassMonitoringID, TraineeID, DisplayName, Cnic, FatherName, " +
//                            "case when AttendanceStatus = 'deleted' then 'Deleted' " +
//                            "when AttendanceStatus = 'dropout' then 'Dropout' " +
//                            "when AttendanceStatus = 'dropout_but_present' then 'Dropout but marked Present' " +
//                            "end as Remarks, " +
//                            "case when AMSClassMonitoringTrainee.AttendanceStatus in  ('deleted' ,'dropout', 'dropout_but_present') then '\u0050' " +
//                            "else '-' " +
//                            "end as status " +
//                            "from AMSClassMonitoringTrainee " +
//                            "inner join UserInfo on UserInfo.ID = AMSClassMonitoringTrainee.TraineeID " +
//                            "inner join AMSClassMonitoring on ClassMonitoringID = AMSClassMonitoring.ID " +
//                            "where AttendanceStatus in ('deleted', 'dropout', 'dropout_but_present') AND ClassMonitoringID in (" + SelectedMonthString + ") order by TraineeID, VisitNo DESC";

//                        DbCommand objTraineeDbCommand = null;
//                        IDataReader objTraineeDataReader = null;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlSelectedMonthNoTsr);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        while (objTraineeDataReader.Read())
//                        {
//                            string TraineeID = objTraineeDataReader["TraineeID"].ToString();
//                            string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                            if (!TraineesList.ContainsKey(TraineeID))
//                            {
//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                trainee.Add("ID", TraineeID);
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Name", objTraineeDataReader["DisplayName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["DisplayName"]);
//                                trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);

//                                trainee.Add(SelectedMonth + " - Visit2", MonitoringID);
//                                trainee.Add(SelectedMonth, objTraineeDataReader["status"].ToString());

//                                trainee.Add(PrevMonth + " - Visit1", "-");
//                                trainee.Add(PrevMonth, "");

//                                trainee.Add("Remarks", objTraineeDataReader["Remarks"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Remarks"]);

//                                TraineesList.Add(TraineeID, trainee);
//                            }
//                        }

//                        if (objTraineeDataReader != null)
//                        {
//                            if (!objTraineeDataReader.IsClosed)
//                                objTraineeDataReader.Close();

//                            objTraineeDataReader.Dispose();
//                            objTraineeDataReader = null;
//                        }

//                        if (objTraineeDbCommand != null)
//                        {
//                            objTraineeDbCommand.Dispose();
//                            objTraineeDbCommand = null;
//                        }
//                    }

//                    if (!string.IsNullOrEmpty(PrevMonthString))
//                    {
//                        string strSqlPrevMonthNoTsr = @"select ClassMonitoringID, TraineeID, DisplayName, Cnic, FatherName, " +
//                            "case when AttendanceStatus = 'deleted' then 'Deleted' " +
//                            "when AttendanceStatus = 'dropout' then 'Dropout' " +
//                            "when AttendanceStatus = 'dropout_but_present' then 'Dropout but marked Present' " +
//                            "end as Remarks, " +
//                            "case when AMSClassMonitoringTrainee.AttendanceStatus in  ('deleted' ,'dropout', 'dropout_but_present') then '\u0050' " +
//                            "else '-' " +
//                            "end as status " +
//                            "from AMSClassMonitoringTrainee " +
//                            "inner join UserInfo on UserInfo.ID = AMSClassMonitoringTrainee.TraineeID " +
//                            "inner join AMSClassMonitoring on ClassMonitoringID = AMSClassMonitoring.ID " +
//                            "where AttendanceStatus in ('deleted', 'dropout', 'dropout_but_present' ) AND ClassMonitoringID in (" + PrevMonthString + ") order by TraineeID, VisitNo DESC";

//                        DbCommand objTraineeDbCommand = null;
//                        IDataReader objTraineeDataReader = null;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(strSqlPrevMonthNoTsr);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        while (objTraineeDataReader.Read())
//                        {
//                            string TraineeID = objTraineeDataReader["TraineeID"].ToString();
//                            string MonitoringID = objTraineeDataReader["ClassMonitoringID"].ToString();

//                            if (!TraineesList.ContainsKey(TraineeID))
//                            {
//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                trainee.Add("ID", TraineeID);
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Name", objTraineeDataReader["DisplayName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["DisplayName"]);
//                                trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);


//                                trainee.Add(PrevMonth + " - Visit1", MonitoringID);
//                                trainee.Add(PrevMonth, objTraineeDataReader["status"].ToString());

//                                trainee.Add(SelectedMonth + " - Visit2", "-");
//                                trainee.Add(SelectedMonth, "");

//                                trainee.Add("Remarks", objTraineeDataReader["Remarks"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Remarks"]);

//                                TraineesList.Add(TraineeID, trainee);
//                            }
//                            else
//                            {
//                                TraineesList[TraineeID][PrevMonth + " - Visit1"] += ", " + MonitoringID;
//                                TraineesList[TraineeID][PrevMonth] = objTraineeDataReader["status"].ToString();
//                                //TraineesList[TraineeID]["Remarks"] = objTraineeDataReader["Remarks"].ToString();
//                            }
//                        }

//                        if (objTraineeDataReader != null)
//                        {
//                            if (!objTraineeDataReader.IsClosed)
//                                objTraineeDataReader.Close();

//                            objTraineeDataReader.Dispose();
//                            objTraineeDataReader = null;
//                        }

//                        if (objTraineeDbCommand != null)
//                        {
//                            objTraineeDbCommand.Dispose();
//                            objTraineeDbCommand = null;
//                        }
//                    }

//                }

//                return TraineesList;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public DataTable LoadDelDOTraineeReport(string SchemeID, string month)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");

//            string SelectedMonth = month.ToDateTime().ToString("MM");
//            string SelectedYear = month.ToDateTime().ToString("yyyy");

//            int SelectedMonthInt = Convert.ToInt32(SelectedMonth);
//            string PrevMonth = (SelectedMonthInt == 1) ? "12" : (SelectedMonthInt - 1).ToString();
//            SelectedMonth = Convert.ToInt32(SelectedMonth).ToString();

//            int SelectedYearInt = Convert.ToInt32(SelectedYear);
//            int PrevMonthYear = (SelectedMonthInt == 1) ? SelectedYearInt - 1 : SelectedYearInt;

//            string PrevMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(PrevMonth)) + " " + PrevMonthYear;
//            string SelectedMonthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(SelectedMonth)) + " " + SelectedYear;

//            string lastDay = (DateTime.DaysInMonth(SelectedYearInt, SelectedMonthInt)).ToString();
//            string PrevMonthStartDate = PrevMonthYear + "/" + PrevMonth + "/1";
//            string currentMonthEndDate = SelectedYear + "/" + SelectedMonth + "/" + lastDay;

//            string DateClause = string.Empty;
//            // DateClause = string.Format(" AND year(VisitDateTime ) = year('{0}')", StartDate);
//            DateClause = " AND CONVERT(date, VisitDateTime) >= '" + PrevMonthStartDate + "' AND CONVERT(date, VisitDateTime) <= '" + currentMonthEndDate + "'";


//            try
//            {

//                string strSQL = @"select TSPName, SchemeName, Cir.ClassCode,Class.Contract_TrainingDuration ,ClassInspectionRequestID as CirId, ClMr.ID as ClMrId, TraineesImported as TraineesImported, month(VisitDateTime) as Month " +
//                                 "from AMSClassMonitoring  ClMr " +

//                                 "inner join AMSClassInspectionRequest Cir " +
//                                 "on Cir.ID = ClMr.ClassInspectionRequestID " +

//                                 "inner join Class " +
//                                 "on Class.ID = Cir.ClassID " +
//                                 "where ClassInspectionRequestID in " +
//                                 "(select ID from AMSClassInspectionRequest where ClassID in " +
//                                     "(select ID from Class where SchemeID = '" + SchemeID + "') " +
//                                 ")" + DateClause + " order by ClassInspectionRequestID, VisitNo DESC";


//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);

//                Dictionary<string, Dictionary<string, List<string>>> Cir = new Dictionary<string, Dictionary<string, List<string>>>();

//                while (objRequestDataReader.Read())
//                {

//                    List<string> SchemeName = objRequestDataReader["SchemeName"].ToString().Split(',').ToList();
//                    List<string> TSPName = objRequestDataReader["TSPName"].ToString().Split(',').ToList();
//                    List<string> ClassCode = objRequestDataReader["ClassCode"].ToString().Split(',').ToList();

//                    string RequestID = objRequestDataReader["CirId"].ToString();
//                    string ClMrId = objRequestDataReader["ClMrId"].ToString();
//                    string Month = objRequestDataReader["Month"].ToString();

//                    if (Cir.ContainsKey(RequestID))
//                    {
//                        if (Month == SelectedMonth)
//                        {
//                            Cir[RequestID]["SelectedMonthVisitIDs"].Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            Cir[RequestID]["PrevMonthVisitIDs"].Add(ClMrId);
//                        }
//                    }
//                    else
//                    {
//                        List<string> SelectedMonthVisitIDs = new List<string>();
//                        List<string> PrevMonthVisitIDs = new List<string>();

//                        if (Month == SelectedMonth)
//                        {
//                            SelectedMonthVisitIDs.Add(ClMrId);
//                        }
//                        else if (Month == PrevMonth)
//                        {
//                            PrevMonthVisitIDs.Add(ClMrId);
//                        }
//                        Dictionary<string, List<string>> CirIds = new Dictionary<string, List<string>>();

//                        CirIds.Add("SelectedMonthVisitIDs", SelectedMonthVisitIDs);
//                        CirIds.Add("PrevMonthVisitIDs", PrevMonthVisitIDs);

//                        CirIds.Add("SchemeName", SchemeName);
//                        CirIds.Add("TspName", TSPName);
//                        CirIds.Add("ClassCode", ClassCode);

//                        Cir.Add(RequestID, CirIds);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;

//                }

//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> TraineesList = getDelDOTraineeFromDb(Cir, SelectedMonthString, PrevMonthString);


//                DataTable dt = new DataTable();

//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Service Provider Name");
//                dt.Columns.Add("Class Code");
//                // dt.Columns.Add("Course Duration (Months)");
//                //dt.Columns.Add("Trainee ID");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Father/Husband Name");
//                dt.Columns.Add("CNIC");

//                // dt.Columns.Add(SelectedMonthString + " - Visit2");

//                dt.Columns.Add(SelectedMonthString);

//                //dt.Columns.Add(PrevMonthString + " - Visit1");

//                dt.Columns.Add(PrevMonthString);
//                dt.Columns.Add("Remarks");

//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in TraineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Service Provider Name"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];

//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["Father/Husband Name"] = entry.Value["FName"];
//                    _row["CNIC"] = entry.Value["CNIC"];

//                    //_row[SelectedMonthString + " - Visit2"] = entry.Value.ContainsKey(SelectedMonthString + " - Visit2") ? entry.Value[SelectedMonthString + " - Visit2"] : "";

//                    _row[SelectedMonthString] = entry.Value.ContainsKey(SelectedMonthString) ? entry.Value[SelectedMonthString] : "";

//                    //_row[PrevMonthString + " - Visit1"] = entry.Value.ContainsKey(PrevMonthString + " - Visit1") ? entry.Value[PrevMonthString + " - Visit1"] : "";

//                    _row[PrevMonthString] = entry.Value.ContainsKey(PrevMonthString) ? entry.Value[PrevMonthString] : "";
//                    _row["Remarks"] = entry.Value["Remarks"];

//                    dt.Rows.Add(_row);
//                    index++;
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }

//        }

//        /************************** Profile Verification Class List *************************************/
//        public DataTable LoadPVClassList(Class objClass, string Month)
//        {
//            string SchemeID = objClass.Scheme.ID.ToString();
//            string TspId = objClass.ServiceProvider.ID.ToString();
//            string StartDate = Month.ToDateTime().ToString("yyyy/MM/dd");

//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(cl_mr.VisitDateTime) = month('{0}') AND year(cl_mr.VisitDateTime ) = year('{0}')", StartDate);

//            IDataReader objIDataReader = null;
//            DbCommand objDbCommand = null;
//            try
//            {
//                string strSql = @"select distinct cir.ClassCode, cir.ID, cir.SchemeName, cir.TSPName, trade.TradeName
//                                from dbo.AMSClassInspectionRequest cir 
//                                inner join dbo.AMSClassMonitoring cl_mr
//                                on cir.ID = cl_mr.ClassInspectionRequestID
//                                inner join dbo.Class class
//                                on class.ID = cir.ClassID
//                                inner join dbo.Trade trade
//                                on trade.ID = class.TradeID
//                                where ClassID in (
//	                            select ID from dbo.Class class 
//	                            where class.SchemeID = '" + SchemeID + "' AND class.SpID = '" + TspId + "') " + DateClause +
//                                " AND TraineesImported = 1";

//                objDbCommand = this.TMData.GetSqlStringCommand(strSql);
//                objIDataReader = TMData.ExecuteReader(objDbCommand);

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Select");
//                dt.Columns.Add("Scheme");
//                dt.Columns.Add("Service Provider");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Trade");
//                dt.Columns.Add("Download");

//                while (objIDataReader.Read())
//                {

//                    string RequestID = objIDataReader["ID"].ToString();
//                    DataRow _row = dt.NewRow();

//                    _row["Select"] = "<input id=\"" + RequestID + "\" type=\"checkbox\" class=\"ReportCheckbox\">";
//                    _row["Class Code"] = objIDataReader["ClassCode"].Equals(DBNull.Value) ? string.Empty : objIDataReader["ClassCode"].ToString();
//                    _row["Scheme"] = objIDataReader["SchemeName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["SchemeName"].ToString();
//                    _row["Service Provider"] = objIDataReader["TSPName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["TSPName"].ToString();
//                    _row["Trade"] = objIDataReader["TradeName"].Equals(DBNull.Value) ? string.Empty : objIDataReader["TradeName"].ToString();
//                    _row["Download"] = "<a href=\"" + "JavaScript:Submit('AmsReports.aspx?requestID=" + RequestID + "&msg=download&reportType=10&date=" + StartDate + "','')" + "\"" + ">Download</a>";

//                    dt.Rows.Add(_row);

//                }

//                return dt;

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objIDataReader != null)
//                {
//                    if (!objIDataReader.IsClosed)
//                        objIDataReader.Close();

//                    objIDataReader.Dispose();
//                    objIDataReader = null;

//                }
//                if (objDbCommand != null)
//                {
//                    objDbCommand.Dispose();
//                    objDbCommand = null;

//                }

//            }


//        }

//        /************************** Ojt Trainee Report *************************************/
//        public DataTable LoadOjtReport(string SchemeID, string month)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            DbCommand objMonitoringDbCommand = null;
//            IDataReader objMonitoringDataReader = null;

//            DbCommand objTraineeDbCommand = null;
//            IDataReader objTraineeDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");
//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(VisitDateTime) = month('{0}') AND year(VisitDateTime ) = year('{0}')", StartDate);


//            try
//            {
//                string strSQL = @"SELECT Ojt.ID, OjtLocation, ClassID, DisplayName as Tsp, OjtClass.TradeName,
//                        OjtClass.ClassCode from AMSOnJobTrainingRequest Ojt
//                        inner join AMSOnJobTrainingClass OjtClass on OjtClass.OnJobTrainingID = Ojt.ID
//                        inner join Class on Class.ID = OjtClass.ClassID
//                        inner join UserInfo on UserInfo.ID = Class.SpID
//                        where Class.SchemeID = '" + SchemeID + "'";

//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);

//                Dictionary<string, Dictionary<string, string>> ClassInfoList = new Dictionary<string, Dictionary<string, string>>();

//                Dictionary<string, string> OjtInfoList = new Dictionary<string, string>();

//                List<string> OjtIds = new List<string>();

//                while (objRequestDataReader.Read())
//                {
//                    string ClassID = objRequestDataReader["ClassID"].ToString();
//                    string OjtID = objRequestDataReader["ID"].ToString();
//                    string OjtLoc = objRequestDataReader["OjtLocation"].ToString();

//                    if (!ClassInfoList.ContainsKey(ClassID))
//                    {
//                        Dictionary<string, string> classObj = new Dictionary<string, string>();

//                        classObj.Add("ClassCode", objRequestDataReader["ClassCode"].ToString());
//                        classObj.Add("Tsp", objRequestDataReader["Tsp"].ToString());
//                        classObj.Add("TradeName", objRequestDataReader["TradeName"].ToString());
//                        //classObj.Add("OjtLocation", objRequestDataReader["OjtLocation"].ToString());

//                        ClassInfoList.Add(ClassID, classObj);

//                    }
//                    if (!OjtIds.Contains(OjtID))
//                    {
//                        OjtIds.Add(OjtID);
//                    }
//                    if (!OjtInfoList.ContainsKey(OjtID))
//                    {
//                        OjtInfoList.Add(OjtID, OjtLoc);
//                    }
//                }

//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;
//                }
//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }

//                Dictionary<string, Dictionary<string, string>> traineesList = new Dictionary<string, Dictionary<string, string>>();

//                foreach (string RequestID in OjtIds)
//                {

//                    string subStrSQL = @"select ID, VisitNo  from AMSOJTMonitoring " +
//                        "where OnJobTrainingID = " + RequestID + DateClause +
//                        " order by OnJobTrainingID, VisitNo ASC;";


//                    objMonitoringDbCommand = TMData.GetSqlStringCommand(subStrSQL);
//                    objMonitoringDataReader = TMData.ExecuteReader(objMonitoringDbCommand);

//                    int MonitoringIndex = 0;
//                    int VisitNoMode = 0;

//                    while (objMonitoringDataReader.Read())
//                    {
//                        string MonitoringID = objMonitoringDataReader["ID"].ToString();
//                        string VisitNo = objMonitoringDataReader["VisitNo"].ToString();
//                        //int VisitNoMode = VisitNo.ToInt() % 4 == 0 ? 4 : VisitNo.ToInt() % 4;
//                        VisitNoMode += 1;

//                        string subTraineeQuery = @"select AMSOJTMonitoringTrainee.TraineeID, ClassID, DisplayName, FatherName, Cnic, Remarks,
//                                        case TraineeFound when 'y' then 'Found'
//                                        when 'n' then 'Not Found'
//                                        end as TraineeStatus
//                                        from AMSOJTMonitoringTrainee
//                                        inner join UserInfo on UserInfo.ID = AMSOJTMonitoringTrainee.TraineeID
//                                        inner join TraineeProfile on TraineeProfile.ID = AMSOJTMonitoringTrainee.TraineeID where OJTMonitoringID = " + MonitoringID;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(subTraineeQuery);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        int TraineeCount = 0;

//                        while (objTraineeDataReader.Read())
//                        {
//                            TraineeCount++;
//                            string TraineeID = objTraineeDataReader["TraineeID"].ToString();
//                            string ClassID = objTraineeDataReader["ClassID"].ToString();

//                            if (!traineesList.ContainsKey(TraineeID))
//                            {
//                                string TspName = "";
//                                string ClassCode = "";
//                                string Trade = "";
//                                string OjtLocation = "";

//                                if (ClassInfoList.ContainsKey(ClassID))
//                                {
//                                    TspName = ClassInfoList[ClassID]["Tsp"];
//                                    ClassCode = ClassInfoList[ClassID]["ClassCode"];
//                                    Trade = ClassInfoList[ClassID]["TradeName"];
//                                    // OjtLocation = ClassInfoList[ClassID]["OjtLocation"];
//                                }
//                                if (OjtInfoList.ContainsKey(RequestID))
//                                {
//                                    OjtLocation = OjtInfoList[RequestID];
//                                }

//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("Trade", Trade);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Name", objTraineeDataReader["DisplayName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["DisplayName"]);
//                                trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);
//                                trainee.Add("OjtLocation", OjtLocation);
//                                trainee.Add("Visit " + VisitNoMode, objTraineeDataReader["TraineeStatus"].ToString());
//                                trainee.Add("Remarks", objTraineeDataReader["Remarks"].Equals(DBNull.Value) ? null : "Visit " + VisitNoMode + ":" + (string)objTraineeDataReader["Remarks"]);
//                                traineesList.Add(TraineeID, trainee);
//                            }
//                            else
//                            {
//                                traineesList[TraineeID]["Visit " + VisitNoMode] = objTraineeDataReader["TraineeStatus"].ToString();
//                                traineesList[TraineeID]["Remarks"] += "\n" + "Visit " + VisitNoMode + ": " + objTraineeDataReader["Remarks"].ToString();
//                            }
//                        }


//                        MonitoringIndex++;

//                        if (objTraineeDataReader != null)
//                        {
//                            if (!objTraineeDataReader.IsClosed)
//                                objTraineeDataReader.Close();

//                            objTraineeDataReader.Dispose();
//                            objTraineeDataReader = null;

//                        }
//                        if (objTraineeDbCommand != null)
//                        {
//                            objTraineeDbCommand.Dispose();
//                            objTraineeDbCommand = null;
//                        }
//                    }
//                    if (objMonitoringDataReader != null)
//                    {
//                        if (!objMonitoringDataReader.IsClosed)
//                            objMonitoringDataReader.Close();

//                        objMonitoringDataReader.Dispose();
//                        objMonitoringDataReader = null;

//                    }

//                    if (objMonitoringDbCommand != null)
//                    {
//                        objMonitoringDbCommand.Dispose();
//                        objMonitoringDbCommand = null;
//                    }

//                }

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Training Service Provider");
//                dt.Columns.Add("Trade");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("CNIC");
//                dt.Columns.Add("Training Location");
//                dt.Columns.Add("Visit 1");
//                dt.Columns.Add("Visit 2");
//                dt.Columns.Add("Visit 3");
//                dt.Columns.Add("Visit 4");
//                dt.Columns.Add("Remarks");

//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in traineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Training Service Provider"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];
//                    _row["Trade"] = entry.Value["Trade"];
//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["CNIC"] = entry.Value["CNIC"];
//                    _row["Training Location"] = entry.Value["OjtLocation"];
//                    _row["Visit 1"] = entry.Value.ContainsKey("Visit 1") ? entry.Value["Visit 1"] : "";
//                    _row["Visit 2"] = entry.Value.ContainsKey("Visit 2") ? entry.Value["Visit 2"] : "";
//                    _row["Visit 3"] = entry.Value.ContainsKey("Visit 3") ? entry.Value["Visit 3"] : "";
//                    _row["Visit 4"] = entry.Value.ContainsKey("Visit 4") ? entry.Value["Visit 4"] : "";
//                    _row["Remarks"] = entry.Value["Remarks"];

//                    dt.Rows.Add(_row);
//                    index++;
//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//        }


//        /************************** Ev Trainee Report *************************************/
//        public DataTable LoadEvReport(string SchemeID, string month)
//        {
//            DbCommand objRequestDbCommand = null;
//            IDataReader objRequestDataReader = null;

//            DbCommand objMonitoringDbCommand = null;
//            IDataReader objMonitoringDataReader = null;

//            DbCommand objTraineeDbCommand = null;
//            IDataReader objTraineeDataReader = null;

//            string StartDate = month.ToDateTime().ToString("yyyy/MM/dd");
//            string DateClause = string.Empty;
//            DateClause = string.Format(" AND month(VisitDateTime) = month('{0}') AND year(VisitDateTime ) = year('{0}')", StartDate);


//            try
//            {
//                string strSQL = @"SELECT 
//                                 Ev.ID, 
//                                 EmplymntLoc as EvLocation, 
//                                 ClassID, 
//                                 EvClass.ClassCode,
//                                 EvClass.TradeName,
//                                 DisplayName as Tsp
//                                 from AMSEmployeeVerificationRequest Ev
//                                 inner join AMSEmployeeVerificationClass EvClass on EvClass.EmployeeVerificationID = Ev.ID
//                                 inner join Class on Class.ID = EvClass.ClassID
//                                 inner join UserInfo on UserInfo.ID = Class.SpID
//                                 where Class.SchemeID = '" + SchemeID + "'";

//                objRequestDbCommand = TMData.GetSqlStringCommand(strSQL);
//                objRequestDataReader = TMData.ExecuteReader(objRequestDbCommand);

//                Dictionary<string, Dictionary<string, string>> ClassInfoList = new Dictionary<string, Dictionary<string, string>>();
//                Dictionary<string, string> EvInfoList = new Dictionary<string, string>();

//                List<string> EvIds = new List<string>();

//                while (objRequestDataReader.Read())
//                {
//                    string ClassID = objRequestDataReader["ClassID"].ToString();
//                    string EvID = objRequestDataReader["ID"].ToString();
//                    string EvLoc = objRequestDataReader["EvLocation"].ToString();
//                    if (!ClassInfoList.ContainsKey(ClassID))
//                    {
//                        Dictionary<string, string> classObj = new Dictionary<string, string>();

//                        classObj.Add("ClassCode", objRequestDataReader["ClassCode"].ToString());
//                        classObj.Add("Tsp", objRequestDataReader["Tsp"].ToString());
//                        classObj.Add("TradeName", objRequestDataReader["TradeName"].ToString());
//                        //classObj.Add("EvLocation", objRequestDataReader["EvLocation"].ToString());

//                        ClassInfoList.Add(ClassID, classObj);

//                    }
//                    if (!EvIds.Contains(EvID))
//                    {
//                        EvIds.Add(EvID);
//                    }
//                    if (!EvInfoList.ContainsKey(EvID))
//                    {
//                        EvInfoList.Add(EvID, EvLoc);
//                    }
//                }


//                Dictionary<string, Dictionary<string, string>> traineesList = new Dictionary<string, Dictionary<string, string>>();

//                foreach (string RequestID in EvIds)
//                {

//                    string subStrSQL = @"select ID, VisitNo  from AMSEvMonitoring" +
//                                        " where EmployeeVerificationID = " + RequestID + DateClause +
//                                        " order by EmployeeVerificationID, VisitNo DESC";


//                    objMonitoringDbCommand = TMData.GetSqlStringCommand(subStrSQL);
//                    objMonitoringDataReader = TMData.ExecuteReader(objMonitoringDbCommand);

//                    int MonitoringIndex = 0;
//                    while (objMonitoringDataReader.Read())
//                    {
//                        string MonitoringID = objMonitoringDataReader["ID"].ToString();
//                        string VisitNo = objMonitoringDataReader["VisitNo"].ToString();

//                        if (MonitoringIndex >= 1)
//                        {
//                            break;
//                        }
//                        string subTraineeQuery = @"select AMSEVMonitoringEmployee.TraineeID, ClassID, DisplayName, FatherName, Cnic, Remarks,
//                                                case TraineeFound
//                                                when 'y' then 'Found'
//                                                when 'n' then 'Not Found'
//                                                end as TraineeStatus
//                                                from AMSEVMonitoringEmployee
//                                                inner join UserInfo on UserInfo.ID = AMSEVMonitoringEmployee.TraineeID
//                                                inner join TraineeProfile on TraineeProfile.ID = AMSEVMonitoringEmployee.TraineeID 
//                                                where EvMonitoringID =" + MonitoringID;

//                        objTraineeDbCommand = TMData.GetSqlStringCommand(subTraineeQuery);
//                        objTraineeDataReader = TMData.ExecuteReader(objTraineeDbCommand);

//                        while (objTraineeDataReader.Read())
//                        {
//                            string TraineeID = objTraineeDataReader["TraineeID"].ToString();
//                            string ClassID = objTraineeDataReader["ClassID"].ToString();

//                            if (!traineesList.ContainsKey(TraineeID))
//                            {
//                                string TspName = "";
//                                string ClassCode = "";
//                                string Trade = "";
//                                string EvLocation = "";

//                                if (EvInfoList.ContainsKey(RequestID))
//                                {
//                                    EvLocation = EvInfoList[RequestID];
//                                }

//                                if (ClassInfoList.ContainsKey(ClassID))
//                                {
//                                    TspName = ClassInfoList[ClassID]["Tsp"];
//                                    ClassCode = ClassInfoList[ClassID]["ClassCode"];
//                                    Trade = ClassInfoList[ClassID]["TradeName"];
//                                    //EvLocation = ClassInfoList[ClassID]["EvLocation"];
//                                }

//                                Dictionary<string, string> trainee = new Dictionary<string, string>();
//                                //trainee.Add("ID", TraineeID);
//                                trainee.Add("Tsp", TspName);
//                                trainee.Add("Trade", Trade);
//                                trainee.Add("ClassCode", ClassCode);
//                                trainee.Add("Name", objTraineeDataReader["DisplayName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["DisplayName"]);
//                                //trainee.Add("FName", objTraineeDataReader["FatherName"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["FatherName"]);
//                                trainee.Add("CNIC", objTraineeDataReader["Cnic"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Cnic"]);
//                                trainee.Add("EvLocation", EvLocation);

//                                trainee.Add("Visit Status", objTraineeDataReader["TraineeStatus"].ToString());
//                                trainee.Add("Remarks", objTraineeDataReader["Remarks"].Equals(DBNull.Value) ? null : (string)objTraineeDataReader["Remarks"]);

//                                traineesList.Add(TraineeID, trainee);
//                            }
//                        }

//                        MonitoringIndex++;
//                    }
//                }

//                DataTable dt = new DataTable();
//                dt.Columns.Add("Sr No");
//                dt.Columns.Add("Training Service Provider");
//                dt.Columns.Add("Trade");
//                dt.Columns.Add("Class Code");
//                dt.Columns.Add("Trainee Name");
//                dt.Columns.Add("Trainee CNIC");
//                dt.Columns.Add("Training Location");
//                dt.Columns.Add("Visit Status");
//                dt.Columns.Add("Remarks");

//                int index = 1;
//                foreach (KeyValuePair<string, Dictionary<string, string>> entry in traineesList)
//                {
//                    DataRow _row = dt.NewRow();
//                    _row["Sr No"] = index;
//                    _row["Training Service Provider"] = entry.Value["Tsp"];
//                    _row["Class Code"] = entry.Value["ClassCode"];
//                    _row["Trade"] = entry.Value["Trade"];
//                    _row["Trainee Name"] = entry.Value["Name"];
//                    _row["Trainee CNIC"] = entry.Value["CNIC"];
//                    _row["Training Location"] = entry.Value["EvLocation"];
//                    _row["Visit Status"] = entry.Value.ContainsKey("Visit Status") ? entry.Value["Visit Status"] : "";
//                    _row["Remarks"] = entry.Value["Remarks"];

//                    dt.Rows.Add(_row);
//                    index++;

//                }

//                return dt;
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (objRequestDataReader != null)
//                {
//                    if (!objRequestDataReader.IsClosed)
//                        objRequestDataReader.Close();

//                    objRequestDataReader.Dispose();
//                    objRequestDataReader = null;

//                }
//                if (objMonitoringDataReader != null)
//                {
//                    if (!objMonitoringDataReader.IsClosed)
//                        objMonitoringDataReader.Close();

//                    objMonitoringDataReader.Dispose();
//                    objMonitoringDataReader = null;

//                }
//                if (objTraineeDataReader != null)
//                {
//                    if (!objTraineeDataReader.IsClosed)
//                        objTraineeDataReader.Close();

//                    objTraineeDataReader.Dispose();
//                    objTraineeDataReader = null;

//                }
//                if (objRequestDbCommand != null)
//                {
//                    objRequestDbCommand.Dispose();
//                    objRequestDbCommand = null;
//                }
//                if (objMonitoringDbCommand != null)
//                {
//                    objMonitoringDbCommand.Dispose();
//                    objMonitoringDbCommand = null;
//                }
//                if (objTraineeDbCommand != null)
//                {
//                    objTraineeDbCommand.Dispose();
//                    objTraineeDbCommand = null;
//                }
//            }

//        }

//    }
//}
