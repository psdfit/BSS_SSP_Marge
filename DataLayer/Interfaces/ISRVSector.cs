using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVSector
    {
        SectorModel GetBySectorID(int SectorID);

        List<SectorModel> SaveSector(SectorModel Sector);

        List<SectorModel> FetchSector(SectorModel mod);

        List<SectorModel> FetchSector();

        List<SectorModel> FetchSector(bool InActive);
        public List<SectorModel> FetchSectorForROSIFilter(ROSIFiltersModel rosiFilters);
        void ActiveInActive(int SectorID, bool? InActive, int CurUserID);
    }
}