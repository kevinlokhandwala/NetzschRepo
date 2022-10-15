using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebIOApp.Hubs
{
    public class IOHub : Hub
    {

        /// <summary>
        /// Sends the received message to all the connected clients
        /// </summary>
        /// <param name="sender">Client that sent the message</param>
        /// <param name="incomingMessage">Message from the sender</param>
        /// <returns></returns>
        public async Task ReceiveMessage(string sender, string incomingMessage)
        {
            //Send messagess to all the clients except the sender client itself
            await Clients.AllExcept(sender).SendAsync("ReceiveMessage", incomingMessage);
        }
    }
}
