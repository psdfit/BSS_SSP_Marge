using DataLayer.Models;using System.Collections.Generic;namespace DataLayer.Interfaces{

    public interface ISRVSections
    {
        SectionsModel GetBySectionID(int SectionID);
        List<SectionsModel> SaveSections(SectionsModel Sections);
        List<SectionsModel> FetchSections(SectionsModel mod);
        List<SectionsModel> FetchSections();
        List<SectionsModel> FetchSections(bool InActive);
        void ActiveInActive(int SectionID, bool? InActive, int CurUserID);
    }}