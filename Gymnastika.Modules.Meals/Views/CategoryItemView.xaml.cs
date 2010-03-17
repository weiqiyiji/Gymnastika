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
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for CategoryItemView.xaml
    /// </summary>
    public partial class CategoryItemView : ICategoryItemView
    {
        public CategoryItemView()
        {
            InitializeComponent();
        }

        #region ICategoryItemView Members

        public ICategoryItemViewModel Context
        {
            get
            {
                return this.DataContext as ICategoryItemViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public event SelectionChangedEventHandler SubCategoryItemSelectionChanged;

        #endregion

        private void SubCateogyrListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubCategoryItemSelectionChanged != null)
                SubCategoryItemSelectionChanged(sender, e);
        }
    }
}
