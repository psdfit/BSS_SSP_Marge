/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVOrgConfig : SRVBase, ISRVOrgConfig
    {
        private ISRVTSPDetail srvTSPDetail;
        private readonly ISRVSendEmail srvSendEmail;
        public SRVOrgConfig(ISRVTSPDetail srv, ISRVSendEmail srvSendEmail)
        {
            this.srvTSPDetail = srv;
            this.srvSendEmail = srvSendEmail;
        }

        public OrgConfigModel GetByConfigID(int ConfigID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ConfigID", ConfigID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfOrgConfig(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrgConfigModel> SaveOrgConfig(OrgConfigModel OrgConfig)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ConfigID", OrgConfig.ConfigID));
                param.Add(new SqlParameter("@OID", OrgConfig.OID));
                param.Add(new SqlParameter("@SchemeID", OrgConfig.SchemeID));
                param.Add(new SqlParameter("@TSPID", OrgConfig.TSPID));
                param.Add(new SqlParameter("@ClassID", OrgConfig.ClassID));
                param.Add(new SqlParameter("@DualRegistration", OrgConfig.DualRegistration));
                param.Add(new SqlParameter("@BracketDaysBefore", OrgConfig.BracketDaysBefore));
                param.Add(new SqlParameter("@BracketDaysAfter", OrgConfig.BracketDaysAfter));
                param.Add(new SqlParameter("@EligibleGenderID", OrgConfig.EligibleGenderID));
                param.Add(new SqlParameter("@MinAge", OrgConfig.MinAge));
                param.Add(new SqlParameter("@MaxAge", OrgConfig.MaxAge));
                param.Add(new SqlParameter("@MinEducation", OrgConfig.MinEducation));
                param.Add(new SqlParameter("@ReportBracketBefore", OrgConfig.ReportBracketBefore));
                param.Add(new SqlParameter("@ReportBracketAfter", OrgConfig.ReportBracketAfter));
                param.Add(new SqlParameter("@StipendPayMethod", OrgConfig.StipendPayMethod));
                param.Add(new SqlParameter("@ClassStartFrom1", OrgConfig.ClassStartFrom1));
                param.Add(new SqlParameter("@ClassStartTo1", OrgConfig.ClassStartTo1));
                param.Add(new SqlParameter("@ClassStartFrom2", OrgConfig.ClassStartFrom2));
                param.Add(new SqlParameter("@ClassStartTo2", OrgConfig.ClassStartTo2));
                param.Add(new SqlParameter("@BISPIndexFrom", OrgConfig.BISPIndexFrom));
                param.Add(new SqlParameter("@BISPIndexTo", OrgConfig.BISPIndexTo));
                param.Add(new SqlParameter("@MinAttendPercent", OrgConfig.MinAttendPercent));
                param.Add(new SqlParameter("@StipendDeductAmount", OrgConfig.StipendDeductAmount));
                param.Add(new SqlParameter("@MinCountDeductPercent", OrgConfig.PhyCountDeductPercent));
                param.Add(new SqlParameter("@DeductDropOutPercent", OrgConfig.DeductDropOutPercent));
                param.Add(new SqlParameter("@StipNoteGenGTMonth", OrgConfig.StipNoteGenGTMonth));
                param.Add(new SqlParameter("@StipNoteGenLTMonth", OrgConfig.StipNoteGenLTMonth));
                param.Add(new SqlParameter("@MPRDenerationDay", OrgConfig.MPRDenerationDay));
                param.Add(new SqlParameter("@EmploymentDeadline", OrgConfig.EmploymentDeadline));
                param.Add(new SqlParameter("@CurUserID", OrgConfig.CurUserID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_OrgConfig]", param.ToArray());
                return FetchOrgConfig();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<OrgConfigModel> LoopinData(DataTable dt)
        {
            List<OrgConfigModel> OrgConfigL = new List<OrgConfigModel>();

            foreach (DataRow r in dt.Rows)
            {
                OrgConfigL.Add(RowOfOrgConfig(r));
            }

            return OrgConfigL;
        }

        public List<OrgConfigModel> FetchOrgConfig(OrgConfigModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
            }
        }

        public List<OrgConfigModel> FetchOrgConfig()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig").Tables[0];
                List<OrgConfigModel> ls = LoopinData(dt);
                dt.Dispose();
                return ls;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //public List<OrgConfigModel> FetchOrgConfig(int OID, string ruleType,int sid, int tid, int cid)
            public List<OrgConfigModel> FetchOrgConfig(int OID, string ruleType, int sid, int tid)
        {
            try
            {
                //List<SqlParameter> prm = new List<SqlParameter>() { new SqlParameter("@BusinessRuleType", ruleType), new SqlParameter("@OID", OID), new SqlParameter("@SchemeID", sid), new SqlParameter("@TSPID", tid), new SqlParameter("@ClassID", cid) };
                List<SqlParameter> prm = new List<SqlParameter>() { new SqlParameter("@BusinessRuleType", ruleType), new SqlParameter("@OID", OID), new SqlParameter("@SchemeID", sid), new SqlParameter("@TSPID", tid)};
                using (DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetOrgConfig", prm.ToArray()).Tables[0])
                {
                    List<OrgConfigModel> ls = LoopinData(dt);
                    dt.Dispose();
                    return ls;
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrgConfigModel> GetOrgConfig(int OID = 0, int SchemeID = 0, int TSPID = 0, int ClassID = 0)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig").Tables[0];
                List<OrgConfigModel> ls = LoopinData(dt);
                dt.Dispose();
                return ls;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrgConfigModel> FetchOrgConfig(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig", new SqlParameter("@InActive", InActive)).Tables[0];
                List<OrgConfigModel> ls = LoopinData(dt);
                dt.Dispose();
                return ls;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrgConfigModel> GetByOID(int OID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig", new SqlParameter("@OID", OID)).Tables[0];
                List<OrgConfigModel> ls = LoopinData(dt);
                dt.Dispose();
                return ls;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrgConfigModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                List<OrgConfigModel> ls = LoopinData(dt);
                dt.Dispose();
                return ls;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public OrgConfigModel GetByClassID(int ClassID, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_OrgConfig", new SqlParameter("@ClassID", ClassID)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig", new SqlParameter("@ClassID", ClassID)).Tables[0];
                }
                OrgConfigModel mod = LoopinData(dt).FirstOrDefault();
                dt.Dispose();
                return mod;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrgConfigModel> GetByTSPID(int TSPID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrgConfig", new SqlParameter("@TSPID", TSPID)).Tables[0];
                List<OrgConfigModel> ls = LoopinData(dt);
                dt.Dispose();
                return ls;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<OrgConfigModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@OID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_OrgConfig]", param);
        }

        public void ActiveInActive(int ConfigID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ConfigID", ConfigID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_OrgConfig]", PLead);
        }

        private OrgConfigModel RowOfOrgConfig(DataRow r)
        {
            OrgConfigModel OrgConfig = new OrgConfigModel();
            OrgConfig.ConfigID = Convert.ToInt32(r["ConfigID"]);
            OrgConfig.OID = Convert.ToInt32(r["OID"]);
            OrgConfig.OName = r["OName"].ToString();
            OrgConfig.BusinessRuleType = r["BusinessRuleType"].ToString();
            OrgConfig.SchemeID = Convert.ToInt32(r["SchemeID"]);
            OrgConfig.SchemeName = r["SchemeName"].ToString();
            OrgConfig.TSPID = Convert.ToInt32(r["TSPID"]);
            OrgConfig.TSPMasterID = Convert.ToInt32(string.IsNullOrEmpty(r["TSPMasterID"].ToString())?0: r["TSPMasterID"]);
            OrgConfig.TSPName = r["TSPName"].ToString();
            OrgConfig.ClassID = Convert.ToInt32(r["ClassID"]);
            OrgConfig.ClassCode = r["ClassCode"].ToString();
            OrgConfig.DualRegistration = Convert.ToBoolean(r["DualRegistration"]);
            OrgConfig.ISDVV = Convert.ToBoolean(r["ISDVV"]);
            OrgConfig.BracketDaysBefore = Convert.ToInt32(r["BracketDaysBefore"]);
            OrgConfig.BracketDaysAfter = Convert.ToInt32(r["BracketDaysAfter"]);
            OrgConfig.EligibleGenderID = Convert.ToInt32(r["EligibleGenderID"]);
            OrgConfig.MinAge = Convert.ToInt32(r["MinAge"]);
            OrgConfig.MaxAge = Convert.ToInt32(r["MaxAge"]);
            OrgConfig.MinEducation = Convert.ToInt32(r["MinEducation"]);
            OrgConfig.ReportBracketBefore = Convert.ToInt32(r["ReportBracketBefore"]);
            OrgConfig.ReportBracketAfter = Convert.ToInt32(r["ReportBracketAfter"]);
            OrgConfig.StipendPayMethod = r["StipendPayMethod"].ToString();
            OrgConfig.ClassStartFrom1 = Convert.ToInt32(r["ClassStartFrom1"]);
            OrgConfig.ClassStartTo1 = Convert.ToInt32(r["ClassStartTo1"]);
            OrgConfig.ClassStartFrom2 = Convert.ToInt32(r["ClassStartFrom2"]);
            OrgConfig.ClassStartTo2 = Convert.ToInt32(r["ClassStartTo2"]);
            OrgConfig.RTPTo = Convert.ToInt32(r["RTPTo"]);
            OrgConfig.RTPFrom = Convert.ToInt32(r["RTPFrom"]);
            OrgConfig.BISPIndexFrom = Convert.ToInt32(r["BISPIndexFrom"]);
            OrgConfig.BISPIndexTo = Convert.ToInt32(r["BISPIndexTo"]);
            OrgConfig.MinAttendPercent = Convert.ToInt32(r["MinAttendPercent"]);
            OrgConfig.StipendDeductAmount = Convert.ToInt32(r["StipendDeductAmount"]);
            OrgConfig.PhyCountDeductPercent = Convert.ToInt32(r["PhyCountDeductPercent"]);
            OrgConfig.DeductDropOutPercent = Convert.ToInt32(r["DeductDropOutPercent"]);
            OrgConfig.StipNoteGenGTMonth = Convert.ToInt32(r["StipNoteGenGTMonth"]);
            OrgConfig.StipNoteGenLTMonth = Convert.ToInt32(r["StipNoteGenLTMonth"]);
            OrgConfig.MPRDenerationDay = Convert.ToInt32(r["MPRDenerationDay"]);
            OrgConfig.InActive = Convert.ToBoolean(r["InActive"]);
            OrgConfig.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            OrgConfig.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            OrgConfig.CreatedDate = r["CreatedDate"].ToString().GetDate();
            OrgConfig.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            OrgConfig.DeductionSinceInceptionPercent = Convert.ToInt32(r["DeductionSinceInceptionPercent"].ToString() == "" ? 0 : r["DeductionSinceInceptionPercent"]);
            OrgConfig.DeductionFailedTraineesPercent = Convert.ToInt32(r["DeductionFailedTraineesPercent"].ToString() == "" ? 0 : r["DeductionFailedTraineesPercent"]);
            OrgConfig.EmploymentDeadline = Convert.ToInt32(r["EmploymentDeadline"].ToString() == "" ? 0 : r["EmploymentDeadline"]);
            OrgConfig.TSROpeningDays = Convert.ToInt32(r["TSROpeningDays"].ToString() == "" ? 0 : r["TSROpeningDays"]);
            OrgConfig.TraineesPerClassThershold = Convert.ToInt32(r["TraineesPerClassThershold"].ToString() == "" ? 0 : r["TraineesPerClassThershold"]);
            OrgConfig.IsCheckBISP = Convert.ToBoolean(r["IsCheckBISP"]);
            OrgConfig.IsCheckPBTE = Convert.ToBoolean(r["IsCheckPBTE"]);
            if (r.Table.Columns.Contains("GenderName"))
            {
                OrgConfig.GenderName = r["GenderName"].ToString();
                OrgConfig.Education = r["Education"].ToString();
            }
            return OrgConfig;
        }

        public bool ComparreNewAndPreviousListOfOrgConfig(List<OrgConfigModel> UpdatedList, List<OrgConfigModel> PreviousOrgConfigList,int UserID)
        {
            try
            {
                var DifferenceBothList = UpdatedList.Where(p1 => !PreviousOrgConfigList.Any(p2 =>
                p2.OID == p1.OID &&
                p2.SchemeID == p1.SchemeID &&
                p2.TSPID == p1.TSPID &&
                p2.ClassID == p1.ClassID &&
                p2.DualRegistration == p1.DualRegistration &&
                p2.BracketDaysBefore == p1.BracketDaysBefore &&
                p2.BracketDaysAfter == p1.BracketDaysAfter &&
                p2.EligibleGenderID == p1.EligibleGenderID &&
                p2.MinAge == p1.MinAge &&
                p2.MaxAge == p1.MaxAge &&
                p2.MinEducation == p1.MinEducation &&
                p2.ReportBracketBefore == p1.ReportBracketBefore &&
                p2.ReportBracketAfter == p1.ReportBracketAfter &&
                p2.StipendPayMethod == p1.StipendPayMethod &&
                p2.ClassStartFrom1 == p1.ClassStartFrom1 &&
                p2.ClassStartTo1 == p1.ClassStartTo1 &&
                p2.BISPIndexFrom == p1.BISPIndexFrom &&
                p2.BISPIndexTo == p1.BISPIndexTo &&
                p2.MinAttendPercent == p1.MinAttendPercent &&
                p2.StipendDeductAmount == p1.StipendDeductAmount &&
                p2.PhyCountDeductPercent == p1.PhyCountDeductPercent &&
                p2.DeductDropOutPercent == p1.DeductDropOutPercent &&
                p2.StipNoteGenGTMonth == p1.StipNoteGenGTMonth &&
                p2.StipNoteGenLTMonth == p1.StipNoteGenLTMonth &&
                p2.MPRDenerationDay == p1.MPRDenerationDay &&
                p2.DeductionSinceInceptionPercent == p1.DeductionSinceInceptionPercent &&
                p2.DeductionFailedTraineesPercent == p1.DeductionFailedTraineesPercent &&
                p2.ISDVV == p1.ISDVV &&
                p2.EmploymentDeadline == p1.EmploymentDeadline &&
                p2.TSROpeningDays == p1.TSROpeningDays &&
                p2.TraineesPerClassThershold == p1.TraineesPerClassThershold)).ToList();
               

                if (DifferenceBothList.Count>0)
                {
                    OrgConfigLog(DifferenceBothList, DifferenceBothList[0].OID, UserID);
                    GetTSPAndSendNotification(PreviousOrgConfigList, DifferenceBothList, UserID);
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool GetTSPAndSendNotification(List<OrgConfigModel> PreviousOrgConfigList, List<OrgConfigModel> DifferenceBothList, int UserID)
        {
            try
            {
                var groupedByScheme = DifferenceBothList
                   .GroupBy(u => u.TSPID)
                   .Select(grp => grp.ToList())
                   .ToList();

                foreach (var TSP in groupedByScheme)
                {
                    string classInfo = string.Empty;
                    if (TSP[0].TSPID > 0)
                    {
                        var TSPID = TSP
                       .GroupBy(u => u.TSPID)
                       .Select(grp => grp.Key)
                       .FirstOrDefault();
                        foreach (var Class in TSP)
                        {


                            var pre = PreviousOrgConfigList.Where(c => c.ConfigID == Class.ConfigID)
                                     .FirstOrDefault();
                            if (pre.ClassCode!=null && pre.ClassCode!="")
                            {
                                classInfo += "[Class Code :" + pre.ClassCode;
                                if (Class.DualRegistration != pre.DualRegistration)
                                {
                                    classInfo += "(DualRegistration : "+pre.DualRegistration + " → " + Class.DualRegistration + ")";
                                }
                                if (Class.BracketDaysBefore != pre.BracketDaysBefore)
                                {
                                    classInfo += "(BracketDaysBefore : " + pre.BracketDaysBefore + " → " + Class.BracketDaysBefore + ")";
                                }
                                if (Class.BracketDaysAfter != pre.BracketDaysAfter)
                                {
                                    classInfo += "(BracketDaysAfter : " + pre.BracketDaysAfter + " → " + Class.BracketDaysAfter + ")";
                                }
                                if (Class.MinAge != pre.MinAge)
                                {
                                    classInfo += "(MinAge : " + pre.MinAge + " → " + Class.MinAge + ")";
                                }
                                if (Class.MaxAge != pre.MaxAge)
                                {
                                    classInfo += "(MaxAge : " + pre.MaxAge + " → " + Class.MaxAge + ")";
                                }
                                if (Class.MinEducation != pre.MinEducation)
                                {
                                    classInfo += "(MinEducation : " + pre.MinEducation + " → " + Class.MinEducation + ")";
                                }
                                if (Class.ReportBracketBefore != pre.ReportBracketBefore)
                                {
                                    classInfo += "(ReportBracketBefore : " + pre.ReportBracketBefore + " → " + Class.ReportBracketBefore + ")";
                                }
                                if (Class.ReportBracketAfter != pre.ReportBracketAfter)
                                {
                                    classInfo += "(ReportBracketAfter : " + pre.ReportBracketAfter + " → " + Class.ReportBracketAfter + ")";
                                }
                                if (Class.StipendPayMethod != pre.StipendPayMethod)
                                {
                                    classInfo += "(StipendPayMethod : " + pre.StipendPayMethod + " → " + Class.StipendPayMethod + ")";
                                }
                                if (Class.ClassStartFrom1 != pre.ClassStartFrom1)
                                {
                                    classInfo += "(ClassStartFrom1 : " + pre.ClassStartFrom1 + " → " + Class.ClassStartFrom1 + ")";
                                }
                                if (Class.ClassStartTo1 != pre.ClassStartTo1)
                                {
                                    classInfo += "(ClassStartTo1 : " + pre.ClassStartTo1 + " → " + Class.ClassStartTo1 + ")";
                                }
                                if (Class.BISPIndexFrom != pre.BISPIndexFrom)
                                {
                                    classInfo += "(BISPIndexFrom : " + pre.BISPIndexFrom + " → " + Class.BISPIndexFrom + ")";
                                }
                                if (Class.BISPIndexTo != pre.BISPIndexTo)
                                {
                                    classInfo += "(BISPIndexTo : " + pre.BISPIndexTo + " → " + Class.BISPIndexTo + ")";
                                }
                                if (Class.MinAttendPercent != pre.MinAttendPercent)
                                {
                                    classInfo += "(MinAttendPercent : " + pre.MinAttendPercent + " → " + Class.MinAttendPercent + ")";
                                }
                                if (Class.StipendDeductAmount != pre.StipendDeductAmount)
                                {
                                    classInfo += "(StipendDeductAmount : " + pre.StipendDeductAmount + " → " + Class.StipendDeductAmount + ")";
                                }
                                if (Class.PhyCountDeductPercent != pre.PhyCountDeductPercent)
                                {
                                    classInfo += "(PhyCountDeductPercent : " + pre.PhyCountDeductPercent + " → " + Class.PhyCountDeductPercent + ")";
                                }
                                if (Class.DeductDropOutPercent != pre.DeductDropOutPercent)
                                {
                                    classInfo += "(DeductDropOutPercent : " + pre.DeductDropOutPercent + " → " + Class.DeductDropOutPercent + ")";
                                }
                                if (Class.StipNoteGenGTMonth != pre.StipNoteGenGTMonth)
                                {
                                    classInfo += "(StipNoteGenGTMonth : " + pre.StipNoteGenGTMonth + " → " + Class.StipNoteGenGTMonth + ")";
                                }
                                if (Class.StipNoteGenLTMonth != pre.StipNoteGenLTMonth)
                                {
                                    classInfo += "(StipNoteGenLTMonth : " + pre.StipNoteGenLTMonth + " → " + Class.StipNoteGenLTMonth + ")";
                                }
                                if (Class.MPRDenerationDay != pre.MPRDenerationDay)
                                {
                                    classInfo += "(MPRDenerationDay : " + pre.MPRDenerationDay + " → " + Class.MPRDenerationDay + ")";
                                }
                                if (Class.DeductionSinceInceptionPercent != pre.DeductionSinceInceptionPercent)
                                {
                                    classInfo += "(DeductionSinceInceptionPercent : " + pre.DeductionSinceInceptionPercent + " → " + Class.DeductionSinceInceptionPercent + ")";
                                }
                                if (Class.DeductionFailedTraineesPercent != pre.DeductionFailedTraineesPercent)
                                {
                                    classInfo += "(DeductionFailedTraineesPercent : " + pre.DeductionFailedTraineesPercent + " → " + Class.DeductionFailedTraineesPercent + ")";
                                }
                                if (Class.EmploymentDeadline != pre.EmploymentDeadline)
                                {
                                    classInfo += "(EmploymentDeadline : " + pre.EmploymentDeadline + " → " + Class.EmploymentDeadline + ")";
                                }
                                if (Class.TSROpeningDays != pre.TSROpeningDays)
                                {
                                    classInfo += "(TSROpeningDays : " + pre.TSROpeningDays + " → " + Class.TSROpeningDays + ")";
                                }
                                if (Class.TraineesPerClassThershold != pre.TraineesPerClassThershold)
                                {
                                    classInfo += "(TraineesPerClassThershold : " + pre.TraineesPerClassThershold + " → " + Class.TraineesPerClassThershold + ")";
                                }
                                classInfo += "]";
                            }
                            
                        }
                        TSPDetailModel TSPUserDetail = new TSPDetailModel();
                        TSPUserDetail = srvTSPDetail.GetUserByTSPID(TSPID);
                        if (TSPUserDetail.UserID > 0)
                        {
                            ApprovalModel approvalModel = new ApprovalModel();
                            ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                            approvalModel.ProcessKey = EnumApprovalProcess.CONFIG_OF_BUSINES_RULE;
                            approvalModel.CurUserID = Convert.ToInt32(UserID);
                            approvalModel.UserIDs = TSPUserDetail.UserID.ToString();
                            approvalModel.CustomComments = classInfo;
                            srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);

                        }
                    }
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return true;
        }


        public int OrgConfigLog(List<OrgConfigModel> DifferenceBothList, int @BatchFkey, int CurUserID)
        {
            List<OrgConfigModel> DiffList = (from emp in DifferenceBothList select emp).Where(x=>x.ClassID!=null&&x.ClassID!=0).ToList();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(DiffList));
            param[1] = new SqlParameter("@OID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_OrgConfigLog]", param);
        }

    }
}