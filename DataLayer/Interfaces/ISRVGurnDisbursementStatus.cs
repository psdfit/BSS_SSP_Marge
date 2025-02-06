using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVGurnDisbursementStatus
    {
        public List<GurnDisbursementModel> FetchGurnDisbursementStatus();
        void UpdateTraineesDisbursement(List<GurnDisbursementModel> ls);
        public List<GurnDisbursementModel> FetchGurnDisbursementStatusByFilters(GurnDisbursementFiltersModel model);

    }
}
