using DataLayer.Models;
using System;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVROSI
    {
        List<ROSIModel> SaveROSI(ROSIModel ROSI);
        List<ROSIModel> FetchROSI(ROSIModel mod);
        List<ROSIModel> FetchROSIByFilters(QueryFilters filters);
        List<ROSIModel> FetchROSIFilters(ROSIFiltersModel mod);
        List<ROSIModel> FetchROSIByScheme(int SchemeID);
        List<ROSIModel> FetchROSIByTSP(int TSPID);
        List<ROSIModel> FetchROSIByClass(int ClassID);

        List<ROSIModel> FetchROSI();
        List<ROSIModel> FetchROSI(bool InActive);
        void ActiveInActive(int ROSIID, bool? InActive, int CurUserID);
        public List<ROSICalculationModel> FetchCalculatedROSIByFilters(ROSIFiltersModel mod);
        public List<ROSICalculationDataSetModel> FetchCalculatedROSIDataSetByFilters(ROSIFiltersModel mod);

    }
}
