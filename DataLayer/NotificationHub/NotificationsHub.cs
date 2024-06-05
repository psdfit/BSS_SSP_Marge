using DataLayer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class NotificationsHub : Hub<INotificationsHub>, INotificationsHub
    {
        public readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public NotificationsHub() { }

        public async Task SendNotification(NotificationDetailModel message)
        {
            await Clients.Caller.SendNotification(message);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public string GetConnectionId(string UserID)
        {
            //Remove Previous Connection
            IEnumerable<string> list = _connections.GetConnections(UserID);


            foreach (var item in list)
            {
                if (item != null)
                {
                    _connections.Remove(UserID, item);

                }
                break;
            }




                //Add Current Connection
                _connections.Add(UserID, Context.ConnectionId);
            return Context.ConnectionId;
        }
    }

    public interface INotificationsHub
    {
        Task SendNotification(NotificationDetailModel message);
    }
}
