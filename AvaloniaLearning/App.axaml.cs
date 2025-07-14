using System;
using System.Xml.Serialization;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaLearning.NavigationStore;
using AvaloniaLearning.NavService;
using AvaloniaLearning.View.Base;
using AvaloniaLearning.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace AvaloniaLearning
{
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        public IServiceProvider ServiceProvider
        {
            get =>
                _serviceProvider ?? throw new InvalidOperationException("Services not initialized");
            set => _serviceProvider = value;
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            InitializeServices();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.DataContext =
                    ServiceProvider.GetRequiredService<MainWindowViewModel>();
            }

            INavigationService navigationService =
                ServiceProvider.GetRequiredService<INavigationService>();

            navigationService.Navigate<StartPageViewModel>();

            base.OnFrameworkInitializationCompleted();
        }

        private void InitializeServices()
        {
            ServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            services.UseMicrosoftDependencyResolver();

            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            ConfigureViewModelServices(services);
            ConfigureNavigationServices(services);
        }

        private void ConfigureViewModelServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<MainPageViewModel>();

            services.AddTransient<StartPageViewModel>();
        }

        private void ConfigureNavigationServices(IServiceCollection services)
        {
            services.AddSingleton<NavStore>();

            services.AddSingleton<INavigationService, NavigationService>();
        }
    }
}
