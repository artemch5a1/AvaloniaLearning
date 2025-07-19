using System.ComponentModel;
using AvaloniaApp.ViewModel;

namespace AvaloniaApp.NavigationStore
{
    public class NavStore : INotifyPropertyChanged
    {
        private ViewModelBase? _currentViewModel;

        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs("PropertyName"));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
