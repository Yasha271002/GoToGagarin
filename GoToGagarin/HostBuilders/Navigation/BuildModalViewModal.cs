using Microsoft.Extensions.Hosting;

namespace GoToGagarin.HostBuilders.Navigation
{
    public static class BuildModalViewModels
    {
        public static IHostBuilder BuildViewModalModels(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
            return builder;
        }
    }
}