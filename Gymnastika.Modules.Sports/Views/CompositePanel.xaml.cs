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

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for ModuleShell.xaml
    /// </summary>
    public partial class CompositePanel : UserControl
    {
        readonly ICategoriesPanelViewModel _categoryPanelModel;
        readonly ISportsPanelViewModel _sportsPanelModel;
        public CompositePanel(ICategoriesPanelViewModel viewmodel,ISportsPanelViewModel sportsPanelModel)
        {
            _categoryPanelModel = viewmodel;
            _sportsPanelModel = sportsPanelModel;
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            BindingViewModels();
            LinkEvents();
            _sportsPanelModel.Category = _categoryPanelModel.CurrentSelectedItem;
        }

        private void BindingViewModels()
        {
            categoriesPanelView.DataContext = _categoryPanelModel;
            sportsPanelView.DataContext = _sportsPanelModel;
        }

        private void LinkEvents()
        {
           _categoryPanelModel.CategorySelectedEvent += CategorySelectedChanged;
        }

        void CategorySelectedChanged(object sender, EventArgs args)
        {

        }
    }
}
