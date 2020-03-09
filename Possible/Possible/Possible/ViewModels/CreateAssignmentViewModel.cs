using Acr.UserDialogs;
using Possible.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

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
        private String _labelItemDescription;

        public String LabelItemDescription
        {
            get { return _labelItemDescription; }
            set { SetProperty(ref _labelItemDescription, value); }

        }
        
        private DateTime _currentDate;

        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }

        }

        private Item _itemObject;

        public Item ItemObject
        {
            get { return _itemObject; }
            set { SetProperty(ref _itemObject, value); }

        }

        private List<ColorPickerObject> _colors;

        public List<ColorPickerObject> Colors
        {
            get { return _colors; }
            set { SetProperty(ref _colors, value); }

        }

        private ColorPickerObject _selectedColor;

        public ColorPickerObject SelectedColor
        {
            get { return _selectedColor; }
            set { SetProperty(ref _selectedColor, value); }

        }

        public CreateAssignmentViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            
        }

        private async void CreateAssignment()
        {
            if (String.IsNullOrEmpty(Assignment.Title))
                await DialogService.DisplayAlertAsync("Warning", "Please insert a title", "OK");
            else if (SelectedColor == null || String.IsNullOrEmpty(SelectedColor.Hexadecimal))
                await DialogService.DisplayAlertAsync("Warning", "Please select a color", "OK");
            else
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Loading...");
                    Assignment.Color = SelectedColor.Hexadecimal;
                    Assignment.ItemID = ItemObject.ItemID;
                    if (Assignment.Date.CompareTo(DateTime.Now) < 0)
                        Assignment.Date = CurrentDate;
                    var resp = await App.SQLiteDb.SaveAssignmentAsync(Assignment);                    
                    MessagingCenter.Send(this, "AssignmentCreated", Assignment);
                    Assignment = new Assignment();
                    await DialogService.DisplayAlertAsync("Succes", "Assignment saved", "OK");
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

        public override void Initialize(INavigationParameters parameters)
        {

            CurrentDate = DateTime.Now;
            Colors = new List<ColorPickerObject>
            {
                new ColorPickerObject{ Hexadecimal = "#FF0000", Label = "Red"},
                new ColorPickerObject{ Hexadecimal = "#00FF00", Label = "Green"},
                new ColorPickerObject{ Hexadecimal = "#0000FF", Label = "Blue"}
            };
            Assignment = new Assignment();
            if (parameters.ContainsKey("Item"))
            {
                ItemObject = (Item)parameters["Item"];
                Assignment.ItemID = ItemObject.ItemID;
                LabelItemDescription =  "Create new assignment for " + ItemObject.Description;
            }
                   

        }
    }
}
