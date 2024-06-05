/* **** Aamer Rehman Malik *****/

using DataLayer.JobScheduler.Scheduler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using PSDF_BSS.Logging.FilterAttribute;
using Rotativa.AspNetCore;
using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using DataLayer.Services;
using DataLayer;
using PSDF_BSS.BackgroundServices;

namespace PSDF_BSS
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/Logging/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddHostedService<BGSSendEmail>();

            services.AddHttpClient();
            IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
            services.AddMyCors(MyAllowSpecificOrigins, configuration.GetSection("AppSettings").GetSection("CorsAllowOrigins").Value);
            //services.AddControllersWithViews();
            services.AddRazorPages().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }); ;
            services.AddControllers((o) =>
            {
                var policy = new AuthorizationPolicyBuilder()
                 .RequireAuthenticatedUser()
                 .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
                o.Filters.Add<ErrorFilter>();
            }).AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            //.AddApplicationPart(Assembly.Load(new AssemblyName("PSDF_BSS.API")));
            services.AddMyAuthentication(configuration.GetSection("AppSettings").GetSection("Secret").Value);

            //services.AddMvc(options =>
            //{
            //    //options.Filters.Add<LogFilter>();
            //    options.Filters.Add<ErrorFilter>();
            //});
            //services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("PSDF_BSS.API")));
            services.AddHttpContextAccessor();
            services.AddTransient<IUserService, UserService>();
            services.AddSingletonClasses();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IScheduler scheduler)
        {

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(route =>
            {
                route.MapHub<NotificationsHub>("/notifications");
            });

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseWebSockets();

            //var webSocketOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(1),
            //};

            //app.UseWebSockets(webSocketOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);
            
           // scheduler.Start();
        }
    }
}