using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace UrbanUpriseSignUp
{
    public partial class ProfilePage : ContentPage
    {
        private readonly string profileFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profile.json");

        public Profile Profile { get; set; }

        public ObservableCollection<Profile> LoadedProfiles { get; set; } = new ObservableCollection<Profile>();

        public ProfilePage()
        {
            InitializeComponent();
            Profile = Profile.LoadProfile(profileFilePath) ?? new Profile();
            LoadedProfiles = new ObservableCollection<Profile>();
            LoadProfiles();
            BindingContext = this;
        }

        private void SaveProfile()
        {
            Profile.SaveProfile(profileFilePath,Profile);
        }

        private void OnSaveProfileClicked(object sender, EventArgs e)
        {
            SaveProfile();
        }

        private void OnLoadProfileClicked(object sender, EventArgs e)
        {
            Profile loadedProfile = Profile.LoadProfile(profileFilePath) ?? new Profile();

            
            if (!LoadedProfiles.Any(p => p.Name == loadedProfile.Name && p.Surname == loadedProfile.Surname))
            {
                LoadedProfiles.Add(loadedProfile);
            }

            BindingContext = this;
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please select a photo"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var fileName = $"profileimage_{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

                using (var fileStream = File.OpenWrite(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                Profile.PicturePath = filePath;
                OnPropertyChanged(nameof(Profile));

                ((ImageButton)sender).Source = ImageSource.FromFile(filePath);
            }
        }

        private void LoadProfiles()
        {
            try
            {
                var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var files = Directory.GetFiles(folderPath, "*.json");
                foreach (var file in files)
                {
                    var json = File.ReadAllText(file);
                    var profile = Profile.FromJson(json);
                    LoadedProfiles.Add(profile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load profiles: {ex.Message}");
            }
        }
    }
}

