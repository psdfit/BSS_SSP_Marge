using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTradeConsumableMaterialMap
    {
        TradeConsumableMaterialMapModel GetByTradeConsumableMaterialMapID(int TradeConsumableMaterialMapID);
        List<TradeConsumableMaterialMapModel> SaveTradeConsumableMaterialMap(TradeConsumableMaterialMapModel TradeConsumableMaterialMap);
        List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMap(TradeConsumableMaterialMapModel mod);
        List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMap();
        List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMap(bool InActive);
        void ActiveInActive(int TradeConsumableMaterialMapID, bool? InActive, int CurUserID);
        List<TradeConsumableMaterialMapModel> FetchTradeConsumableMaterialMapAll(int TradeID);

    }
}
