using Microsoft.Extensions.Hosting;

namespace GoToGagarin.HostBuilders
{
    public static class BuildBodyViewModelsExtensions
    {
        public static IHostBuilder BuildBodyViewModels(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
            });
            return builder;
        }
    }
}