
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class ROSICalculationModel
    {

        //public int ROSIID	{ get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public int TotalCostPerClass { get; set; }
        public int ContractualWeightedTotal { get; set; }
        public int ActualWeightedTotal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int ProgramTypeID { get; set; }
        public string ProgramTypeName { get; set; }
        public int FundingSourceID { get; set; }
        public string FundingSourceName { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public int SectorID { get; set; }
        public string SectorName { get; set; }
        public int ClusterID { get; set; }
        public string ClusterName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string TSPName { get; set; }
        public int TSPID { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public double Duration { get; set; }
        public double VerifiedAverageWageRate { get; set; }
        public double VerifiedAverageWageRateForecastedActual { get; set; }
        public double OpportunityCost { get; set; }
        public double OpportunityCostForecastedActual { get; set; }
        //public int ContractualEmploymentCommitment { get; set; }
        public double ContractualEmploymentCommitment { get; set; }
        public int NoOfReportedTrainees { get; set; }
        public int NoOfVerifiedTrainees { get; set; }
        public double ContractualCTM { get; set; }
        public double ActualCTM { get; set; }
        public double ContractualAverageDuration { get; set; }
        public double ActualAverageDuration { get; set; }
        public int NoOfContractedTrainees { get; set; }
        public int NoOfActualCompletedTrainees { get; set; }
        public double EmploymentMarginReported { get; set; }
        public double EmploymentMarginVerified { get; set; }
        public double ContractualROSI { get; set; }
        public double ReportedForcastedROSI { get; set; }
        public double ActualROSI { get; set; }


    }


    public class ROSICalculationDataSetModel
    {
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public int EmploymentPayout { get; set; }
        public int OverallEmploymentCommitment { get; set; }
        public int TotalCostPerClass { get; set; }
        public int ActualCostPerClass { get; set; }
        public int ContractualWeightedTotal { get; set; }
        public int ActualWeightedTotal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int ProgramTypeID { get; set; }
        public string ProgramTypeName { get; set; }
        public int FundingSourceID { get; set; }
        public string FundingSourceName { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public int SectorID { get; set; }
        public string SectorName { get; set; }
        public int ClusterID { get; set; }
        public string ClusterName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string TSPName { get; set; }
        public int TSPID { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public double Duration { get; set; }
        public double VerifiedAverageWageRate { get; set; }
        public double OpportunityCost { get; set; }
        //public int ContractualEmploymentCommitment { get; set; }
        public double ContractualEmploymentCommitment { get; set; }
        public int NoOfReportedTrainees { get; set; }
        public int NoOfVerifiedTrainees { get; set; }
        public double ContractualCTM { get; set; }
        public double ActualCTM { get; set; }
        public double ContractualAverageDuration { get; set; }
        public double ActualAverageDuration { get; set; }
        public int NoOfContractedTrainees { get; set; }
        public int NoOfActualCompletedTrainees { get; set; }
        public double EmploymentMarginReported { get; set; }
        public double EmploymentMarginVerified { get; set; }
        public double ContractualROSINumerator { get; set; }
        public double ContractualROSIDenominator { get; set; }
        public double ReportedForecastedROSINumerator { get; set; }
        public double ReportedForecastedROSIDenominator { get; set; }
        public double VerifiedForecastedROSINumerator { get; set; }
        public double VerifiedForecastedROSIDenominator { get; set; }
        public double ActualROSINumerator { get; set; }
        public double ActualROSIDenominator { get; set; }
        public double VerifiedActualROSINumerator { get; set; }
        public double VerifiedActualROSIDenominator { get; set; }


    }

}
