using Possible.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Possible.ViewModels
{
    public class CreateAssignmentViewModel : ViewModelBase
    {
        public DelegateCommand CreateAssignmentCommand => new DelegateCommand(CreateAssignment);

        private Assignment _assignment;

        public Assignment Assignment
        {
            get { return _assignment; }
            set { SetProperty(ref _assignment, value); }

        }

        public CreateAssignmentViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {

        }

        private void CreateAssignment()
        {

        }
    }
}
