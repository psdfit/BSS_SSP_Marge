using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTakamolRecommendationNote
    {
        TakamolRecommendationNoteModel GetByTakamolRecommendationNoteId(int TakamolRecommendationNoteID);
        List<TakamolRecommendationNoteModel> FetchTakamolRecommendationNote(TakamolRecommendationNoteModel mod);
        List<TakamolRecommendationNoteModel> FetchVRN(TakamolRecommendationNoteModel mod);
        List<TakamolRecommendationNoteModel> FetchTakamolRecommendationNote();
        List<TakamolRecommendationNoteModel> FetchTakamolRecommendationNote(bool InActive);
        void ActiveInActive(int TakamolRecommendationNoteId, bool? InActive, int CurUserID);
        bool TakamolRecommendationNoteApproveReject(TakamolRecommendationNoteModel model, SqlTransaction transaction = null);
        bool TRNApproveReject(TRNMasterModel model, SqlTransaction transaction = null);

        bool PO_TRNApproveReject(POHeaderModel model, SqlTransaction transaction = null);
        void GenerateInvoiceHeader_TakamolRecommendationNote(int PoHeaderID, SqlTransaction _transaction, string ProcessKey);
    }
}
