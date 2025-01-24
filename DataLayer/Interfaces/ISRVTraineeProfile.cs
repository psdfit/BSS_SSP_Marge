using DataLayer.Models;
using DataLayer.Models.DVV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace DataLayer.Interfaces
{
    public interface ISRVTraineeProfile
    {
        TraineeProfileModel GetByTraineeID(int traineeID);

        TraineeProfileModel GetTraineeProfileByCNIC(string cnic);

        List<TraineeProfileModel> SaveTraineeProfile(TraineeProfileModel traineeProfile);

        List<TraineeProfileModel> FetchTraineeProfile(TraineeProfileModel model);

        List<TraineeProfileModel> FetchTraineeProfile();

        List<TraineeProfileModel> FetchTraineeProfileByClass(int classId, bool isSubmitted = false);

        List<TraineeProfileModel> FetchTraineeDraftDataByTsp(int? TspId);

        List<TraineeProfileModel> FetchTraineeDraftDataByKam(int? KamId);

        List<TraineeProfileDVV> FetchTraineeProfileByClass_DVV(int classId, bool? isSubmitted = null);

        List<TraineeProfileModel> FetchTraineeProfile(bool inActive);

        int FetchTraineeProfileNextCodeVal(int classID);

        int CalculateAgeEligibility(DateTime dateOfBirth, int classID, out string errMsg);

        /// <summary>
        /// TraineeID, TraineeCNIC,ClassID shoud be mandatory in model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errMsg"></param>
        /// <returns>true if trainee is eligible;otherwise false</returns>
        bool isEligibleTrainee(TraineeProfileModel model, out string errMsg);

        bool isEligibleTraineeEmail(TraineeProfileModel model, out string errMsg);

        bool IsAlreadyExistsByCNIC(int traineeID, string traineeCNIC, int classId, bool isDual, out string errMsg);

        bool IsAlreadyExistsByEmail(int traineeID, string traineeCNIC, out string errMsg);

        bool IsAcceptableByBISP(string traineeCNIC, int classId, out string message);

        bool IsAcceptableByPBTE(string traineeCNIC, out string message);

        int BatchInsert(List<TraineeProfileModel> ls, int @BatchFkey, int currentUserID);

        List<TraineeProfileModel> GetByGenderID(int genderID);

        List<TraineeProfileModel> GetByTradeID(int tradeID);

        List<TraineeProfileModel> GetBySectionID(int sectionID);

        List<TraineeProfileModel> GetBySchemeID(int schemeID);

        List<TraineeProfileModel> GetByTspID(int tspID);

        void ActiveInActive(int traineeID, bool? inActive, int curUserID);

        List<TraineeProfileModel> FetchTraineeProfileByFilters(int[] filters);

        List<TraineeProfileModel> FetchTraineeProfileByFiltersPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        TraineeProfileModel TraineeManualVerification(TraineeProfileModel traineeProfile);

        bool UpdateTraineeStatus(int traineeID, int traineeStatusTypeID, int CurUserID, string TraineeReason);

        TraineeProfileModel UpdateTraineeResultStatus(TraineeProfileModel model);

        List<TraineeProfileModel> FetchTraineesByFilters(int[] filters);

        List<TraineeProfileModel> FetchTraineesByUser(QueryFilters filters);

        bool BatchUpdateExtraTrainees(string json, int currentUserID);

        bool UpdateTraineeCNICImg(TraineeProfileModel model);

        int SaveTraineeProfileDVV(TraineeProfileDVV model, out string errMsg);

        void SaveTraineeProfileDVVResponse(TraineeProfileDVV model);

        bool SaveTraineeAttendance(TraineeAttendanceDVV model, out string errMsg);

        bool SaveTraineeAcknowledgement(TraineeAcknowledgementDVV model, out string errMsg);

        List<TraineeProfileModel> FetchTraineesDataByUser(int UserID);

        List<TraineeProfileModel> GetByTraineeCNIC(string TraineeCNIC);

        public List<CNICStatusModel> GetTraineesFromFile(List<CNICStatusModel> cnic);

        bool NadraVerysisExcel(int traineeProfileId, string CNICNumber, string traineeName, string fatherNAME,
            DateTime dOB, string gender, bool cNICVerified, string cNICUnVerifiedReason, int intDistrictVerified,
            bool isAgeVerified, string underOverAge, string disrictUnverifiedReason,
            string ResidenceDistrict, string ResidenceTehsil, string TraineeReligion, string permanentAddress
            , string permanentDistrict, string permanentTehsil);

        void DeleteDraftTrainee(int traineeID);

        bool IsValidCNICFormat(string cnic, out string errMsg);

        bool IsValidMobileNoFormat(string phoneNo, out string errMsg);

        public List<TraineeProfileModel> FetchCRTraineesByUser(QueryFilters filters);

        public List<TraineeProfileModel> FetchTraineeHistoryByTraineeID(int TraineeID);

        public List<TraineeProfileModel> FetchTraineesByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        public List<TraineeProfileModel> FetchCRTraineesByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        TSPMasterModel GetTSPUserAndKAMUserByTraineeID(int traineeID);

        public List<TraineeProfileModel> FetchTraineeProfileVerificationByFiltersPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);
        List<TraineeProfileModel> FetchSubmittedTraineeDataByTsp(int? TraineeFilter, int? programid, int? districtid, int? tradeid, int UserID, int? trainingLocationID);
        List<TraineeProfileModel> SaveTraineeIntrestProfile(TraineeProfileModel traineeProfile);
        List<TraineeProfileModel> SaveTraineeInterviewSubmission(TraineeProfileModel traineeProfile);
        public DataTable FetchReport(int UserID, string SpName);

        List<CheckRegistrationCriteriaModel> checkTSPTradeCriteria(int programid, int tradeid, int UserID);

        DataTable SaveTraineeBiometricData(BiometricTraineeDataModel model);

        DataTable SaveBiometricAttendance(BiometricTraineeDataModel model);

        void DeleteTraineeandAttandance(string CNIC);
    }
}