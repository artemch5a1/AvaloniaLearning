using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaLearning.NavigationStore;
using AvaloniaLearning.ViewModel;

namespace AvaloniaLearning
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
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
