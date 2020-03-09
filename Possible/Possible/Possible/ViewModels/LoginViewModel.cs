using Acr.UserDialogs;
using Possible.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace Possible.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        public DelegateCommand GoToUserCommand => new DelegateCommand(GoToUser);

        public DelegateCommand LoginCommand => new DelegateCommand(Login);

        private String _name;

        public String Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }

        }

        private String _password;

        public String Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }

        }

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
            try
            {
                UserDialogs.Instance.ShowLoading("Loading...");
                User user = await App.SQLiteDb.GetUserAsync(Name, Password);
                if (user == null)
                    await DialogService.DisplayAlertAsync("Aviso", "Falha ao inserir usuário", "OK");
                else
                {
                    Preferences.Set("LoggedUserID", user.UserID);
                    Preferences.Set("LoggedUserName", user.Name);
                   
                    await NavigationService.NavigateAsync("//NavigationPage/MainPage");
                }
            }catch(Exception e)
            {
                await DialogService.DisplayAlertAsync("Aviso", "Falha ao inserir usuário", "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
