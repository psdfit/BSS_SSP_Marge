using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVSubSector
    {
        SubSectorModel GetBySubSectorID(int SubSectorID);

        List<SubSectorModel> SaveSubSector(SubSectorModel SubSector);

        List<SubSectorModel> FetchSubSector(SubSectorModel mod);

        List<SubSectorModel> FetchSubSector();

        List<SubSectorModel> FetchSubSector(bool InActive);

        void ActiveInActive(int SubSectorID, bool? InActive, int CurUserID);
    }
}