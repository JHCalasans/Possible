using Acr.UserDialogs;
using Newtonsoft.Json;
using Possible.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace Possible.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        public DelegateCommand GoToUserCommand => new DelegateCommand(GoToUser);

        public DelegateCommand LoginCommand => new DelegateCommand(Login);

        public DelegateCommand SwitchCommand => new DelegateCommand(SwitchChanged);

        private String _name;

        public String Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }

        }

        private Boolean _useWCF;

        public Boolean UseWCF
        {
            get { return _useWCF; }
            set { SetProperty(ref _useWCF, value); }

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
            if (!Preferences.ContainsKey("UseWCF"))
            {
                UseWCF = false;
                Preferences.Set("UseWCF", UseWCF);
            }
            
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
                if (!UseWCF)
                {
                    User user = await App.SQLiteDb.GetUserAsync(Name, Password);
                    if (user == null)
                        await DialogService.DisplayAlertAsync("Warning", "User not found", "OK");
                    else
                    {
                        Preferences.Set("LoggedUserID", user.UserID);
                        Preferences.Set("LoggedUserName", user.Name);

                        await NavigationService.NavigateAsync("//NavigationPage/MainPage");
                    }
                }
                else
                {

                    var client = new HttpClient
                    {
                        Timeout = TimeSpan.FromMilliseconds(15000),
                        BaseAddress = new Uri(GetUrlBase())
                    };
                    User user = new User();
                    user.Name = Name;
                    user.Password = Password;
                   var json = JsonConvert.SerializeObject(user);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("Login", content))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var respStr = await response.Content.ReadAsStringAsync();
                                if (String.IsNullOrEmpty(respStr))
                                    await DialogService.DisplayAlertAsync("Warning", "User not found", "OK");
                                else
                                {
                                    user = JsonConvert.DeserializeObject<User>(respStr);
                                    Preferences.Set("LoggedUserID", user.UserID);
                                    Preferences.Set("LoggedUserName", user.Name);
                                    await NavigationService.NavigateAsync("//NavigationPage/MainPage", null, true);
                                }
                            }
                            else
                            {
                                await DialogService.DisplayAlertAsync("Warning", "Connection Failed", "OK");
                            }
                        }
                    }

                }                
            }catch(Exception e)
            {
                await DialogService.DisplayAlertAsync("Warning", "Something went wrong", "OK");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void SwitchChanged()
        {
            Preferences.Set("UseWCF", UseWCF);
        }
    }
}
