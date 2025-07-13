using AvaloniaLearning.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaLearning.Navigation
{
    public class NavStore : INotifyPropertyChanged
    {
        private ViewModelBase? _currentViewModel;

        public ViewModelBase? CurrentViewModel { get => _currentViewModel; set
            {
                _currentViewModel = value;
                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs("PropertyName"));
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Navigate<TViewModel>() where TViewModel : ViewModelBase
        {
            Type viewModelType = typeof(TViewModel);

            ViewModelBase? viewModel = (ViewModelBase?)Activator.CreateInstance(viewModelType, this);

            CurrentViewModel = viewModel;
        }
    }
}
