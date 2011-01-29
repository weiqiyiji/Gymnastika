using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Gymnastika.Modules.Sports.Interface;
using System.Windows.Media;

namespace Gymnastika.Modules.Sports.Models
{
    public class Sport : NotificationObject , ISport
    {

        #region ISport Members

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }


        ImageSource _image;
        public ImageSource Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    RaisePropertyChanged("Image");
                }
            }
        }

        #endregion
    }
}
