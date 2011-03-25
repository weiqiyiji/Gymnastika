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
using Visifire.Charts;
using Gymnastika.Modules.Sports.Models;
using Gymnastika.Common.Extensions;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for PlanDetailView.xaml
    /// </summary>
    public partial class PlanDetailView : UserControl
    {
        public PlanDetailView()
        {
            InitializeComponent();
        }


        public IPlanDetailViewModel ViewModel
        {
            set 
            {
                if (DataContext != value)
                {
                    DataContext = value;
                    value.PlanChangedEvent += OnPlanChanged;
                    Refresh();
                }
            }
            get { return DataContext as IPlanDetailViewModel; }
        }

        void OnPlanChanged(object sender, EventArgs args)
        {
            Refresh();
        }

        void UpdateChart()
        {
            Dictionary<double, double> timeCalorie = new Dictionary<double, double>();
            Dictionary<double, double> timeTotalCalorie = new Dictionary<double, double>();
            //timeTotalCalorie.Add(0, 0);
            //timeCalorie.Add(0, 0);
            //timeCalorie.Add(24, 0);
            DataSeries dataSeriesOfCalories = chart.Series[0];
            DataSeries dataSeriesOfTotalCalories = chart.Series[1];

            DataSeries dataSeriesOfItems = chart1.Series[0];

            dataSeriesOfCalories.DataPoints.Clear();
            dataSeriesOfTotalCalories.DataPoints.Clear();
            dataSeriesOfItems.DataPoints.Clear();

            double totalCalorie = 0;

            if (ViewModel != null && ViewModel.SportsPlan != null)
            {

                IList<SportsPlanItem> items = ViewModel.SportsPlan.SportsPlanItems.OrderBy(t => t.Minute).OrderBy(t => t.Hour).ToList();
                foreach (var item in items)
                {


                    //key
                    double key = item.Hour + item.Minute / 60;

                    //value
                    double calorie = GetCalories(item);
                    totalCalorie += calorie;

                    dataSeriesOfItems.DataPoints.Add(new DataPoint() { AxisXLabel = item.Sport.Name, YValue = calorie });


                    //update calorie of one time point
                    double value;
                    if (timeCalorie.TryGetValue(key, out value))
                        timeCalorie[key] = calorie;
                    else
                        timeCalorie.Add(key, calorie);

                    //update total calorie
                    double oldTotalCalorie;
                    if (timeTotalCalorie.TryGetValue(key, out oldTotalCalorie))
                        timeTotalCalorie[key] = totalCalorie;
                    else
                        timeTotalCalorie.Add(key, totalCalorie);

                }
            }

            var points1 = timeCalorie.ToList().OrderBy(t => t.Key);
            foreach (var value in points1)
            {
                dataSeriesOfCalories.DataPoints.Add(new DataPoint() { XValue = value.Key, YValue = value.Value });
            }

            var points2 = timeTotalCalorie.ToList().OrderBy(t => t.Key);
            foreach (var value in points2)
            {
                dataSeriesOfTotalCalories.DataPoints.Add(new DataPoint() { XValue = value.Key, YValue = value.Value });
            }
            
        }

        void Refresh()
        {
            UpdateChart();
            //chart.RaiseEvent(new RoutedEventArgs(Chart.LoadedEvent, chart));
        }

        double GetCalories(SportsPlanItem item)
        {
            return item.Sport.Calories / item.Sport.Minutes * item.Minute;
        }




    }
}
