using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVHomeStats
    {
        HomeStatsModel GetByID(int ID);
        //public List<HomeStatsModel> SaveHomeStats(HomeStatsModel HomeStats);
        List<HomeStatsModel> FetchHomeStats(HomeStatsModel mod);
        List<HomeStatsModel> FetchHomeStats();
        List<HomeStatsModel> FetchHomeStats(bool InActive);

        List<HomeStatsModel> FetchHomeStatsByFilters(int[] filters);

        void ActiveInActive(int ID, bool? InActive, int CurUserID);
        List<HomeStatsModel> FetchHomeStatsByClass(int ClassID);
        List<HomeStatsModel> FetchHomeStatsByUser(QueryFilters filters);
        public List<HomeStatsModel> FetchPendingHomeStatsByUser(QueryFilters filters);

    }
}
