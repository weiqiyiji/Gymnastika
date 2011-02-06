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
    /// Interaction logic for DietPlanListView.xaml
    /// </summary>
    public partial class DietPlanListView : IDietPlanListView
    {
        public DietPlanListView()
        {
            InitializeComponent();
        }

        #region IDietPlanView Members

        public IDietPlanListViewModel Context
        {
            get
            {
                return this.DataContext as IDietPlanListViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public void ExpandAll()
        {
            DietPlanList.SelectionMode = AccordionSelectionMode.OneOrMore;
            DietPlanList.SelectAll();
        }

        #endregion
    }
}
