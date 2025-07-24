using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationKit.Abstractions;

namespace AvaloniaApp.ViewModel
{
    public partial class ConfirmViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _title = "Подтвердить действие?";

        public ConfirmViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public event Action? ConfrimAction;

        [RelayCommand]
        private void Confirm()
        {
            ConfrimAction?.Invoke();
            CloseOverlay();
        }

        [RelayCommand]
        private void CloseOverlay()
        {
            _navigationService.CloseOverlay();
        }
    }
}
