using Possible.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var resp = await App.SQLiteDb.SaveUserAsync(user);
            if (resp == 1)
            {
                await DialogService.DisplayAlertAsync("Aviso", "Usuário inserido", "OK");
                await NavigationService.NavigateAsync("//NavigationPage/Login", null, true);
            }
            else
                await DialogService.DisplayAlertAsync("Aviso", "Falha ao inserir usuário", "OK");
        }
    }
}
