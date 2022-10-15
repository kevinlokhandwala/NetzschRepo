using DesktopIOApp.Commands;
using DesktopIOApp.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DesktopIOApp
{
    class DesktopIOViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        private IOAppService _ioService;
        private string _output;
        private string _input;
        private string _serverUrl;
        private bool _connected;
        private RelayCommand _connectCommand;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Class Properties

        /// <summary>
        /// Enables the manual connection button
        /// </summary>
        public bool ConnectEnabled
        {
            get { return !_connected; }

            set
            {
                if (_connected != !value)
                {
                    _connected = !value;
                    NotifyPropertyChanged(nameof(ConnectEnabled));
                }
            }
        }

        /// <summary>
        /// Input text to be displayed
        /// </summary>
        public string InputText
        {
            get { return _input; }
            set
            {
                if (_input != value)
                {
                    _input = value;
                    NotifyPropertyChanged(nameof(InputText));
                    SendMessageToClient(value);
                }
            }
        }

        /// <summary>
        /// Output text to be displayed
        /// </summary>
        public string OutputText
        {
            get { return _output; }
            set
            {
                if (_output != value)
                {
                    _output = value;
                    NotifyPropertyChanged(nameof(OutputText));
                }
            }
        }

        /// <summary>
        /// The URL of the server
        /// </summary>
        public string ServerUrl
        {
            get { return _serverUrl; }
            set
            {
                if (_serverUrl != value)
                {
                    _serverUrl = value;
                    NotifyPropertyChanged(nameof(ServerUrl));
                }
            }
        }

        /// <summary>
        /// Command to Connect to server
        /// </summary>
        public RelayCommand ConnectCommand 
        {
            get { return _connectCommand; }
        }

        #endregion


        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="ioModel">Business Logic for the View Model</param>
        public DesktopIOViewModel(IOAppService ioModel)
        {
            _ioService = ioModel;
            _ioService.MessageReceived += MessageReceivedHandler;
            _connectCommand = new RelayCommand(EstablishConnection);
            
        }

        /// <summary>
        /// Create the view model
        /// </summary>
        /// <param name="ioModel"></param>
        /// <returns></returns>
        public static DesktopIOViewModel CreateConnectedVM(IOAppService ioModel)
        {
            DesktopIOViewModel vm = new DesktopIOViewModel(ioModel);
            vm.ConnectEnabled = false;
            vm.ConnectToServer();

            return vm;
        }


        #region Class Methods

        /// <summary>
        /// Configures the connection and connects to the server
        /// </summary>
        /// <param name="obj"></param>
        public void EstablishConnection(object obj)
        {
            ConfigureConnection();
            ConnectToServer();
        }

        /// <summary>
        /// Configures the connection with the user provided server Url
        /// </summary>
        public void ConfigureConnection()
        {
           ServerUrl = _ioService.BuildConnection(ServerUrl);
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public async void ConnectToServer()
        {
            await _ioService.Connect().ContinueWith((task) =>
            {
                if (task.Exception != null)
                {
                    ConnectEnabled = true;
                }
                else
                    ConnectEnabled = false;
            }); 
        }

        /// <summary>
        /// Send the message to the client
        /// </summary>
        /// <param name="message">input message</param>
        private async void SendMessageToClient(string message)
        { 
           await _ioService.SendMessage(message);
        }

        /// <summary>
        /// Handle the on message received event
        /// </summary>
        /// <param name="msg"></param>
        private void MessageReceivedHandler(string msg)
        {
            OutputText = msg;
        }

        /// <summary>
        /// Trigger Property changed handler
        /// </summary>
        /// <param name="propertyName"></param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
