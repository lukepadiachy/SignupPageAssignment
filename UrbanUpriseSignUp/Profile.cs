using System;
using System.IO;
using System.Text.Json;

namespace UrbanUpriseSignUp
{
    public class Profile
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string PicturePath { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static void SaveProfile(string filePath, Profile profile)
        {
            try
            {
                var json = profile.ToJson();
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save profile: {ex.Message}");
            }
        }

        public static Profile LoadProfile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    return FromJson(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load profile: {ex.Message}");
            }
            return null;
        }

        public static Profile FromJson(string json)
        {
            return JsonSerializer.Deserialize<Profile>(json);
        }
    }
}

