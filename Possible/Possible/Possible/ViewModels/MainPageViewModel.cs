using Possible.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Possible.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public DelegateCommand<int?> SelectItemCommand =>
         new DelegateCommand<int?>(SelectItem);

        public DelegateCommand NewItemCommand => new DelegateCommand(GoToNewItem);

        private List<Assignment> _assignments;

        public List<Assignment> Assignments
        {
            get { return _assignments; }
            set { SetProperty(ref _assignments, value); }
        }


        private ObservableCollection<ListObject> _assignmentsAgrupados;

        public ObservableCollection<ListObject> AssignmentsAgrupados
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

        public override  void OnNavigatedTo(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<CreateItemViewModel, Item>(this, "ItemCreated");
            MessagingCenter.Subscribe<CreateItemViewModel, Item>(this, "ItemCreated", (sender, args) =>
            {
                ListObject objeto = new ListObject() { ItemID = args.ItemID, ItemDescription = args.Description };
                AssignmentsAgrupados.Add(objeto);

            });

            //if (!parameters.ContainsKey("Itens"))
            //{
            //    Itens = await App.SQLiteDb.GetItensByUserAsync(Preferences.Get("LoggedUserID", 0)); 
            //}
            //EstadosAgrupados = Estados.records.GroupBy(r => r.fields.Regiao)
            //    .Select(grp => grp.ToList())
            //    .ToList();            

        }

        public override void Initialize(INavigationParameters parameters)
        {
           
            AssignmentsAgrupados = new ObservableCollection<ListObject>();
            if (parameters.ContainsKey("Itens"))
            {
                Itens = (List<Item>)parameters["Itens"];
                ListObject listaObj;
                int count = 0;
                foreach(Item element in Itens)
                {
                    listaObj = new ListObject() { ItemDescription = element.Description, ItemID = element.ItemID };
                    listaObj.Add(new Assignment { Title = "teste" + count });
                    count++;
                    AssignmentsAgrupados.Add(listaObj);
                }
            }
            //EstadosAgrupados = Estados.records.GroupBy(r => r.fields.Regiao)
            //    .Select(grp => grp.ToList())
            //    .ToList();            

        }

        private async void GoToNewItem()
        {
            await NavigationService.NavigateAsync("CreateItem");
        }

        private void SelectItem(int? ItemID)
        {

        }

    }
}
