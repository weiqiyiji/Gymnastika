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
using Gymnastika.Modules.Sports.Views;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Modules.Sports.ViewModels;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for SportsListView.xaml
    /// </summary>
    public partial class SportsListView : UserControl, ISportsListView
    {
        [ImportingConstructor]
        public SportsListView(ISportsListViewModel model) 
        {
            ViewModel = model;
            InitializeComponent();
        }

        [Export(typeof(ISportsListViewModel))]
        public ISportsListViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
            get
            {
                return this.DataContext as ISportsListViewModel;
            }
        }
    }
}
