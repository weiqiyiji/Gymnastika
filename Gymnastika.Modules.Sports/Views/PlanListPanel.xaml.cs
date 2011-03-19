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
using System.Windows.Media.Animation;
using Gymnastika.Common.Navigation;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Modules.Sports.ViewModels;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for PlanListPanel.xaml
    /// </summary>
    public partial class PlanListPanel : UserControl
    {
        IPlanListViewModel _listViewModel;
        
        public PlanListPanel()
        {
            InitializeComponent();
            _listViewModel = ListView.DataContext as IPlanListViewModel;
            _listViewModel.SelectedItemChangedEvent += OnSelectedItemChanged;
        }

        public bool IsExpanded { get; set; }

        void Expand()
        {
            if (IsExpanded == false)
            {
                IsExpanded = true;
                ListView.Expand();
                (FindResource("ExpandStoryboard") as Storyboard).Begin();
                UpdateViewModel();
            }
        }

        void Minimize()
        {
            if (IsExpanded == true)
            {
                IsExpanded = false;
                ListView.Minimize();
                (FindResource("MinimizeStoryboard") as Storyboard).Begin();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Expand();
        }

        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            Minimize();
        }

        
        public void StateChanging(ViewState targetState)
        {
            Expand();
            IPlanListViewModel viewModel = _listViewModel;
            if(viewModel!=null)
            {
                int day = -1;
                switch (targetState.Name)
                {
                    case "Sunday":
                        day = 0;
                        break;
                    case "Monday":
                        day = 1;
                        break;
                    case "Tuesday":
                        day = 2;
                        break;
                    case "Wednesday":
                        day = 3;
                        break;
                    case "Thursday":
                        day = 4;
                        break;
                    case "Friday":
                        day = 5;
                        break;
                    case "Saturday":
                        day = 6;
                        break;
                }
                viewModel.GotoDayOfWeek(day);
            }
        }

        private void surfaceButton1_Click_1(object sender, RoutedEventArgs e)
        {
            Expand();
        }

        private void surfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            Minimize();
        }


        void OnSelectedItemChanged(object sender, EventArgs e)
        {
            if (IsExpanded)
            {
                UpdateViewModel();
            }
        }

        void UpdateViewModel()
        {
            if (DetailView.DataContext != _listViewModel.SelectedItem)
                DetailView.DataContext = _listViewModel.SelectedItem;
        }
    }
}
