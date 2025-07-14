using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.ViewModel;

namespace AvaloniaLearning.NavigationStore
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
