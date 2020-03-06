using Xamarin.Forms;

namespace Possible.Resources
{
    public partial class GeneralDevicesStyle : ResourceDictionary
    {

        public static GeneralDevicesStyle SharedInstance { get; } = new GeneralDevicesStyle();
        public GeneralDevicesStyle()
        {
            InitializeComponent();
        }
    }
}
