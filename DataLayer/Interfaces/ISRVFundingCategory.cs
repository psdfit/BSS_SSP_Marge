using DataLayer.Models;using System.Collections.Generic;namespace DataLayer.Interfaces{

    public interface ISRVFundingCategory
    {
        FundingCategoryModel GetByFundingCategoryID(int FundingCategoryID);
        List<FundingCategoryModel> SaveFundingCategory(FundingCategoryModel FundingCategory);
        List<FundingCategoryModel> FetchFundingCategory(FundingCategoryModel mod);
        List<FundingCategoryModel> FetchFundingCategory();
        List<FundingCategoryModel> FetchFundingCategory(bool InActive);
        void ActiveInActive(int FundingCategoryID, bool? InActive, int CurUserID);
    }}