using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using PCLStorage;
using System.IO;

namespace Helper
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddJobPage : ContentPage
    {
        
        CancellationTokenSource cts;
        FirebaseClient firebase = new FirebaseClient("https://helper-306821-default-rtdb.firebaseio.com/");
        FirebaseStorage firebaseStorage = new FirebaseStorage("helper-306821.appspot.com");
        string img_url = "none";
        FileResult photo = null;
        public AddJobPage()
        {
            InitializeComponent();
        }
        async void AddJobButton_Clicked(object sender, EventArgs args)
        {
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                name.Placeholder = "Plese enter your name";
                name.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(lastname.Text))
            {
                lastname.Placeholder = "Plese enter your lastname";
                lastname.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(phone.Text))
            {
                phone.Placeholder = "Plese enter your phone";
                phone.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(title.Text))
            {
                title.Placeholder = "Plese describe your request.";
                title.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(job.Text))
            {
                job.Placeholder = "Plese describe your request.";
                job.Focus();
                return;
            }

            spinner.IsRunning = true;
            GetCurrentLocation(name.Text + " " + lastname.Text, phone.Text, job.Text, title.Text);
            
        }
        
        private void AddImageButton_Clicked(object sender, EventArgs args)
        {
            TakePhotoAsync();
        }

        
        private void DeleteImageButton_Clicked(object sender, EventArgs args)
        {
            ImageInfoStack.IsVisible = false;
            photo = null;
        }
        async Task GetCurrentLocation(String fullname, String phone, String job, String title)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                 
                  if (photo != null)
                    {
                        var stream = await photo.OpenReadAsync();

                        // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
                        var task = firebaseStorage
                            .Child("images")
                            .Child(Guid.NewGuid().ToString() + ".png")
                            .PutAsync(stream);

                        // Track progress of the upload
                        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                        // await the task to wait until upload completes and get the download url
                        img_url = await task;
                    }

                    var newJob = await firebase
                       .Child("jobs")
                       .PostAsync(new Job() { fullname = fullname, phone = phone, title = title, desc = job, img_url = img_url, lon = location.Longitude, lat = location.Latitude});


                    IFolder root = PCLStorage.FileSystem.Current.LocalStorage;

                    var check = root.CheckExistsAsync("jobs.txt");

                    if(check.Result == ExistenceCheckResult.NotFound)
                    {
                        var file = root.CreateFileAsync("jobs.txt", CreationCollisionOption.ReplaceExisting);
                        file.Result.WriteAllTextAsync(newJob.Key+";");
                    }
                    else
                    {
                        byte[] str = Encoding.ASCII.GetBytes(newJob.Key+";");
                        var file = root.GetFileAsync("jobs.txt");
                        using (Stream streamToWrite = await file.Result.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                        {
                            streamToWrite.Position = streamToWrite.Length;
                            if (streamToWrite.CanWrite)
                            {
                                await streamToWrite.WriteAsync(str, 0, str.Length);
                            }
                        }

                    }

                    Console.WriteLine($"Check: {check.Result}");
                    Console.WriteLine($"Path: {root.Path}");

                    Console.WriteLine($"Id: {newJob.Key}");
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    spinner.IsRunning = false;
                    
                    await Navigation.PopAsync();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine($"{fnsEx.Message}");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine($"{fneEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine($"{pEx.Message}");
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine($"{ex.Message}");
            }
        }

        async Task TakePhotoAsync()
        {
            try
            {
                photo = await MediaPicker.CapturePhotoAsync();

                ImageInfoStack.IsVisible = true;

                Console.WriteLine($"CapturePhotoAsync COMPLETED: {photo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

 
        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
    }
}