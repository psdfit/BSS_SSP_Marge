using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVTakamolRecommendationNoteDetails
    {
        List<TakamolRecommendationNoteDetailsModel> FetchTakamolRecommendationNoteDetails(TakamolRecommendationNoteDetailsModel model);
        List<TakamolRecommendationNoteDetailsModel> FetchTakamolRecommendationNoteDetailsFiltered(TakamolRecommendationNoteDetailsModel model);
        TakamolRecommendationNoteDetailsModel UpdateTakamolRecommendationNoteDetails(TakamolRecommendationNoteDetailsModel TakamolRecommendationNote);
        List<TakamolRecommendationNoteDetailsModel> GetTakamolRecommendationNoteExcelExport(TakamolRecommendationNoteDetailsModel TakamolRecommendationNote);
        public List<TakamolRecommendationNoteDetailsModel> GetTakamolRecommendationNoteExcelExportByIDs(string ids);

    }
}
