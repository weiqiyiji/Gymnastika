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
using Gymnastika.Modules.Sports.ViewModels;
using Microsoft.Practices.Unity;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for CategoriesPanelView.xaml
    /// </summary>
    public partial class CategoriesPanelView : UserControl , ICategoriesPanelView
    {
        public CategoriesPanelView()
        {
            InitializeComponent();
        }

        public ICategoriesPanelViewModel ViewModel
        {
            set
            {
                DataContext = value;
                var viewmodel = DataContext as ICategoriesPanelViewModel;
                if (viewmodel != null && categorylist.Items.Count !=0)
                    categorylist.SelectedIndex = 0;
            }
            get
            {
                return DataContext as ICategoriesPanelViewModel;
            }
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ICategoriesPanelViewModel viewmodel = (DataContext as ICategoriesPanelViewModel);
            if (viewmodel != null && viewmodel.SelectCommand.CanExecute(null))
            {
                viewmodel.SelectCommand.Execute(categorylist.SelectedItem);
            }
        }
    }

    public interface ICategoriesPanelView
    {
        ICategoriesPanelViewModel ViewModel { get; set; }
    }
}
