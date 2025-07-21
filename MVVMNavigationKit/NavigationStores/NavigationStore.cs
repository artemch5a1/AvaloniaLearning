using MvvmNavigationKit.Abstractions;
using MvvmNavigationKit.Abstractions.ViewModelBase;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmNavigationKit.NavigationStores
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
