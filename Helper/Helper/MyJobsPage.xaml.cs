using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PCLStorage;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using System.IO;

namespace Helper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyJobsPage : ContentPage
    {
        FirebaseClient firebase = new FirebaseClient("https://helper-306821-default-rtdb.firebaseio.com/");
        ObservableCollection<Job> jobsList = new ObservableCollection<Job>();
        bool isLoaded;
        public MyJobsPage()
        {
            InitializeComponent();
            list.ItemsSource = jobsList;

            list.RefreshCommand = new Command(() => {
                jobsList = new ObservableCollection<Job>();
                list.ItemsSource = jobsList;
                GetDetails();
                list.IsRefreshing = false;
            });
        }
        async Task GetDetails()
        {
            IFolder root = PCLStorage.FileSystem.Current.LocalStorage;

            var check = root.CheckExistsAsync("jobs.txt");

            if (check.Result == ExistenceCheckResult.NotFound)
            {
                Console.WriteLine($"Check: {check.Result}");
            }
            else
            {
                var file = root.GetFileAsync("jobs.txt");
                string ids = (file.Result.ReadAllTextAsync()).Result;
                string[] id = ids.Split(';');

                var jobs = await firebase
                       .Child("jobs")
                       .OnceAsync<Job>();

                foreach (var job in jobs)
                {
                    foreach(string i in id)
                    {
                        if (i.Equals(job.Key))
                        {
                            Job newJob = job.Object;
                            newJob.id = job.Key;
                            jobsList.Add(newJob);
                            Console.WriteLine($"ITEM: {job.Key}");
                        }
                    }
                    
                }
            }
        }

       

        protected async override void OnAppearing()
        {
      
                jobsList = new ObservableCollection<Job>();
                list.ItemsSource = jobsList;
                await GetDetails();        
        }
        private async void JobItemTapped(object sender, ItemTappedEventArgs e)
        {
            list.SelectedItem = null;
            Navigation.PushAsync(new DetailPage(((Job)e.Item).id, "yes"));
        }
        public void AddButton_Clicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AddJobPage());
        }
    }
}