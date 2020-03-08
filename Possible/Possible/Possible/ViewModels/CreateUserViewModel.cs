using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void CreateUser()
        {

        }
    }
}
