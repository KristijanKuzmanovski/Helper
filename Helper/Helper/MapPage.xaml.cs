using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Firebase.Database;
using Firebase.Database.Query;
using System.Reactive.Linq;
using Plugin.LocalNotification;
using Xamarin.Essentials;

namespace Helper
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            
            InitializeComponent();
            map.MoveToRegion(new MapSpan(new Position(41.99646, 21.43141), 0.1, 0.1));
            

            var firebase = new FirebaseClient("https://helper-306821-default-rtdb.firebaseio.com/");
            Compass.ReadingChanged += Compass_ReadingChanged;
            var observable = firebase
              .Child("jobs")
              .AsObservable<Job>()
              .Subscribe(d => {
                  CustomPin pin = new CustomPin();
                  pin.Label = d.Object.fullname;
                  pin.Type = PinType.Place;
                  pin.Address = d.Object.title;
                  pin.Position = new Position(d.Object.lat, d.Object.lon);
                  pin.id = d.Key;

                  pin.InfoWindowClicked += async (s, args) =>
                  {
                      Navigation.PushAsync(new DetailPage(((CustomPin)s).id));
                  };

                  map.Pins.Add(pin);

                  var notification = new NotificationRequest
                  {
                      NotificationId = 100,
                      Title = "New Location Added",
                      Description = "Check it out",
                  };
                  NotificationCenter.Current.Show(notification);

                  Console.WriteLine($"ITEM: {d.Object}");
              });
        }
        protected override void OnAppearing()
        {
            if (!Compass.IsMonitoring)
            {
                Compass.Start(SensorSpeed.UI, true);
            }
        }
        protected override void OnDisappearing()
        {
            if (Compass.IsMonitoring)
            {
                Compass.Stop();
            }
        }
        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var data = (int)e.Reading.HeadingMagneticNorth;
           // Console.WriteLine($"Reading: {data} degrees");
            if(data < 45 || data > 315)
            {
                compassLabel.Text = "Compass: N";
            }

            if (data > 45 && data < 125)
            {
                compassLabel.Text = "Compass: E";
            }

            if (data > 125 && data < 225)
            {
                compassLabel.Text = "Compass: S";
            }

            if (data > 225 && data < 315)
            {
                compassLabel.Text = "Compass: W";
            }
        }
    }
}
