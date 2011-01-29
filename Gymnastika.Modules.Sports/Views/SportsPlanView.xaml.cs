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
using Gymnastika.Modules.Sports.ViewModels;
using Gymnastika.Modules.Sports.Models;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for SportsItemView.xaml
    /// </summary>
    public partial class SportsPlanView : UserControl
    {
        public SportsPlanView()
        {
            InitializeComponent();
            ////for debug
            //DataContext = new SportsPlanItemViewModel()
            //{
            //    Model = new SportsPlan()
            //    {
            //    new SportsPlanItem()
            //    {
            //       Sport = new Sport()
            //       {
            //           Name = "Badminton"
            //       },
            //       PlanItem = new PlanItem()
            //       {
            //           HasCompleted = false,
            //           Score = 90,
            //           Duration = 60,
            //           Hour = 9,
            //           Min = 30
            //       }
            //    }
            //    }
            //};
        }
    }
}
