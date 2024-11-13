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
using GoToGagarin.ViewModel.Popup;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;
using CommunityToolkit.Mvvm.Messaging;
using MvvmNavigationLib.Services;
using GoToGagarin.Model;

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
                    var terminalId = context.Configuration.GetValue<int>("terminalId");

                    var refitSettings = new RefitSettings
                    {
                        ContentSerializer = new NewtonsoftJsonContentSerializer()
                    };
                    services.AddUtilityNavigationServices<ModalNavigationStore>();
                    services.AddSingleton<IMessenger>(_ => new WeakReferenceMessenger());
                    services.AddHttpClient<ImageLoadingHttpClient>(c => c.BaseAddress = new Uri(host ?? string.Empty));
                    services.AddRefitClient<IMainApiClient>(settings: refitSettings)
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri(host ?? string.Empty));

                    var inactivityTime = context.Configuration.GetValue<int>("inactivityTime");
                    services.AddSingleton<InactivityHelper>(_ => new InactivityHelper(inactivityTime));

                    services.AddSingleton<ModalNavigationStore>();

                    services.AddSingleton<MapViewModel>(s => new MapViewModel(
                        s.GetRequiredService<ImageLoadingHttpClient>(),
                        s.GetRequiredService<IMainApiClient>(),
                        terminalId));
                    services.AddSingleton<SearchViewModel>();
                    services.AddSingleton<ObjectInfoViewModel>();
                    services.AddSingleton<NavigationViewModel>();
                    services.AddSingleton<MainWindowViewModel>();

                    services.AddParameterNavigationService<ContentSliderPopupViewModel, ModalNavigationStore, MapViewModel>(
                        s => param => new ContentSliderPopupViewModel(param, s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>()));
                    services.AddParameterNavigationService<InactivityPopupViewModel, ModalNavigationStore, ObjectInfoViewModel>(
                        s => param => new InactivityPopupViewModel(param, s.GetRequiredService<CloseNavigationService<ModalNavigationStore>>()));

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