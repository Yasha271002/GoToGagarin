using CommunityToolkit.Mvvm.Messaging;
using GoToGagarin.Helpers;
using GoToGagarin.View.Window;
using GoToGagarin.ViewModel.Window;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GoToGagarin.HostBuilders;

public static class BuildViewsExtension
{
    public static IHostBuilder BuildViews(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<IMessenger>(s => new WeakReferenceMessenger());
            services.AddSingleton(_ => new InactivityHelper(context.Configuration.GetValue<int>("inactivityTime")));
            services.AddSingleton(_ => new UpdateCacheInactivityHelper(context.Configuration.GetValue<int>("updateInactivityTime")));

            services.AddSingleton(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });
        });
        return builder;
    }
}