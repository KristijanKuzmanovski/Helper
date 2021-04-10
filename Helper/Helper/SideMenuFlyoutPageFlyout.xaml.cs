using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenuFlyoutPageFlyout : ContentPage
    {
        public ListView ListView;

        public SideMenuFlyoutPageFlyout()
        {
            InitializeComponent();

            BindingContext = new SideMenuFlyoutPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class SideMenuFlyoutPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<SideMenuFlyoutPageFlyoutMenuItem> MenuItems { get; set; }

            public SideMenuFlyoutPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<SideMenuFlyoutPageFlyoutMenuItem>(new[]
                {
                    new SideMenuFlyoutPageFlyoutMenuItem { Id = 0, Title = "Map", TargetType = typeof(MainPage), icon = "map.png" },
                    new SideMenuFlyoutPageFlyoutMenuItem { Id = 1, Title = "My Jobs", TargetType = typeof(MyJobsPage),icon = "job.png" },
                    new SideMenuFlyoutPageFlyoutMenuItem { Id = 2, Title = "Info", TargetType = typeof(InfoPage), icon = "info_black.png" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}