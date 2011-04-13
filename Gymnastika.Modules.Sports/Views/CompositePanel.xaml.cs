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
using Gymnastika.Common.Navigation;
using System.Windows.Media.Animation;
using Gymnastika.Modules.Sports.Models;
using System.Windows.Threading;
using Gymnastika.Modules.Sports.Facilities;

namespace Gymnastika.Modules.Sports.Views
{
    /// <summary>
    /// Interaction logic for ModuleShell.xaml
    /// </summary>
    public partial class CompositePanel : UserControl
    {
        public CompositePanel()
        {
            InitializeComponent();
            LoadViewModel();
        }

        void LoadViewModel()
        {
            AsychronousLoadHelper.AsychronousResolve<ICompositePanelViewModel>((model) =>
                {
                    ViewModel = model;
                }, this.Dispatcher);
        }

        public ICompositePanelViewModel ViewModel
        {
            set 
            {
                DataContext = value;
                value.CalorieChartViewModel.RequestShowDetailEvent += OnRequestShowDetailEvent;
                
            }
            get { return DataContext as ICompositePanelViewModel; }
        }

        void OnRequestShowDetailEvent(object sender, ShowSportsDetailEventArgs args)
        {
            var viewmdel = ViewModel.SportViewModel;
            viewmdel.Sport = args.Sport;
            SportView window = new SportView();
            window.DataContext = viewmdel;
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();

        }

        public void StateChanging(ViewState targetState)
        {
            string viewname = targetState.Name;
        }

