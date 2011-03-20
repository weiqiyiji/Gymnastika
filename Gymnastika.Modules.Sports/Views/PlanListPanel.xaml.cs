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
                BeginExpandAnimation();
                //(FindResource("ExpandStoryboard") as Storyboard).Begin();
                UpdateViewModel();
            }
        }

        void BeginExpandAnimation()
        {
            ListView.Expand();
            if (Double.IsNaN(ListView.Height))
            {
                ListView.Height = ListView.ActualHeight;
            }
            if (double.IsNaN(DetailView.Height))
            {
                DetailView.Height = DetailView.ActualHeight;
            }
            ListView.BeginAnimation(UserControl.HeightProperty, GetAnimation(0.2, Height1));
            DetailView.BeginAnimation(UserControl.HeightProperty, GetAnimation(0.2, TopPos));
        }
        double TopPos
        {
            get { return ActualHeight / 3 * 2 ; }
        }

        double Height1
        {
            get { return ActualHeight - TopPos; }
        }

        DoubleAnimation GetAnimation(double duration, double to)
        {
            //DoubleAnimation ani = new DoubleAnimation();
            //if (to != null && to.HasValue)
            //{
            //    ani.To = to.Value;
            //}
            //ani.Duration = TimeSpan.FromSeconds(duration != null ? duration.Value : 0.2d);
            //return ani;
            return new DoubleAnimation(to, TimeSpan.FromSeconds(duration));
        }

        void Minimize()
        {
            if (IsExpanded == true)
            {
                IsExpanded = false;
                BeginMinimizeAnimation();
                //(FindResource("MinimizeStoryboard") as Storyboard).Begin();
            }
        }
        void BeginMinimizeAnimation()
        {
            DetailView.Height = 0;
            ListView.Height = grid.ActualHeight;
            ListView.Minimize();
            if (Double.IsNaN(ListView.Height))
            {
                ListView.Height = ActualHeight;
            }
            if (double.IsNaN(DetailView.Height))
            {
                DetailView.Height = 0;
            }
            ListView.BeginAnimation(UserControl.HeightProperty, GetAnimation(0.2, ActualHeight));
            DetailView.BeginAnimation(UserControl.HeightProperty, GetAnimation(0.2, 0));
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
                    default:
                        Minimize();
                        return;
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

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DetailView.Width = grid.ActualWidth;
            ListView.Width = grid.ActualWidth;
            if (IsExpanded)
            {
                BeginExpandAnimation();
            }
            else
            {
                BeginMinimizeAnimation();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DetailView.Width = grid.ActualWidth;
            ListView.Width = grid.ActualWidth;
            DetailView.Height = 0;
            ListView.Height = grid.ActualHeight;
        }
    }
}
