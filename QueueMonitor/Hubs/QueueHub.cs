using System;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.SignalR;

namespace QueueMonitor.Hubs
{
    public class QueueHub : Hub
    {
        public QueueHub()
        {
         
        }

        public async Task SendMessage(string queue, int data)
        {
            await Clients.All.SendAsync("ReceiveMessage", queue, data);
        }
    }
}

