using Microsoft.Extensions.Hosting;
using MvvmNavigationLib.Services.ServiceCollectionExtensions;
using MvvmNavigationLib.Stores;

namespace GoToGagarin.HostBuilders.Navigation
{
    public static class BuildModalNavigationExtensions
    {
        public static IHostBuilder BuildModalNavigation(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddUtilityNavigationServices<ModalNavigationStore>();
            });
            return builder;
        }
    }
}