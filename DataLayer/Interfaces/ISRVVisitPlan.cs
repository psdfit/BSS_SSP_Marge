using DataLayer.Models;
using System;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVVisitPlan
    {
        VisitPlanModel GetByVisitPlanID(int VisitPlanID);
        List<VisitPlanModel> SaveVisitPlan(VisitPlanModel VisitPlan);
        List<VisitPlanModel> FetchVisitPlan(VisitPlanModel mod);
        List<VisitPlanModel> FetchVisitPlan();
        List<VisitPlanModel> FetchVisitPlan(bool InActive);
        void ActiveInActive(int VisitPlanID, bool? InActive, int CurUserID);
        List<VisitPlanModel> FetchVisitPlanByDate(DateTime? date);
        List<VisitPlanModel> GetUserEventReport(int visitplanid);


        List<VisitPlanModel> GetByVisitType(int userLevel);

        List<VisitPlanModel> FetchCalendarVisitPlan(VisitPlanModel mod);
        void UpdateUserEventStatus(UserEventMapModel uvm);

        List<VisitPlanModel> FetchCallCenterVisitPlan();


        void SaveNewCallCenterAgent(UserEventMapModel callCenterAgent);
        void UpdateCallCenterAgentEventStatus(UserEventMapModel vpm);


        List<UserEventMapModel> GetEventUsers(int visitplanid);

        List<UsersModel> FetchTSPUsers(String ids);



    }
}
