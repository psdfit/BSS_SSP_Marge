using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVRTP
    {
        public List<RTPModel> GetByRTPID(int RTPID);
        //public List<RTPModel> SaveRTP(RTPModel RTP);
        void SaveRTP(RTPModel R);
        List<RTPModel> FetchRTP(RTPModel mod);
        List<RTPModel> FetchRTP();
        List<RTPModel> FetchRTP(bool InActive);
        List<RTPModel> FetchRTPByKAM(int userid, int OID = 0);
        List<RTPModel> FetchRTPByTPM();
        List<RTPModel> GetByClassID(int ClassID);

        List<CenterInspectionModel> GetCenterInspectionData(int classid);
        List<CenterInspectionModel> GetCenterInspectionDataSecurity(int classid);
        List<CenterInspectionModel> GetCenterInspectionDataIncharge(int classid);
        List<CenterInspectionModel> GetCenterInspectionDataIntegrity(int classid);
        List<CenterInspectionComplianceModel> GetCenterInspectionAdditionalCompliance(int classid);
        List<CenterInspectionTradeDetailModel> GetCenterInspectionTradeDetail(int classid);
        List<CenterInspectionClassDetailModel> GetCenterInspectionClassDetail(int classid);
        List<CenterInspectionNecessaryFcilitiesModel> GetCenterInspectionNecessaryFacilities(int classid);
        List<CenterInspectionTradeToolModel> GetCenterInspectionTradeTools(int classid);
        void GetKAMUserByClassID(int ClassID, string ClassCode, int CurUserID);
        void SenedNotificationApprovedAndRejectRTP(RTPModel R);
        void ApproveRTPRequest(RTPModel D);
        void UpdateNTP(RTPModel rtp);
        List<RTPModel> FetchRTPByTSP(NTPByUserModel ntp);
        void UpdateCenterInspection(int classid);

        List<CenterInspectionModel> GetCenterInspection(int classid);
        void ActiveInActive(int RTPID, bool? InActive, int CurUserID);
        public List<RTPModel> FetchRTPByKAMUser(RTPByKAMModel rtp);
        bool saveCentreMonitoringClassRecordNotification(List<RTPModel> model, int? CurUserID);
    }
}
