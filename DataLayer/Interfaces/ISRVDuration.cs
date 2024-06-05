using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVDuration
    {
        DurationModel GetByDurationID(int DurationID);
        List<DurationModel> SaveDuration(DurationModel Duration);
        List<DurationModel> FetchDuration(DurationModel mod);
        List<DurationModel> FetchDuration();
        List<DurationModel> FetchDuration(bool InActive);
        void ActiveInActive(int DurationID, bool? InActive, int CurUserID);
        public List<DurationModel> FetchDurationForROSIFilter(bool InActive);
        public List<DurationModel> FetchDurationForROSIFilter(ROSIFiltersModel rosiFilters);
    }
}
