using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVSrnDisbursementStatus
    {
        public List<SrnDisbursementModel> FetchSrnDisbursementStatus();
        void UpdateTraineesDisbursement(List<SrnDisbursementModel> ls);
        public List<SrnDisbursementModel> FetchSrnDisbursementStatusByFilters(SrnDisbursementFiltersModel model);

    }
}
