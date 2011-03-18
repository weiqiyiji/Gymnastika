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


        Duration duration
        {
            get { return TimeSpan.FromSeconds(0.2); }
        }
        DoubleAnimation DoubleToZeroAnimation
        {
            get { return new DoubleAnimation(0d, duration); }
        }

        DoubleAnimation DoubleClearValueAnimation
        {
            get { return new DoubleAnimation() { Duration = duration }; }
        }


        double Width1
        {
            get { return ActualWidth / 2; }
        }

        double Left1
        {
            get { return Width1; }
        }

        double Top1
        {
            get { return ActualHeight / 9; }
        }

        double Height1
        {
            get { return Top1; }
        }

        double Height2
        {
            get { return ActualHeight - Height1; }
        }

        TranslateTransform GetTranslateTransform(UIElement element)
        {
            Transform transform = element.RenderTransform;
            if (transform is TranslateTransform)
            {
                return transform as TranslateTransform;
            }
            else if (transform is TransformGroup)
            {
                foreach (var tran in (transform as TransformGroup).Children)
                {
                    if (tran is TranslateTransform)
                        return tran as TranslateTransform;
                }
            }
            TransformGroup group = new TransformGroup();
            group.Children.Add(transform);
            TranslateTransform trans = new TranslateTransform();
            group.Children.Add(trans);
            element.RenderTransform = group;
            return trans;
        }

        double Width2
        {
            get { return ActualWidth - Left1; }
        }

        public CompositePanel()
        {
            IsExpanded = true;
            InitializeComponent();
            Run();
        }

        public void Run()
        {
            try
            {
                IServiceLocator locator = ServiceLocator.Current;
                SetModel(locator.GetInstance<ICategoriesPanelViewModel>(), locator.GetInstance<ISportsPanelViewModel>(), locator.GetInstance<ISportViewModel>());
            }
            catch (Exception)
            {
            }
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
           _sportsPanelModel.RequestShowDetailEvent += OnRequestShowDetail;
           _sportViewModel.CloseRequest += OnCloseRequest;
        }

        void CategorySelectedChanged(object sender, EventArgs args)
        {
            _sportsPanelModel.Category = _categoryPanelModel.CurrentSelectedItem;
        }

        public void StateChanging(ViewState targetState)
        {
            string viewname = targetState.Name;
            switch (viewname)
            {
                case "CreatePlan":
                    Expand();
                    break;
                case "SportDetail":
                    Minimize();
                    break;
            }
        }

        public void OnRequestShowDetail(object sender, SportEventArgs args)
        {
            Minimize();
            _sportViewModel.Sport = args.Sport;
        }

        void OnCloseRequest(object sender, EventArgs Args)
        {
            Expand();
        }

        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            Expand();
        }

        void Expand()
        {
            if (IsExpanded == false)
            {
                IsExpanded = true;
                BeginExpandAnimation();
            }
        }

        void FillSize(ContentControl element)
        {
            element.Width = element.ActualWidth;
            element.Height = element.ActualHeight;
        }

        DoubleAnimation GetAnimation(double? to = null, double? dur = null)
        {
            if (dur != null)
                return new DoubleAnimation(to.Value, TimeSpan.FromSeconds(dur.Value));
            else if (to != null)
                return new DoubleAnimation(to.Value, duration);
            else
                return new DoubleAnimation() { Duration = duration };
        }

        void GotoRect(ContentControl element, Rect rect)
        {
            TranslateTransform trans = GetTranslateTransform(element);
            var aniLeft = GetAnimation(rect.Left);
            var aniTop = GetAnimation(rect.Top);
            var aniWidth = GetAnimation(rect.Width);
            var aniHeight = GetAnimation(rect.Height);
            trans.BeginAnimation(TranslateTransform.XProperty, aniLeft);
            trans.BeginAnimation(TranslateTransform.YProperty, aniTop);
            element.BeginAnimation(UserControl.WidthProperty, aniWidth);
            element.BeginAnimation(UserControl.HeightProperty, aniHeight);
        }

        void LoadedAnimation()
        {
            FillSize(sportView);
            FillSize(categoriesPanelView);
            FillSize(sportView);
            BeginExpandAnimation();

        }
        void BeginMinimizeAnimation()
        {
            GotoRect(sportsPlanView, new Rect(0, 0, 0, ActualHeight));
            GotoRect(sportsPanelView, new Rect(0, Top1, Width1, Height2));
            GotoRect(categoriesPanelView, new Rect(0, 0, Width1, Height1));
            GotoRect(sportView, new Rect(Left1, Top1, Width2, Height2));
        }
        void BeginExpandAnimation()
        {
            GotoRect(sportsPlanView, new Rect(0, 0, Left1, ActualHeight));
            GotoRect(sportsPanelView, new Rect(Left1, Top1, Width2, Height2));
            GotoRect(categoriesPanelView, new Rect(Left1, 0, Width2, Height1));
            GotoRect(sportView, new Rect(ActualWidth, Top1, 0, Height2));
            
        }



        void Minimize()
        {
            if (IsExpanded == true)
            {
                IsExpanded = false;
                BeginMinimizeAnimation();
            }
            
        }

        private void surfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            Minimize();
        }

        bool IsExpanded { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadedAnimation();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsExpanded)
                BeginExpandAnimation();
            else
                BeginMinimizeAnimation();
        }
    }

}
