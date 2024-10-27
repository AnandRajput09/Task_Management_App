using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Management.Hubs
{
    public class NotificationHub : Hub
    {
        // Method to send a message to all connected clients
        public static void NotifyAllClients(string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.receiveNotification(message);
        }
    }
}