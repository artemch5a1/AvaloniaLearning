using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApp.DataServices;
using AvaloniaApp.NavStore;
using AvaloniaApp.NavService;
using AvaloniaApp.ServiceAbstractions;
using AvaloniaApp.View.Base;
using AvaloniaApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace AvaloniaApp
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
            ConfigureOtherSevice(services);
        }

        private void ConfigureViewModelServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<MainPageViewModel>();

            services.AddTransient<StartPageViewModel>();

            services.AddTransient<EditPageViewModel>();
        }

        private void ConfigureNavigationServices(IServiceCollection services)
        {
            services.AddSingleton<NavigationStore>();

            services.Configure<NavigationOptions>(options => { });

            services.AddSingleton<INavigationService, NavigationService>();
        }

        private void ConfigureOtherSevice(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }
    }
}
