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
    /// Interaction logic for CategoryListView.xaml
    /// </summary>
    public partial class CategoryListView : ICategoryListView
    {
        public CategoryListView()
        {
            InitializeComponent();
        }

        #region ICategoryListView Members

        public ICategoryListViewModel Context
        {
            get
            {
                return this.DataContext as ICategoryListViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public event SelectionChangedEventHandler CategoryItemSelectionChanged;

        #endregion

        private void CategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryItemSelectionChanged != null)
                CategoryItemSelectionChanged(sender, e);
        }
    }
}
