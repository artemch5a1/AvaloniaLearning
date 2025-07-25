using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaApp.ServiceAbstractions;
using AvaloniaApp.Services.DataServices;
using AvaloniaApp.View.Base;
using AvaloniaApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmNavigationKit.Abstractions;
using MvvmNavigationKit.Abstractions.ViewModelBase;
using MVVMNavigationKit.ServiceBuild;
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
            ConfigureLoggerService(services);
        }

        private void ConfigureLoggerService(IServiceCollection services)
        {
            services
                .AddLogging(config =>
                {
                    config.AddDebug();
                    config.AddFile("log.txt");
                    config.SetMinimumLevel(LogLevel.Information);
                })
                .UseMicrosoftDependencyResolver();
        }

        private void ConfigureViewModelServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<MainPageViewModel>();

            services.AddTransient<StartPageViewModel>();

            services.AddTransient<EditPageViewModel>();

            services.AddTransient<CreateUserViewModel>();

            services.AddTransient<ConfirmViewModel>();

            services.AddTransient<ShowUserViewModel>();
        }

        private void ConfigureNavigationServices(IServiceCollection services)
        {
            NavigationServicesHelper.CreateServiceCollections(services);
        }

        private void ConfigureOtherSevice(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }
    }
}
