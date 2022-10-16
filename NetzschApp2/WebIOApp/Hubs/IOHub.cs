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
        /// Sends the received web message to all the connected Desktop clients
        /// </summary>
        /// <param name="sender">Client that sent the message</param>
        /// <param name="incomingMessage">Message from the sender</param>
        /// <returns></returns>
        public async Task SendDesktopMessage(string sender, string incomingMessage)
        {
            //Send messagess to all the clients except the sender client itself
            await Clients.AllExcept(sender).SendAsync("ReceiveDesktopMessage", incomingMessage);
        }

        /// <summary>
        /// Sends the received message from Desktop to all the connected web clients
        /// </summary>
        /// <param name="sender">Client that sent the message</param>
        /// <param name="incomingMessage">Message from the sender</param>
        /// <returns></returns>
        public async Task SendWebMessage(string sender, string incomingMessage)
        {
            //Send messagess to all the clients except the sender client itself
            await Clients.AllExcept(sender).SendAsync("ReceiveWebMessage", incomingMessage);
        }
    }
}
