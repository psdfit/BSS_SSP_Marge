using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
   public interface ISRVTSPColor
    {
        List<TSPColorModel> FetchTSPMasterData();
        List<TSPColorModel> FetchTSPColor();
        bool saveTSPColor(TSPColorModel model);
        List<TSPColorModel> FetchTSPColorHistory(int? TSPMasterID);
        public List<TSPColorModel> FetchTSPColorByID(int userid);
        public List<TSPColorModel> FetchTSPColorByFilters(TSPColorFiltersModel model);
        public List<BlackListCriteriaModel> CheckBlacklistingCriteriaCriteria(TSPColorFiltersModel model);



    }
}
