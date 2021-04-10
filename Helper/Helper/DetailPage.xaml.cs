using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Firebase.Database;
using Firebase.Database.Query;
using System.Reflection;
using PCLStorage;

namespace Helper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        FirebaseClient firebase = new FirebaseClient("https://helper-306821-default-rtdb.firebaseio.com/");
        string gid;
        public DetailPage(string id,String owned = "no")
        {
            InitializeComponent();
            gid = id;
            if (owned.Equals("yes"))
            {
                buttons.IsVisible = true;
            }

            GetDetails(id);
        }

        async Task GetDetails(String id)
        {
          
            var job = (await firebase
                        .Child("jobs")
                        .OnceAsync<Job>()).Where(a => a.Key == id).FirstOrDefault();
            fullname.Text = job.Object.fullname;
            phone.Text = job.Object.phone;
            title.Text = job.Object.title;
            desc.Text = job.Object.desc;
            image.Source = ImageSource.FromUri(new Uri(job.Object.img_url));
            
            Console.WriteLine($"ITEM: {job}");
            
        }
        
        public void DeleteJob(object sender, EventArgs args)
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
                string new_ids = "";
                foreach (string i in id)
                {
                    if (!i.Equals(gid))
                    {
                        new_ids += i + ";";
                    }
                }
                file.Result.WriteAllTextAsync(new_ids);

                DeleteJob();
            }
        }
        async Task DeleteJob()
        {
            await firebase
            .Child("jobs")
            .Child(gid)
            .DeleteAsync();

            await Navigation.PopAsync();
        }
    }
}