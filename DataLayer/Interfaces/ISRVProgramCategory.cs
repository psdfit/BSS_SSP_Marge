using DataLayer.Models;using System.Collections.Generic;namespace DataLayer.Interfaces{

    public interface ISRVProgramCategory
    {
        ProgramCategoryModel GetByPCategoryID(int PCategoryID);
        List<ProgramCategoryModel> SaveProgramCategory(ProgramCategoryModel ProgramCategory);
        List<ProgramCategoryModel> FetchProgramCategory(ProgramCategoryModel mod);
        List<ProgramCategoryModel> FetchProgramCategory();
        List<ProgramCategoryModel> FetchProgramCategory(bool InActive);
        void ActiveInActive(int PCategoryID, bool? InActive, int CurUserID);
    }}