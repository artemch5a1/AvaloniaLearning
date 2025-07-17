using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaLearning.Models;
using AvaloniaLearning.NavService;
using AvaloniaLearning.ServiceAbstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaLearning.ViewModel
{
    public partial class EditPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;

        private int _idUser;

        [ObservableProperty]
        public string _userName = string.Empty;

        [ObservableProperty]
        public string _userSurname = string.Empty;

        [ObservableProperty]
        public string _userEmail = string.Empty;

        public EditPageViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
        }

        protected override void InitializeParams<T>(T @params)
        {
            if(@params is int id)
            {
                _idUser = id;
                LoadUser();
            }
            else
            {
                throw new ArgumentException("Несоответсвие типов");
            }
        }

        private void LoadUser()
        {
            User? user = _userService.GetUserById(_idUser);
            if (user != null) 
            {
                UserName = user.Name;
                UserSurname = user.Surname;
                UserEmail = user.Email;
            }
        }

        [RelayCommand]
        private void UpdateUser()
        {
            User user = new User() 
            {
                Id = _idUser,
                Name = UserName,
                Surname = UserSurname,
                Email = UserEmail,
            };

            bool success = _userService.UpdateUser(user);

            if (success) 
            {
                NavigateBack();
            }
        }

        private void NavigateBack()
        {
            _navigationService.Navigate<MainPageViewModel>();
        }
    }
}
