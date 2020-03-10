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
using Xamarin.Forms;

namespace Possible.ViewModels
{
    public class CreateItemViewModel : ViewModelBase
    {

        public DelegateCommand CreateItemCommand => new DelegateCommand(CreateItem);

        private String _description;

        public String Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }

        }

        public CreateItemViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {

        }

        public async void CreateItem()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading...");
                Item item = new Item() { Description = this.Description, UserID = Preferences.Get("LoggedUserID", 0) };
                bool useWCF = Preferences.Get("UseWCF", false);
                if (!useWCF)
                {
                    var resp = await App.SQLiteDb.SaveItemAsync(item);
                    if (resp == 1)
                    {
                        MessagingCenter.Send(this, "ItemCreated", item);
                        Description = "";
                        await DialogService.DisplayAlertAsync("Succes", "Item saved", "OK");
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
                  
                    var json = JsonConvert.SerializeObject(item);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("SaveItem", content))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var respStr = await response.Content.ReadAsStringAsync();
                                if (String.IsNullOrEmpty(respStr))
                                    await DialogService.DisplayAlertAsync("Warning", "Connection Failed", "OK");
                                else
                                {
                                    item = JsonConvert.DeserializeObject<Item>(respStr);
                                    MessagingCenter.Send(this, "ItemCreated", item);
                                    Description = "";
                                    await DialogService.DisplayAlertAsync("Succes", "Item saved", "OK");
                                }
                            }
                            else
                            {
                                await DialogService.DisplayAlertAsync("Warning", "Connection Failed", "OK");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
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
