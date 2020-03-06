using Prism;
using Prism.Ioc;
using Possible.ViewModels;
using Possible.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Possible.Resources;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Possible
{
    public partial class App
    {

        const int smallWightResolution = 768;
        const int smallHeightResolution = 1280;
     
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        void LoadStyles()
        {
            if (IsASmallDevice())
            {
                dictionary.MergedDictionaries.Add(SmallDevicesStyle.SharedInstance);
            }
            else
            {
                dictionary.MergedDictionaries.Add(GeneralDevicesStyle.SharedInstance);
            }
        }

        public static bool IsASmallDevice()
        {
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            return (width <= smallWightResolution && height <= smallHeightResolution);
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            LoadStyles();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
        }
    }
}
