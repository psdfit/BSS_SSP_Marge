using DataLayer;
using DataLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSDF_BSS.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly IHubContext<NotificationsHub, INotificationsHub> _hubContext;

        public SendController(IHubContext<NotificationsHub, INotificationsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        //[HttpGet("signalr/{id}")]
        //public string Signalr(string id)
        //{
        //    _hubContext.Clients.Client(id).SendNotification();
        //    return "message send to: " + id;
        //}
    }
}
