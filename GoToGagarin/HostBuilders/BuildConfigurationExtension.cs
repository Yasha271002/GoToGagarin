using Microsoft.Extensions.Hosting;

namespace GoToGagarin.HostBuilders
{
    public static class BuildConfigurationExtension
    {
        public static IHostBuilder BuildViews(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
            return builder;
        }
    }
}