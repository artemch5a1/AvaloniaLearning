using AvaloniaApp.NavigationStore;
using AvaloniaApp.NavService;
using AvaloniaApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AvaloniaApp.Tests
{
    public class NavigationServiceTests
    {
        private ServiceProvider _serviceProvider;

        public NavigationServiceTests()
        {
            ServiceCollection serviceDescriptors = new ServiceCollection();

            serviceDescriptors.AddTransient<ViewModelBase>();

            serviceDescriptors.AddSingleton<NavigationService>();

            serviceDescriptors.AddSingleton<NavStore>();

            _serviceProvider = serviceDescriptors.BuildServiceProvider();
        }

        [Fact]
        public void Navigate_NavigateToVM_ShouldChangeCurrentVM()
        {
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            Assert.NotNull(navigationService);

            navigationService.Navigate<ViewModelBase>();

            NavStore navStore = _serviceProvider.GetRequiredService<NavStore>();

            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(ViewModelBase));
        }
    }
}
