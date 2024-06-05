using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVStipendStatus
    {
        StipendStatusModel GetByStipendStatusID(int StipendStatusID);

        List<StipendStatusModel> SaveStipendStatus(StipendStatusModel StipendStatus);

        List<StipendStatusModel> FetchStipendStatus(StipendStatusModel mod);

        List<StipendStatusModel> FetchStipendStatus();

        List<StipendStatusModel> FetchStipendStatus(bool InActive);

        void ActiveInActive(int StipendStatusID, bool? InActive, int CurUserID);
    }
}