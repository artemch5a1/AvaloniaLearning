using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AvaloniaApp.Models;
using AvaloniaApp.ServiceAbstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApp.ViewModel
{
    internal partial class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private ViewModelBase? _currentOverlayViewModel;

        [ObservableProperty]
        private bool _setOvetlay = false;

        [ObservableProperty]
        private List<User> _users = new();

        private string _errorText = string.Empty;

        public MainPageViewModel(INavigationService navService, IUserService userService)
        {
            _navService = navService;
            _userService = userService;
            _ = LoadUsers();
        }

        public override void RefreshPage()
        {
            _ = LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                Users = _userService.GetAllUsers();
            }
            catch (Exception ex)
            {
                _errorText = ex.Message;
                Debug.WriteLine(_errorText);
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        private void NavToBack() => _navService.DestroyAndNavigate<StartPageViewModel>();

        [RelayCommand]
        private void NavToEditUser(int id)
        {
            _navService.Navigate<EditPageViewModel, int>(id);
        }

        [RelayCommand]
        private void NavToAddUser() 
        {
            SetOvetlay = true;
            _navService.NavigateOverlay<CreateUserViewModel>(overlayAction: vm =>
            {
                CurrentOverlayViewModel = vm;
            },
            onClose:() => {
                _ = LoadUsers();
                SetOvetlay = false;
            });
        }

        [RelayCommand]
        private void DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            RefreshPage();
        }
    }
}
