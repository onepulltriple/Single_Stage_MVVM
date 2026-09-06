using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SingleStage.DAC;
using SingleStage.DAC.Interfaces;
using SingleStage.Entities;
using SingleStage.Infrastructure;
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
                    services.AddTransient<IArtistDAC, ArtistDAC>(); 
                      // whenever something asks for an IArtistDAC, give it an ArtistDAC
                    //services.AddTransient<ArtistPerformance>();
                    services.AddTransient<PerformanceDAC>();
                    services.AddTransient<ArtistPerformanceDAC>();
                    services.AddTransient<ShowDAC>();
                    services.AddTransient<TicketholderDAC>();
                    services.AddTransient<EmployeeDAC>();

                    // ViewModels and Windows
                    services.AddTransient<EmployeeLoginViewModel>();
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<ManageArtistsViewModel>();
                    services.AddTransient<ManagePerformancesViewModel>();
                    services.AddTransient<ManageShowsViewModel>();
                    services.AddTransient<ManageTicketholdersViewModel>();

                    services.AddTransient<EmployeeLoginWindow>();
                    services.AddTransient<MainWindow>();
                    services.AddTransient<ManageArtistsWindow>();
                    services.AddTransient<ManagePerformancesWindow>();
                    services.AddTransient<ManageShowsWindow>();
                    services.AddTransient<ManageTicketholdersWindow>();

                    // Infrastructure and Services
                    services.AddTransient<ShowScheduleValidator>();
                    services.AddTransient<PerformanceScheduleValidator>();

                    // depricated
                    services.AddTransient<EmployeeLoginWindowNoMVVM>();
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
