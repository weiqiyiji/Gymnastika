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
using Gymnastika.Modules.Meals.Models;
using System.Globalization;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for MealsManagementView.xaml
    /// </summary>
    public partial class MealsManagementView : IMealsManagementView
    {
        public MealsManagementView()
        {
            InitializeComponent();

            //SearchBox.ItemFilter = (search, item) =>
            //{
            //    Food food = item as Food;
            //    if (food != null)
            //    {
            //        string filter = search.ToUpper(CultureInfo.InvariantCulture);
            //        return (food.Name.ToUpper(CultureInfo.InvariantCulture).StartsWith(filter));
            //    }

            //    return false;
            //};
        }

        #region IMealsManagementView Members

        public IMealsManagementViewModel Context
        {
            get
            {
                return this.DataContext as IMealsManagementViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        public BindingExpression GetBindingSearchString()
        {
            return SearchBox.GetBindingExpression(AutoCompleteBox.TextProperty);
        }

        public event KeyEventHandler SearchKeyDown;

        #endregion

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrEmpty(SearchBox.Text)) return;

            if (SearchKeyDown != null)
                SearchKeyDown(sender, e);
        }
    }
}
