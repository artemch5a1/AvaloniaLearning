using System;
using AvaloniaApp.Models;
using AvaloniaApp.ServiceAbstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmNavigationKit.Abstractions;

namespace AvaloniaApp.ViewModel
{
    public partial class CreateUserViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;

        [ObservableProperty]
        private User _user = new User();

        [ObservableProperty]
        private string _error = string.Empty;

        public CreateUserViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
        }

        [RelayCommand]
        private void CreateUser()
        {
            (bool success, string? error) validRes = ValidateUser();
            if (validRes.success)
            {
                TryCreateUser();
                NavToBack();
            }
            else
            {
                Error = validRes.error ?? string.Empty;
            }
        }

        [RelayCommand]
        private void NavToBack()
        {
            _navigationService.CloseOverlay();
        }

        private (bool, string?) ValidateUser()
        {
            if (
                User.Name == string.Empty
                || User.Email == string.Empty
                || User.Surname == string.Empty
            )
            {
                return (false, "Есть не заполненные поля");
            }
            return (true, null);
        }

        private void TryCreateUser()
        {
            try
            {
                _userService.CreateUser(User);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
    }
}
