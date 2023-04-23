using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Crossdocking.Services;
using Crossdocking.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Crossdocking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection() { };

            services.AddSingleton<NavigationService, NavigationService>();
            services.AddSingleton<MainViewModel, MainViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
            services.AddSingleton<ExcelParserService, ExcelParserService>();
            services.AddTransient<LoginPageVM, LoginPageVM>();
            services.AddTransient<AdminPageVM, AdminPageVM>();
            services.AddTransient<ComputePageVM, ComputePageVM>();

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>();

            var navigationService = serviceProvider.GetService<NavigationService>();
            navigationService.CurrentViewModel = serviceProvider.GetService<LoginPageVM>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
