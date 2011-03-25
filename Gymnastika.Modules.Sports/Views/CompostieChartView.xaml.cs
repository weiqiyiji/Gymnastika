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

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for CompostieChartView.xaml
    /// </summary>
    public partial class CompostieChartView : UserControl
    {
        IPlanDetailViewModel _planDetailViewModel;
        IPlanListViewModel _planListViewModel;
        public CompostieChartView(IPlanListViewModel planListViewModel,IPlanDetailViewModel planDetailViewModel)
        {
            InitializeComponent();
            _planDetailViewModel = planDetailViewModel;
            _planListViewModel = planListViewModel;
            InitializeViewModels();
        }

        private void InitializeViewModels()
        {
            planListView.ViewModel = _planListViewModel;
            _planListViewModel.SelectedItemChangedEvent += OnSelectedItemChanged;
        }

        public void OnSelectedItemChanged(object sender, EventArgs args)
        {
            if (_planListViewModel.SelectedItem != null)
                _planDetailViewModel.SetPlan(_planListViewModel.SelectedItem.SportsPlan);
        }
    }
}
