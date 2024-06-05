using DataLayer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer
{
    public class NotificafationManager
    {
        private readonly IHubContext<NotificationsHub, INotificationsHub> _hubContext;
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public NotificafationManager(IHubContext<NotificationsHub, INotificationsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public string Send(string id, NotificationDetailModel message)
        {
            var connectionId = NotificationsHub._connections.GetConnections(id);
            if (connectionId is ICollection<string> list)
            {
                if (list.Count==1)
                {
                    _hubContext.Clients.Client(connectionId.FirstOrDefault().ToString()).SendNotification(message);

                }
            }
            return "message send to: " + id;

        }
        public string SendToAll(NotificationDetailModel message)
        {
            _hubContext.Clients.All.SendNotification(message);
            return "message send to All ";
        }
        public string SendToSpecificUsers(List<string> id, NotificationDetailModel message)
        {
            _hubContext.Clients.Users(id).SendNotification(message);
            return "message send to: " + id;
        }
    }
}
