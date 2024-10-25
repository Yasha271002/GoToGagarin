using GoToGagarin.Helpers;
using GoToGagarin.Utilities;
using GoToGagarin.View.Window;
using GoToGagarin.ViewModel.Controls;
using GoToGagarin.ViewModel.Window;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using System.Windows;

namespace GoToGagarin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(s => s.AddJsonFile("appsettings.json"))
            .ConfigureServices((context, services) =>
                {
                    var host = context.Configuration.GetValue<string>("host");

                    var refitSettings = new RefitSettings
                    {
                        ContentSerializer = new NewtonsoftJsonContentSerializer()
                    };
                    services.AddHttpClient<ImageLoadingHttpClient>(c => c.BaseAddress = new Uri(host ?? string.Empty));
                    services.AddRefitClient<IMainApiClient>(settings: refitSettings)
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(host ?? string.Empty));

                    var inactivityTime = context.Configuration.GetValue<int>("inactivityTime");
                    services.AddSingleton<InactivityHelper>(_ => new InactivityHelper(inactivityTime));

                    //var terminalId = context.Configuration.GetValue<int>("terminalId");
                    //services.AddSingleton<MapViewModel>(s =>
                    //    new MapViewModel(
                    //        s.GetRequiredService<IMainApiClient>(),
                    //        s.GetRequiredService<ImageLoadingHttpClient>(),
                    //        terminalId));
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<ObjectInfoViewModel>
                    (s => new ObjectInfoViewModel(
                        s.GetRequiredService<IMainApiClient>(),
                        s.GetRequiredService<ImageLoadingHttpClient>()));
                    services.AddSingleton<MainWindow>(
                        s => new MainWindow
                        {
                            DataContext = s.GetRequiredService<MainWindowViewModel>()
                        });
                }
            ).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            await _host.StartAsync();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}