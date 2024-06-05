using DataLayer.Models;using System.Collections.Generic;namespace DataLayer.Interfaces{

    public interface ISRVReligion
    {
        ReligionModel GetByReligionID(int ReligionID);
        List<ReligionModel> SaveReligion(ReligionModel Religion);
        List<ReligionModel> FetchReligion(ReligionModel mod);
        List<ReligionModel> FetchReligion();
        List<ReligionModel> FetchReligion(bool InActive);
        void ActiveInActive(int ReligionID, bool? InActive, int CurUserID);
    }}