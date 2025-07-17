using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApp.DataServices;
using AvaloniaApp.Models;
using AvaloniaApp.NavigationStore;
using AvaloniaApp.NavService;
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
        private List<User> _users = new();

        private string _errorText = string.Empty;

        public MainPageViewModel(INavigationService navService, IUserService userService)
        {
            _navService = navService;
            _userService = userService;
            LoadUsers();
        }

        private void LoadUsers()
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
        }

        [RelayCommand]
        private void NavToBack() => _navService.Navigate<StartPageViewModel>();

        [RelayCommand]
        private void NavToEditUser(int id)
        {
            _navService.Navigate<EditPageViewModel, int>(id);
        }

        [RelayCommand]
        private void NavToAddUser()
        {

        }
    }
}
