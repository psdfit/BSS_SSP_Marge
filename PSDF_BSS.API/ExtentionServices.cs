using DataLayer;
using DataLayer.Interfaces;
using DataLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PSDF_BSS.API.Logging.Interface;
using System.Text;
using TestAPI.Logging.Service;

namespace PSDF_BSS.API
{
    public static class ExtentionServices
    {
        public static IServiceCollection AddSingletonService(this IServiceCollection services)
        {
            ///Add service to serviceContainer
            services.AddSingleton(typeof(ISRVTraineeProfile), typeof(SRVTraineeProfile));
            services.AddSingleton(typeof(ISRVUsers), typeof(SRVUsers));
            services.AddSingleton(typeof(ISRVInstructor), typeof(SRVInstructor));
            services.AddSingleton(typeof(ISRVInstructorMaster), typeof(SRVInstructorMaster));
            services.AddSingleton(typeof(ISRVClass), typeof(SRVClass));
            services.AddSingleton(typeof(ISRVScheme), typeof(SRVScheme));
            services.AddSingleton(typeof(ISRVTSPDetail), typeof(SRVTSPDetail));
            services.AddSingleton(typeof(ISRVOrgConfig), typeof(SRVOrgConfig));
            services.AddSingleton(typeof(ISRVSendEmail), typeof(SRVSendEmail));
            services.AddSingleton(typeof(ISRVReligion), typeof(SRVReligion));
            services.AddSingleton(typeof(ISRVDistrict), typeof(SRVDistrict));
            services.AddSingleton(typeof(ISRVTehsil), typeof(SRVTehsil));
            services.AddSingleton(typeof(ISRVGender), typeof(SRVGender));
            services.AddSingleton(typeof(ISRVTraineeStatus), typeof(SRVTraineeStatus));
            services.AddSingleton(typeof(ISRVEducationTypes), typeof(SRVEducationTypes));
            services.AddSingleton(typeof(INLogManager), new NLogManager());
            services.AddSingleton(typeof(ISRVComplaint), typeof(SRVComplaint));
            services.AddSingleton(typeof(ISRVWebsite), typeof(SRVWebsite));
            services.AddSingleton(typeof(INotificationsHub), typeof(NotificationsHub));
            services.AddSingleton(typeof(ISRVNotificationMap), typeof(SRVNotificationMap));



            return services;
        }
        public static IServiceCollection AddCorsService(this IServiceCollection services, string allowOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(allowOrigins, builder =>
                {
                    builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });
            return services;
        }

        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, string key)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return services;
        }
    }
}
