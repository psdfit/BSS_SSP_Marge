using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVPBTEDataSharingTimelines
    {
        PBTEDataSharingTimelinesModel GetByID(int ID);
        List<PBTEDataSharingTimelinesModel> SavePBTEDataSharingTimelines(PBTEDataSharingTimelinesModel PBTEDataSharingTimelines);
        List<PBTEDataSharingTimelinesModel> FetchPBTEDataSharingTimelines(PBTEDataSharingTimelinesModel mod);
        List<PBTEDataSharingTimelinesModel> FetchPBTEDataSharingTimelines();
        List<PBTEDataSharingTimelinesModel> FetchPBTEDataSharingTimelines(bool InActive);
        void ActiveInActive(int ID, bool? InActive, int CurUserID);
    }
}
