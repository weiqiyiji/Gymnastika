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
    /// Interaction logic for SportsPlanItemView.xaml
    /// </summary>
    public partial class SportsPlanItemView : UserControl
    {
        public readonly static DependencyProperty ExpandedProperty 
            = DependencyProperty.Register("Expanded",
                                            typeof(bool), 
                                            typeof(SportsPlanItemView),
                                            new PropertyMetadata(false));
        public SportsPlanItemView()
        {
            
            InitializeComponent();
            DataContext = new SportsPlanItemViewModel()
            {
                Model = new SportsPlanItem()
                {
                    Sport = new Sport()
                    {
                        Name = "Badminton"
                    },
                    Hour = 9,
                    Min = 30,
                    Duration = 60,
                    HasCompleted = false
                }
            };
        }
        public bool Expanded
        {
            set
            {
                SetValue(ExpandedProperty,value);
            }
            get
            {
                return (bool)GetValue(ExpandedProperty);
            }
        }
    }
}
