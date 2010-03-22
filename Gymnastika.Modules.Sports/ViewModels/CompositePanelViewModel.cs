using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gymnastika.Modules.Sports.ViewModels
{
    public interface ICompositePanelViewModel
    {
        ICategoriesPanelViewModel CategoriesPanelViewModel{get;}
        ISportsPanelViewModel SportsPanelViewModel { get; }
        ISportViewModel SportViewModel { get; }
        ISportsPlanViewModel PlanViewModel { get; }
        ISportCalorieChartViewModel CalorieChartViewModel { get; }
    }

    public class CompositePanelViewModel : ICompositePanelViewModel
    {
        ICategoriesPanelViewModel _categoryPanelModel;
        ISportsPanelViewModel _sportsPanelModel;
        ISportViewModel _sportViewModel;
        ISportsPlanViewModel _planViewModel;
        ISportCalorieChartViewModel _calorieChartViewModel;

        public ISportCalorieChartViewModel CalorieChartViewModel
        {
            get { return _calorieChartViewModel; }
            private set { _calorieChartViewModel = value; } 
        }

        public ICategoriesPanelViewModel CategoriesPanelViewModel 
        {
            get { return _categoryPanelModel; }
            private set { _categoryPanelModel = value; }
        }
        public ISportsPanelViewModel SportsPanelViewModel 
        {
            get { return _sportsPanelModel; }
            private set { _sportsPanelModel = value; }
        }

        public ISportViewModel SportViewModel 
        {
            get { return _sportViewModel; }
            set { _sportViewModel = value; }
        }
        public ISportsPlanViewModel PlanViewModel 
        { 
            get { return _planViewModel; }
            set { _planViewModel = value; }
        }

        public CompositePanelViewModel(ICategoriesPanelViewModel categoriesPanelViewModel,
            ISportsPanelViewModel sportsPanelViewModel,
            ISportViewModel sportViewModel,
            ISportsPlanViewModel sportsPlanViewModel,
            ISportCalorieChartViewModel calorieChartViewModel)
        {
            CategoriesPanelViewModel = categoriesPanelViewModel;
            SportsPanelViewModel = sportsPanelViewModel;
            SportViewModel = sportViewModel;
            PlanViewModel = sportsPlanViewModel;
            CalorieChartViewModel = calorieChartViewModel;
            CategoriesPanelViewModel.CategorySelectedEvent += OnSelectedCategoryChanged;
            SportsPanelViewModel.Category = CategoriesPanelViewModel.CurrentSelectedItem;
            CalorieChartViewModel.RequestAddToPlanEvent += OnAddToPlan;

            PlanViewModel.SetPlan(DateTime.Now);

        }

        void OnAddToPlan(object sender, AddToPlanEventArgs args)
        {
            PlanViewModel.AddPlanItem(args.Item);
        }

        public void OnSelectedCategoryChanged(object sender, EventArgs e)
        {
            SportsPanelViewModel.Category = CategoriesPanelViewModel.CurrentSelectedItem;
        }
    }
}
