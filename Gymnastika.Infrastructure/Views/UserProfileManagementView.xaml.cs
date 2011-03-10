using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gymnastika.ViewModels;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for UserProfileManagementView.xaml
    /// </summary>
    public partial class UserProfileManagementView : UserControl
    {
        public UserProfileManagementView(UserProfileManagementViewModel vm)
        {
            InitializeComponent();
            Model = vm;
        }

        public UserProfileManagementViewModel Model
        {
            get
            {
                return DataContext as UserProfileManagementViewModel;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
