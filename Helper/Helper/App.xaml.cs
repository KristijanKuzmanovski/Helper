using Plugin.LocalNotification;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helper
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;
            MainPage = new SideMenuFlyoutPage();
        }

        private void OnLocalNotificationTapped(NotificationTappedEventArgs e)
        {
            Console.WriteLine("TAPPED");
            MainPage = new SideMenuFlyoutPage();
        }
        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
