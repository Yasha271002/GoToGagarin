using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Stores;

namespace GoToGagarin.HostBuilders.Navigation
{
    public static class BuildMainNavigationExtensions
    {
        public static IHostBuilder BuildMainNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<ModalNavigationStore>();
            });

            return builder;
        }
    }
}