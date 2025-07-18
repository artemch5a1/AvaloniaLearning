using AvaloniaApp.NavigationStore;
using AvaloniaApp.NavService;
using AvaloniaApp.Tests.TestHelper;
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

            serviceDescriptors.AddScoped<FakeViewModel>();

            serviceDescriptors.AddScoped<ViewModelBase>();

            serviceDescriptors.AddScoped<NavigationService>();

            serviceDescriptors.AddScoped<NavStore>();

            _serviceProvider = serviceDescriptors.BuildServiceProvider();
        }

        [Fact]
        public void Navigate_NavigateToVM_ShouldChangeCurrentVM()
        {
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            navigationService.Navigate<ViewModelBase>();

            NavStore navStore = _serviceProvider.GetRequiredService<NavStore>();

            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(ViewModelBase));
        }

        [Fact]
        public void Navigate_NavigateWithParams_ShouldCallInitializeWithRightParams()
        {
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            (int, string) @param = (12, "string");

            navigationService.Navigate<FakeViewModel, (int, string)>(@param);

            FakeViewModel fakeView = _serviceProvider.GetRequiredService<FakeViewModel>();

            NavStore navStore = _serviceProvider.GetRequiredService<NavStore>();

            Assert.True(fakeView.itemParam == @param);
            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(FakeViewModel));
        }

        [Fact]
        public void Navigate_NavigateWithWrongTypeParams_ShouldThrowArgumentException()
        {
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            int @param = 12;

            Assert.Throws<ArgumentException>(() =>
                navigationService.Navigate<FakeViewModel, int>(@param)
            );
        }
    }
}
