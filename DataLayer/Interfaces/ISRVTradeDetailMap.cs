using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTradeDetailMap
    {
        TradeDetailMapModel GetByTradeDetailMapID(int TradeDetailMapID);
        List<TradeDetailMapModel> SaveTradeDetailMap(TradeDetailMapModel TradeDetailMap);
        List<TradeDetailMapModel> FetchTradeDetailMap(TradeDetailMapModel mod);
        List<TradeDetailMapModel> FetchTradeDetailMap();
        List<TradeDetailMapModel> FetchTradeDetailMap(bool InActive);
        void ActiveInActive(int TradeDetailMapID, bool? InActive, int CurUserID);
        List<TradeDetailMapModel> FetchTradeDetailMapAll(int TradeID);

    }
}
