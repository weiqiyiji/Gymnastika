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
    /// Interaction logic for SelectDietPlanView.xaml
    /// </summary>
    public partial class SelectDietPlanView : ISelectDietPlanView
    {
        public SelectDietPlanView()
        {
            InitializeComponent();
        }

        #region ISelectDietPlanView Members

        public ISelectDietPlanViewModel Context
        {
            get
            {
                return this.DataContext as ISelectDietPlanViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public void ShowView()
        {
            this.Show();
        }

        public void CloseView()
        {
            this.Close();
        }

        #endregion
    }
}
