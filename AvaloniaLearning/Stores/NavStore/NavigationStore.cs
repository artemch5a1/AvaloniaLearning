using System.ComponentModel;
using System.Runtime.CompilerServices;
using AvaloniaApp.Services.NavService.Absract;
using AvaloniaApp.ViewModel;

namespace AvaloniaApp.Stores.NavStore
{
    public class NavigationStore : INavigationStore
    {
        private ViewModelTemplate? _currentViewModel;

        public ViewModelTemplate? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
