using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using FluidKit.Controls;
using Gymnastika.Common.Navigation;
using Microsoft.Practices.ServiceLocation;

namespace Gymnastika.Common.Navigation
{
    public class NavigationContainer
    {
        public static bool GetIsContainer(TransitionPresenter obj)
        {
            return (bool)obj.GetValue(IsContainerProperty);
        }

        public static void SetIsContainer(TransitionPresenter obj, bool value)
        {
            obj.SetValue(IsContainerProperty, value);
        }

        public static readonly DependencyProperty IsContainerProperty =
            DependencyProperty.RegisterAttached("IsContainer", typeof(bool), typeof(NavigationManager), new UIPropertyMetadata(false, OnSetContainer));

        private static void OnSetContainer(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (bool.Parse(e.NewValue.ToString()))
            {
                UpdateContainer((TransitionPresenter)sender);
            }
        }

        private static void UpdateContainer(TransitionPresenter presenter)
        {
            if (presenter.Items != null)
                presenter.Items.Clear();

            INavigationContainerAccessor containerAccessor = ServiceLocator.Current.GetInstance<INavigationContainerAccessor>();
            containerAccessor.Container = presenter;
        }
    }
}
