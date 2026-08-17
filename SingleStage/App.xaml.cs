using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SingleStage.DAC;
using SingleStage.Entities;
using SingleStage.ViewModels;
using SingleStage.Windows;
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
                    // DbContext was made transient to get a new context instance per resolve
                    services.AddTransient<SingleStageMvvmContext>();

                    // DACs - register concrete types (constructor takes SingleStageMvvmContext)
                    services.AddTransient<AppearanceDAC>();
                    services.AddTransient<ArtistDAC>();
                    services.AddTransient<EmployeeDAC>();
                    services.AddTransient<ShowDAC>();
                    services.AddTransient<TicketholderDAC>();

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
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }

}
