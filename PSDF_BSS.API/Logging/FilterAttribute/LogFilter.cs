using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSDF_BSS.API.Logging.Interface;
using PSDF_BSS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace PSDF_BSS.API.Logging.FilterAttribute
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

            //if (actionName == "GetClassesByScheme")
            //{
            //    base.OnActionExecuting(context);
            //    return;
            //}
            Dictionary<string, string> argKeyValues = new Dictionary<string, string>();
            //var jsonObject = new JObject();
            //foreach (var arg in context.ActionArguments)
            //{
            //    if (arg.Value.GetType().BaseType.Name != "Object")
            //    {
            //        var jsonObject = JObject.FromObject(arg.Value);
            //        if (jsonObject.ContainsKey("UserPassword"))
            //        {
            //            jsonObject.Property("UserPassword").Value = string.Empty;
            //        }
            //        argKeyValues.Add(arg.Key, jsonObject.ToString());
            //    }
            //    else
            //    {
            //        argKeyValues.Add(arg.Key, arg.Value.ToString());
            //    }

                /////Shallow Copy
                //dynamic value = arg.Value?.GetType().GetMethod("MemberwiseClone",  BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(arg.Value, null);
                //if (value != null)
                //{
                //    value.GetType().GetProperty("UserPassword")?.SetValue(value, null);
                //    argKeyValues.Add(arg.Key,JsonConvert.SerializeObject(value));
                //}
                // }

                //var identity = context.HttpContext.User.Identity.Name as ClaimsIdentity;
                //if (identity != null)
                //{
                //     IEnumerable<Claim> claims = identity.Claims;
                //     // or
                //     identity.FindFirst("ClaimName").Value;

                 //}

                this.log = new Dictionary<string, object>()
                    {
                        {"ControllerName",controllerName}
                        ,{"ActionName", actionName}
                        ,{"ActionArguments",  argKeyValues}
                        ,{"UserId", Convert.ToInt32(context.HttpContext.User.Identity.Name)},
                        //,{"UserName", Convert.ToInt32(User.Identity.Name)},
                       //,{"UserEmail", Convert.ToInt32(User.Identity.Name)},
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
