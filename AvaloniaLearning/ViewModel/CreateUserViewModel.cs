using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApp.Models;
using AvaloniaApp.ServiceAbstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
            if (ValidateUser())
            {
                TryCreateUser();
            }
            else
            {
                Error = "Есть не заполненные поля";
            }
        }

        [RelayCommand]
        private void NavToBack()
        {
            _navigationService.CloseOverlay();
        }

        private bool ValidateUser()
        {
            if(User.Name == string.Empty || User.Email == string.Empty || User.Surname == string.Empty)
            {
                return false;
            }
            return true;
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
