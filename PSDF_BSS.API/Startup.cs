using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NLog;
using PSDF_BSS.API.Logging.FilterAttribute;

namespace PSDF_BSS.API
{
    public class Startup
    {
        private readonly string _allowOrigins = "_allowOrigins";
        private readonly IConfigurationRoot configurationRoot;
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/Logging/nlog.config"));
            Configuration = configuration;
            configurationRoot = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           services.AddSignalR();

            IConfigurationSection appSettings = configurationRoot.GetSection("AppSettings");
            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add<LogFilter>();
                
            }).AddNewtonsoftJson(jsonOptions =>
            {
                jsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.AddHttpContextAccessor();
            services.AddSingletonService();
            services.AddCorsService(_allowOrigins);
            services.AddAuthenticationService(appSettings.GetSection("Secret").Value);
            services.AddSwaggerGen(option=>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "PSDF", Version = "v1.1.0" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {securitySchema,new List<string>() }
                };
                option.AddSecurityDefinition("Bearer", securitySchema);
                option.AddSecurityRequirement(securityRequirement);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{


        //    app.Use(async (context, next) =>
        //    {
        //        context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; object-src 'none'; frame-ancestors 'none';");
        //        await next();
        //    });


        //    app.Use(async (context, next) =>
        //    {
        //        context.Response.Headers.Add("X-Frame-Options", "DENY");
        //        await next();
        //    });

        //    app.Use(async (context, next) =>
        //    {
        //        context.Response.Headers.Remove("X-Powered-By");
        //        await next();
        //    });

        //    app.Use(async (context, next) =>
        //    {
        //        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        //        await next();
        //    });


        //    app.Use(async (context, next) =>
        //    {
        //        context.Response.Headers.Remove("Server");
        //        await next();
        //    });

        //    app.Use(async (context, next) =>
        //    {
        //        context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
        //        context.Response.Headers.Add("Pragma", "no-cache");
        //        await next();
        //    });

        //    if (!env.IsDevelopment())
        //    {
        //        app.UseExceptionHandler("/Error");
        //        app.UseHsts();
        //    }

        //    //if (env.IsDevelopment())
        //    //{
        //    //    app.UseDeveloperExceptionPage();
        //    //}

        //    app.UseSignalR(route =>
        //    {
        //        route.MapHub<NotificationsHub>("/notifications");
        //    });

        //    if (env.IsDevelopment())
        //    {
        //        app.UseSwagger();
        //        app.UseSwaggerUI(option =>
        //        {
        //            option.SwaggerEndpoint("/swagger/v1/swagger.json", "PSDF-API");
        //        }); 
        //    }

        //    app.UseHttpsRedirection();

        //    app.UseRouting();

        //    app.UseCors(_allowOrigins);

        //    app.UseAuthentication();

        //    app.UseAuthorization();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Apply security headers
            app.Use(async (context, next) =>
            {
                // Content Security Policy (CSP)
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; object-src 'none'; frame-ancestors 'none';");

                // X-Frame-Options for clickjacking protection
                context.Response.Headers.Add("X-Frame-Options", "DENY");

                // X-Content-Type-Options to prevent MIME type sniffing
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                // Cache control headers
                context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate, max-age=0");
                context.Response.Headers.Add("Pragma", "no-cache");

                await next();
            });

            // Remove headers that leak information
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("X-Powered-By");  // Remove X-Powered-By header
                context.Response.Headers.Remove("Server");        // Remove Server header
                await next();
            });

            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts(); // Enable HSTS (Strict Transport Security) for production
            }

            if (env.IsDevelopment())
            {
                // Enable Swagger only in development
                app.UseSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "PSDF-API");
                });
            }

            // Enable SignalR
            app.UseSignalR(route =>
            {
                route.MapHub<NotificationsHub>("/notifications");
            });

            app.UseHttpsRedirection();    // Force HTTPS

            app.UseRouting();

            app.UseCors(_allowOrigins);   // Apply CORS policy

            app.UseAuthentication();      // Enable authentication
            app.UseAuthorization();       // Enable authorization

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();  // Map controller routes
            });
        }

    }
}
