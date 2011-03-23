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
    /// Interaction logic for HistoryDietPlanView.xaml
    /// </summary>
    public partial class HistoryDietPlanView : IHistoryDietPlanView
    {
        public HistoryDietPlanView()
        {
            InitializeComponent();
        }

        #region IHistoryDietPlanView Members

        public IHistoryDietPlanViewModel Context
        {
            get
            {
                return this.DataContext as IHistoryDietPlanViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        #endregion
    }
}
