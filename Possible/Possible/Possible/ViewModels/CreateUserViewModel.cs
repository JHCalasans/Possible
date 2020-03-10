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
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Possible.ViewModels
{
    public class CreateUserViewModel : ViewModelBase
    {
        public DelegateCommand CreateUserCommand => new DelegateCommand(CreateUser);

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
        public CreateUserViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {

        }

        public async void CreateUser()
        {
            User user = new User() { Name = this.Name, Password = this.Password };

            try
            {
                UserDialogs.Instance.ShowLoading("Loading...");
                bool useWCF = Preferences.Get("UseWCF", false);
                if (!useWCF) {
                    var resp = await App.SQLiteDb.SaveUserAsync(user);
                    if (resp == 1)
                    {
                        await DialogService.DisplayAlertAsync("Success", "User saved", "OK");
                    }
                    else
                        await DialogService.DisplayAlertAsync("Warning", "Something went wrong", "OK");
                }
                else
                {
                    var client = new HttpClient
                    {
                        Timeout = TimeSpan.FromMilliseconds(15000),
                        BaseAddress = new Uri(GetUrlBase())
                    };

                    var json = JsonConvert.SerializeObject(user);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("SaveUser", content))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var respStr = await response.Content.ReadAsStringAsync();
                                await DialogService.DisplayAlertAsync("Success", "User saved", "OK");
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
    }
}
