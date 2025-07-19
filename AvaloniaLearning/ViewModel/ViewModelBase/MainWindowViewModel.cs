using System.ComponentModel;
using AvaloniaApp.NavigationStore;

namespace AvaloniaApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase? CurrentViewModel => _navStore.CurrentViewModel;

        private readonly NavStore _navStore;

        public MainWindowViewModel(NavStore navStore)
        {
            _navStore = navStore;
            _navStore.PropertyChanged += OnViewModelChanged;
        }

        private void OnViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
