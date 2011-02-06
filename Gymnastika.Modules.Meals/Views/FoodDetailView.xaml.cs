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
    /// Interaction logic for FoodDetailView.xaml
    /// </summary>
    public partial class FoodDetailView : IFoodDetailView
    {
        public FoodDetailView()
        {
            InitializeComponent();
        }

        #region IFoodDetailView Members

        public IFoodDetailViewModel Context
        {
            get
            {
                return this.DataContext as IFoodDetailViewModel;
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

        #endregion
    }
}
