using AvaloniaLearning.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaLearning.ViewModel
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
