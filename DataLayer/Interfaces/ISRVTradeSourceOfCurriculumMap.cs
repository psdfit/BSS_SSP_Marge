using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTradeSourceOfCurriculumMap
    {
        TradeSourceOfCurriculumMapModel GetByTradeSourceOfCurriculumMapID(int TradeSourceOfCurriculumMapID);
        List<TradeSourceOfCurriculumMapModel> SaveTradeSourceOfCurriculumMap(TradeSourceOfCurriculumMapModel TradeSourceOfCurriculumMap);
        List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMap(TradeSourceOfCurriculumMapModel mod);
        List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMap();
        List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMap(bool InActive);
        void ActiveInActive(int TradeSourceOfCurriculumMapID, bool? InActive, int CurUserID);

        List<TradeSourceOfCurriculumMapModel> FetchTradeSourceOfCurriculumMapAll(int TradeID);

    }
}
