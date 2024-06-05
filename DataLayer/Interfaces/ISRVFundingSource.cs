using DataLayer.Models;using System.Collections.Generic;namespace DataLayer.Interfaces{

    public interface ISRVFundingSource
    {
        FundingSourceModel GetByFundingSourceID(int FundingSourceID);
        List<FundingSourceModel> SaveFundingSource(FundingSourceModel FundingSource);
        List<FundingSourceModel> FetchFundingSource(FundingSourceModel mod);
        List<FundingSourceModel> FetchFundingSource();
        List<FundingSourceModel> FetchFundingSource(bool InActive);
        void ActiveInActive(int FundingSourceID, bool? InActive, int CurUserID);
        public List<FundingSourceModel> FetchFundingSourceForROSIFilter(ROSIFiltersModel rosiFilters);

    }}