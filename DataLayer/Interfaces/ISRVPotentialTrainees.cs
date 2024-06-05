using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVPotentialTrainees
    {
        PotentialTraineesModel GetByID(int ID);
        List<PotentialTraineesModel> SavePotentialTrainees(PotentialTraineesModel PotentialTrainees);
        List<PotentialTraineesModel> FetchPotentialTrainees();
        List<PotentialTraineesModel> FetchPotentialTraineesByFilters(int[] filters);
        void ActiveInActive(int ID, bool? InActive, int CurUserID);
        public List<PotentialTraineesModel> FetchPotentialTraineesByPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);
        public List<PotentialTraineesModel> FetchPotentialTraineesFiltersData(int userID);


    }
}
