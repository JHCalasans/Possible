using Acr.UserDialogs;
using Newtonsoft.Json;
using Possible.Models;
using Possible.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Possible.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public DelegateCommand<int?> SelectItemCommand =>
         new DelegateCommand<int?>(SelectItem);

        public DelegateCommand PeriodChangedCommand =>
        new DelegateCommand(PeriodChanged);

        public DelegateCommand<int?> RemoveItemCommand =>
        new DelegateCommand<int?>(RemoveItem);

        public DelegateCommand LogOffCommand =>
       new DelegateCommand(LogOff);

        public DelegateCommand<Assignment> RemoveAssignmentCommand =>
     new DelegateCommand<Assignment>(RemoveAssignment);

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


        private List<DatePickerObject> _periods;

        public List<DatePickerObject> Periods
        {
            get { return _periods; }
            set { SetProperty(ref _periods, value); }
        }

        private DatePickerObject _selectedPeriod;

        public DatePickerObject SelectedPeriod
        {
            get { return _selectedPeriod; }
            set { SetProperty(ref _selectedPeriod, value); }

        }


        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<CreateItemViewModel, Item>(this, "ItemCreated");
            MessagingCenter.Subscribe<CreateItemViewModel, Item>(this, "ItemCreated", (sender, args) =>
            {
                ListObject objeto = new ListObject() { ItemID = args.ItemID, ItemDescription = args.Description };
                AssignmentsAgrupados.Add(objeto);
                if (Itens == null)
                    Itens = new List<Item>();
                Itens.Add(args);

            });

            MessagingCenter.Unsubscribe<CreateAssignmentViewModel, Assignment>(this, "AssignmentCreated");
            MessagingCenter.Subscribe<CreateAssignmentViewModel, Assignment>(this, "AssignmentCreated", (sender, args) =>
            {

                ListObject objectList = AssignmentsAgrupados.Where(obj => obj.ItemID == args.ItemID).FirstOrDefault();
                objectList.Add(args);

            });

            //if (!parameters.ContainsKey("Itens"))
            //{
            //    Itens = await App.SQLiteDb.GetItensByUserAsync(Preferences.Get("LoggedUserID", 0)); 
            //}
            //EstadosAgrupados = Estados.records.GroupBy(r => r.fields.Regiao)
            //    .Select(grp => grp.ToList())
            //    .ToList();            

        }

        public override async void Initialize(INavigationParameters parameters)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading...");

                bool useWCF = Preferences.Get("UseWCF", false);
                AssignmentsAgrupados = new ObservableCollection<ListObject>();
                if (!useWCF)
                {
                    Itens = await App.SQLiteDb.GetItensByUserAsync(Preferences.Get("LoggedUserID", 0));
                    //AssignmentsAgrupados = new ObservableCollection<ListObject>();

                    ListObject listaObj;
                    foreach (Item element in Itens)
                    {
                        listaObj = new ListObject() { ItemDescription = element.Description, ItemID = element.ItemID };
                        List<Assignment> ListAssignments = await App.SQLiteDb.GetAssignmentsByItemAsync(element.ItemID);
                        foreach (Assignment assi in ListAssignments)
                        {
                            listaObj.Add(assi);
                        }
                        AssignmentsAgrupados.Add(listaObj);
                    }
                }
                else
                {
                   // AssignmentsAgrupados = new ObservableCollection<ListObject>();
                    var client = new HttpClient
                    {
                        Timeout = TimeSpan.FromMilliseconds(15000),
                        BaseAddress = new Uri(GetUrlBase())
                    };
                    using (var response = await client.GetAsync("GetItensByUser/" + Preferences.Get("LoggedUserID", 0)))
                    {
                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var respStr = await response.Content.ReadAsStringAsync();
                                if (!String.IsNullOrEmpty(respStr))
                                {
                                    Itens = JsonConvert.DeserializeObject<List<Item>>(respStr);
                                    if (Itens.Count > 0)
                                    {
                                        using (var response2 = await client.GetAsync("GetAssignmentsByUser/" + Preferences.Get("LoggedUserID", 0)))
                                        {
                                            if (response2 != null)
                                            {
                                                if (response2.IsSuccessStatusCode)
                                                {
                                                    var respStr2 = await response2.Content.ReadAsStringAsync();
                                                    if (!String.IsNullOrEmpty(respStr2))
                                                    {
                                                        AssignmentsAgrupados = JsonConvert.DeserializeObject<ObservableCollection<ListObject>>(respStr2);
                                                    }
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
                            else
                            {
                                await DialogService.DisplayAlertAsync("Warning", "Connection Failed", "OK");
                            }
                        }
                        else
                        {
                            await DialogService.DisplayAlertAsync("Warning", "Connection Failed", "OK");
                        }
                    }
                }              


                Periods = new List<DatePickerObject>
                        {
                            new DatePickerObject{ Value = 1, Label = "Today"},
                            new DatePickerObject{ Value = 2, Label = "This Week"},
                            new DatePickerObject{ Value = 3, Label = "This Month"},
                            new DatePickerObject{ Value = 4, Label = "Open-Ended"}
                        };
                SelectedPeriod = Periods[0];
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

        private async void GoToNewItem()
        {
            await NavigationService.NavigateAsync("CreateItem");
        }

        private async void PeriodChanged()
        {
            AssignmentsAgrupados.Clear();
            ListObject listaObj;


            if (Itens != null)
            {
                foreach (Item element in Itens)
                {
                    listaObj = new ListObject() { ItemDescription = element.Description, ItemID = element.ItemID };
                    List<Assignment> ListAssignments = await App.SQLiteDb.GetAssignmentsByItemAsync(element.ItemID);
                    foreach (Assignment assi in ListAssignments)
                    {
                        if (SelectedPeriod.Value == 2 && DatesAreInTheSameWeek(assi.AssignmentDate, DateTime.Now))
                            listaObj.Add(assi);
                        else if (SelectedPeriod.Value == 1 && DatesAreInTheSameDay(assi.AssignmentDate, DateTime.Now))
                            listaObj.Add(assi);
                        else if (SelectedPeriod.Value == 3 && assi.AssignmentDate.Month == DateTime.Now.Month && assi.AssignmentDate.Year == DateTime.Now.Year)
                            listaObj.Add(assi);
                        else if (SelectedPeriod.Value == 4 && assi.AssignmentDate.CompareTo(DateTime.Today) >= 0)
                            listaObj.Add(assi);

                    }
                    listaObj.ItemID = element.ItemID;
                    listaObj.ItemDescription = element.Description;
                    AssignmentsAgrupados.Add(listaObj);
                }
            }

        }


        private async void SelectItem(int? ItemID)
        {
            Item item = Itens.Find(it => it.ItemID == ItemID);
            NavigationParameters navParam = new NavigationParameters();
            navParam.Add("Item", item);
            await NavigationService.NavigateAsync("CreateAssignment", navParam);
        }
        private async void LogOff()
        {
            Preferences.Clear();
            Application.Current.MainPage = new NavigationPage(new Login());
        }

        private async void RemoveItem(int? ItemID)
        {
            Item item = Itens.Find(it => it.ItemID == ItemID);
            var resposta = await UserDialogs.Instance.ConfirmAsync("Remove List Item?", item.Description, "Yes", "No");
            if (resposta)
            {
                List<Assignment> ListAssignments = await App.SQLiteDb.GetAssignmentsByItemAsync(item.ItemID);
                foreach (Assignment assi in ListAssignments)
                {
                    await App.SQLiteDb.DeleteAssignmentAsync(assi);
                }
                ListObject obj = AssignmentsAgrupados.Where(assi => assi.ItemID == item.ItemID).FirstOrDefault();
                AssignmentsAgrupados.Remove(obj);
                Itens.Remove(item);
                await App.SQLiteDb.DeleteItemAsync(item);
            }
        }

        private async void RemoveAssignment(Assignment assignment)
        {

            var resposta = await UserDialogs.Instance.ConfirmAsync("Remove Assignment?", assignment.Title, "Yes", "No");
            if (resposta)
            {

               ListObject listaObj =  AssignmentsAgrupados.Where(obj => obj.ItemID == assignment.ItemID).FirstOrDefault();
                listaObj.Remove(assignment);
               // AssignmentsAgrupados.Where(assi => assi.Assignments.Remove(assignment));

                await App.SQLiteDb.DeleteAssignmentAsync(assignment);
            }
        }

        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        }


        private bool DatesAreInTheSameDay(DateTime date1, DateTime date2)
        {


            return date1.Day == date2.Day && date1.Month == date2.Month && date1.Year == date2.Year;
        }
    }
}
