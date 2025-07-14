using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaLearning.NavigationStore;
using AvaloniaLearning.NavService;
using AvaloniaLearning.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace AvaloniaLearning
{
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<NavStore>();

            services.AddSingleton<NavigationService>();

            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<MainPageViewModel>();

            services.AddTransient<StartPageViewModel>();

            services.UseMicrosoftDependencyResolver();

            _serviceProvider = services.BuildServiceProvider();

            NavStore navStore = new NavStore();

            navStore.Navigate<StartPageViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                desktop.MainWindow.DataContext = new MainWindowViewModel(navStore);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
