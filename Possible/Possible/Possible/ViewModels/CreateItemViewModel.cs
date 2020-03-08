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
            Item item = new Item() {Description = this.Description, UserID = Preferences.Get("LoggedUserID",0) };
            var resp = await App.SQLiteDb.SaveItemAsync(item);
            if (resp == 1)
            {
                await DialogService.DisplayAlertAsync("Aviso", "Item inserido", "OK");
            }
            else
                await DialogService.DisplayAlertAsync("Aviso", "Falha ao inserir item", "OK");
        }
    }
}
