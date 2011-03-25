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
using Gymnastika.Modules.Meals.ViewModels;
using System.Windows.Threading;
using Microsoft.Practices.ServiceLocation;
using System.ComponentModel;

namespace Gymnastika.Modules.Meals.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : UserControl
    {
        //private BackgroundWorker backgroundWorker;
        //DispatcherTimer _animationTimer;
        public Shell()
        {
            InitializeComponent();

            //backgroundWorker = new BackgroundWorker();
            //_animationTimer = new DispatcherTimer();
            //_animationTimer.Interval = TimeSpan.FromSeconds(5);
            //_animationTimer.Tick += Timer_Tick;
            //_animationTimer.Start();
            this.Loaded += new RoutedEventHandler(Shell_Loaded);
            //InitializeBackgoundWorker();
            //backgroundWorker.RunWorkerAsync();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
        }

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            var a = ServiceLocator.Current.GetInstance<IMealsManagementViewModel>();
            Model.MealsManangementViewModel = a;
        }

        [Dependency]
        public ShellViewModel Model
        {
            get { return DataContext as ShellViewModel; }
            set { DataContext = value; }
        }

        //private void InitializeBackgoundWorker()
        //{
        //    backgroundWorker.DoWork +=
        //        new DoWorkEventHandler(backgroundWorker_DoWork);
        //    backgroundWorker.RunWorkerCompleted +=
        //        new RunWorkerCompletedEventHandler(
        //    backgroundWorker_RunWorkerCompleted);
        //    backgroundWorker.ProgressChanged +=
        //        new ProgressChangedEventHandler(
        //    backgroundWorker_ProgressChanged);
        //}

        private void backgroundWorker_DoWork(object sender,
            DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = ServiceLocator.Current.GetInstance<IMealsManagementViewModel>();
        }

        private void backgroundWorker_RunWorkerCompleted(
            object sender, RunWorkerCompletedEventArgs e)
        {
            Model.MealsManangementViewModel = (IMealsManagementViewModel)e.Result;
            this.LoadingView.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void backgroundWorker_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            //this.progressBar1.Value = e.ProgressPercentage;
        }



        //void Shell_Loaded(object sender, RoutedEventArgs e)
        //{
        //    Func<IMealsManagementViewModel> asyncAction = this.ModuleLoadingBackground;

        //    Action<IAsyncResult> resultHandler = delegate(IAsyncResult asyncResult)
        //    {
        //        var mealsManagementViewModel = asyncAction.EndInvoke(asyncResult);
        //        this.UpdateUIWhenModuleLoaded(mealsManagementViewModel);
        //    };

        //    AsyncCallback asyncActionCallback = delegate(IAsyncResult asyncResult)
        //    {
        //        this.Dispatcher.BeginInvoke(DispatcherPriority.Send, resultHandler, asyncResult);
        //    };

        //    asyncAction.BeginInvoke(asyncActionCallback, null); 
        //}

        private IMealsManagementViewModel ModuleLoadingBackground()
        {
            var mealsManagementViewModel = ServiceLocator.Current.GetInstance<IMealsManagementViewModel>();
            return mealsManagementViewModel;
        }

        private void UpdateUIWhenModuleLoading()
        {
        }

        private void UpdateUIWhenModuleLoaded(IMealsManagementViewModel mealsManagementViewModel)
        {
            Model.MealsManangementViewModel = mealsManagementViewModel;
            this.LoadingView.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
