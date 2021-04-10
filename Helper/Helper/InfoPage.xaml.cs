using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Helper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        
        public InfoPage()
        {
            InitializeComponent();

           InfoLabel.Text = "This app is meant to be used as an aid to the elderly and the sick by providing humanitarian help by volunteers.\n\n" +
                "If you are a volunteer who wants to help others you can use the Map in the app to find peaplo that need help. Tap on the pin to see what the pearson needs and to see thei contact information\n\n"
                + "If you are a person in need then go to \"My Jobs\" to add a job to the Map.";
        }
    }
}