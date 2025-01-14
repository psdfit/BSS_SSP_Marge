using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVSkillsScholarshipInitiative
    {
      public List<SkillsScholarshipInitiativeModel> GetSkillsScholarshipBySchemeID(int SchemeID, int? TSPId, int Locality, int Cluster, int? District);
      public List<SkillsScholarshipInitiativeModel> GetFilteredSessionCount(int SchemeID, int? TSPId);

      public List<SkillsScholarshipInitiativeModel> GetSkillsScholarshipBySchemeIDReport(int SchemeID, int? TSPId, int Locality, int Cluster);

        public List<SkillsScholarshipInitiativeModel> FetchClustersByLocality(int Locality);
        public List<SkillsScholarshipInitiativeModel> FetchDistrictsByCluster(int Cluster);
        void GetStartRace(int SchemeID, int ClusterID, int DistrictID, int TradeID, int UserID);
      void GetStopRace(int SchemeID, int ClusterID, int DistrictID, int TradeID, int UserID);
      void GetDeleteSession(int SchemeID, int TSPID,int SessionID);
    }
}
