using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Possible.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ListItens_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListItens.SelectedItem = null;
        }
    }
}