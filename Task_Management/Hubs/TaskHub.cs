using Microsoft.AspNet.SignalR;
using Task_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Management.Hubs
{
    public class TaskHub : Hub
    {
        public void SendTaskUpdate(Task task)
        {
            Clients.All.ReceiveTaskUpdate(task);
        }
    }
}