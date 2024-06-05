using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVCluster
    {
        ClusterModel GetByClusterID(int ClusterID);

        List<ClusterModel> SaveCluster(ClusterModel Cluster);

        List<ClusterModel> FetchCluster(ClusterModel mod);

        List<ClusterModel> FetchCluster();

        List<ClusterModel> FetchCluster(bool InActive);
        public List<ClusterModel> FetchClusterForROSIFilter(ROSIFiltersModel rosiFilters);

        void ActiveInActive(int ClusterID, bool? InActive, int CurUserID);
    }
}