using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVROSI : SRVBase, ISRVROSI
    {

        public SRVROSI() { }

        public List<ROSIModel> SaveROSI(ROSIModel ROSI)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[2];
                //param[0] = new SqlParameter("@DurationID", ROSI.DurationID);
                param[0] = new SqlParameter("@ROSI", ROSI.ROSI);

                param[1] = new SqlParameter("@CurUserID", ROSI.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Duration]", param);
                return FetchROSI();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<ROSIModel> LoopinData(DataTable dt)
        {
            List<ROSIModel> ROSIL = new List<ROSIModel>();

            foreach (DataRow r in dt.Rows)
            {
                ROSIL.Add(RowOfROSI(r));

            }
            return ROSIL;
        }
        private List<ROSIModel> LoopinTSPROSIData(DataTable dt)
        {
            List<ROSIModel> ROSIL = new List<ROSIModel>();

            foreach (DataRow r in dt.Rows)
            {
                ROSIL.Add(RowOfTSPROSI(r));

            }
            return ROSIL;
        }
        private List<ROSICalculationModel> LoopinROSICalculationData(DataTable dt)
        {
            List<ROSICalculationModel> ROSIL = new List<ROSICalculationModel>();

            foreach (DataRow r in dt.Rows)
            {
                ROSIL.Add(RowOfROSICalculation(r));

            }
            return ROSIL;
        }

        private List<ROSICalculationDataSetModel> LoopinROSICalculationDataSet(DataTable dt)
        {
            List<ROSICalculationDataSetModel> ROSIL = new List<ROSICalculationDataSetModel>();

            foreach (DataRow r in dt.Rows)
            {
                ROSIL.Add(RowOfROSICalculationDataSet(r));

            }
            return ROSIL;
        }
        private List<ROSIModel> LoopinROSIbyTimeData(DataTable dt)
        {
            List<ROSIModel> ROSIL = new List<ROSIModel>();

            foreach (DataRow r in dt.Rows)
            {
                ROSIL.Add(RowOfROSIByTimeDuration(r));

            }
            return ROSIL;
        }
        public List<ROSIModel> FetchROSI(ROSIModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP2", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ROSIModel> FetchROSI()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP2").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }


        public List<ROSIModel> FetchROSIByFilters(QueryFilters filters)
        {
            List<ROSIModel> list = new List<ROSIModel>();
            //if (filters.Length > 0)
            //{
            //int schemeId = filters[0];
            //int tspId = filters[1];
            //int classId = filters[2];
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeID", filters.SchemeID);
                param[1] = new SqlParameter("@TSPID", filters.TSPID);
                param[2] = new SqlParameter("@ClassID", filters.ClassID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP2", param).Tables[0];
                list = LoopinData(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            return list;
        }

        public List<ROSIModel> FetchROSIByScheme(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP2", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ROSIModel> FetchROSIByTSP(int TSPID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP2", new SqlParameter("@TSPID", TSPID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ROSIModel> FetchROSIByClass(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP2", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<ROSIModel> FetchROSIFilters(ROSIFiltersModel mod)
        {
            List<ROSIModel> list = new List<ROSIModel>();
            //int schemeId = filters[0];
            //int schemeId = filters[0];
            //int tspId = filters[1];
            //int classId = filters[2];
            //DateTime startdate = filters[3];
            //DateTime enddate = filters[4];

            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@StartDate", mod.StartDate);
                param[1] = new SqlParameter("@EndDate", mod.EndDate);
                param[2] = new SqlParameter("@SchemeID", mod.SchemeIDs);
                param[3] = new SqlParameter("@TSPID", mod.TSPIDs);
                param[4] = new SqlParameter("@PTypeID", mod.PTypeIDs);
                param[5] = new SqlParameter("@SectorID", mod.SectorIDs);
                param[6] = new SqlParameter("@ClusterID", mod.ClusterIDs);
                param[7] = new SqlParameter("@DistrictID", mod.DistrictIDs);
                param[8] = new SqlParameter("@TradeID", mod.TradeIDs);
                param[9] = new SqlParameter("@OrganizationID", mod.OrganizationIDs);
                param[10] = new SqlParameter("@FundingSourceID", mod.FundingSourceIDs);

                //param[0] = new SqlParameter("@StartDate", mod.StartDate);
                //param[1] = new SqlParameter("@EndDate", mod.EndDate);
                //param[2] = new SqlParameter("@SchemeID", mod.SchemeID);
                //param[3] = new SqlParameter("@TSPID", mod.TSPID);
                //param[4] = new SqlParameter("@PTypeID", mod.PTypeID);
                //param[5] = new SqlParameter("@SectorID", mod.SectorID);
                //param[6] = new SqlParameter("@ClusterID", mod.ClusterID);
                //param[7] = new SqlParameter("@DistrictID", mod.DistrictID);
                //param[8] = new SqlParameter("@TradeID", mod.TradeID);
                //param[9] = new SqlParameter("@OrganizationID", mod.OrganizationID);
                //param[10] = new SqlParameter("@FundingSourceID", mod.FundingSourceID);
                param[11] = new SqlParameter("@GenderIDs", mod.GenderIDs);

                param[12] = new SqlParameter("@VerifiedEmploymentROSI", mod.EmploymentFlag);

                if (mod.SchemeIDs != "" && mod.TSPIDs == "")
                //if (mod.SchemeID != 0 && mod.TSPID == 0)
                {
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP5", param).Tables[0];
                    return LoopinData(dt);
                }
                if (mod.TSPIDs != "")
                //if (mod.TSPID != 0)
                {
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP5", param).Tables[0];
                    return LoopinTSPROSIData(dt);
                }
                else
                {
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP5", param).Tables[0];
                    return LoopinROSIbyTimeData(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public List<ROSICalculationModel> FetchCalculatedROSIByFilters(ROSIFiltersModel mod)
        {
            List<ROSIModel> list = new List<ROSIModel>();

            try
            {
                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@StartDate", mod.StartDate);
                param[1] = new SqlParameter("@EndDate", mod.EndDate);
                param[2] = new SqlParameter("@ActualContractualFlag", mod.ActualContractualFlag);
                param[3] = new SqlParameter("@VerifiedEmploymentROSIs", mod.EmploymentFlag);
                param[4] = new SqlParameter("@OrganizationIDs", mod.OrganizationIDs);
                param[5] = new SqlParameter("@PTypeIDs", mod.PTypeIDs);
                param[6] = new SqlParameter("@FundingSourceIDs", mod.FundingSourceIDs);
                param[7] = new SqlParameter("@SchemeIDs", mod.SchemeIDs);
                param[8] = new SqlParameter("@SectorIDs", mod.SectorIDs);
                param[9] = new SqlParameter("@ClusterIDs", mod.ClusterIDs);
                param[10] = new SqlParameter("@DistrictIDs", mod.DistrictIDs);
                param[11] = new SqlParameter("@TradeIDs", mod.TradeIDs);
                param[12] = new SqlParameter("@TSPIDs", mod.TSPIDs);
                param[13] = new SqlParameter("@GenderIDs", mod.GenderIDs);
                param[14] = new SqlParameter("@DurationIDs", mod.DurationIDs);

                if (mod.EmploymentFlag)
                {
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CalculatedVerifiedEmploymentROSIByFilters", param).Tables[0];
                    return LoopinROSICalculationData(dt);
                }
                else
                {
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CalculatedROSIByFilters", param).Tables[0];
                    return LoopinROSICalculationData(dt);
                }

                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CalculatedROSIByFilters", param).Tables[0];
                //return LoopinROSICalculationData(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ROSICalculationDataSetModel> FetchCalculatedROSIDataSetByFilters(ROSIFiltersModel mod)
        {
            List<ROSICalculationDataSetModel> list = new List<ROSICalculationDataSetModel>();

            try
            {
                SqlParameter[] param = new SqlParameter[13];
                param[0] = new SqlParameter("@StartDate", mod.StartDate);
                param[1] = new SqlParameter("@EndDate", mod.EndDate);
                param[2] = new SqlParameter("@OrganizationIDs", mod.OrganizationIDs);
                param[3] = new SqlParameter("@PTypeIDs", mod.PTypeIDs);
                param[4] = new SqlParameter("@FundingSourceIDs", mod.FundingSourceIDs);
                param[5] = new SqlParameter("@SchemeIDs", mod.SchemeIDs);
                param[6] = new SqlParameter("@SectorIDs", mod.SectorIDs);
                param[7] = new SqlParameter("@ClusterIDs", mod.ClusterIDs);
                param[8] = new SqlParameter("@DistrictIDs", mod.DistrictIDs);
                param[9] = new SqlParameter("@TradeIDs", mod.TradeIDs);
                param[10] = new SqlParameter("@TSPIDs", mod.TSPIDs);
                param[11] = new SqlParameter("@GenderIDs", mod.GenderIDs);
                param[12] = new SqlParameter("@DurationIDs", mod.DurationIDs);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CalculatedROSIDataSetByFilters", param).Tables[0];
                return LoopinROSICalculationDataSet(dt);


                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CalculatedROSIByFilters", param).Tables[0];
                //return LoopinROSICalculationData(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ROSIModel> FetchROSI(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ROSI_IP2", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private ROSIModel RowOfROSI(DataRow r)
        {
            ROSIModel ROSI = new ROSIModel();
            ROSI.SchemeName = r["SchemeName"].ToString();
            ROSI.SchemeCode = r["SchemeCode"].ToString();
            ROSI.SchemeType = r["SchemeType"].ToString();
            ROSI.TSPName = r["TSPName"].ToString();
            //ROSI.ClassCode = r["ClassCode"].ToString();
            ROSI.OpportunityCost = Convert.ToInt32(r["OpportunityCost"]);
            ROSI.CompletedTrainees = Convert.ToInt32(r["CompletedTrainees"]);
            ROSI.VerifiedTrainees = Convert.ToInt32(r["VerifiedTrainees"]);
            ROSI.ROSI = Convert.ToDouble(r["ROSI"]);
            ROSI.AverageWageRate = Convert.ToInt32(r["AverageWageRate"]);
            ROSI.NetIncrease = Convert.ToInt32(r["NetIncrease"]);
            ROSI.AverageTrainingCost = Convert.ToInt32(r["AverageTrainingCost"]);


            ROSI.Contractual = Convert.ToInt32(r["Contractual"]);
            ROSI.NetContractual = Convert.ToInt32(r["NetContractual"]);
            ROSI.CancelledClasses = Convert.ToInt32(r["CancelledClasses"]);
            ROSI.ReportedEmployed = r["ReportedEmployed"].ToString();
            ROSI.CostPerAppendix = r["CostPerAppendix"].ToString();
            ROSI.Appendix = r["Appendix"].ToString();
            ROSI.Testing = r["Testing"].ToString();
            ROSI.Total = r["Total"].ToString();
            ROSI.VerifiedOverCommitmentRatio = Convert.ToDouble(r["VerifiedOverCommitmentRatio"]);
            ROSI.MarginofEmployment = Convert.ToDouble(r["MarginofEmployment"]);
            //ROSI.InActive = Convert.ToBoolean(r["InActive"]);
            //ROSI.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            //ROSI.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            //ROSI.CreatedDate =r["CreatedDate"].ToString().GetDate();
            //ROSI.ModifiedDate =r["ModifiedDate"].ToString().GetDate();

            return ROSI;
        }
        private ROSIModel RowOfTSPROSI(DataRow r)
        {
            ROSIModel ROSI = new ROSIModel();
            ROSI.TSPName = r["TSPName"].ToString();
            //ROSI.ClassCode = r["ClassCode"].ToString();
            ROSI.OpportunityCost = Convert.ToInt32(r["OpportunityCost"]);
            ROSI.CompletedTrainees = Convert.ToInt32(r["CompletedTrainees"]);
            ROSI.VerifiedTrainees = Convert.ToInt32(r["VerifiedTrainees"]);
            ROSI.ROSI = Convert.ToDouble(r["ROSI"]);
            ROSI.AverageWageRate = Convert.ToInt32(r["AverageWageRate"]);
            ROSI.NetIncrease = Convert.ToInt32(r["NetIncrease"]);
            ROSI.AverageTrainingCost = Convert.ToInt32(r["AverageTrainingCost"]);


            //ROSI.Contractual = Convert.ToInt32(r["Contractual"]);
            //ROSI.NetContractual = Convert.ToInt32(r["NetContractual"]);
            //ROSI.CancelledClasses = Convert.ToInt32(r["CancelledClasses"]);
            ROSI.ReportedEmployed = r["ReportedEmployed"].ToString();
            ROSI.CostPerAppendix = r["CostPerAppendix"].ToString();
            ROSI.Appendix = r["Appendix"].ToString();
            ROSI.Testing = r["Testing"].ToString();
            ROSI.Total = r["Total"].ToString();
            ROSI.VerifiedOverCommitmentRatio = Convert.ToDouble(r["VerifiedOverCommitmentRatio"]);
            ROSI.MarginofEmployment = Convert.ToDouble(r["MarginofEmployment"]);
            //ROSI.InActive = Convert.ToBoolean(r["InActive"]);
            //ROSI.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            //ROSI.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            //ROSI.CreatedDate =r["CreatedDate"].ToString().GetDate();
            //ROSI.ModifiedDate =r["ModifiedDate"].ToString().GetDate();

            return ROSI;
        }
        private ROSIModel RowOfROSIByTimeDuration(DataRow r)
        {
            ROSIModel ROSI = new ROSIModel();

            ROSI.OpportunityCost = Convert.ToInt32(r["OpportunityCost"]);
            ROSI.CompletedTrainees = Convert.ToInt32(r["CompletedTrainees"]);
            ROSI.VerifiedTrainees = Convert.ToInt32(r["VerifiedTrainees"]);
            ROSI.ROSI = Convert.ToDouble(r["ROSI"]);
            ROSI.AverageWageRate = Convert.ToInt32(r["AverageWageRate"]);
            ROSI.NetIncrease = Convert.ToInt32(r["NetIncrease"]);
            ROSI.AverageTrainingCost = Convert.ToInt32(r["AverageTrainingCost"]);


            ROSI.Contractual = Convert.ToInt32(r["Contractual"]);
            ROSI.NetContractual = Convert.ToInt32(r["NetContractual"]);
            ROSI.CancelledClasses = Convert.ToInt32(r["CancelledClasses"]);
            ROSI.VerifiedOverCommitmentRatio = Convert.ToDouble(r["VerifiedOverCommitmentRatio"]);
            ROSI.MarginofEmployment = Convert.ToDouble(r["MarginofEmployment"]);
            //ROSI.ReportedEmployed = r["ReportedEmployed"].ToString();
            //ROSI.InActive = Convert.ToBoolean(r["InActive"]);
            //ROSI.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            //ROSI.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            //ROSI.CreatedDate =r["CreatedDate"].ToString().GetDate();
            //ROSI.ModifiedDate =r["ModifiedDate"].ToString().GetDate();

            return ROSI;
        }
        public void ActiveInActive(int ROSIID, bool? InActive, int CurUserID)
        {
            throw new NotImplementedException();
        }


        private ROSICalculationModel RowOfROSICalculation(DataRow r)
        {
            ROSICalculationModel ROSI = new ROSICalculationModel();
            if (r.Table.Columns.Contains("ClassID"))
            {
                ROSI.ClassID = Convert.ToInt32(r["ClassID"]);
            }
            if (r.Table.Columns.Contains("ClassCode"))
            {
                ROSI.ClassCode = r["ClassCode"].ToString();
            }
            if (r.Table.Columns.Contains("TotalCostPerClass"))
            {
                ROSI.TotalCostPerClass = Convert.ToInt32(r["TotalCostPerClass"]);
            }
            if (r.Table.Columns.Contains("ActualWeightedTotal"))
            {
                ROSI.ActualWeightedTotal = Convert.ToInt32(r["ActualWeightedTotal"]);
            }
            if (r.Table.Columns.Contains("ContractualWeightedTotal"))
            {
                ROSI.ContractualWeightedTotal = Convert.ToInt32(r["ContractualWeightedTotal"]);
            }
            if (r.Table.Columns.Contains("OrganizationID"))
            {
                ROSI.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
            }
            if (r.Table.Columns.Contains("OrganizationName"))
            {
                ROSI.OrganizationName = r["OrganizationName"].ToString();
            }
            if (r.Table.Columns.Contains("ProgramTypeID"))
            {
                ROSI.ProgramTypeID = Convert.ToInt32(r["ProgramTypeID"]);
            }
            if (r.Table.Columns.Contains("ProgramTypeName"))
            {
                ROSI.ProgramTypeName = r["ProgramTypeName"].ToString();
            }
            if (r.Table.Columns.Contains("FundingSourceID"))
            {
                ROSI.FundingSourceID = Convert.ToInt32(r["FundingSourceID"]);
            }
            if (r.Table.Columns.Contains("FundingSourceName"))
            {
                ROSI.FundingSourceName = r["FundingSourceName"].ToString();
            }
            if (r.Table.Columns.Contains("SchemeID"))
            {
                ROSI.SchemeID = Convert.ToInt32(r["SchemeID"]);
            }
            if (r.Table.Columns.Contains("SchemeName"))
            {
                ROSI.SchemeName = r["SchemeName"].ToString();
            }
            if (r.Table.Columns.Contains("SectorID"))
            {
                ROSI.SectorID = Convert.ToInt32(r["SectorID"]);
            }
            if (r.Table.Columns.Contains("SectorName"))
            {
                ROSI.SectorName = r["SectorName"].ToString();
            }
            if (r.Table.Columns.Contains("ClusterID"))
            {
                ROSI.ClusterID = Convert.ToInt32(r["ClusterID"]);
            }
            if (r.Table.Columns.Contains("ClusterName"))
            {
                ROSI.ClusterName = r["ClusterName"].ToString();
            }
            if (r.Table.Columns.Contains("DistrictID"))
            {
                ROSI.DistrictID = Convert.ToInt32(r["DistrictID"]);
            }
            if (r.Table.Columns.Contains("DistrictName"))
            {
                ROSI.DistrictName = r["DistrictName"].ToString();
            }
            if (r.Table.Columns.Contains("TSPID"))
            {
                ROSI.TSPID = Convert.ToInt32(r["TSPID"]);
            }
            if (r.Table.Columns.Contains("TSPName"))
            {
                ROSI.TSPName = r["TSPName"].ToString();
            }

            if (r.Table.Columns.Contains("TradeID"))
            {
                ROSI.TradeID = Convert.ToInt32(r["TradeID"]);
            }
            if (r.Table.Columns.Contains("TradeName"))
            {
                ROSI.TradeName = r["TradeName"].ToString();
            }
            if (r.Table.Columns.Contains("GenderID"))
            {
                ROSI.GenderID = Convert.ToInt32(r["GenderID"]);
            }
            if (r.Table.Columns.Contains("GenderName"))
            {
                ROSI.GenderName = r["GenderName"].ToString();
            }
            if (r.Table.Columns.Contains("Duration"))
            {
                ROSI.Duration = Convert.ToDouble(r["Duration"]);
            }
            if (r.Table.Columns.Contains("VerifiedAverageWageRateForecastedActual"))
            {
                ROSI.VerifiedAverageWageRateForecastedActual = Convert.ToDouble(r["VerifiedAverageWageRateForecastedActual"]);
            }
            if (r.Table.Columns.Contains("OpportunityCostForecastedActual"))
            {
                ROSI.OpportunityCostForecastedActual = Convert.ToDouble(r["OpportunityCostForecastedActual"]);
            }
            ROSI.VerifiedAverageWageRate = Convert.ToDouble(r["VerifiedAverageWageRate"]);
            ROSI.OpportunityCost = Convert.ToDouble(r["OpportunityCost"]);
            //ROSI.ContractualEmploymentCommitment = Convert.ToInt32(r["ContractualEmploymentCommitment"]);
            ROSI.ContractualEmploymentCommitment = Convert.ToDouble(r["ContractualEmploymentCommitment"]);
            ROSI.NoOfReportedTrainees = Convert.ToInt32(r["NoOfReportedTrainees"]);
            ROSI.NoOfVerifiedTrainees = Convert.ToInt32(r["NoOfVerifiedTrainees"]);
            ROSI.ContractualCTM = Convert.ToDouble(r["ContractualCTM"]);
            ROSI.ActualCTM = Convert.ToDouble(r["ActualCTM"]);
            ROSI.ContractualAverageDuration = Convert.ToDouble(r["ContractualAverageDuration"]);
            ROSI.ActualAverageDuration = Convert.ToDouble(r["ActualAverageDuration"]);
            ROSI.NoOfContractedTrainees = Convert.ToInt32(r["NoOfContractedTrainees"]);
            ROSI.NoOfActualCompletedTrainees = Convert.ToInt32(r["NoOfActualCompletedTrainees"]);
            ROSI.EmploymentMarginReported = Convert.ToDouble(r["EmploymentMarginReported"]);
            ROSI.EmploymentMarginVerified = Convert.ToDouble(r["EmploymentMarginVerified"]);
            ROSI.ContractualROSI = Convert.ToDouble(r["ContractualROSI"]);
            ROSI.ReportedForcastedROSI = Convert.ToDouble(r["ReportedForcastedROSI"]);
            ROSI.ActualROSI = Convert.ToDouble(r["ActualROSI"]);

            return ROSI;
        }

        private ROSICalculationDataSetModel RowOfROSICalculationDataSet(DataRow r)
        {
            ROSICalculationDataSetModel ROSI = new ROSICalculationDataSetModel();
            if (r.Table.Columns.Contains("ClassID"))
            {
                ROSI.ClassID = Convert.ToInt32(r["ClassID"]);
            }
            if (r.Table.Columns.Contains("ClassCode"))
            {
                ROSI.ClassCode = r["ClassCode"].ToString();
            }
            if (r.Table.Columns.Contains("EmploymentPayout"))
            {
                ROSI.EmploymentPayout = Convert.ToInt32(r["EmploymentPayout"]);
            }
            if (r.Table.Columns.Contains("OverallEmploymentCommitment"))
            {
                ROSI.OverallEmploymentCommitment = Convert.ToInt32(r["OverallEmploymentCommitment"]);
            }
            if (r.Table.Columns.Contains("TotalCostPerClass"))
            {
                ROSI.TotalCostPerClass = Convert.ToInt32(r["TotalCostPerClass"]);
            }
            if (r.Table.Columns.Contains("ActualCostPerClass"))
            {
                ROSI.ActualCostPerClass = Convert.ToInt32(r["ActualCostPerClass"]);
            }
            if (r.Table.Columns.Contains("ActualWeightedTotal"))
            {
                ROSI.ActualWeightedTotal = Convert.ToInt32(r["ActualWeightedTotal"]);
            }
            if (r.Table.Columns.Contains("ContractualWeightedTotal"))
            {
                ROSI.ContractualWeightedTotal = Convert.ToInt32(r["ContractualWeightedTotal"]);
            }
            if (r.Table.Columns.Contains("OrganizationID"))
            {
                ROSI.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
            }
            if (r.Table.Columns.Contains("OrganizationName"))
            {
                ROSI.OrganizationName = r["OrganizationName"].ToString();
            }
            if (r.Table.Columns.Contains("ProgramTypeID"))
            {
                ROSI.ProgramTypeID = Convert.ToInt32(r["ProgramTypeID"]);
            }
            if (r.Table.Columns.Contains("ProgramTypeName"))
            {
                ROSI.ProgramTypeName = r["ProgramTypeName"].ToString();
            }
            if (r.Table.Columns.Contains("FundingSourceID"))
            {
                ROSI.FundingSourceID = Convert.ToInt32(r["FundingSourceID"]);
            }
            if (r.Table.Columns.Contains("FundingSourceName"))
            {
                ROSI.FundingSourceName = r["FundingSourceName"].ToString();
            }
            if (r.Table.Columns.Contains("SchemeID"))
            {
                ROSI.SchemeID = Convert.ToInt32(r["SchemeID"]);
            }
            if (r.Table.Columns.Contains("SchemeName"))
            {
                ROSI.SchemeName = r["SchemeName"].ToString();
            }
            if (r.Table.Columns.Contains("SectorID"))
            {
                ROSI.SectorID = Convert.ToInt32(r["SectorID"]);
            }
            if (r.Table.Columns.Contains("SectorName"))
            {
                ROSI.SectorName = r["SectorName"].ToString();
            }
            if (r.Table.Columns.Contains("ClusterID"))
            {
                ROSI.ClusterID = Convert.ToInt32(r["ClusterID"]);
            }
            if (r.Table.Columns.Contains("ClusterName"))
            {
                ROSI.ClusterName = r["ClusterName"].ToString();
            }
            if (r.Table.Columns.Contains("DistrictID"))
            {
                ROSI.DistrictID = Convert.ToInt32(r["DistrictID"]);
            }
            if (r.Table.Columns.Contains("DistrictName"))
            {
                ROSI.DistrictName = r["DistrictName"].ToString();
            }
            if (r.Table.Columns.Contains("TSPID"))
            {
                ROSI.TSPID = Convert.ToInt32(r["TSPID"]);
            }
            if (r.Table.Columns.Contains("TSPName"))
            {
                ROSI.TSPName = r["TSPName"].ToString();
            }

            if (r.Table.Columns.Contains("TradeID"))
            {
                ROSI.TradeID = Convert.ToInt32(r["TradeID"]);
            }
            if (r.Table.Columns.Contains("TradeName"))
            {
                ROSI.TradeName = r["TradeName"].ToString();
            }
            if (r.Table.Columns.Contains("GenderID"))
            {
                ROSI.GenderID = Convert.ToInt32(r["GenderID"]);
            }
            if (r.Table.Columns.Contains("GenderName"))
            {
                ROSI.GenderName = r["GenderName"].ToString();
            }
            if (r.Table.Columns.Contains("Duration"))
            {
                ROSI.Duration = Convert.ToDouble(r["Duration"]);
            }

            ROSI.VerifiedAverageWageRate = Convert.ToDouble(r["VerifiedAverageWageRate"]);
            ROSI.OpportunityCost = Convert.ToDouble(r["OpportunityCost"]);
            ROSI.ContractualEmploymentCommitment = Convert.ToDouble(r["ContractualEmploymentCommitment"]);
            ROSI.NoOfReportedTrainees = Convert.ToInt32(r["NoOfReportedTrainees"]);
            ROSI.NoOfVerifiedTrainees = Convert.ToInt32(r["NoOfVerifiedTrainees"]);
            ROSI.ContractualCTM = Convert.ToDouble(r["ContractualCTM"]);
            ROSI.ActualCTM = Convert.ToDouble(r["ActualCTM"]);
            ROSI.ContractualAverageDuration = Convert.ToDouble(r["ContractualAverageDuration"]);
            ROSI.ActualAverageDuration = Convert.ToDouble(r["ActualAverageDuration"]);
            ROSI.NoOfContractedTrainees = Convert.ToInt32(r["NoOfContractedTrainees"]);
            ROSI.NoOfActualCompletedTrainees = Convert.ToInt32(r["NoOfActualCompletedTrainees"]);
            ROSI.EmploymentMarginReported = Convert.ToDouble(r["EmploymentMarginReported"]);
            ROSI.EmploymentMarginVerified = Convert.ToDouble(r["EmploymentMarginVerified"]);
            ROSI.ContractualROSINumerator = Convert.ToDouble(r["ContractualROSINumerator"]);
            ROSI.ContractualROSIDenominator = Convert.ToDouble(r["ContractualROSIDenominator"]);
            ROSI.ReportedForecastedROSINumerator = Convert.ToDouble(r["ReportedForecastedROSINumerator"]);
            ROSI.ReportedForecastedROSIDenominator = Convert.ToDouble(r["ReportedForecastedROSIDenominator"]);
            ROSI.VerifiedForecastedROSINumerator = Convert.ToDouble(r["VerifiedForecastedROSINumerator"]);
            ROSI.VerifiedForecastedROSIDenominator = Convert.ToDouble(r["VerifiedForecastedROSIDenominator"]);
            ROSI.ActualROSINumerator = Convert.ToDouble(r["ActualROSINumerator"]);
            ROSI.ActualROSIDenominator = Convert.ToDouble(r["ActualROSIDenominator"]);
            ROSI.VerifiedActualROSINumerator = Convert.ToDouble(r["VerifiedActualROSINumerator"]);
            ROSI.VerifiedActualROSIDenominator = Convert.ToDouble(r["VerifiedActualROSIDenominator"]);
            //ROSI.ReportedForcastedROSI = Convert.ToDouble(r["ReportedForcastedROSI"]);
            //ROSI.ActualROSI = Convert.ToDouble(r["ActualROSI"]);

            return ROSI;
        }

    }
}
