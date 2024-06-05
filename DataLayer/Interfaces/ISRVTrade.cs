using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{

    public interface ISRVTrade
    {
        TradeModel GetByTradeID(int TradeID, SqlTransaction transaction = null);
        TradeCategoryModel GetTradeCertificationType(int TradeID, SqlTransaction transaction = null);

        List<TradeModel> SaveTrade(TradeModel Trade);
        List<TradeModel> FetchTrade(TradeModel mod);
        List<TradeModel> FetchTrade();
        List<TradeModel> FetchTrade(bool InActive);
        int GetTradeCodeSequence();
        public List<TradeModel> FetchTradeForROSIFilter(ROSIFiltersModel rosiFilters);

        void ActiveInActive(int TradeID, bool? InActive, int CurUserID);
        public List<TradeModel> FinalSubmitTrade(TradeModel model);

        List<TradeModel> GetByTradeName(string TradeName);
        List<TradeModel> GetByTradeCode(string TradeCode);
        List<SubmittedTradesModel> GetSubmittedTrades(int OID = 0);

        bool TradeApproveReject(TradeModel model, SqlTransaction transaction = null);

        bool UpdateTradeSAPID(int TradeID, string sapObjId, SqlTransaction transaction = null);
        public List<TradeModel> FetchTradeForCRUD();
        //public List<TradeModel> FetchTradeForROSIFilter(ROSIFiltersModel rosiFilters);
        public DataTable FetchTradeLayer();

        TradeModel GetByTradeID_Notification(int TradeID, SqlTransaction transaction = null);
        public List<TradeModel> SaveTradeDetail(TradeModel Trade);
        List<TradeModel> FetchTradeTSP(int programid, int districtid, int UserID);

    }

}
