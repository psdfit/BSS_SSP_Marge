using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTradeEquipmentToolsMap
    {
        TradeEquipmentToolsMapModel GetByTradeEquipmentToolsMap(int TradeEquipmentToolsMap);
        List<TradeEquipmentToolsMapModel> SaveTradeEquipmentToolsMap(TradeEquipmentToolsMapModel TradeEquipmentToolsMap);
        List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMap(TradeEquipmentToolsMapModel mod);
        List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMap();
        List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMap(bool InActive);
        void ActiveInActive(int TradeEquipmentToolsMap, bool? InActive, int CurUserID);
        List<TradeEquipmentToolsMapModel> FetchTradeEquipmentToolsMapAll(int TradeID);

    }
}
