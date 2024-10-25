using GoToGagarin.HostBuilders.Navigation;
using Microsoft.Extensions.Hosting;

namespace GoToGagarin.HostBuilders
{
    public static class BuildViewModelExtensions
    {
        public static IHostBuilder BuildViewModels(this IHostBuilder hostBuilder)
        {
            return hostBuilder.BuildBodyViewModels().BuildViewModalModels();
        }
    }
}