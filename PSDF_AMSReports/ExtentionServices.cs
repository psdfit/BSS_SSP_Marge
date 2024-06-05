
using DataLayer.Interfaces;
using DataLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using PSDF_AMSReports.Dapper;
using PSDF_AMSReports.Interfaces;
using PSDF_AMSReports.Services;
using System.Text;

namespace PSDF_AMSReports
{
    public static class ExtentionServices
    {
        public static IServiceCollection AddSingletonService(this IServiceCollection services)
        {
            ///Add service to serviceContainer
            services.AddSingleton(typeof(IDapperConfig), typeof(DapperConfig));
            services.AddSingleton(typeof(IAMSReportService), typeof(AMSReportService));
            services.AddSingleton(typeof(ISRVRTP), typeof(SRVRTP));
            return services;
        }
    }
}
