using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmNavigationKit.Abstractions;
using MvvmNavigationKit.NavigationServices;
using MvvmNavigationKit.NavigationStores;
using MvvmNavigationKit.Options;

namespace MVVMNavigationKit.ServiceBuild
{
    public static class NavigationServicesHelper
    {
        public static void CreateServiceCollections(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Information);
            });

            services.AddSingleton<INavigationStore, NavigationStore>();

            services.Configure<NavigationOptions>(opt => { });

            services.AddSingleton<INavigationService, NavigationService>();
        }
    }
}
