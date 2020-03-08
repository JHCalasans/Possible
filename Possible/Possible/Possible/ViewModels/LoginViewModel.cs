using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Possible.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        public DelegateCommand GoToUserCommand => new DelegateCommand(GoToUser);

        public DelegateCommand LoginCommand => new DelegateCommand(Login);
        public LoginViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {

        }

        private async void GoToUser()
        {
            await NavigationService.NavigateAsync("CreateUser");
        }

        private async void Login()
        {
            await NavigationService.NavigateAsync("CreateUser");
        }
    }
}
