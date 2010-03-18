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
using Microsoft.Practices.Unity;
using Gymnastika.Modules.Sports.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for ModuleShell.xaml
    /// </summary>
    public partial class CompositePanel : UserControl
    {
        ICategoriesPanelViewModel _categoryPanelModel;
        ISportsPanelViewModel _sportsPanelModel;
        ISportViewModel _sportViewModel;

        public CompositePanel()
        {
            InitializeComponent();
            Run();
        }

        public void Run()
        {
            IServiceLocator locator = ServiceLocator.Current;
            SetModel(locator.GetInstance<ICategoriesPanelViewModel>(), locator.GetInstance<ISportsPanelViewModel>(), locator.GetInstance<ISportViewModel>());
        }

        void SetModel(ICategoriesPanelViewModel viewmodel,ISportsPanelViewModel sportsPanelModel,ISportViewModel sportViewModel)
        {
            _categoryPanelModel = viewmodel;
            _sportViewModel = sportViewModel;
            _sportsPanelModel = sportsPanelModel;
            Initialize();
        }
        void Initialize()
        {
            BindingViewModels();
            LinkEvents();
            _sportsPanelModel.Category = _categoryPanelModel.CurrentSelectedItem;
        }

        void BindingViewModels()
        {
            categoriesPanelView.DataContext = _categoryPanelModel;
            sportsPanelView.DataContext = _sportsPanelModel;
            sportView.DataContext = _sportViewModel;
        }

        void LinkEvents()
        {
           _categoryPanelModel.CategorySelectedEvent += CategorySelectedChanged;
        }

        void CategorySelectedChanged(object sender, EventArgs args)
        {
            _sportsPanelModel.Category = _categoryPanelModel.CurrentSelectedItem;
        }
    }
}
