using Microsoft.Maui.LifecycleEvents;

namespace ContactInfoApp.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}