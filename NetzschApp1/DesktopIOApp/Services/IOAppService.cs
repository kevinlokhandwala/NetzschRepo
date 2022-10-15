using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopIOApp.Services
{
    public class IOAppService
    {
        #region Private fields

        private HubConnection _connection;
        public event Action<string> MessageReceived;
        private string _serverUrl;

        #endregion

        #region Class Properties

        /// <summary>
        /// Input string
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Output string
        /// </summary>
        public string Output { get; set; }

        #endregion

        public IOAppService(HubConnection connection, string _url)
        {
            _connection = connection;
            _connection.On<string>("ReceiveMessage", (msg) => MessageReceived?.Invoke(msg));
            _serverUrl = _url;
        }

        /// <summary>
        /// Establish the connection
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string BuildConnection(string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                _connection = new HubConnectionBuilder()
               .WithUrl(_serverUrl)
               .Build();
            }
            else
            {
                _connection = new HubConnectionBuilder()
               .WithUrl(url)
               .Build();
                _serverUrl = url;
            }
            return _serverUrl;
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <returns></returns>
        public async Task Connect()
        {
            try
            {
                await _connection.StartAsync();
            }

            catch (HttpRequestException e)
            {
                MessageBox.Show("Connection was refused by the target! \n" +
                    "Maybe the Url is incorrect " +
                    "or the server is inactive. \n" +
                    "Please try to connect again manually.",
                    "Error Connecting",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message +
                    "\nPlease try to connect again manually.",
                    "Error Connecting",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        /// <summary>
        /// Send Message to the server
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string message)
        {
            if (_connection.State == HubConnectionState.Connected)
                await _connection?.SendAsync("ReceiveMessage", _connection.ConnectionId, message);
        }
    }
}
