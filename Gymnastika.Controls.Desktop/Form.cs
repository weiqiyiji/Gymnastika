using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Gymnastika.Controls.Desktop
{
    public class Form : ItemsControl
    {
        public ContentControl Header
        {
            get { return (ContentControl)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(ContentControl), typeof(Form), new UIPropertyMetadata(null));

        public FormItemCollection FormItems
        {
            get { return (FormItemCollection)GetValue(FormItemsProperty); }
            set { SetValue(FormItemsProperty, value); }
        }
     
        public static readonly DependencyProperty FormItemsProperty =
            DependencyProperty.Register("FormItems", typeof(FormItemCollection), typeof(Form), new UIPropertyMetadata(null));

        
    }
}
