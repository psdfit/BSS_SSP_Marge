using DataLayer.Models;using System.Collections.Generic;namespace DataLayer.Interfaces{

    public interface ISRVInstructor
    {
        InstructorModel GetByInstrID(int InstrID);
        List<InstructorModel> GetBySchemeID(int SchemeID);
        InstructorModel SaveInstructor(InstructorModel Instructor);
        List<InstructorModel> FetchInstructor(InstructorModel mod);
        List<InstructorModel> FetchInstructor_DVV(int classID);
        List<InstructorModel> FetchInstructorByScheme(int SchemeID);
        List<InstructorModel> FetchInstructor();
        List<InstructorModel> FetchInstructor(bool InActive);
        void ActiveInActive(int InstrID, bool? InActive, int CurUserID);
        bool SaveInstructorAttendance(InstructorAttendanceDVV model, out string errMsg);
        bool SaveInstructorDVV(InstructorDVV model, out string errMsg);
        public List<InstructorModel> FetchInstructorDataByUser(int UserID);
        public List<InstructorModel> GetByClassID(int ClassID);
        public List<InstructorModel> GetByTSPUserID(int userid);
        public List<InstructorModel> GetByInstructorID(int InstrID);
        public List<InstructorModel> FetchCRInstructorDataByUser(InstructorCRFiltersModel mod);
        public List<CheckInstructorCNICModel> FetchOldInstructorCNICs();




    }}