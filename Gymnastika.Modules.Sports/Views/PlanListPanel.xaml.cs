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

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for PlanListPanel.xaml
    /// </summary>
    public partial class PlanListPanel : UserControl
    {
        public PlanListPanel()
        {
            InitializeComponent();
        }

        void SwitchToDetail()
        {
            DoubleAnimation DetailViewWidth = new DoubleAnimation(this.Width, TimeSpan.FromSeconds(0.1));
            DoubleAnimation ListViewWidth = new DoubleAnimation(0, TimeSpan.FromSeconds(0.1));
            Storyboard storyBoard = new Storyboard();
            storyBoard.Children.Add(DetailViewWidth);
            storyBoard.Children.Add(ListViewWidth);
            Storyboard.SetTargetName(DetailViewWidth, "DetailView");
            Storyboard.SetTargetName(ListViewWidth, "ListView");
            Storyboard.SetTargetProperty(DetailViewWidth, new PropertyPath("WidthProperty"));
            Storyboard.SetTargetProperty(ListViewWidth, new PropertyPath("WidthProperty"));
            storyBoard.Begin();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SwitchToDetail();
        }

        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            SwitchToDetail();
        }
    }
}
