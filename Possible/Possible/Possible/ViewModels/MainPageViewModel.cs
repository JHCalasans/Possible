using Possible.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Possible.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public DelegateCommand NewItemCommand => new DelegateCommand(GoToNewItem);

        private List<Assignment> _assignments;

        public List<Assignment> Assignments
        {
            get { return _assignments; }
            set { SetProperty(ref _assignments, value); }
        }


        private List<List<Assignment>> _assignmentsAgrupados;

        public List<List<Assignment>> _AssignmentsAgrupados
        {
            get { return _assignmentsAgrupados; }
            set { SetProperty(ref _assignmentsAgrupados, value); }
        }


        private List<Item> _itens;

        public List<Item> Itens
        {
            get { return _itens; }
            set { SetProperty(ref _itens, value); }
        }
      

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
          
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
          
                //EstadosAgrupados = Estados.records.GroupBy(r => r.fields.Regiao)
                //    .Select(grp => grp.ToList())
                //    .ToList();            

        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Itens"))
            {
                Itens = (List<Item>)parameters["Itens"];
            }
            //EstadosAgrupados = Estados.records.GroupBy(r => r.fields.Regiao)
            //    .Select(grp => grp.ToList())
            //    .ToList();            

        }

        private async void GoToNewItem()
        {
            await NavigationService.NavigateAsync("CreateItem");
        }

    }
}
