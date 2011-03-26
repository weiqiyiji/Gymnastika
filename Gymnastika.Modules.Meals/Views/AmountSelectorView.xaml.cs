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
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Gymnastika.Modules.Meals.ViewModels;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for AmountSelectorView.xaml
    /// </summary>
    public partial class AmountSelectorView : UserControl
    {
        public AmountSelectorView()
        {
            InitializeComponent();
            this.MouseMove += new MouseEventHandler(TestControl_MouseMove);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(TestControl_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(TestControl_MouseLeftButtonUp);
        }

        public AmountSelectorViewModel Model
        {
            get { return this.DataContext as AmountSelectorViewModel; }
            set { this.DataContext = value; }
        }

        private void UserControl_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
        }

        private void UserControl_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Model.CurrentValue = (int)e.DeltaManipulation.Translation.Y;
        }

        double oy, orginValue;
        bool IsMouseDown = false;

        void TestControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDown = false;
            this.ReleaseMouseCapture();
        }

        void TestControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            oy = e.GetPosition(this).Y;

            orginValue = Model.CurrentValue;
            IsMouseDown = true;
            this.CaptureMouse();
        }

        void TestControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                Model.CurrentValue = (int)(orginValue - (e.GetPosition(this).Y - oy) / 15);
            }
        }
    }
}
