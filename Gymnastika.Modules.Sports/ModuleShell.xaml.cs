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

namespace Gymnastika.Modules.Sports
{
    /// <summary>
    /// Interaction logic for ModuleShell.xaml
    /// </summary>
    public partial class ModuleShell : UserControl
    {
        readonly IUnityContainer _container;
        public ModuleShell(IUnityContainer container)
        {
            InitializeComponent();
            _container = container;
            Initialize();
        }

        private void Initialize()
        {
            compositePanel.SetModel(_container.Resolve<ICategoriesPanelViewModel>()
                , _container.Resolve<ISportsPanelViewModel>());
            planListView1.ViewModel = _container.Resolve<IPlanListViewModel>();
        }
    }
}
