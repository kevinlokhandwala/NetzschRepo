using DesktopIOApp.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopIOApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string _defaultUrl = "https://localhost:5001/netzschhub";

            HubConnection _connection = new HubConnectionBuilder()
                .WithUrl(_defaultUrl)
                .Build();
            
            IOAppService _core = new IOAppService(_connection, _defaultUrl);

            DesktopIOViewModel _vm = DesktopIOViewModel.CreateConnectedVM(_core);

            _vm.ServerUrl = _defaultUrl;

            DesktopIOView MainView = new DesktopIOView();

            MainView.DataContext = _vm;

            MainView.Show();
        }
    }
}
