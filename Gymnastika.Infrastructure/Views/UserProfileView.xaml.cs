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
using System.Windows.Shapes;
using Gymnastika.ViewModels;
using Microsoft.Practices.Unity;

namespace Gymnastika.Views
{
    /// <summary>
    /// Interaction logic for LogOnView.xaml
    /// </summary>
    public partial class UserProfileView : Window, IUserProfileView
    {
        public UserProfileView(UserProfileViewModel vm)
        {
            InitializeComponent();
            Model = vm;
        }

        public UserProfileViewModel Model
        {
            get
            {
                return DataContext as UserProfileViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

        private void HeaderTextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
        	TextBlock block = (TextBlock)sender;
            block.TextDecorations.Add(TextDecorations.Underline);
        }
		
		private void HeaderTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
		    TextBlock block = (TextBlock) sender;
            block.TextDecorations.Clear();
		}
    }
}
