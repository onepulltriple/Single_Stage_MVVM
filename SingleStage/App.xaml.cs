using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.ViewModels;
using SingleStage.Windows;
using System;
using System.Windows;

namespace SingleStage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // the host is an object that manages the lifetime, services, and infrastructure of the application
        private IHost? _host;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    // DbContext is transient by default to get a new context instance per resolve
                    services.AddDbContext<SingleStageMvvmContext>();

                    // DACs - register concrete types (constructor takes SingleStageMvvmContext)
                    services.AddScoped<AppearanceDAC>();
                    services.AddScoped<ArtistDAC>();
                    services.AddScoped<EmployeeDAC>();
                    services.AddScoped<ShowDAC>();
                    services.AddScoped<TicketholderDAC>();

                    // ViewModels and Windows
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<ManageArtistsViewModel>();
                    services.AddTransient<ManageTicketholdersViewModel>();

                    services.AddTransient<EmployeeLoginWindow>();
                    services.AddTransient<MainWindow>();
                    services.AddTransient<ManageArtistsWindow>();
                    services.AddTransient<ManageTicketholdersWindow>();
                })
                .Build();
            
            _host.Start();

            // resolve the login window via DI so its constructor dependencies are injected
            var loginWindow = _host.Services.GetRequiredService<EmployeeLoginWindow>();
            loginWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
        }
    }

}