        private void AutoCompleteBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoSearch();
            }
        }

        void DoSearch()
        {
            ISportsPanelViewModel viewmodel = ViewModel.SportsPanelViewModel;
            ICommand searchCommand = viewmodel.SearchCommand;
            viewmodel.SearchName = searchBox.Text;
            if (searchCommand.CanExecute(null))
            {
                searchCommand.Execute(null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectDateView window = new SelectDateView();
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
            DateTime date = window.Date;
            ViewModel.PlanViewModel.SetPlan(date);
        }
    }
    //    ICategoriesPanelViewModel _categoryPanelModel;
    //    ISportsPanelViewModel _sportsPanelModel;
    //    ISportViewModel _sportViewModel;
    //    ISportsPlanViewModel _planViewModel;

    //    public ICategoriesPanelViewModel CategoriesPanelViewModel { get { return _categoryPanelModel; } }
    //    public ISportsPanelViewModel SportsPanelViewModel { get { return _sportsPanelModel; } }
    //    public ISportViewModel SportViewModel { get { return _sportViewModel; } }
    //    public ISportsPlanViewModel PlanViewModel { get { return _planViewModel; } }

    //    Duration duration
    //    {
    //        get { return TimeSpan.FromSeconds(0.2); }
    //    }
    //    DoubleAnimation DoubleToZeroAnimation
    //    {
    //        get { return new DoubleAnimation(0d, duration); }
    //    }

    //    DoubleAnimation DoubleClearValueAnimation
    //    {
    //        get { return new DoubleAnimation() { Duration = duration }; }
    //    }


    //    double Width1
    //    {
    //        get { return ActualWidth / 2; }
    //    }

    //    double Left1
    //    {
    //        get { return Width1; }
    //    }

    //    double Top1
    //    {
    //        get { return ActualHeight / 9; }
    //    }

    //    double Height1
    //    {
    //        get { return Top1; }
    //    }

    //    double Height2
    //    {
    //        get { return ActualHeight - Height1; }
    //    }

    //    TranslateTransform GetTranslateTransform(UIElement element)
    //    {
    //        Transform transform = element.RenderTransform;
    //        if (transform is TranslateTransform)
    //        {
    //            return transform as TranslateTransform;
    //        }
    //        else if (transform is TransformGroup)
    //        {
    //            foreach (var tran in (transform as TransformGroup).Children)
    //            {
    //                if (tran is TranslateTransform)
    //                    return tran as TranslateTransform;
    //            }
    //        }
    //        TransformGroup group = new TransformGroup();
    //        group.Children.Add(transform);
    //        TranslateTransform trans = new TranslateTransform();
    //        group.Children.Add(trans);
    //        element.RenderTransform = group;
    //        return trans;
    //    }

        

    //    double Width2
    //    {
    //        get { return ActualWidth - Left1; }
    //    }

    //    public CompositePanel()
    //    {
    //        IsExpanded = true;
    //        InitializeComponent();
    //        Run();
    //    }

    //    public void Run()
    //    {
    //        try
    //        {
    //            IServiceLocator locator = ServiceLocator.Current;
    //            SetModel(locator.GetInstance<ICategoriesPanelViewModel>(), locator.GetInstance<ISportsPanelViewModel>(), locator.GetInstance<ISportViewModel>(),locator.GetInstance<ISportsPlanViewModel>());
    //        }
    //        catch (Exception)
    //        {
    //        }
    //    }

    //    void SetModel(ICategoriesPanelViewModel viewmodel,ISportsPanelViewModel sportsPanelModel,ISportViewModel sportViewModel,ISportsPlanViewModel planViewmodel)
    //    {
    //        _categoryPanelModel = viewmodel;
    //        _sportViewModel = sportViewModel;
    //        _sportsPanelModel = sportsPanelModel;
    //        _planViewModel = planViewmodel;
    //        Initialize();
    //    }
    //    void Initialize()
    //    {
    //        BindingViewModels();
    //        LinkEvents();
    //        _sportsPanelModel.Category = _categoryPanelModel.CurrentSelectedItem;
    //    }

    //    void BindingViewModels()
    //    {
    //        categoriesPanelView.ViewModel = _categoryPanelModel;
    //        sportsPanelView.ViewModel = _sportsPanelModel;
    //        sportView.ViewModel = _sportViewModel;
    //        sportsPlanView.ViewModel = _planViewModel;
    //    }

    //    void LinkEvents()
    //    {
    //       _categoryPanelModel.CategorySelectedEvent += CategorySelectedChanged;
    //       _sportsPanelModel.RequestShowDetailEvent += OnRequestShowDetail;
    //       _sportViewModel.RequestCloseEvent += OnCloseRequest;
    //    }

    //    void CategorySelectedChanged(object sender, EventArgs args)
    //    {
    //        _sportsPanelModel.Category = _categoryPanelModel.CurrentSelectedItem;
    //    }



    //    public void OnRequestShowDetail(object sender, SportEventArgs args)
    //    {
    //        Minimize();
    //        _sportViewModel.Sport = args.Sport;
    //    }

    //    void OnCloseRequest(object sender, EventArgs Args)
    //    {
    //        Expand();
    //    }

    //    private void surfaceButton1_Click(object sender, RoutedEventArgs e)
    //    {
    //        Expand();
    //    }

    //    void Expand()
    //    {
    //        if (IsExpanded == false)
    //        {
    //            IsExpanded = true;
    //            BeginExpandAnimation();
    //        }
    //    }

    //    void FillSize(ContentControl element)
    //    {
    //        element.Width = element.ActualWidth;
    //        element.Height = element.ActualHeight;
    //    }

    //    DoubleAnimation GetAnimation(double? to = null, double? dur = null)
    //    {
    //        if (dur != null)
    //            return new DoubleAnimation(to.Value, TimeSpan.FromSeconds(dur.Value));
    //        else if (to != null)
    //            return new DoubleAnimation(to.Value, duration);
    //        else
    //            return new DoubleAnimation() { Duration = duration };
    //    }

    //    void GotoRect(ContentControl element, Rect rect,bool animated = true)
    //    {
    //        TranslateTransform trans = GetTranslateTransform(element);

    //            var dur = animated ? null : new Double?(0d);
    //            var aniLeft = GetAnimation(rect.Left, dur);
    //            var aniTop = GetAnimation(rect.Top, dur);
    //            var aniWidth = GetAnimation(rect.Width, dur);
    //            var aniHeight = GetAnimation(rect.Height, dur);
    //            trans.BeginAnimation(TranslateTransform.XProperty, aniLeft);
    //            trans.BeginAnimation(TranslateTransform.YProperty, aniTop);
    //            element.BeginAnimation(UserControl.WidthProperty, aniWidth);
    //            element.BeginAnimation(UserControl.HeightProperty, aniHeight);
            
    //    }

    //    void GotoCollapse(ContentControl element,double? dur=null)
    //    {
            
    //        //element.Visibility = Visibility.Collapsed;
            
    //        //DoubleAnimation ani = new DoubleAnimation(0d, TimeSpan.FromSeconds(dur != null ? dur.Value : 0.2d));
    //        //Brush brush = GetMask(element);
    //        //brush.BeginAnimation(Brush.OpacityProperty, ani);

    //        ObjectAnimationUsingKeyFrames keyAni = new ObjectAnimationUsingKeyFrames();
    //        DiscreteObjectKeyFrame key = new DiscreteObjectKeyFrame(Visibility.Collapsed, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)));
    //        keyAni.KeyFrames.Add(key);
    //        element.BeginAnimation(ContentControl.VisibilityProperty, keyAni);
            
    //    }

    //    Brush GetMask(ContentControl element)
    //    {
    //        if (element.OpacityMask == null)
    //        {
    //            element.OpacityMask = new SolidColorBrush(new Color() { R = 225, G = 255, B = 255, A = 255 });
    //        }
    //        return element.OpacityMask;
    //    }

    //    void GotoVisible(ContentControl element, double? dur = null)
    //    {
    //        //DoubleAnimation ani = new DoubleAnimation(100d, TimeSpan.FromSeconds(dur != null ? dur.Value : 0.2d));
    //        //Brush brush = GetMask(element);
    //        //brush.BeginAnimation(Brush.OpacityProperty, ani);

    //        ObjectAnimationUsingKeyFrames keyAni = new ObjectAnimationUsingKeyFrames();
    //        DiscreteObjectKeyFrame key = new DiscreteObjectKeyFrame(Visibility.Visible, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2)));
    //        keyAni.KeyFrames.Add(key);
    //        element.BeginAnimation(ContentControl.VisibilityProperty, keyAni);
    //    }


    //    void LoadedAnimation()
    //    {
    //        FillSize(sportView);
    //        FillSize(categoriesPanelView);
    //        FillSize(sportView);
    //        BeginExpandAnimation();

    //    }
    //    void BeginMinimizeAnimation(bool animated = true)
    //    {
    //        return;
    //        GotoRect(sportsPlanView, new Rect(0, 0, 0, ActualHeight),animated);
    //        GotoRect(sportsPanelView, new Rect(0, Top1, Width1, Height2), animated);
    //        GotoRect(categoriesPanelView, new Rect(0, 0, Width1, Height1), animated);
    //        GotoRect(sportView, new Rect(Left1, Top1, Width2, Height2), animated);
    //        GotoVisible(sportView);
    //    }
    //    void BeginExpandAnimation(bool animated = true)
    //    {
    //        return;
    //        GotoRect(sportsPlanView, new Rect(0, 0, Left1, ActualHeight), animated);
    //        GotoRect(sportsPanelView, new Rect(Left1, Top1, Width2, Height2), animated);
    //        GotoRect(categoriesPanelView, new Rect(Left1, 0, Width2, Height1), animated);
    //        GotoRect(sportView, new Rect(ActualWidth, Top1, Width2, Height2), animated);
    //        GotoCollapse(sportView);
    //    }



    //    void Minimize()
    //    {
    //        if (IsExpanded == true)
    //        {
    //            IsExpanded = false;
    //            BeginMinimizeAnimation();
    //        }
            
    //    }

    //    private void surfaceButton2_Click(object sender, RoutedEventArgs e)
    //    {
    //        Minimize();
    //    }

    //    bool IsExpanded { get; set; }

    //    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    //    {
    //        LoadedAnimation();
    //        GotoRect(sportView, new Rect(Left1, Top1, Width2, Height2));
    //        _sportViewModel.Sport = _sportsPanelModel.CurrentSports.FirstOrDefault() ?? new Sport();

    //    }

    //    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    //    {
    //        if (IsExpanded)
    //            BeginExpandAnimation(false);
    //        else
    //            BeginMinimizeAnimation(false);
    //    }
    //}
}
