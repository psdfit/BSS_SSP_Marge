using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using PSDF_BSS.Reports.Services;
using PSDF_BSS.Reports.Interfaces;
namespace PSDF_BSS.Reports
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ISRVProfileVerification, SRVProfileVerification>();
            container.RegisterType<ISRVTSPMasterData, SRVTSPMasterData>();
            container.RegisterType<ISRVTradeData, SRVTradeData>();
            container.RegisterType<ISRVTraineeStatusReport, SRVTraineeStatusReport>();
            container.RegisterType<ISRVReports, SRVReports>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}