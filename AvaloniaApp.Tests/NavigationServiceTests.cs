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

            serviceDescriptors.AddScoped<FakeViewModel2>();

            serviceDescriptors.AddScoped<ViewModelBase>();

            serviceDescriptors.Configure<NavigationOptions>(op => op.MaxSizeHistory = 20);

            serviceDescriptors.AddScoped<NavigationService>();

            serviceDescriptors.AddScoped<NavStore>();

            _serviceProvider = serviceDescriptors.BuildServiceProvider();
        }

        [Fact]
        public void Navigate_NavigateToVM_ShouldChangeCurrentVM()
        {
            //Arrange
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            //Act
            navigationService.Navigate<ViewModelBase>();

            //Assert
            NavStore navStore = _serviceProvider.GetRequiredService<NavStore>();

            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(ViewModelBase));
        }

        [Fact]
        public void Navigate_NavigateWithParams_ShouldCallInitializeWithRightParams()
        {
            //Arrange
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            (int, string) @param = (12, "string");

            //Act
            navigationService.Navigate<FakeViewModel, (int, string)>(@param);

            //Assert
            FakeViewModel fakeView = _serviceProvider.GetRequiredService<FakeViewModel>();
            NavStore navStore = _serviceProvider.GetRequiredService<NavStore>();

            Assert.True(fakeView.itemParam == @param);
            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(FakeViewModel));
        }

        [Fact]
        public void Navigate_NavigateWithWrongTypeParams_ShouldThrowArgumentException()
        {
            //Arrange
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            int @param = 12;

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
                navigationService.Navigate<FakeViewModel, int>(@param)
            );
        }

        [Fact]
        public void Navigate_NavigateWithReferenceTypeParam_ShouldInitialize()
        {
            //Arrange
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            Animal animal = new Animal() { Name = "Foo", Age = 10 };

            //Act
            navigationService.Navigate<FakeViewModel2, Animal>(animal);

            //Assert
            FakeViewModel2 fakeView = _serviceProvider.GetRequiredService<FakeViewModel2>();

            Assert.True(animal.Equals(fakeView.animal));
        }

        [Fact]
        public void NavigateBack_NavigateWithNotEmptyHistory_NavigateToRightVM()
        {
            //Arrange
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            NavStore navStore = _serviceProvider.GetRequiredService<NavStore>();

            //Act
            navigationService.Navigate<FakeViewModel>();

            navigationService.Navigate<FakeViewModel2>();

            navigationService.Navigate<ViewModelBase>();

            //Act & Assert
            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(ViewModelBase));

            navigationService.NavigateBack();

            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(FakeViewModel2));

            navigationService.NavigateBack();

            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(FakeViewModel));
        }

        [Fact]
        public void NavigateBack_NavigateStackOverflow_NotNavigateToFirstVM()
        {
            //Arrange
            NavigationService navigationService =
                _serviceProvider.GetRequiredService<NavigationService>();

            NavStore navStore = _serviceProvider.GetRequiredService<NavStore>();

            //Act
            navigationService.Navigate<FakeViewModel>();

            navigationService.Navigate<FakeViewModel2>();

            for (int i = 0; i < 20; i++)
            {
                navigationService.Navigate<ViewModelBase>();
            }

            //Act & Assert
            for (int i = 0; i < 19; i++)
            {
                navigationService.NavigateBack();
                Assert.True(navStore.CurrentViewModel!.GetType() == typeof(ViewModelBase));
            }

            navigationService.NavigateBack();

            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(FakeViewModel2));

            navigationService.NavigateBack();

            Assert.True(navStore.CurrentViewModel!.GetType() == typeof(FakeViewModel2));
        }
    }
}
