using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Services.Session;
using Gymnastika.Services.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using Gymnastika.Data;
using Gymnastika.Services.Contracts;
using Microsoft.Win32;
using System.Windows;
using System.IO;
using Gymnastika.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Gymnastika.Common.Utils;

namespace Gymnastika.ViewModels
{
    public class UserProfileManagementViewModel : NotificationObject
    {
        private readonly ISessionManager _sessionManager;
        private readonly IServiceLocator _serviceLocator;
        private readonly INavigationManager _navigationManager;
        private const int AvatarWidth = 120;
        private const int AvatarHeight = 120;
        private User _user;
        private string _avatarPath;
        private bool _hasPassword;

        public UserProfileManagementViewModel(
            ISessionManager sessionManager, IServiceLocator serviceLocator, INavigationManager navigationManager)
        {
            _sessionManager = sessionManager;
            _serviceLocator = serviceLocator;
            _navigationManager = navigationManager;
            _user = _sessionManager.GetCurrentSession().AssociatedUser;
            AvatarPath = _user.AvatarPath;
            _hasPassword = !string.IsNullOrEmpty(_user.Password);
        }

        public string AvatarPath
        {
            get { return _avatarPath; }
            set
            {
                if (_avatarPath != value)
                {
                    _avatarPath = value;
                    RaisePropertyChanged("AvatarPath");
                }
            }
        }
        
        public string UserName
        {
            get { return _user.UserName; }
            set
            {
                if (_user.UserName != value)
                {
                    _user.UserName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        
        public Gender Gender
        {
            get { return _user.Gender; }
            set
            {
                if (_user.Gender != value)
                {
                    _user.Gender = value;
                    RaisePropertyChanged("Gender");
                }
            }
        }
				
        public string Age
        {
            get
            {
                return _user.Age == 0 ? string.Empty : _user.Age.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                int age = int.Parse(value);
                if (_user.Age != age)
                {
                    _user.Age = age;
                    RaisePropertyChanged("Age");
                }
            }
        }

        public string Height
        {
            get
            {
                return _user.Height == 0 ? string.Empty : _user.Height.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                int height = int.Parse(value);
                if (_user.Height != height)
                {
                    _user.Height = height;
                    RaisePropertyChanged("Height");
                }
            }
        }

        public string Weight
        {
            get
            {
                return _user.Weight == 0 ? string.Empty : _user.Weight.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                int weight = int.Parse(value);
                if (_user.Weight != weight)
                {
                    _user.Weight = weight;
                    RaisePropertyChanged("Weight");
                }
            }
        }
        
        public bool HasPassword
        {
            get { return _hasPassword; }
            set
            {
                if (_hasPassword != value)
                {
                    _hasPassword = value;
                    RaisePropertyChanged("HasPassword");
                }
            }
        }
        
        private string _oldPassword;

        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                if (_oldPassword != value)
                {
                    _oldPassword = value;
                    RaisePropertyChanged("OldPassword");
                }
            }
        }
        
        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    RaisePropertyChanged("NewPassword");
                }
            }
        }
        
        private string _retypedNewPassword;

        public string RetypedNewPassword
        {
            get { return _retypedNewPassword; }
            set
            {
                if (_retypedNewPassword != value)
                {
                    _retypedNewPassword = value;
                    RaisePropertyChanged("RetypedNewPassword");
                }
            }
        }
        
        private ICommand _selectAvatarCommand;

        public ICommand SelectAvatarCommand
        {
            get
            {
                if (_selectAvatarCommand == null)
                    _selectAvatarCommand = new DelegateCommand(SelectAvatar);

                return _selectAvatarCommand;
            }
        }
								
        private ICommand _saveProfileCommand;

        public ICommand SaveProfileCommand
        {
            get
            {
                if (_saveProfileCommand == null)
                    _saveProfileCommand = new DelegateCommand(SaveProfile);

                return _saveProfileCommand;
            }
        }

        private void SelectAvatar()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "图像文件(*.bmp;*.jpg;*.jpeg;*.png)|(*.bmp;*.jpg;*.jpeg;*.png)";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dialog.Multiselect = false;

            if (dialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                AvatarPath = dialog.FileName;
            }
        }

        private const string AvatarFolder = "avatars";

        private void SaveAvatar()
        {
            if (_user.AvatarPath == AvatarPath) return;

            string dir = Directory.GetCurrentDirectory();
            string avatarFolderPath = Path.Combine(dir, AvatarFolder);

            if (!Directory.Exists(avatarFolderPath))
            {
                Directory.CreateDirectory(avatarFolderPath);
            }

            string avatarExtension = Path.GetExtension(AvatarPath).ToLower();
            string saveTargetPath = Path.Combine(avatarFolderPath, _user.UserName + avatarExtension);

            ImageHelper.SaveBitmap(AvatarPath, saveTargetPath, AvatarWidth, AvatarHeight);

            _user.AvatarPath = saveTargetPath;
        }

        private void SaveProfile()
        {
            if (HasPassword)
            {
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    if (NewPassword == RetypedNewPassword)
                    {
                        if (_user.Password == OldPassword)
                        {
                            _user.Password = NewPassword;
                        }
                    }
                }
            }
            else
                _user.Password = string.Empty;

            SaveAvatar();

            using (IWorkContextScope scope = _serviceLocator.GetInstance<IWorkEnvironment>().GetWorkContextScope())
            {
                IUserService userService = _serviceLocator.GetInstance<IUserService>();
                userService.Update(_user);
            }

            MessageBox.Show("保存成功");
            _navigationManager.CurrentPage = _navigationManager.PreviousPage;
        }
    }
}
