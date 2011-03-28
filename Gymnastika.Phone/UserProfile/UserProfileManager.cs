using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
namespace Gymnastika.Phone.UserProfile
{
    public class UserProfileManager
    {
        static UserProfileManager()
        {
            ActiveProfile = null;
        }

        /// <summary>
        /// Current User's profile
        /// </summary>
        public static Profile ActiveProfile { get; set; }
        /// <summary>
        /// Delete a stored profile.
        /// </summary>
        /// <param name="profile">Profile to delete.</param>
        public static void DeleteStoredPofile(Profile profile)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.DirectoryExists(Config.StoredProfileFolder))
                {
                    if (storage.DirectoryExists(string.Format(Config.StoredProfileFolder + "\\" + profile.Username)))
                    {
                        DeleteDirectory(Config.StoredProfileFolder + "\\" + profile.Username);
                    }
                }
            }
        }
        private static void DeleteDirectory(string path)
        {
            while (path.EndsWith("\\"))
                path = path.Substring(0, path.Length - 1);
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                foreach (string subPath in storage.GetDirectoryNames(path+"\\"))
                {
                    DeleteDirectory(path+"\\"+ subPath);
                }
                foreach (string file in storage.GetFileNames(path+"\\"))
                {
                    storage.DeleteFile(path + "\\" + file);
                }
                storage.DeleteDirectory(path);
            }

        }
        /// <summary>
        /// Get all profile stored.
        /// </summary>
        /// <returns>Array Of Profiles loaded</returns>
        public static Profile[] GetAllStoredProfiles()
        {
            using (IsolatedStorageFile Storage
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                List<Profile> Profiles = new List<Profile>();
                if (!Storage.DirectoryExists(Config.StoredProfileFolder))
                    return new Profile[0];
                string[] UserNames = Storage.GetDirectoryNames(Config.StoredProfileFolder + "\\");
                foreach (string Username in UserNames)
                {
                    try
                    {
                        Profile p = GetStoredProfile(Username);
                        if (p != null)
                            Profiles.Add(p);
                    }
                    catch
                    {
                        continue;
                    }
                }
                return Profiles.ToArray();
            }
        }
        /// <summary>
        /// Load a stored profile.
        /// </summary>
        /// <param name="Username">Username of the profile to be loaded.</param>
        /// <returns>Profile loaded.</returns>
        public static Profile GetStoredProfile(string Username)
        {
            using (IsolatedStorageFile Storage
                = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (Storage.FileExists(string.Format(Config.StoredProfileFilename,Username)))
                {
                    using (BinaryReader Reader = new BinaryReader(
                        Storage.OpenFile(string.Format(
                        Config.StoredProfileFilename,
                        Username)
                        , FileMode.Open)))
                    {
                        char[] sign = Reader.ReadChars(2);
                        if (sign[0] == 'S' && sign[1] == 'P') //magic number
                        {
                            if (Reader.ReadByte() == 0) // version
                            {
                                return
                                    new Profile(Reader.ReadString(),
                                        Reader.ReadString())
                                {
                                    AutoLogin = Reader.ReadBoolean(),
                                    Gender = (Gender)Reader.ReadByte(),
                                    Height = Reader.ReadDouble(),
                                    Weight = Reader.ReadDouble(),
                                    UserId=Reader.ReadString(),
                                    Icon=GetUserIcon(Username)
                                };
                            }
                            else
                            {
                                throw new Exception("Profile version not supported.");
                            }
                        }
                        else
                            throw new Exception("Bad file.");
                    }
                }
                return null;
            }

        }
        /// <summary>
        /// Sotre a user's profile
        /// </summary>
        /// <param name="Profile">Profile to sotre</param>
        public static void StoreProfice(Profile Profile)
        {
            using (IsolatedStorageFile Storage
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!Storage.DirectoryExists(Config.StoredProfileFolder + "\\" + Profile.Username))
                    Storage.CreateDirectory(Config.StoredProfileFolder + "\\" + Profile.Username);
                using (BinaryWriter Writer = new BinaryWriter(
                    Storage.OpenFile(string.Format(
                    Config.StoredProfileFilename,
                    Profile.Username)
                    , FileMode.Create)))
                {
                    Writer.Write('S');
                    Writer.Write('P');
                    Writer.Write((byte)0);
                    Writer.Write(Profile.Username);
                    Writer.Write(Profile.Password);
                    Writer.Write(Profile.AutoLogin);
                    Writer.Write((byte)Profile.Gender);
                    Writer.Write(Profile.Height);
                    Writer.Write(Profile.Weight);
                    Writer.Write(Profile.UserId);
                    if (Profile.Icon is BitmapSource)
                    {
                        StoreUserIcon(Profile.Username, Profile.Icon as BitmapSource);
                    }
                }
            }
        }
        /// <summary>
        /// Sotre User's Icon
        /// </summary>
        /// <param name="Username">Username</param>
        /// <param name="Source">User's Icon</param>
        public static void StoreUserIcon(string Username, BitmapSource Source)
        {
            WriteableBitmap bitmap = new WriteableBitmap(Source);
            using (IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream Stream = Storage.OpenFile(string.Format(
                    Config.StoredProfileIconFolder, Username), FileMode.Create))
                {
                    Extensions.SaveJpeg(bitmap,
                        Stream, bitmap.PixelWidth, bitmap.PixelHeight,
                        0, 100);
                }
            }
        }
        /// <summary>
        /// Load a user's icon.
        /// </summary>
        /// <param name="Username">Username</param>
        /// <returns>User's icon loaded</returns>
        public static ImageSource GetUserIcon(string Username)
        {
            using (IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    using (IsolatedStorageFileStream Stream = Storage.OpenFile(string.Format(
                        Config.StoredProfileIconFilename, Username), FileMode.Create))
                    {
                        byte[] Data = new byte[Stream.Length];
                        Stream.Read(Data, 0, Data.Length);
                        Stream.Close();
                        BitmapImage image = new BitmapImage();
                        image.SetSource(new MemoryStream(Data));
                        return image;
                    }
                }
                catch
                {
                    BitmapImage defaultIcon = new BitmapImage();
                    defaultIcon.SetSource(App.GetResourceStream(new Uri("/Gymnastika.Phone;component/Avatar3.jpg", UriKind.Relative)).Stream);
                    return defaultIcon;
                }
            }
        }
    }
}
