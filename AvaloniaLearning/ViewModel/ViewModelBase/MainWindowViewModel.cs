using System;
using System.ComponentModel;
using AvaloniaApp.Services.NavService.Absract;
using AvaloniaApp.Stores.NavStore;

namespace AvaloniaApp.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ViewModelTemplate? CurrentViewModel => _navStore.CurrentViewModel;

        private readonly INavigationStore _navStore;

        public MainWindowViewModel(INavigationStore navStore)
        {
            _navStore = navStore;
            _navStore.PropertyChanged += OnViewModelChanged;
        }

        private void OnViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_navStore.CurrentViewModel))
            {
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            _navStore.PropertyChanged -= OnViewModelChanged;
            CurrentViewModel?.Dispose();
            base.Dispose();
        }
    }
}
