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
using System.Windows.Threading;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for LoadDataView.xaml
    /// </summary>
    public partial class LoadDataView
    {
        DispatcherTimer _animationTimer;
        public LoadDataView()
        {
            InitializeComponent();
            _animationTimer = new DispatcherTimer();
            _animationTimer.Interval = TimeSpan.FromSeconds(5);
            _animationTimer.Tick += Timer_Tick;
            _animationTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
