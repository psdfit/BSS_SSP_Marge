using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PSDF_BSS.API.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace PSDF_BSS.API.Logging.FilterAttribute
{
    public class ErrorFilter : ActionFilterAttribute
    {
        private readonly INLogManager logManager;
        public ErrorFilter(INLogManager nLogManager)
        {
            this.logManager = nLogManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = (context.Controller as ControllerBase).ControllerContext.ActionDescriptor;
            string controllerName = descriptor.ControllerName;
            string actionName = descriptor.ActionName;
            string argKeyValues = string.Empty;
            string result = string.Empty;
            ///request Body / Argument
            foreach (var arg in context.ActionArguments)
            {
                ///Shallow Copy
                var value = arg.Value?.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(arg.Value, null);
                if (value != null)
                {
                    value.GetType().GetProperty("UserPassword")?.SetValue(value, null);
                    argKeyValues = $"{arg.Key} : {JsonConvert.SerializeObject(value)} \n";
                }
            }

            ///Response Body / Result
            if (context.Result is JsonResult json)
            {
                //var statusCode = json.StatusCode;
                //var responseBody = json.Value;
                if (json.StatusCode != (int)HttpStatusCode.OK)
                {
                    result = JsonConvert.SerializeObject(context.Result);
                }

            }

            var log = new Dictionary<string, object>()
                    {
                        {"ControllerName",controllerName}
                        ,{"ActionName", actionName}
                        ,{"Arguments", argKeyValues}
                        ,{"Result", result}
                        ,{"UserId", Convert.ToInt32(context.HttpContext.User.Identity.Name)},
                       // ,{"UserName", Convert.ToInt32(User.Identity.Name)},
                       // ,{"UserEmail", Convert.ToInt32(User.Identity.Name)},
                    };
            logManager.LogError("Log Error", log);

            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
