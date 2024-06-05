using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSDF_BSS.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PSDF_BSS.Logging.FilterAttribute
{
    public class LogFilter : ActionFilterAttribute
    {
        private readonly INLogManager nLogManager;
        private Dictionary<string, object> log;
        private readonly IConfigurationRoot configuration;
        public LogFilter(INLogManager nLogManager)
        {
            this.nLogManager = nLogManager;
            log = new Dictionary<string, object>();
            configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = (context.Controller as ControllerBase).ControllerContext.ActionDescriptor;
            string controllerName = descriptor.ControllerName;
            string actionName = descriptor.ActionName;
            string argKeyValues = string.Empty;
            foreach (var arg in context.ActionArguments)
            {
                ///Shallow Copy
                var value = arg.Value?.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(arg.Value, null);
                if (value!=null)
                {
                    value.GetType().GetProperty("UserPassword")?.SetValue(value, null);
                    argKeyValues = $"{arg.Key} : {JsonConvert.SerializeObject(value)} \n"; 
                }
            }
            var log = new Dictionary<string, object>()
                    {
                        {"ControllerName",controllerName}
                        ,{"ActionName", actionName}
                        ,{"ArgKeyValues", argKeyValues}
                        ,{"UserId", Convert.ToInt32(context.HttpContext.User.Identity.Name)},
                       // ,{"UserName", Convert.ToInt32(User.Identity.Name)},
                       // ,{"UserEmail", Convert.ToInt32(User.Identity.Name)},
                    };
            //nLogManager.LogInformation("Log Information", log);

            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string level = configuration.GetSection("Logging:LogLevel:Default").Value;
            if (level == "Debug")
            {
                var response = (context.Result as ObjectResult)?.Value as ApiResponse;
                this.log.Add("ActionResponse", JsonConvert.SerializeObject(response));
            }

            nLogManager.LogInformation("Log Information", log);
            base.OnActionExecuted(context);
        }
    }
}
