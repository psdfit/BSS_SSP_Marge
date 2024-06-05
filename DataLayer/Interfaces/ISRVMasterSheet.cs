using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVMasterSheet
    {
        MasterSheetModel GetByID(int ID);
        List<MasterSheetModel> SaveMasterSheet(MasterSheetModel MasterSheet);
        List<MasterSheetModel> FetchMasterSheet(MasterSheetModel mod);
        List<MasterSheetModel> FetchMasterSheet();
        List<MasterSheetModel> FetchMasterSheet(bool InActive);
        List<MasterSheetModel> FetchMasterSheetByFilters(int[] filters);
        void ActiveInActive(int ID, bool? InActive, int CurUserID);
        public List<MasterSheetModel> FetchMasterSheetByPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

    }
}
