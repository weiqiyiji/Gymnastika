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
        /// Get all profile stored.
        /// </summary>
        /// <returns>Array Of Profiles loaded</returns>
        public static Profile[] GetAllStoredProfiles()
        {
            using (IsolatedStorageFile Storage
                = IsolatedStorageFile.GetUserStoreForApplication())
            {
                List<Profile> Profiles = new List<Profile>();
                string[] UserNames = Storage.GetDirectoryNames(Config.StoredProfileFolder);
                foreach (string Username in UserNames)
                {
                    try
                    {
                        Profiles.Add(GetStoredProfile(Username));
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
                =IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (Storage.FileExists(Config.StoredProfileFilename))
                {
                    using (BinaryReader Reader = new BinaryReader(
                        File.Open(string.Format(
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
                                    Gender=(Gender)Reader.ReadByte(),
                                    Height=Reader.ReadDouble(),
                                    Weight=Reader.ReadDouble()
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
                    using (BinaryWriter Writer = new BinaryWriter(
                        File.Open(string.Format(
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
        public static BitmapImage GetUserIcon(string Username)
        {
            using (IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream Stream = Storage.OpenFile(string.Format(
                    Config.StoredProfileIconFilename, Username), FileMode.Create))
                {
                    byte[] Data = new byte[Stream.Length];
                    Stream.Read(Data, 0, Data.Length);
                    BitmapImage image = new BitmapImage();
                    image.SetSource(new MemoryStream(Data));
                    return image;
                }
            }
        }
    }
}
